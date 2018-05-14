export interface UserResponse {
  isEmailConfirmed: boolean;
  firstName: string;
  lastName: string;
  address: string;
  email: string;
  canCreatePrivateProjects: boolean;
  registrationDate: Date;
}
