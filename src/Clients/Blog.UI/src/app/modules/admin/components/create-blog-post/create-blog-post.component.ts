import { Component, OnInit, OnDestroy, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { BlogPostService, ImageResponse } from '../../../../core/services/blog-post.service';
import { Router } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import { QuillModule } from 'ngx-quill';
import Quill from 'quill';
import { AuthService } from '../../../../core/services/auth.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-create-blog-post',
  templateUrl: './create-blog-post.component.html',
  styleUrls: ['./create-blog-post.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    QuillModule
  ]
})
export class CreateBlogPostComponent implements OnInit, OnDestroy {
  postForm: FormGroup;
  isSubmitting = false;
  errorMessage = '';
  uploadedImages: ImageResponse[] = [];
  thumbnailImage: ImageResponse | null = null;
  hasSavedDraft = false;
  draftCreated = false;
  draftId: string | null = null;
  private formChanged = false;
  private subscriptions: Subscription[] = [];

  quillConfig = {
    toolbar: {
      container: [
        ['bold', 'italic', 'underline', 'strike'],
        ['blockquote', 'code-block'],
        [{ 'header': 1 }, { 'header': 2 }],
        [{ 'list': 'ordered'}, { 'list': 'bullet' }],
        [{ 'script': 'sub'}, { 'script': 'super' }],
        [{ 'indent': '-1'}, { 'indent': '+1' }],
        [{ 'direction': 'rtl' }],
        [{ 'size': ['small', false, 'large', 'huge'] }],
        [{ 'header': [1, 2, 3, 4, 5, 6, false] }],
        [{ 'color': [] }, { 'background': [] }],
        [{ 'font': [] }],
        [{ 'align': [] }],
        ['clean'],
        ['link', 'image']
      ],
      handlers: {
        image: this.imageHandler.bind(this)
      }
    }
  };

  constructor(
    private fb: FormBuilder,
    private blogPostService: BlogPostService,
    private router: Router,
    private sanitizer: DomSanitizer,
    private authService: AuthService
  ) {
    this.postForm = this.fb.group({
      id: [''],
      title: ['', [Validators.required, Validators.minLength(3)]],
      content: ['', [Validators.required, Validators.minLength(10)]],
      preface: ['', [Validators.required, Validators.minLength(10)]]
    });

    this.checkAuthState();
  }

  private checkAuthState(): void {
    if (!this.authService.validateAndRefreshTokenIfNeeded()) {
      this.errorMessage = 'You are not logged in or your session has expired. Please log in to create a post.';
      this.authService.logout();
    }
  }

  ngOnInit(): void {
    this.checkForSavedDraft();
    this.initializeDraft();
    
    // Watch for form changes to enable auto-save
    this.subscriptions.push(
      this.postForm.valueChanges.subscribe(() => {
        this.formChanged = true;
      })
    );
  }
  
