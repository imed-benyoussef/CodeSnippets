export const environment = {
    production: false,
    defaultLanguage: 'fr',
    translationUrl: './assets/i18n/', // URL par défaut pour les traductions
    apiUrl: 'https://localhost:4233',
    oidc: {
      issuer: 'https://localhost:4233/',
      clientId: 'iam-client',
      scope: 'openid profile email api',
      redirectUri: 'https://localhost:4233//auth/callback',
      silentRefreshUri: 'https://localhost:4233//auth/silent-refresh',
      postLogoutRedirectUri: 'https://localhost:4233'
    }
};
