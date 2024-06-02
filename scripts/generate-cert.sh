#!/bin/bash

# Variables
CERT_DIR="./"
CERT_NAME="certificate-dev"
CERT_DAYS=365
PFX_PASSWORD=""

# Créer le répertoire de certificats s'il n'existe pas
mkdir -p "${CERT_DIR}"

# Générer une clé privée
openssl genrsa -out "${CERT_DIR}/${CERT_NAME}_private_key.pem" 2048

# Créer une demande de signature de certificat (CSR) avec les informations minimales
openssl req -new -key "${CERT_DIR}/${CERT_NAME}_private_key.pem" -out "${CERT_DIR}/${CERT_NAME}_csr.pem" -subj "/CN=aiglusoft.com"

# Vérifier si la CSR a été créée avec succès
if [ ! -f "${CERT_DIR}/${CERT_NAME}_csr.pem" ]; then
    echo "Erreur : la CSR n'a pas été créée"
    exit 1
fi

# Générer un certificat auto-signé à partir de la clé privée et de la CSR
openssl x509 -req -days ${CERT_DAYS} -in "${CERT_DIR}/${CERT_NAME}_csr.pem" -signkey "${CERT_DIR}/${CERT_NAME}_private_key.pem" -out "${CERT_DIR}/${CERT_NAME}.pem"

# Vérifier si le certificat a été créé avec succès
if [ ! -f "${CERT_DIR}/${CERT_NAME}.pem" ]; then
    echo "Erreur : le certificat n'a pas été créé"
    exit 1
fi

# Convertir le certificat et la clé privée en un fichier PFX
openssl pkcs12 -export -out "${CERT_DIR}/${CERT_NAME}.pfx" -inkey "${CERT_DIR}/${CERT_NAME}_private_key.pem" -in "${CERT_DIR}/${CERT_NAME}.pem" -password pass:${PFX_PASSWORD}

# Vérifier si le fichier PFX a été créé avec succès
if [ ! -f "${CERT_DIR}/${CERT_NAME}.pfx" ]; then
    echo "Erreur : le fichier PFX n'a pas été créé"
    exit 1
fi

echo "Certificat généré avec succès : ${CERT_DIR}/${CERT_NAME}.pfx"
