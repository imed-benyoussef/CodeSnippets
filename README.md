# Aiglusoft.IAM


# Générer une clé privée
openssl genrsa -out private_key.pem 2048

# Créer une demande de signature de certificat (CSR)
openssl req -new -key private_key.pem -out csr.pem -subj "/CN=aiglusoft.com"

# Générer un certificat auto-signé à partir de la clé privée et de la CSR
openssl x509 -req -days 365 -in csr.pem -signkey private_key.pem -out certificate.pem

# Convertir le certificat et la clé privée en un fichier PFX
openssl pkcs12 -export -out certificate.pfx -inkey private_key.pem -in certificate.pem -certfile certificate.pem
