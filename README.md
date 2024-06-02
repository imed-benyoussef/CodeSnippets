# Aiglusoft.IAM


# G�n�rer une cl� priv�e
openssl genrsa -out private_key.pem 2048

# Cr�er une demande de signature de certificat (CSR)
openssl req -new -key private_key.pem -out csr.pem -subj "/CN=aiglusoft.com"

# G�n�rer un certificat auto-sign� � partir de la cl� priv�e et de la CSR
openssl x509 -req -days 365 -in csr.pem -signkey private_key.pem -out certificate.pem

# Convertir le certificat et la cl� priv�e en un fichier PFX
openssl pkcs12 -export -out certificate.pfx -inkey private_key.pem -in certificate.pem -certfile certificate.pem
