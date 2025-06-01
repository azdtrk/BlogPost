import { User } from './user.model';

export interface BlogPost {
  id?: string;
  title: string;
  Title?: string;
  content?: string;
  preface?: string;
  Preface?: string;
  dateCreated?: Date;
  comments?: Comment[];
  author?: {
    id: string;
    username: string;
  };
  images?: Image[];
  thumbNailImage?: {
    id: string;
    path: string;
    fileName: string;
  };
}

export interface Comment {
  id: number;
  content: string;
  userName: string;
  dateCreated: Date;
  blogPostId: number;
}

export interface Image {
  id: string;
  fileName: string;
  path: string;
}