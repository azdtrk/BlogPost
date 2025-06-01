import { User } from "./user.model";

export interface Comment {
  id: string;
  content: string;
  blogPostId: string;
  authorId: string;
  author?: User;
  createdAt?: Date;
}