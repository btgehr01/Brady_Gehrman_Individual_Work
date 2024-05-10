#include <stdio.h>
#include <stdlib.h>
#include <string.h>

// ready queue that holds each of the processes from the file (amount of lines can be specified via cla or not specified to load the whole file)
// when something finishes write it to the file
struct ReadyQueue
{
    int currentTime;
    int numOfProcesses;
    struct Process *head;
    struct Process *tail;
};

struct List
{
    int numOfProcesses;
    struct Process *head;
    struct Process *tail;
};

struct Process
{
    int pid;
    int arrivalTime;
    int burstTime;
    int finishTime;
    int waitingTime;
    int lastTimeExecuted;
    struct Process *next;
    struct Process *prev;
};

struct Process *createProcess(int pid, int arrivalTime, int burstTime)
{
    struct Process *process = malloc(sizeof(struct Process));
    if (process == NULL)
    {
        exit(1); // exit the program bc there isn't enough memory to allocate
    }
    else
    {
        process->pid = pid;
        process->arrivalTime = arrivalTime;
        process->burstTime = burstTime;
        process->finishTime = 0;
        process->waitingTime = 0;
        process->lastTimeExecuted = arrivalTime;
        process->prev = NULL;
        process->next = NULL;
    }
    return process;
}

struct Process *recreateProcess(int pid, int arrivalTime, int burstTime, int waitingTime, int lastTimeExecuted)
{
    struct Process *process = malloc(sizeof(struct Process));
    if (process == NULL)
    {
        exit(1); // exit the program bc there isn't enough memory to allocate
    }
    else
    {
        process->pid = pid;
        process->arrivalTime = arrivalTime;
        process->burstTime = burstTime;
        process->finishTime = 0;
        process->waitingTime = waitingTime;
        process->lastTimeExecuted = lastTimeExecuted;
        process->prev = NULL;
        process->next = NULL;
    }
    return process;
}

struct ReadyQueue *createQueue()
{
    // allocate memory for a new LLink struct
    struct ReadyQueue *queue = malloc(sizeof(struct ReadyQueue));
    if (queue == NULL)
    {
        exit(1); // exit the program bc there isn't enough memory to allocate
    }
    else
    {
        // initialize ReadyQueue properties to NULL or 0
        queue->currentTime = 0;
        queue->numOfProcesses = 0;
        queue->head = NULL;
        queue->tail = NULL;
    }
    return queue;
}

void deleteQueue(struct ReadyQueue *queue)
{
    // start at the head of the queue
    struct Process *currentNode = queue->head;
    struct Process *tmp = NULL;
    // start iterating through the queue process by process, free up any memory that we previously allocated for each process
    while (currentNode != NULL)
    {
        tmp = currentNode;
        currentNode = currentNode->next;
        free(tmp);
    }
    // free up memory for the queue
    free(queue);
}

void enqueue(struct ReadyQueue *queue, struct Process *process)
{
    // check to see if the queue is empty
    if (queue->head == NULL && queue->tail == NULL)
    {
        queue->head = process;
        queue->tail = process;
    }
    else
    {
        queue->tail->next = process;
        process->prev = queue->tail;
        queue->tail = process;
    }
    // increment the number of processes that are in the queue
    queue->numOfProcesses = queue->numOfProcesses + 1;
}

void dequeue(struct ReadyQueue *queue)
{
    struct Process *tmp = NULL;
    if (queue->head == NULL && queue->tail == NULL)
    {
        printf("queue is empty, cannot dequeue");
    }
    else
    {
        tmp = queue->head;
        queue->head = queue->head->next;
        if (queue->head != NULL)
        {
            queue->head->prev = NULL;
        }
        else
        {
            queue->tail = NULL;
        }

        free(tmp);
        queue->numOfProcesses = queue->numOfProcesses - 1;
    }
}

void loadItemsToReadyQueue(struct ReadyQueue *queue, char *fileName)
{
    int pid;
    int arrivalTime;
    int burstTime;
    FILE *textFile = fopen(fileName, "r");
    if (textFile == NULL)
    {
        exit(1); // file didn't open, end the program
    }

    // while loop that loops over the lines in a file
    while (fscanf(textFile, "%d %d %d", &pid, &arrivalTime, &burstTime) == 3)
    {
        // create new process using a line from the file
        struct Process *newProcess = createProcess(pid, arrivalTime, burstTime);
        // add process to the queue
        enqueue(queue, newProcess);
    }
    // close the file
    fclose(textFile);
}

