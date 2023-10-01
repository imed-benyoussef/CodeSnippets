# Configure Sudo Without Password Script

This script allows you to add a user to the sudo group and configure sudo to allow that user to execute commands without entering a password.

## Usage

To use the script, follow these steps:

1. Open a terminal on your Ubuntu server.

2. Run the script with the following command:

   ```bash
   chmod +x configure_sudo.sh
   sudo ./configure_sudo.sh [-u <username>]
   ```

# Change Hostname Script

This Bash script allows you to change the hostname of a Linux machine. It uses the `hostnamectl` command to update the hostname. You can customize the new hostname by editing the script.

## Usage

1. Open a terminal on your Linux machine.

2. Navigate to the directory containing the script, or provide the full path to the script.

3. Run the script with root privileges (sudo) as follows:

   ```bash
   chmod +x change_hostname.sh
   sudo ./change_hostname.sh new_hostname
