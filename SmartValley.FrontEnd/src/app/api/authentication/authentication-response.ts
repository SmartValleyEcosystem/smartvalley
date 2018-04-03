export interface AuthenticationResponse {
  id: number;
  email: string;
  token: string;
  roles: string[];
}
