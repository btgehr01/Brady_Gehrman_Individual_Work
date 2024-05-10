#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <sys/stat.h>
#include <sys/types.h>
#include <dirent.h>
#include <fcntl.h>
#include <unistd.h>

#define blockSize 4096

int main(int argc, char *argv[])
{
    if (atoi(argv[1]) == 1)
    {
        const char *fileName = argv[2];                              // name of the file to be created
        long fileSize_bytes = (long)atoi(argv[3]) * blockSize;       // calculate the file size in bytes
        int fd = open(fileName, O_CREAT | O_WRONLY | O_TRUNC, 0664); // create a file using the open system call, truncate the file if it exists, set the file permissions to 664
        if (fd == -1)
        {
            printf("error opening file");
            exit(1); // exit the program bc error opening file
        }

        const int maxBytesPerWriteCall = 2000000000; // define a constant variable that holds the maximum number of bytes that can be written to file using the write system command

        while (fileSize_bytes > 0)
        {
            // if the number of bytes needed to be written to the file is greater than maxBytesPerWriteCall, write the maxBytesPerWriteCall bytes into the file
            if (fileSize_bytes - maxBytesPerWriteCall >= 0)
            {
                char *zerosArray = malloc(maxBytesPerWriteCall); // allocate heap memory for the zerosArray
                memset(zerosArray, 0, maxBytesPerWriteCall);     // populate zerosArray with all asci zero's
                write(fd, zerosArray, maxBytesPerWriteCall);     // write the zerosArray into the specified file
                free(zerosArray);                                // free zerosArray
            }
            else
            {
                char *finalZerosArray = malloc(fileSize_bytes); // allocate heap memory for the finalZerosArray
                memset(finalZerosArray, 0, fileSize_bytes);     // populate finalZerosArray with all asci zero's
                write(fd, finalZerosArray, fileSize_bytes);     // write the finalZerosArray into the specified file
                free(finalZerosArray);                          // free finalZerosArray
            }
            fileSize_bytes = fileSize_bytes - maxBytesPerWriteCall; // decrement the fileSize_bytes variable based on the number of bytes written to the output file during this cycle
        }
        fsync(fd); // sync memory
        close(fd); // close the file
    }
    else if (atoi(argv[1]) == 2)
    {
    }
    else
    {
        printf("unrecognized first paramter, exiting...");
    }

    return 0;
}