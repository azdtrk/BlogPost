import { User } from './user.model';

export interface BlogPost {
  id: number;
  title: string;
  content: string;
  dateCreated: Date;
  comments?: Comment[];
  author?: User;
}

export interface Comment {
  id: number;
  content: string;
  userName: string;
  dateCreated: Date;
  blogPostId: number;
} 