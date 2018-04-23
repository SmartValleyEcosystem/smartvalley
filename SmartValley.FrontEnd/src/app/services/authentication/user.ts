export class User {
  id: number;
  account: string;
  signature: string;
  token: string;
  email: string;
  roles: string[];

  constructor(id: number, account: string, signature: string, token: string, email: string, roles: string[]) {
    this.id = id;
    this.account = account;
    this.signature = signature;
    this.token = token;
    this.email = email;
    this.roles = roles;
  }

  get isExpert(): boolean {
    return this.roles.includes('Expert');
  }

  get isAdmin(): boolean {
    return this.roles.includes('Admin');
  }
}
