#include <stdio.h>
#include <stdlib.h>
#include <string.h>

float findAverageTurnAroundTime(char *fileName)
{
    int pid;
    int arrivalTime;
    int finishTime;
    int waitingTime;
    int numOfRead = 0;
    float totalTurnaroundTime = 0.0;
    FILE *textFile = fopen(fileName, "r");
    if (textFile == NULL)
    {
        exit(1); // file didn't open, end the program
    }

    // while loop that loops over the lines in a file
    while (fscanf(textFile, "%d %d %d %d", &pid, &arrivalTime, &finishTime, &waitingTime) == 3)
    {
        numOfRead++;
        totalTurnaroundTime = totalTurnaroundTime + (finishTime - arrivalTime);
    }
    // close the file
    fclose(textFile);
    // do average calculation
    return totalTurnaroundTime / numOfRead;
}

float findAverageWaitingTime(char *fileName)
{
    int pid;
    int arrivalTime;
    int finishTime;
    int waitingTime;
    int numOfRead = 0;
    float totalWaitingTime = 0.0;
    FILE *textFile = fopen(fileName, "r");
    if (textFile == NULL)
    {
        exit(1); // file didn't open, end the program
    }

    // while loop that loops over the lines in a file
    while (fscanf(textFile, "%d %d %d %d", &pid, &arrivalTime, &finishTime, &waitingTime) == 3)
    {
        numOfRead++;
        totalWaitingTime = totalWaitingTime + waitingTime;
    }
    // close the file
    fclose(textFile);
    // do average calculation
    return totalWaitingTime / numOfRead;
}

int main(int argc, char *argv[])
{
    printf("Results for %s\n", argv[1]);
    float averageWaitingTime = findAverageWaitingTime(argv[1]);
    float averageTurnAroundTime = findAverageTurnAroundTime(argv[1]);
    printf("averageWaitingTime: %f averageTurnAroundTime: %f", averageWaitingTime, averageTurnAroundTime);
}