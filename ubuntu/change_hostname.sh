#!/bin/bash

# Check if the script is running with root privileges
if [ "$EUID" -ne 0 ]; then
  echo "This script must be run as an administrator (root)."
  exit 1
fi

# Define the new hostname (customize this)
new_hostname="localhost"

# Check if a new hostname is provided
if [ -z "$new_hostname" ]; then
  echo "Please specify the new hostname by editing this script."
  exit 1
fi

# Change the hostname using hostnamectl
hostnamectl set-hostname "$new_hostname"

# Display the new hostname
new_host_name=$(hostnamectl --static)
echo "The hostname has been changed to: $new_host_name"

# You may need to restart your system for the hostname changes to take effect
# echo "Please restart your system to apply the hostname changes."

exit 0
