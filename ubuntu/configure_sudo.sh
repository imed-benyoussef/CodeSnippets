#!/bin/bash

# Check if the current user has administrative privileges
if [[ $(id -u) -ne 0 ]]; then
    echo "This script must be run as an administrator (root)."
    exit 1
fi

# Default variable values
username="ubuntu"

# Function to display usage
function display_usage {
    echo "Usage: $0 [-u <username>]"
    echo "Options:"
    echo "  -u <username>: Specify the username to add to the sudo group (default: ubuntu)."
    exit 1
}

# Process command-line options
while getopts "u:h" opt; do
    case "$opt" in
        u) username="$OPTARG";;
        h) display_usage;;
        \?) echo "Invalid option: -$OPTARG" >&2; display_usage;;
    esac
done

# Add the user to the sudo group
usermod -aG sudo "$username"

# Modify the sudoers file to allow the user to use sudo without a password
echo "$username ALL=(ALL:ALL) NOPASSWD: ALL" >> /etc/sudoers

echo "The user $username has been added to the sudo group and can now use sudo without a password."

exit 0
