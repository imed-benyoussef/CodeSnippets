export const environment = {
    production: true,
    defaultLanguage: '__DEFAULT_LANGUAGE__',
    translationUrl: '__TRANSLATION_URL__',
    apiUrl: 'https://api.aiglusoft.com/api',
    oidc: {
      issuer: 'https://identity.aiglusoft.com',
      clientId: 'iam-client',
      scope: 'openid profile email api',
      redirectUri: 'https://iam.aiglusoft.com/auth/callback',
      silentRefreshUri: 'https://iam.aiglusoft.com/auth/silent-refresh',
      postLogoutRedirectUri: 'https://iam.aiglusoft.com'
    }
};
