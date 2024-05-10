Project 1
Brady Gehrman	Student ID: 5320689

1.1: 
I chose to go through with a dual boot system installation so I could gain that experience, 
Initially, I didn't have enough space on my C drive on my laptop to partition so I first installed wsl onto my current system,
and then went through a dual boot installation for my old laptop. The video tutorial was very informative and made that process very simple.
First, we navigated to the ubuntu website and downloaded the 22.04 LTS iso file. Then we downloaded an sd formator application that we then used to 
write the 22.04 LTS iso file to our USB drive, making it a bootable drive. We then created a 30 GB partition inside our disk that will later house Ubuntu.
After that, we restarted our computer with the USB drive connected and pull up the boot menu, and select the USB drive, then we are prompted to install Ubuntu.
After going through the installation process of Ubuntu, we then configured ubuntu so that it would appear as a selectable within the boot menu.

1.2: 
10 Linux commands I experimented with
pdw - this command shows the current path you are in within the terminal
ls - this command lets you see the files located within your current directory inside of the terminal
cd - this command lets you change directories or folders within your terminal 
mkdir - this command lets you make a new directory/folder within your current directory
rmdir - this command lets you remove or delete a directory or folder within your terminal
touch - this command lets you create an empty file within your current directory
sudo - this command lets you execute root privilege commands without being logged in as the root user
head - this command lets you view the first couple of lines of a file
tail - this command lest you view the last couple lines of a file
cat - this command is used to output the content of a specified file to the terminal

1.3:
I am using the 5.15.0-46-generic kernel version (5) according to the output of the uname -r command
The kernel executable is located within the /boot directory and it is named vmlinuz

1.4:
After extracting the tarball (5.15.63), here are the top-level sub-directory names:
arch: the architecture subdirectory houses all of the code that is required to configure and run different architectures 
block: this sub-directory houses all of the code that is necessary to run and be able to direct different connected block devices
certs: the certs directory contains all of the certifications and files that are used for cyber security purposes
crypto: this subdirectory contains all of the drivers used for cryptography processes
Documentation: this folder holds all of the documentation related to the kernel's devices
drivers: contains all of the code used for driver support, these drivers could be used for video, Bluetooth, etc. hardware
fs: holds information regarding the Linux files system, this includes read and write file systems
include: consists of C header files for different kernel-related files
init: hold code that is related to the Linux kernel boot process
ipc: inter-process communication information
kernel: holds code that relates to Linux subsystem processes
lib: this subdirectory includes different library routines that are commonly used, such as debugging operations and common hardware operations
LICENSES: this folder contains the licenses that are related to different source files
mm: memory management sub-directory 
net: the network stack sub-directory houses internet communication protocols
samples: sample code folder for kernel-related config files
scripts: this folder stores code scripts used to build the Linux kernel
security: contain the SELinux framework
sounds: houses all sound system-related source code
tools: this sub-directory holds all the tools that are essential for kernel development
usr: contain initramfs, which is used to establish the root file system during the kernel init process
virt: contain the KVM (kernel virtual machine) project

Sources used: https://tldp.org/LDP/tlk/sources/sources.html and https://linuxhint.com/browse_linux_kernel_source/

1.5: 
Architecture: x86_64
locate the file listing the system calls for your architecture
Name: syscall_64.tbl
Location: arch/x86/entry/syscalls/syscall_64.tbl
System Call Number Names:
0: read
8: lseek
57: fork
90: chmod
327: preadv2

3. 
ascending order of system calls made when running strace ./empsort in.txt out.txt (beginning to end of execution):

execve
brk                            
arch_prctl
mmap
access
openat
newfstatat
close                               
read
pread64
set_tid_address
set_robust_list
rseq
mprotect
prlimit64
munmap
getrandom
write
exit_group                         

4. 
Using man time...
real: the total amount of time that the command took to finish running on your system, can be affected by other processes
user: the total amount of CPU time the program spent while progressing user/user-mode operations, outside of the kernal operations
sys: the total amount of CPU time the program spent while processing the system/system-mode commands, within the kernal operations

Time performance values for my empsort.c code running EASY blackbox test
real    0m0.016s
user    0m0.005s
sys     0m0.001s

Time performance values for my empsort.c code running MEDIUM blackbox test
real    0m0.893s
user    0m0.396s
sys     0m0.060s

Time performance values for my empsort.c code running HARD blackbox test
real    0m7.159s
user    0m2.122s
sys     0m0.494s

Time performance values for my empsort.c code running all of the blackbox tests in series
real    0m8.242s
user    0m2.592s
sys     0m0.457s

