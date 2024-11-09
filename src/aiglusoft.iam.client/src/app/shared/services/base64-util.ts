// src/app/services/base64-util.ts
export function base64Encode(input: string): string {
    return btoa(input);
  }
  
  export function base64Decode(input: string): string {
    return atob(input);
  }
  
  export function base64UrlEncode(input: string): string {
    return base64Encode(input).replace(/\+/g, '-').replace(/\//g, '_').replace(/=+$/, '');
  }
  
  export function base64UrlDecode(input: string): string {
    input = input.replace(/-/g, '+').replace(/_/g, '/');
    while (input.length % 4) {
      input += '=';
    }
    return base64Decode(input);
  }
  