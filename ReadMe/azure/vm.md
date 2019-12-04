<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
**Table of Contents**  *generated with [DocToc](https://github.com/thlorenz/doctoc)*

- [Virtual Machines Management](#virtual-machines-management)
  - [Login interactively and set a subscription to be the current active subscription](#login-interactively-and-set-a-subscription-to-be-the-current-active-subscription)
  - [Create a resource group, then query the list of resource groups in our subscription](#create-a-resource-group-then-query-the-list-of-resource-groups-in-our-subscription)
  - [Create virtual network (vnet) and Subnet](#create-virtual-network-vnet-and-subnet)
  - [Create public IP address](#create-public-ip-address)
  - [Create network security group](#create-network-security-group)
  - [Create a virtual network interface and associate with public IP address and NSG](#create-a-virtual-network-interface-and-associate-with-public-ip-address-and-nsg)
  - [Create a virtual machine](#create-a-virtual-machine)
  - [Open remote access port](#open-remote-access-port)
  - [Grab the public IP of the virtual machine](#grab-the-public-ip-of-the-virtual-machine)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

# Virtual Machines Management

> Installing Azure CLI For mac or linux terminal\
> brew update && brew install azure-cli\
> [Check for other Operating Systems](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest)

## Login interactively and set a subscription to be the current active subscription

az login --subscription "Subscript name."

## Create a resource group, then query the list of resource groups in our subscription

az group create \
    --name "psdemo-rg" \
    --location "centralus"

az group list -o table

## Create virtual network (vnet) and Subnet

az network vnet create \
    --resource-group "psdemo-rg" \
    --name "psdemo-vnet-1" \
    --address-prefix "10.1.0.0/16" \
    --subnet-name "psdemo-subnet-1" \
    --subnet-prefix "10.1.1.0/24"

az network vnet list -o table

## Create public IP address

az network public-ip create \
    --resource-group "psdemo-rg" \
    --name "psdemo-linux-1-pip-1"

## Create network security group

az network nsg create \
    --resource-group "psdemo-rg" \
    --name "psdemo-linux-nsg-1"

az network nsg list --output table

## Create a virtual network interface and associate with public IP address and NSG

az network nic create \
  --resource-group "psdemo-rg" \
  --name "psdemo-linux-1-nic-1" \
  --vnet-name "psdemo-vnet-1" \
  --subnet "psdemo-subnet-1" \
  --network-security-group "psdemo-linux-nsg-1" \
  --public-ip-address "psdemo-linux-1-pip-1"

az network nic list --output table

## Create a virtual machine

* Linux

    az vm create \
        --resource-group "psdemo-rg" \
        --location "centralus" \
        --name "psdemo-linux-1" \
        --nics "psdemo-linux-1-nic-1" \
        --image "rhel" \
        --admin-username "demoadmin" \
        --authentication-type "ssh" \
        --ssh-key-value ~/.ssh/id_rsa.pub

* Windows

    az vm create \
        --resource-group "psdemo-rg" \
        --name "psdemo-win-1" \
        --location "centralus" \
        --nics "psdemo-win-1-nic-1" \
        --image "win2016datacenter" \
        --admin-username "demoadmin" \
        --admin-password "password123412123$%^&*"

## Open remote access port

* linux

    az vm open-port \
        --resource-group "psdemo-rg" \
        --name "psdemo-linux-1" \
        --port "22"

* Windows

    az vm open-port \
        --port "3389" \
        --resource-group "psdemo-rg" \
        --name "psdemo-win-1"

## Grab the public IP of the virtual machine

az vm list-ip-addresses --name "psdemo-linux-1" --output table

ssh -l demoadmin 168.61.212.180