void loadLimitedItemsToReadyQueue(struct ReadyQueue *queue, char *fileName, int limit)
{
    int pid;
    int arrivalTime;
    int burstTime;
    int count = 0;
    FILE *textFile = fopen(fileName, "r");
    if (textFile == NULL)
    {
        exit(1); // file didn't open, end the program
    }

    // while loop that loops over the lines in a file
    while (count < limit)
    {
        fscanf(textFile, "%d %d %d", &pid, &arrivalTime, &burstTime);
        // create new process using a line from the file
        struct Process *newProcess = createProcess(pid, arrivalTime, burstTime);
        // add process to the queue
        enqueue(queue, newProcess);
        count++;
    }
    // close the file
    fclose(textFile);
}

void writeProcessToFile(struct Process *process, char *fileName)
{
    FILE *ptr = fopen(fileName, "a");
    if (ptr == NULL)
    {
        exit(1); // file didn't open, end the program
    }
    fprintf(ptr, "%d %d %d %d\n", process->pid, process->arrivalTime, process->finishTime, process->waitingTime);
    // close the file
    fclose(ptr);
}

void performFCFS(struct ReadyQueue *queue, char *fileName)
{
    // loop through the processes in the queue and simulate completion on each of them in order (FCFS)
    struct Process *process = queue->head;
    while (process != NULL)
    {
        process->finishTime = queue->currentTime + process->burstTime;
        process->waitingTime = process->finishTime - process->arrivalTime - process->burstTime;
        queue->currentTime = process->finishTime;
        writeProcessToFile(process, fileName);
        process = process->next;
    }
}

void performRR(struct ReadyQueue *queue, int quantum, char *inputFileName, char *outputFileName)
{
    int pid;
    int arrivalTime;
    int burstTime;
    int numOfProcessesReadFromTheFile = 0;
    struct Process *nextProcessToAddToTheQueue = NULL;
    FILE *textFile = fopen(inputFileName, "r");
    if (textFile == NULL)
    {
        exit(1); // file didn't open, end the program
    }

    // enqueue first item in the file, if it exists
    if (fscanf(textFile, "%d %d %d", &pid, &arrivalTime, &burstTime) == 3)
    {
        // create new process using 1st line from the file
        nextProcessToAddToTheQueue = createProcess(pid, arrivalTime, burstTime);
        numOfProcessesReadFromTheFile++;
        // add first process to the queue
        enqueue(queue, nextProcessToAddToTheQueue);
        // put the second item in the list into the nextProcessToAddToTheQueue object, if it exists

        if (fscanf(textFile, "%d %d %d", &pid, &arrivalTime, &burstTime) == 3)
        {
            // assign the next process in the file to the nextProcessToAddToTheQueue
            nextProcessToAddToTheQueue = createProcess(pid, arrivalTime, burstTime);
            numOfProcessesReadFromTheFile++;
        }
        else
        {
            nextProcessToAddToTheQueue = NULL;
        }
    }

    int numOfFinishedProcesses = 0;
    // start the process with a while loop that finishes when all processes inside of the queue have finished and there isn't a next process still inside of the file
    while (nextProcessToAddToTheQueue != NULL || numOfFinishedProcesses < numOfProcessesReadFromTheFile)
    {
        // if queue is empty fast forward to the next process and add it to the queue
        if (queue->head == NULL && nextProcessToAddToTheQueue != NULL)
        {
            queue->currentTime = nextProcessToAddToTheQueue->arrivalTime;
            enqueue(queue, nextProcessToAddToTheQueue);
            // get the next process in the file
            if (fscanf(textFile, "%d %d %d", &pid, &arrivalTime, &burstTime) == 3)
            {
                // create new process using a line from the file
                nextProcessToAddToTheQueue = createProcess(pid, arrivalTime, burstTime);
                numOfProcessesReadFromTheFile++;
            }
            // line in file is empty, set nextProcessToAddToTheQueue to null
            else
            {
                nextProcessToAddToTheQueue = NULL;
            }
        }

        int startTime = queue->currentTime;
        // simulate burst duration of the current process at the head of the queue
        int stopTime = queue->currentTime + quantum;
        // create a boolean to keep track of the completion status of the process
        int processCompleted = 0;
        while (queue->currentTime < stopTime && !processCompleted)
        {
            // increment the current time variable in the queue
            queue->currentTime = queue->currentTime + 1;
            // decrease the burst time property of the process
            queue->head->burstTime = queue->head->burstTime - 1;
            // check to see if the process has finished, if so write it to the file, and dequeue it
            if (queue->head->burstTime == 0)
            {
                queue->head->finishTime = queue->currentTime;
                queue->head->waitingTime = queue->head->waitingTime + (startTime - queue->head->lastTimeExecuted);
                writeProcessToFile(queue->head, outputFileName);
                dequeue(queue);
                processCompleted = 1;
                numOfFinishedProcesses++;
            }
            // if the process hasn't finished and the current time is the stopTime then dequeue the queue and re-enqueue the process
            else if (queue->currentTime == stopTime)
            {
                queue->head->waitingTime = queue->head->waitingTime + (startTime - queue->head->lastTimeExecuted);
                queue->head->lastTimeExecuted = queue->currentTime;
                struct Process *tmp = recreateProcess(queue->head->pid, queue->head->arrivalTime, queue->head->burstTime, queue->head->waitingTime, queue->head->lastTimeExecuted);
                dequeue(queue);
                enqueue(queue, tmp);
            }

            //  check to see if we can add another process to the queue
            if (nextProcessToAddToTheQueue != NULL)
            {
                if (nextProcessToAddToTheQueue->arrivalTime == queue->currentTime)
                {
                    // enqueue the process
                    enqueue(queue, nextProcessToAddToTheQueue);
                    // get the next process in the file
                    if (fscanf(textFile, "%d %d %d", &pid, &arrivalTime, &burstTime) == 3)
                    {
                        // create new process using a line from the file
                        nextProcessToAddToTheQueue = createProcess(pid, arrivalTime, burstTime);
                        numOfProcessesReadFromTheFile++;
                    }
                    // line in file is empty, set nextProcessToAddToTheQueue to null
                    else
                    {
                        nextProcessToAddToTheQueue = NULL;
                    }
                }
            }
        }
    }
    // close the file
    fclose(textFile);
}

