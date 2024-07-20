const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:7265';

const mfe_signup = 'http://localhost:42022'

const PROXY_CONFIG = [{
    context: [
        "/api",
        "/connect",
        "/.well-known",
        "/encryption"
    ],
    target,
    secure: false
}]

module.exports = PROXY_CONFIG;
