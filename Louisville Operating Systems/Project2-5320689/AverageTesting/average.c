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
    while (fscanf(textFile, "%d %d %d %d", &pid, &arrivalTime, &finishTime, &waitingTime) == 4)
    {
        totalTurnaroundTime = totalTurnaroundTime + (finishTime - arrivalTime);
        numOfRead++;
    }
    // close the file
    fclose(textFile);
    // do average calculation
    float average = totalTurnaroundTime / numOfRead;
    return average;
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
    while (fscanf(textFile, "%d %d %d %d", &pid, &arrivalTime, &finishTime, &waitingTime) == 4)
    {
        totalWaitingTime = totalWaitingTime + waitingTime;
        numOfRead++;
    }
    // close the file
    fclose(textFile);
    // do average calculation
    float average = totalWaitingTime / numOfRead;
    return average;
}

int main(int argc, char *argv[])
{
    printf("Results for %s\n", argv[1]);
    float averageWaitingTime = findAverageWaitingTime(argv[1]);
    float averageTurnAroundTime = findAverageTurnAroundTime(argv[1]);
    printf("averageWaitingTime: %f averageTurnAroundTime: %f\n", averageWaitingTime, averageTurnAroundTime);
}