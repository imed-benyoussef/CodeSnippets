#!/bin/bash

# Valeurs par défaut
DEFAULT_DOMAIN="localhost"
DEFAULT_COUNTRY="FR"
DEFAULT_STATE=""
DEFAULT_CITY=""
DEFAULT_ORGANIZATION="Aiglusoft"
DEFAULT_ORG_UNIT="Labs"
DEFAULT_DAYS=3650
CERT_DIR="certs"
DEFAULT_PASSWORD="P@ssword"

# Utilisation
usage() {
  echo "Usage: $0 [-d domain] [-c country] [-s state] [-l city] [-o organization] [-u org_unit] [-a days] [-p password]"
  exit 1
}

# Lire les options
while getopts "d:c:s:l:o:u:a:p:" opt; do
  case "${opt}" in
    d)
      DOMAIN=${OPTARG}
      ;;
    c)
      COUNTRY=${OPTARG}
      ;;
    s)
      STATE=${OPTARG}
      ;;
    l)
      CITY=${OPTARG}
      ;;
    o)
      ORGANIZATION=${OPTARG}
      ;;
    u)
      ORG_UNIT=${OPTARG}
      ;;
    a)
      DAYS=${OPTARG}
      ;;
    p)
      PASSWORD=${OPTARG}
      ;;
    *)
      usage
      ;;
  esac
done

# Utiliser les valeurs par défaut si aucun argument n'est fourni
DOMAIN=${DOMAIN:-$DEFAULT_DOMAIN}
COUNTRY=${COUNTRY:-$DEFAULT_COUNTRY}
STATE=${STATE:-$DEFAULT_STATE}
CITY=${CITY:-$DEFAULT_CITY}
ORGANIZATION=${ORGANIZATION:-$DEFAULT_ORGANIZATION}
ORG_UNIT=${ORG_UNIT:-$DEFAULT_ORG_UNIT}
DAYS=${DAYS:-$DEFAULT_DAYS}
PASSWORD=${PASSWORD:-$DEFAULT_PASSWORD}

# Créer le répertoire des certificats s'il n'existe pas
mkdir -p ${CERT_DIR}

# Générer une clé privée avec mot de passe
openssl genpkey -algorithm RSA -out ${CERT_DIR}/${DOMAIN}.key -aes256 -pass pass:${PASSWORD}

# Créer un fichier de demande de signature de certificat (CSR)
openssl req -new -key ${CERT_DIR}/${DOMAIN}.key -out ${CERT_DIR}/${DOMAIN}.csr -subj "/C=${COUNTRY}/ST=${STATE}/L=${CITY}/O=${ORGANIZATION}/OU=${ORG_UNIT}/CN=${DOMAIN}" -passin pass:${PASSWORD}

# Créer un certificat auto-signé
openssl x509 -req -days ${DAYS} -in ${CERT_DIR}/${DOMAIN}.csr -signkey ${CERT_DIR}/${DOMAIN}.key -out ${CERT_DIR}/${DOMAIN}.crt -passin pass:${PASSWORD}

# Vérifier le certificat
openssl x509 -in ${CERT_DIR}/${DOMAIN}.crt -text -noout

echo "Certificat généré avec succès pour ${DOMAIN} et stocké dans le répertoire ${CERT_DIR}/"
