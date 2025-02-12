import { User } from "./user.model";

export interface Comment {
  id: number;
  content: string;
  blogPostId: number;
  authorId: number;
  author: User;
  createdAt: Date;
}