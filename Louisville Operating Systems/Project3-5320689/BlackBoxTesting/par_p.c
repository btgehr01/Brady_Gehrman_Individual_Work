#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <sys/stat.h>
#include <sys/types.h>
#include <dirent.h>
#include <unistd.h>
#include <errno.h>
#include <sys/wait.h>
#include <sched.h>

typedef struct node
{
    int value;
    struct node *next;
    struct node *prev;
} Node;

typedef struct list
{
    int size;
    int max;
    Node *head;
    Node *tail;
} List;

List *createList(int);
Node *createNode(int);
void addNode(List *, int);
void deleteList(List *);

int main(int argc, char const *argv[])
{
    const int K = atoi(argv[1]);
    char basePath[120];
    char filePath[120];
    strcpy(basePath, argv[2]);

    DIR *directory = opendir(basePath);
    struct dirent *entry;
    struct stat fileStats;

    if (directory == NULL)
    {
        fprintf(stderr, "Directory couldn't open %s\n", strerror(errno));
        exit(1); // directory didn't open, end the program
    }

    // get number of files to read
    int numFiles = 0;
    while ((entry = readdir(directory)) != NULL)
    {
        if (entry->d_name[0] != '.')
            numFiles++;
    }
    rewinddir(directory);

    int pipes[numFiles][2];
    pid_t pids[numFiles];
    int index = 0;

    // initialize all pipes
    for (int i = 0; i < numFiles; i++)
    {
        if (pipe(pipes[i]) == -1)
        {
            fprintf(stderr, "Error Creating Pipe %s\n", strerror(errno));
            return EXIT_FAILURE;
        }
    }

    // Read trough directory entries
    while ((entry = readdir(directory)) != NULL)
    {
        strcpy(filePath, basePath);
        strcat(filePath, "/");
        strcat(filePath, entry->d_name);

        if (stat(filePath, &fileStats) == 0)
        {
            if (entry->d_name[0] != '.')
            {
                pids[index] = fork(); // Create Child

                // Child Process
                if (pids[index] == 0)
                {
                    // Close unused pipes
                    for (int j = 0; j < numFiles; j++)
                    {
                        if (j != index)
                        {
                            close(pipes[j][0]);
                            close(pipes[j][1]);
                        }
                        else
                        {
                            close(pipes[j][0]);
                        }
                    }

                    // File processing
                    FILE *inFile = fopen(filePath, "r");
                    int value;
                    if (inFile == NULL)
                    {
                        fprintf(stderr, "%s: Failed to open; %s\n", filePath, strerror(errno));
                        exit(1); // file didn't open, end the program
                    }
                    List *childList = createList(K);
                    while (fscanf(inFile, "%d", &value) == 1)
                        addNode(childList, value);

                    fclose(inFile);
                    closedir(directory);

                    // Write to parent
                    for (Node *out = childList->head; out != NULL; out = out->next)
                    {
                        if (write(pipes[index][1], &(out->value), sizeof(int)) == -1)
                        {
                            fprintf(stderr, "Child %d: Error Writing to Parent; %s\n", index, strerror(errno));
                            exit(1);
                        }
                    }
                    
                    // Terminate Child Process
                    close(pipes[index][1]);
                    deleteList(childList);
                    return 0;
                }

                index++; // Go to next File
            }
        }
    }

    // Parent process
    List *parentList = createList(K);
    for (int y = 0; y < numFiles; y++)
    {
        close(pipes[y][1]);
        for (int z = 0; z < K; z++)
        {
            int val;
            if (read(pipes[y][0], &val, sizeof(int)) == -1)
            {
                fprintf(stderr, "Parent: Error Reading From %d; %s\n", y, strerror(errno));
                exit(1);
            }
            addNode(parentList, val);
        }
        close(pipes[y][0]);
        waitpid(pids[y], NULL, 0); //Wait for child to terminate
    }

    // Write Result to File
    FILE *outFile = fopen(argv[3], "w");
    if (outFile == NULL)
    {
        fprintf(stderr, "%s: Failed to open; %s\n", argv[3], strerror(errno));
        exit(1); // file didn't open, end the program
    }
    for (Node *out = parentList->head; out != NULL; out = out->next)
        fprintf(outFile, "%d\n", out->value);

    // Terminate Parent Process
    fclose(outFile);
    closedir(directory);
    deleteList(parentList);
    return 0;
}

List *createList(int max)
{
    // allocate memory for a new LLink struct
    List *list = malloc(sizeof(List));
    if (list == NULL)
    {
        fprintf(stderr, "Error Creating List %s\n", strerror(errno));
        exit(1); // exit the program bc there isn't enough memory to allocate
    }
    else
    {
        // initialize LList properties to NULL
        list->size = 0;
        list->max = max;
        list->head = NULL;
        list->tail = NULL;
    }
    return list;
}

void deleteList(List *list)
{
    // start at the head of the list
    Node *currentNode = list->head;
    // start iterating through the list node by node, free up any memory that we previously allocated for each node
    while (currentNode != NULL)
    {
        Node *tmp = currentNode;
        currentNode = currentNode->next;
        free(tmp);
    }
    // free up memory for the list
    free(list);
}

Node *createNode(int value)
{
    Node *node = malloc(sizeof(Node));
    if (node == NULL)
    {
        exit(1); // exit the program bc there isn't enough memory to allocate
    }
    else
    {
        node->value = value;
        node->next = NULL;
        node->prev = NULL;
    }
    return node;
}

void addNode(List *list, int value)
{   
    // Empty List
    if (list->size == 0)
    {
        Node *node = createNode(value);
        list->head = node;
        list->tail = node;
        list->size += 1;
    }
    // New value less than tail
    else if (value < list->tail->value)
    {
        if (list->size == list->max)
            return; // Cant add val to list
        
        // Add to tail    
        Node *node = createNode(value);
        node->prev = list->tail;
        list->tail->next = node;
        list->tail = node;
        list->size += 1;
    }
    else 
    {
        for (Node *curr = list->head; curr != NULL; curr = curr->next)
        {
            if (value == curr->value)
                return; // Duplicates Don't get added

            // Add before current Node
            if (value > curr->value)
            {
                Node *node = createNode(value);
                node->next = curr;
                node->prev = curr->prev;
                // current node has a previous node
                if (curr->prev != NULL)
                    curr->prev->next = node;
                // current node is the head of the list
                else
                    list->head = node;
                curr->prev = node;
                list->size += 1;

                // List greater than max size
                if (list->size > list->max)
                {
                    Node *tmp = list->tail;
                    list->tail = tmp->prev;
                    list->tail->next = NULL;
                    list->size -= 1;
                    free(tmp);
                }
                return;
            }
        }
    }
}