#!/bin/bash

# Check if the script is running with root privileges
if [ "$EUID" -ne 0 ]; then
  echo "This script must be run as an administrator (root)."
  exit 1
fi

# Check if the new hostname is specified as an argument; use "localhost" as the default
new_hostname="${1:-localhost}"

# Change the hostname using hostnamectl
hostnamectl set-hostname "$new_hostname"

# Display the new hostname
new_host_name=$(hostnamectl --static)
echo "The hostname has been changed to: $new_host_name"

# You may need to restart your system for the hostname changes to take effect
# echo "Please restart your system to apply the hostname changes."

exit 0
