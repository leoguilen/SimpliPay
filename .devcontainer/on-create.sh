# Update apt packages
sudo apt update -y && \
    sudo apt upgrade -y && \
    sudo apt autoremove -y && \
    sudo apt clean && \
    sudo rm -rf /var/lib/apt/lists/*