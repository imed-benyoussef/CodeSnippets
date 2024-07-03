#!/bin/bash

# Arrêter les services Kubernetes
echo "Stopping Kubernetes services..."
sudo systemctl stop kubelet

# Désinstaller Kubernetes
echo "Uninstalling Kubernetes..."
sudo apt-get purge -y kubeadm kubectl kubelet kubernetes-cni kube*
sudo apt-get autoremove -y
sudo apt-get autoclean -y

# Supprimer les fichiers de configuration Kubernetes
echo "Removing Kubernetes configuration files..."
sudo rm -rf ~/.kube
sudo rm -rf /etc/kubernetes
sudo rm -rf /var/lib/etcd
sudo rm -rf /var/lib/kubelet
sudo rm -rf /var/lib/kube-proxy
sudo rm -rf /var/lib/cni

# Forcer la suppression des répertoires qui sont encore occupés
echo "Force removing remaining Kubernetes directories..."
sudo rm -rf /var/lib/kubelet/*

# Arrêter les services Docker
echo "Stopping Docker services..."
sudo systemctl stop docker
sudo systemctl stop docker.socket
sudo systemctl stop containerd

# Désinstaller Docker et containerd
echo "Uninstalling Docker and containerd..."
sudo apt-get purge -y docker-ce docker-ce-cli containerd containerd.io docker-compose-plugin
sudo apt-get autoremove -y
sudo apt-get autoclean -y

# Supprimer les fichiers de configuration et de données Docker et containerd
echo "Removing Docker and containerd configuration and data files..."
sudo rm -rf /var/lib/docker
sudo rm -rf /var/lib/containerd
sudo rm -rf /etc/docker
sudo rm -rf /etc/containerd
sudo rm -rf /etc/systemd/system/docker.service
sudo rm -rf /etc/systemd/system/docker.socket
sudo rm -rf /etc/systemd/system/containerd.service
sudo rm -rf /usr/bin/docker*
sudo rm -rf /usr/local/bin/docker*
sudo rm -rf /usr/local/bin/docker-compose
sudo rm -rf /var/run/docker*
sudo rm -rf /var/run/containerd*

# Vérification de la désinstallation
echo "Verifying uninstallation..."
if ! which kubeadm && ! which kubectl && ! which kubelet && ! which docker && ! which containerd; then
    echo "Kubernetes, Docker, and containerd have been successfully uninstalled."
else
    echo "Error: Some components of Kubernetes, Docker, or containerd are still installed."
fi
