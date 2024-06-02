# Génération d'une clé privée
openssl genrsa -out private.key 2048

# Génération d'une demande de certificat (CSR)
openssl req -new -key private.key -out csr.csr -subj "/C=FR/O=AiglusoftDevLab/CN=aiglusoft.dev"

# Génération d'un certificat auto-signé à partir de la clé privée et de la demande de certificat
openssl x509 -req -days 365 -in csr.csr -signkey private.key -out certificate.crt

# Conversion du certificat et de la clé privée en un fichier PFX
openssl pkcs12 -export -out certificate.pfx -inkey private.key -in certificate.crt

# Suppression des fichiers temporaires
rm private.key csr.csr certificate.crt