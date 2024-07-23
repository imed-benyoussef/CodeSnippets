#!/bin/sh

# Remplacer les variables d'environnement dans les fichiers JavaScript générés
for file in /app/wwwroot/*.js; do
  sed -i "s|__SIGNIN_REMOTE__|${SIGNIN_REMOTE}|g" $file
  sed -i "s|__SIGNUP_REMOTE__|${SIGNUP_REMOTE}|g" $file
  sed -i "s|__ACCOUNT_REMOTE__|${ACCOUNT_REMOTE}|g" $file
done

# Démarrer nginx
exec "$@"
