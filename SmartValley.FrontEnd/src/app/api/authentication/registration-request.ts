export interface RegistrationRequest {
  address: string;
  email: string;
  signedText: string;
  signature: string;
  canCreatePrivateProjects: boolean;
}
