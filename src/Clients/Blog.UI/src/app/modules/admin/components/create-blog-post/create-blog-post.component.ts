import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { BlogPostService } from '../../../../core/services/blog-post.service';
import { Router } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import { QuillModule } from 'ngx-quill';
import Quill from 'quill';

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
export class CreateBlogPostComponent {
  postForm: FormGroup;
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
    private sanitizer: DomSanitizer
  ) {
    this.postForm = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(3)]],
      content: ['', [Validators.required, Validators.minLength(10)]]
    });
  }

  imageHandler() {
    const input = document.createElement('input');
    input.setAttribute('type', 'file');
    input.setAttribute('accept', 'image/*');
    input.click();

    input.onchange = async () => {
      const file = input.files?.[0];
      if (file) {
        try {
          const formData = new FormData();
          formData.append('image', file);

          const imageUrl = await this.blogPostService.uploadImage(formData).toPromise();

          const editorElement = document.querySelector('.ql-editor') as HTMLElement;

          if (!editorElement) {
            throw new Error('Quill editor not found');
          }

          const quillInstance = (Quill as any).find(editorElement.parentElement);

          if (!quillInstance) {
            throw new Error('Quill instance not found');
          }

          const range = quillInstance.getSelection(true);

          if (range) {
            quillInstance.insertEmbed(range.index, 'image', imageUrl);
            quillInstance.setSelection(range.index + 1);
          }

        } catch (error) {
          console.error('Image upload failed:', error);
        }
      }
    };
  }

  onSubmit(): void {
    if (this.postForm.valid) {
      this.blogPostService.create(this.postForm.value)
        .subscribe({
          next: () => {
            this.router.navigate(['/admin']);
          },
          error: (error) => {
            console.error('Error creating post:', error);
          }
        });
    }
  }
} 