#!/bin/bash

# Mettre à jour le système
echo "Updating the system..."
sudo apt update && sudo apt upgrade -y

# Installer MicroK8s
echo "Installing MicroK8s..."
sudo snap install microk8s --classic

# Ajouter l'utilisateur au groupe microk8s
echo "Adding user to microk8s group..."
sudo usermod -a -G microk8s $USER

# Changer le propriétaire du répertoire .kube
echo "Changing ownership of .kube directory..."
sudo chown -f -R $USER ~/.kube

# Appliquer les changements de groupe sans déconnexion
newgrp microk8s

# Vérifier l'installation de MicroK8s
echo "Checking MicroK8s status..."
microk8s status --wait-ready

# Activer les composants supplémentaires
echo "Enabling additional components: dashboard, dns, storage..."
microk8s enable dashboard dns storage

# Créer un alias pour kubectl
echo "Creating alias for kubectl..."
alias kubectl='microk8s kubectl'

# Ajouter l'alias au fichier ~/.bashrc
echo "Adding alias to ~/.bashrc..."
echo 'alias kubectl="microk8s kubectl"' >> ~/.bashrc

# Appliquer les changements de ~/.bashrc
source ~/.bashrc

# Vérifier le cluster
echo "Verifying the cluster..."
kubectl get nodes
kubectl get services

# Obtenir le jeton d'accès pour le tableau de bord
echo "Getting access token for the Kubernetes dashboard..."
token=$(microk8s kubectl -n kube-system get secret | grep default-token | cut -d " " -f1)
microk8s kubectl -n kube-system describe secret $token

# Afficher l'instruction pour accéder au tableau de bord
echo "To access the Kubernetes dashboard, run the following command to start the proxy:"
echo "  microk8s kubectl proxy"
echo "Then open the following URL in your browser:"
echo "  http://localhost:8001/api/v1/namespaces/kube-system/services/https:kubernetes-dashboard:/proxy/"

echo "Installation and configuration of MicroK8s are complete."