  ngOnDestroy(): void {
    // Clean up subscriptions
    this.subscriptions.forEach(sub => sub.unsubscribe());
    
    // Auto-save draft when leaving component if there are changes
    if (this.formChanged && this.draftId) {
      this.saveDraftToDatabase();
    }
  }
  
  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any): void {
    if (this.formChanged && this.draftId) {
      // Try to save the draft before the page unloads
      this.saveDraftToDatabase();
      $event.returnValue = 'You have unsaved changes. Are you sure you want to leave?';
    }
  }

  private initializeDraft(): void {
    // Get the current user info
    const currentUser = this.authService.getCurrentUser();
    if (!currentUser || !currentUser.id) {
      console.error('Unable to create draft: User data not available');
      return;
    }
    
    // Create a draft blog post in the database
    this.subscriptions.push(
      this.blogPostService.createDraft(currentUser.id).subscribe({
        next: (draftPost) => {
          console.log('Draft blog post created:', draftPost);
          if (draftPost && draftPost.id) {
            this.draftId = draftPost.id;
            this.draftCreated = true;
            
            // Update the form with the draft ID
            this.postForm.patchValue({
              id: draftPost.id
            });
          }
        },
        error: (error) => {
          console.error('Failed to create draft blog post:', error);
          this.errorMessage = 'Failed to initialize a draft. ' + (error.message || '');
        }
      })
    );
  }

  async onThumbnailSelected(event: Event): Promise<void> {
    console.log('onThumbnailSelected called', event);
    const input = event.target as HTMLInputElement;
    if (!input.files || input.files.length === 0) {
      console.log('No files selected');
      return;
    }

    const file = input.files[0];
    console.log('File selected:', file.name, file.type, file.size);
    if (!this.validateImageFile(file)) {
      console.log('File validation failed');
      return;
    }

    try {
      this.errorMessage = '';

      const formData = new FormData();
      formData.append('Image', file);
      formData.append('IsThumbnail', 'true');

      // Use the draft ID for the blog post association
      if (this.draftId) {
        formData.append('BlogPostId', this.draftId);
        formData.append('ThumbnailForBlogPostId', this.draftId);
      }

      console.log('Uploading thumbnail image...');
      this.blogPostService.uploadImage(formData)
        .subscribe({
          next: (response) => {
            console.log('Thumbnail upload successful:', response);
            this.thumbnailImage = response;
            this.formChanged = true;
          },
          error: (error) => {
            console.error('Thumbnail upload failed:', error);
            this.handleUploadError(error);
          }
        });
    } catch (error: any) {
      console.error('Error in onThumbnailSelected:', error);
      this.handleUploadError(error);
    }
  }

  removeThumbnail(): void {
    this.thumbnailImage = null;
    this.formChanged = true;
  }

  private validateImageFile(file: File): boolean {
    const maxSize = 5 * 1024 * 1024; // 5MB
    const allowedTypes = ['image/jpeg', 'image/png', 'image/gif'];

    if (file.size > maxSize) {
      this.errorMessage = 'Image size must be less than 5MB';
      return false;
    }

    if (!allowedTypes.includes(file.type)) {
      this.errorMessage = 'Only JPEG, PNG, and GIF images are allowed';
      return false;
    }

    return true;
  }

  private handleUploadError(error: any): void {
    const statusCode = error.status ||
      (error.message && error.message.includes('401') ? 401 : null) ||
      (error.message && error.message.includes('Unauthorized') ? 401 : null);

    // If session has expired, save it as a draft.
    if (statusCode === 401) {
      this.saveDraft();
      this.errorMessage = 'Your login session has expired. Your draft has been saved. Please log in again to continue.';

      setTimeout(() => {
        localStorage.removeItem('token');
        localStorage.setItem('redirectUrl', '/admin/create-post');
        this.router.navigate(['/login']).then(() => {
          console.log('Redirected to login page');
        });
      }, 2000);
    } else {
      this.errorMessage = error.message || 'Failed to upload image. Please try again.';
    }
  }

  imageHandler() {
    if (!this.authService.validateAndRefreshTokenIfNeeded()) {
      this.handleUploadError({ status: 401, message: 'Not authenticated or token expired' });
      return;
    }

    const input = document.createElement('input');
    input.setAttribute('type', 'file');
    input.setAttribute('accept', 'image/*');
    input.click();

    input.onchange = () => {
      if (!input.files || input.files.length === 0)
        return;

      const file = input.files[0];
      if (!this.validateImageFile(file))
        return;

      this.errorMessage = '';
      const placeholder = this.insertPlaceholder('Uploading image...');

      const formData = new FormData();
      formData.append('Image', file);
      formData.append('IsThumbnail', 'false');

      // Use the draft ID for the blog post association
      if (this.draftId) {
        formData.append('BlogPostId', this.draftId);
      }

      this.blogPostService.uploadImage(formData)
        .subscribe({
          next: (response) => {
            this.uploadedImages.push(response);
            this.formChanged = true;

            const editorElement = document.querySelector('.ql-editor') as HTMLElement;
            if (!editorElement)
              throw new Error('Quill editor not found');

            const quillInstance = (Quill as any).find(editorElement.parentElement);
            if (!quillInstance)
              throw new Error('Quill instance not found');

            this.replacePlaceholder(placeholder, quillInstance, response.path);
          },
          error: (error) => {
            this.removePlaceholder(placeholder);
            this.handleUploadError(error);
          }
        });
    };
  }

  private insertPlaceholder(text: string): { index: number, length: number } {
    const editorElement = document.querySelector('.ql-editor') as HTMLElement;
    if (!editorElement)
      throw new Error('Quill editor not found');

    const quillInstance = (Quill as any).find(editorElement.parentElement);
    if (!quillInstance)
      throw new Error('Quill instance not found');

    const range = quillInstance.getSelection(true);
    const index = range ? range.index : 0;

    quillInstance.insertText(index, text, {
      color: '#999',
      italic: true
    });

    return { index, length: text.length };
  }

  private replacePlaceholder(placeholder: { index: number, length: number }, quillInstance: any, imageUrl: string) {
    quillInstance.deleteText(placeholder.index, placeholder.length);
    quillInstance.insertEmbed(placeholder.index, 'image', imageUrl);
    quillInstance.setSelection(placeholder.index + 1);
  }

  private removePlaceholder(placeholder: { index: number, length: number }) {
    const editorElement = document.querySelector('.ql-editor') as HTMLElement;
    if (!editorElement)
      return;

    const quillInstance = (Quill as any).find(editorElement.parentElement);
    if (!quillInstance)
      return;

    quillInstance.deleteText(placeholder.index, placeholder.length);
  }

  onSubmit(): void {
    if (this.postForm.valid && this.thumbnailImage) {
      this.isSubmitting = true;
      this.errorMessage = '';

      const blogPostData = {
        ...this.postForm.value,
        images: this.uploadedImages.map(img => ({
          id: img.id,
          fileName: img.fileName,
          path: img.path
        })),
        thumbNailImage: this.thumbnailImage ? {
          id: this.thumbnailImage.id,
          fileName: this.thumbnailImage.fileName,
          path: this.thumbnailImage.path
        } : undefined,
        canBePublished: true  // Set to true when explicitly publishing
      };

      // If we have a draft ID, update the existing draft
      if (this.draftId) {
        this.blogPostService.update(this.draftId, blogPostData)
          .subscribe({
            next: (response) => {
              this.isSubmitting = false;
              this.router.navigate(['/admin']);
            },
            error: (error) => {
              this.isSubmitting = false;
              this.errorMessage = error.message || 'Error updating post';
            }
          });
      } else {
        // Create a new blog post
        this.blogPostService.create(blogPostData)
          .subscribe({
            next: (response) => {
              this.isSubmitting = false;
              this.router.navigate(['/admin']);
            },
            error: (error) => {
              this.isSubmitting = false;
              this.errorMessage = error.message || 'Error creating post';
            }
          });
      }
    }
  }

  private saveDraftToDatabase(): void {
    if (!this.draftId || !this.formChanged) {
      return;
    }

    const blogPostData = {
      id: this.draftId,
      title: this.postForm.get('title')?.value || 'Draft',
      content: this.postForm.get('content')?.value || '',
      preface: this.postForm.get('preface')?.value || '',
      images: this.uploadedImages.map(img => ({
        id: img.id,
        fileName: img.fileName,
        path: img.path
      })),
      thumbNailImage: this.thumbnailImage ? {
        id: this.thumbnailImage.id,
        fileName: this.thumbnailImage.fileName,
        path: this.thumbnailImage.path
      } : undefined,
      canBePublished: false  // Always false for drafts
    };

    // Save the current state to the database
    this.blogPostService.update(this.draftId, blogPostData)
      .subscribe({
        next: (response) => {
          console.log('Draft saved successfully', response);
          this.formChanged = false;
        },
        error: (error) => {
          console.error('Failed to save draft', error);
        }
      });
  }

  private saveDraft(): void {
    // Local storage fallback if database save fails
    const draftData = {
      title: this.postForm.get('title')?.value,
      preface: this.postForm.get('preface')?.value,
      content: this.postForm.get('content')?.value,
      savedAt: new Date().toISOString()
    };
    localStorage.setItem('blogPostDraft', JSON.stringify(draftData));

    this.errorMessage = 'Your draft has been saved locally. You will see it when you log back in.';
  }

  private checkForSavedDraft(): void {
    try {
      const savedDraft = localStorage.getItem('blogPostDraft');
      if (savedDraft) {
        const draftData = JSON.parse(savedDraft);

        if (draftData) {
          this.hasSavedDraft = true;

          const confirmRestore = confirm(
            `You have a saved draft from ${new Date(draftData.savedAt).toLocaleString()}. Would you like to restore it?`
          );

          if (confirmRestore) {
            this.postForm.patchValue({
              title: draftData.title || '',
              preface: draftData.preface || '',
              content: draftData.content || ''
            });
            localStorage.removeItem('blogPostDraft');
          } else {
            localStorage.removeItem('blogPostDraft');
          }
        }
      }
    } catch (e) {
      localStorage.removeItem('blogPostDraft');
    }
  }
}