void performLimitedRR(struct ReadyQueue *queue, int quantum, int limit, char *inputFileName, char *outputFileName)
{
    int pid;
    int arrivalTime;
    int burstTime;
    int numOfProcessesReadFromTheFile = 0;
    struct Process *nextProcessToAddToTheQueue = NULL;
    FILE *textFile = fopen(inputFileName, "r");
    if (textFile == NULL)
    {
        exit(1); // file didn't open, end the program
    }

    // enqueue first item in the file, if it exists
    if (numOfProcessesReadFromTheFile < limit && fscanf(textFile, "%d %d %d", &pid, &arrivalTime, &burstTime) == 3)
    {
        // create new process using 1st line from the file
        nextProcessToAddToTheQueue = createProcess(pid, arrivalTime, burstTime);
        numOfProcessesReadFromTheFile++;
        // add first process to the queue
        enqueue(queue, nextProcessToAddToTheQueue);
        // put the second item in the list into the nextProcessToAddToTheQueue object, if it exists

        if (numOfProcessesReadFromTheFile < limit && fscanf(textFile, "%d %d %d", &pid, &arrivalTime, &burstTime) == 3)
        {
            // assign the next process in the file to the nextProcessToAddToTheQueue
            nextProcessToAddToTheQueue = createProcess(pid, arrivalTime, burstTime);
            numOfProcessesReadFromTheFile++;
        }
        else
        {
            nextProcessToAddToTheQueue = NULL;
        }
    }

    int numOfFinishedProcesses = 0;
    // start the process with a while loop that finishes when all processes inside of the queue have finished and there isn't a next process still inside of the file
    while (nextProcessToAddToTheQueue != NULL || numOfFinishedProcesses < numOfProcessesReadFromTheFile)
    {
        // if queue is empty fast forward to the next process and add it to the queue
        if (queue->head == NULL && nextProcessToAddToTheQueue != NULL)
        {
            queue->currentTime = nextProcessToAddToTheQueue->arrivalTime;
            enqueue(queue, nextProcessToAddToTheQueue);
            // get the next process in the file
            if (numOfProcessesReadFromTheFile < limit && fscanf(textFile, "%d %d %d", &pid, &arrivalTime, &burstTime) == 3)
            {
                // create new process using a line from the file
                nextProcessToAddToTheQueue = createProcess(pid, arrivalTime, burstTime);
                numOfProcessesReadFromTheFile++;
            }
            // line in file is empty, set nextProcessToAddToTheQueue to null
            else
            {
                nextProcessToAddToTheQueue = NULL;
            }
        }

        int startTime = queue->currentTime;
        // simulate burst duration of the current process at the head of the queue
        int stopTime = queue->currentTime + quantum;
        // create a boolean to keep track of the completion status of the process
        int processCompleted = 0;
        while (queue->currentTime < stopTime && !processCompleted)
        {
            // increment the current time variable in the queue
            queue->currentTime = queue->currentTime + 1;
            // decrease the burst time property of the process
            queue->head->burstTime = queue->head->burstTime - 1;
            // check to see if the process has finished, if so write it to the file, and dequeue it
            if (queue->head->burstTime == 0)
            {
                queue->head->finishTime = queue->currentTime;
                queue->head->waitingTime = queue->head->waitingTime + (startTime - queue->head->lastTimeExecuted);
                writeProcessToFile(queue->head, outputFileName);
                dequeue(queue);
                processCompleted = 1;
                numOfFinishedProcesses++;
            }
            // if the process hasn't finished and the current time is the stopTime then dequeue the queue and re-enqueue the process
            else if (queue->currentTime == stopTime)
            {
                queue->head->waitingTime = queue->head->waitingTime + (startTime - queue->head->lastTimeExecuted);
                queue->head->lastTimeExecuted = queue->currentTime;
                struct Process *tmp = recreateProcess(queue->head->pid, queue->head->arrivalTime, queue->head->burstTime, queue->head->waitingTime, queue->head->lastTimeExecuted);
                dequeue(queue);
                enqueue(queue, tmp);
            }

            //  check to see if we can add another process to the queue
            if (nextProcessToAddToTheQueue != NULL)
            {
                if (nextProcessToAddToTheQueue->arrivalTime == queue->currentTime)
                {
                    // enqueue the process
                    enqueue(queue, nextProcessToAddToTheQueue);
                    // get the next process in the file
                    if (numOfProcessesReadFromTheFile < limit && fscanf(textFile, "%d %d %d", &pid, &arrivalTime, &burstTime) == 3)
                    {
                        // create new process using a line from the file
                        nextProcessToAddToTheQueue = createProcess(pid, arrivalTime, burstTime);
                        numOfProcessesReadFromTheFile++;
                    }
                    // line in file is empty, set nextProcessToAddToTheQueue to null
                    else
                    {
                        nextProcessToAddToTheQueue = NULL;
                    }
                }
            }
        }
    }
    // close the file
    fclose(textFile);
}

