<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
**Table of Contents**  *generated with [DocToc](https://github.com/thlorenz/doctoc)*

- [Manage Disk (Linux)](#manage-disk-linux)
  - [login interactively and set a subscription to be the current active subscription](#login-interactively-and-set-a-subscription-to-be-the-current-active-subscription)
  - [Creating a new data disk with Azure CLI and attach it to our VM. This can be done hot](#creating-a-new-data-disk-with-azure-cli-and-attach-it-to-our-vm-this-can-be-done-hot)
    - [1 - Attach the new disk](#1---attach-the-new-disk)
    - [2 - Prepare the disk for use by the operating system](#2---prepare-the-disk-for-use-by-the-operating-system)
    - [3 - Find the new block decvice, we know /dev/sda is the OS, and /dev/sdb is the temporary disk](#3---find-the-new-block-decvice-we-know-devsda-is-the-os-and-devsdb-is-the-temporary-disk)
    - [4 - partition the disk with fdisk and use the following commands to name a new primary parition](#4---partition-the-disk-with-fdisk-and-use-the-following-commands-to-name-a-new-primary-parition)
    - [5 - format the new partition with ext4](#5---format-the-new-partition-with-ext4)
    - [6 - Make a directory to mount the new disk under](#6---make-a-directory-to-mount-the-new-disk-under)
    - [7 - Add the following line to /etc/fstab. First find the UUID for this device, in our case it's /dev/sdc1](#7---add-the-following-line-to-etcfstab-first-find-the-uuid-for-this-device-in-our-case-its-devsdc1)
    - [8 - mount the volume and verify the file system is mounted](#8---mount-the-volume-and-verify-the-file-system-is-mounted)
    - [9 - Exit from the Linux VM](#9---exit-from-the-linux-vm)
  - [Resizing a disk](#resizing-a-disk)
    - [1 - Stop and deallocate the VM. this has to be an offline operation](#1---stop-and-deallocate-the-vm-this-has-to-be-an-offline-operation)
    - [2 - Find the disk's name we want to expand](#2---find-the-disks-name-we-want-to-expand)
    - [3 - Update the disk's size to the desired size](#3---update-the-disks-size-to-the-desired-size)
    - [4 - start up the VM again](#4---start-up-the-vm-again)
    - [5 - Log into the guest OS and resize the volume](#5---log-into-the-guest-os-and-resize-the-volume)
    - [6 - Unmount filesystem and expand the partition](#6---unmount-filesystem-and-expand-the-partition)
    - [7 - fsck, expand and mount the filesystem](#7---fsck-expand-and-mount-the-filesystem)
    - [8 - Verify the added space is available](#8---verify-the-added-space-is-available)
  - [Removing a disk](#removing-a-disk)
    - [1 - Umount the disk in the OS, remove the disk we added above from fstab](#1---umount-the-disk-in-the-os-remove-the-disk-we-added-above-from-fstab)
    - [2 - Detaching the disk from the virtual machine. This can be done online too](#2---detaching-the-disk-from-the-virtual-machine-this-can-be-done-online-too)
    - [3 - Delete the disk](#3---delete-the-disk)
  - [Snapshotting the OS disk](#snapshotting-the-os-disk)
    - [1 - Find the disk we want to snapshot and create a snapshot of the disk](#1---find-the-disk-we-want-to-snapshot-and-create-a-snapshot-of-the-disk)
    - [2 - Getting a list of the snapshots available](#2---getting-a-list-of-the-snapshots-available)
    - [3 - Create a new disk from the snapshot we just created](#3---create-a-new-disk-from-the-snapshot-we-just-created)
    - [4 - Create a VM from the disk we just created](#4---create-a-vm-from-the-disk-we-just-created)
    - [5 - If we want we can delete a snapshot when we're finished](#5---if-we-want-we-can-delete-a-snapshot-when-were-finished)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

# Manage Disk (Linux)

* 1 - Attach a disk to an existing VM
* 2 - Resizing a disk
* 3 - Removing a disk
* 4 - Snapshotting an OS volume and creating a VM from a snapshot

## login interactively and set a subscription to be the current active subscription

az login --subscription "Demonstration Account"

## Creating a new data disk with Azure CLI and attach it to our VM. This can be done hot

### 1 - Attach the new disk

az vm disk attach \
    --resource-group "psdemo-rg" \
    --vm-name "psdemo-linux-1c" \
    --disk "psdemo-linux-1c-st0" \
    --new \
    --size-gb 25 \
    --sku "Premium_LRS" #Other options are StandardSSD_LRS and Standard_LRS

### 2 - Prepare the disk for use by the operating system

az vm list-ip-addresses \
    --name "psdemo-linux-1c" \
    --output table

ssh -l demoadmin 168.61.222.248

### 3 - Find the new block decvice, we know /dev/sda is the OS, and /dev/sdb is the temporary disk

lsblk

> We can also use dmesg, like docs.microsoft.com says

dmesg | grep SCSI

### 4 - partition the disk with fdisk and use the following commands to name a new primary parition

sudo fdisk /dev/sdc\

m\
n\
p\
1\
w

### 5 - format the new partition with ext4

sudo mkfs -t ext4 /dev/sdc1

### 6 - Make a directory to mount the new disk under

sudo mkdir /data1

### 7 - Add the following line to /etc/fstab. First find the UUID for this device, in our case it's /dev/sdc1

sudo -i blkid | grep sdc1

UUID=7d9f3af0-8927-4c73-aafe-a221e659d109      /data1  ext4   defaults        0 0
sudo vi /etc/fstab

### 8 - mount the volume and verify the file system is mounted

sudo mount -a
df -h

### 9 - Exit from the Linux VM

exit

## Resizing a disk

### 1 - Stop and deallocate the VM. this has to be an offline operation

az vm deallocate \
    --resource-group "psdemo-rg" \
    --name "psdemo-linux-1c"

### 2 - Find the disk's name we want to expand

az disk list \
    --output table

### 3 - Update the disk's size to the desired size

az disk update \
    --resource-group "psdemo-rg" \
    --name "psdemo-linux-1c-st0" \
    --size-gb 100

### 4 - start up the VM again

az vm start \
    --resource-group "psdemo-rg" \
    --name "psdemo-linux-1c"

### 5 - Log into the guest OS and resize the volume

az vm list-ip-addresses \
    --name "psdemo-linux-1c" \
    --output table

ssh -l demoadmin 40.122.37.234

### 6 - Unmount filesystem and expand the partition

sudo vi /etc/fstab #comment out our mount

sudo umount /data1
sudo parted /dev/sdc

### 7 - fsck, expand and mount the filesystem

sudo e2fsck -f /dev/sdc1
sudo resize2fs /dev/sdc1
sudo mount /dev/sdc1 /data1
sudo vi /etc/fstab
sudo mount -a

### 8 - Verify the added space is available

df -h  | grep data1

## Removing a disk

### 1 - Umount the disk in the OS, remove the disk we added above from fstab

ssh -l demoadmin w.x.y.z
sudo vi /etc/fstab
sudo umount /data1
df -h | grep /data1
exit

### 2 - Detaching the disk from the virtual machine. This can be done online too

az vm disk detach \
    --resource-group "psdemo-rg" \
    --vm-name "psdemo-linux-1c" \
    --name "psdemo-linux-1c-st0"

### 3 - Delete the disk

az disk delete \
    --resource-group "psdemo-rg" \
    --name "psdemo-linux-1c-st0"

## Snapshotting the OS disk

### 1 - Find the disk we want to snapshot and create a snapshot of the disk

az disk list --output table | grep psdemo-linux-1c

> update the --source parameter with the disk from the last command.

az snapshot create \
    --resource-group "psdemo-rg" \
    --source "psdemo-linux-1c_disk1_9c9f359319204c3a8d1f128685c13d22" \
    --name "psdemo-linux-1c-OSDisk-1-snap-1"

### 2 - Getting a list of the snapshots available

az snapshot list \
    --output table

### 3 - Create a new disk from the snapshot we just created

> If this was a data disk, we could just attach and mount this disk to a VM

az disk create \
    --resource-group "psdemo-rg" \
    --name "psdemo-linux-1f-OSDisk-1" \
    --source "psdemo-linux-1c-OSDisk-1-snap-1" \
    --size-gb "40"

### 4 - Create a VM from the disk we just created

az vm create \
    --resource-group "psdemo-rg" \
    --name "psdemo-linux-1f" \
    --attach-os-disk "psdemo-linux-1f-OSDisk-1" \
    --os-type "Linux"

### 5 - If we want we can delete a snapshot when we're finished

az snapshot delete \
    --resource-group "psdemo-rg" \
    --name "psdemo-linux-1c-OSDisk-1-snap-1"
