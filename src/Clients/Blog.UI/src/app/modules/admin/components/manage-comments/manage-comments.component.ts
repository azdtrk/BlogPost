import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Comment } from '../../../../core/models/comment.model';
import { CommentService } from '../../../../core/services/comment.service';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-manage-comments',
  templateUrl: './manage-comments.component.html',
  styleUrls: ['./manage-comments.component.scss'],
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule]
})
export class ManageCommentsComponent implements OnInit {
  comments: Comment[] = [];
  filteredComments: Comment[] = [];
  isLoading = false;
  searchTerm = '';

  constructor(private commentService: CommentService) {}

  ngOnInit(): void {
    this.loadComments();
  }

  filterComments(): void {
    if (!this.searchTerm.trim()) {
      this.filteredComments = [...this.comments];
      return;
    }

    const searchTermLower = this.searchTerm.toLowerCase();
    this.filteredComments = this.comments.filter(comment =>
      comment.content.toLowerCase().includes(searchTermLower) ||
      comment.author?.username.toLowerCase().includes(searchTermLower)
    );
  }

  deleteComment(commentId: string): void {
    if (confirm('Are you sure you want to delete this comment?')) {
      this.commentService.delete(commentId).subscribe({
        next: () => {
          this.comments = this.comments.filter(comment => comment.id !== commentId);
          this.filterComments();
        }
      });
    }
  }

  private loadComments(): void {
    this.isLoading = true;
    this.commentService.getAll().subscribe({
      next: (comments) => {
        this.comments = comments;
        this.filteredComments = comments;
        this.isLoading = false;
      },
      error: (error) => {
        this.isLoading = false;
      }
    });
  }
}