int main(int argc, char *argv[])
{
    if (argc < 4)
    {
        // too few command line arguments {./sched <inputfile> <outputfile> <algorithm> [quantum] [limit]}
        exit(1);
    }

    if (strcmp(argv[3], "FCFS") == 0)
    {
        // initialize ready queue
        struct ReadyQueue *queue = createQueue();

        // check if there was a limit sent through command line args
        if (argc == 5)
        {
            // load specified number of processes from the input file into the queue struct
            loadLimitedItemsToReadyQueue(queue, argv[1], atoi(argv[4]));
        }

        // load the whole input file into the queue
        else
        {
            // load all processes from the input file into the queue struct
            loadItemsToReadyQueue(queue, argv[1]);
        }

        // perform CPU scheduling algorithm
        performFCFS(queue, argv[2]);

        // delete the queue and free memory
        deleteQueue(queue);
    }
    else if (strcmp(argv[3], "RR") == 0)
    {
        if (argc < 5)
        {
            exit(1); // quantum value is required
        }
        else
        {
            // initialize ready queue
            struct ReadyQueue *queue = createQueue();
            // check if there was a limit sent through command line args
            if (argc == 6)
            {
                // limit was specified
                performLimitedRR(queue, atoi(argv[4]), atoi(argv[5]), argv[1], argv[2]);
            }
            // else perform CPU scheduling algorithm without a limit
            else
            {
                performRR(queue, atoi(argv[4]), argv[1], argv[2]);
            }

            // delete the queue and free memory
            deleteQueue(queue);
        }
    }
    else
    {
        exit(1); // specified algorithm isn't FCFS or RR
    }
    return 0;
}