
export interface User {
  userId?: string;
  firstName?: string;
  lastName?: string;
  birthDate?: string;
  gender?: string;
  email?: string;
  emailVerified?: boolean;
  phoneNumber?: string;
  phoneVerified?: boolean;
}
export interface EncryptedData {
  encryptedPassword?: string | null;
}

export interface Token {
  access_token: string;
  expires_at: Date
}

export interface Error {
  error: string;
  error_description: string | null | undefined
}

