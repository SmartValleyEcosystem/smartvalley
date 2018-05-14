export interface AuthenticationResponse {
  id: number;
  email: string;
  token: string;
  canCreatePrivateProjects: boolean;
  roles: string[];
}
