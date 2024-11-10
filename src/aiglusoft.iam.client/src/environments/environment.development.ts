export const environment = {
    production: false,
    defaultLanguage: 'fr',
    translationUrl: './assets/i18n/', // URL par défaut pour les traductions
    apiUrl: 'http://localhost:5000/api',
    oidc: {
      issuer: 'http://localhost:5000',
      clientId: 'iam-client',
      scope: 'openid profile email api',
      redirectUri: 'http://localhost:4200/auth/callback',
      silentRefreshUri: 'http://localhost:4200/auth/silent-refresh',
      postLogoutRedirectUri: 'http://localhost:4200'
    }
};
