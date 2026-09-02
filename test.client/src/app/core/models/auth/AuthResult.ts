export interface AuthResult {
  token: string;
  expiresAtUtc: string;
  email: string;
  roles: string[];
}
