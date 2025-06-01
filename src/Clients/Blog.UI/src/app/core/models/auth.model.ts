import { User } from "./user.model";

export interface LoginRequest {
  userNameOrEmail: string;
  password: string;
}

export interface RegisterRequest {
  username: string;
  email: string;
  password: string;
  role?: string;
}

export interface AuthResponse {
  data: {
    id: string;
    userName: string;
    token: {
      accessToken: string;
      expiration: Date;
      refreshToken: string;
    };
    role: string;
  };
  succeeded: boolean;
  message: string;
  errors: string[];
}

export interface BackendAuthResponse {
  Value: {
    Id: string;
    UserName: string;
    Token: {
      AccessToken: string;
      Expiration: string;
      RefreshToken: string;
    };
    Role: number;
  };
  IsSuccess: boolean;
  Id: string;
}
