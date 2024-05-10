#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <sys/stat.h>
#include <sys/types.h>
#include <dirent.h>
#include <unistd.h>
#include <pthread.h>

struct Node
{
    int value;
    struct Node *next;
    struct Node *prev;
};

struct LList
{
    int size;
    struct Node *head;
    struct Node *tail;
};

struct TList
{
    struct TNode *head;
    struct TNode *tail;
};

struct TNode
{
    pthread_t tid;
    struct TNode *next;
    struct TNode *prev;
};

// initialize global linked list
struct LList *globalList;
// global k variable
int globalK;
// initialize global mutex lock
pthread_mutex_t lock = PTHREAD_MUTEX_INITIALIZER;

struct LList *createList()
{
    // allocate memory for a new LLink struct
    struct LList *list = malloc(sizeof(struct LList));
    if (list == NULL)
    {
        exit(1); // exit the program bc there isn't enough memory to allocate
    }
    else
    {
        // initialize LList properties to NULL
        list->size = 0;
        list->head = NULL;
        list->tail = NULL;
    }
    return list;
}

struct TList *createTList()
{
    // allocate memory for a new LLink struct
    struct TList *list = malloc(sizeof(struct TList));
    if (list == NULL)
    {
        exit(1); // exit the program bc there isn't enough memory to allocate
    }
    else
    {
        // initialize LList properties to NULL
        list->head = NULL;
        list->tail = NULL;
    }
    return list;
}

void deleteList(struct LList *list)
{
    // start at the head of the list
    struct Node *currentNode = list->head;
    struct Node *tmp = NULL;
    // start iterating through the list node by node, free up any memory that we previously allocated for each node
    while (currentNode != NULL)
    {
        tmp = currentNode;
        currentNode = currentNode->next;
        free(tmp);
    }
    // free up memory for the list
    free(list);
}

void deleteTList(struct TList *list)
{
    // start at the head of the list
    struct TNode *currentNode = list->head;
    struct TNode *tmp = NULL;
    // start iterating through the list node by node, free up any memory that we previously allocated for each node
    while (currentNode != NULL)
    {
        tmp = currentNode;
        currentNode = currentNode->next;
        free(tmp);
    }
    // free up memory for the list
    free(list);
}

struct Node *createNode(int value)
{
    struct Node *node = malloc(sizeof(struct Node));
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

struct TNode *createTNode(pthread_t tid)
{
    struct TNode *node = malloc(sizeof(struct TNode));
    if (node == NULL)
    {
        exit(1); // exit the program bc there isn't enough memory to allocate
    }
    else
    {
        node->tid = tid;
        node->next = NULL;
        node->prev = NULL;
    }
    return node;
}

void insertTNode(struct TList *list, struct TNode *node)
{
    // check to see if the TList is empty
    if (list->head == NULL && list->tail == NULL)
    {
        list->head = node;
        list->tail = node;
    }
    else
    {
        list->tail->next = node;
        node->prev = list->tail;
        list->tail = node;
    }
}

void printLList(struct LList *list)
{
    // start at the head of the list
    struct Node *currentNode = list->head;
    printf("\nprinting local LList:\n");
    // start iterating through the list node by node, free up any memory that we previously allocated for each node
    while (currentNode != NULL)
    {
        printf("%d ", currentNode->value);
        currentNode = currentNode->next;
    }
    printf("\n\n");
}

void popListTail(struct LList *list)
{
    struct Node *tmp = NULL;
    tmp = list->tail;
    list->tail = list->tail->prev;
    list->tail->next = NULL;
    free(tmp);
    list->size = list->size - 1;
}

int insert_sorted(struct LList *list, struct Node *node)
{
    if (list->head != NULL && list->tail != NULL) // non-empty list
    {
        struct Node *curr = list->head;
        while (curr != NULL)
        {
            if (node->value > curr->value) // add value into List
            {
                node->next = curr;
                node->prev = curr->prev;
                // current node has a previous node
                if (curr->prev != NULL)
                    curr->prev->next = node;
                // current node is the head of the list
                else
                    list->head = node;
                curr->prev = node;
                list->size = list->size + 1;
                return 1;
            }
            else if (curr->value == node->value)
                return 0; // Don't add Duplicates
            curr = curr->next;
        }
        // adding node to end of list if num items < k
        node->prev = list->tail;
        list->tail->next = node;
        list->tail = node;
        list->size = list->size + 1;
        return -1;
    }
    else
    {
        list->head = node;
        list->tail = node;
        list->size = list->size + 1;
        return -1;
    }
}

/* thread function */
void *thr_func(void *arg)
{
    FILE *textFile;
    int value;
    char *filePath = (char *)arg;

    // local llist for the thread
    struct LList *list = createList();
    // open the file given by the filePath
    textFile = fopen(filePath, "r");
    free(filePath);
    if (textFile == NULL)
    {
        printf("couldn't open file");
        exit(1); // file didn't open, end the program
    }
    // while loop that loops over the lines in a file
    while (fscanf(textFile, "%d", &value) == 1)
    {
        if (list->size >= globalK) // Check values against those in list
        {
            // if value is greater than the smallest value in the list, look to add the value into the list
            if (value > list->tail->value)
            {
                // create new node using a line from the file
                struct Node *newNode = createNode(value);
                // add node to the list using insert_sorted
                int inserted = insert_sorted(list, newNode);
                if (inserted)
                    popListTail(list);
                else
                    free(newNode);
            }
        }
        else // insert the first unique k numbers into the list that are encountered
        {
            //  create new node using a line from the file
            struct Node *newNode = createNode(value);
            // add node to the list using insert_sorted
            int inserted = insert_sorted(list, newNode);
            if (!inserted)
                free(newNode);
        }
    }
    // close the file
    fclose(textFile);

    // loop through the populated llist and use the mutex lock to access the global llist and decide if a value needs to be included in it or not
    struct Node *currentNode = list->head;
    // start iterating through the local list node by node, check if node needs to be included within the global list
    while (currentNode != NULL)
    {
        pthread_mutex_lock(&lock);
        if (globalList->size >= globalK) // Check values against those in globalList
        {
            // if value is greater than the smallest value in the globalList, look to add the value into the globalList
            if (currentNode->value > globalList->tail->value)
            {
                // create new node using a line from the file
                struct Node *newNode = createNode(currentNode->value);
                // add node to the globalList using insert_sorted
                int inserted = insert_sorted(globalList, newNode);
                if (inserted)
                    popListTail(globalList);
                else
                    free(newNode);
            }
        }
        else // insert the first unique k numbers into the globalList that are encountered
        {
            //  create new node using a line from the file
            struct Node *newNode = createNode(currentNode->value);
            // add node to the globalList using insert_sorted
            int inserted = insert_sorted(globalList, newNode);
            if (!inserted)
                free(newNode);
        }
        pthread_mutex_unlock(&lock);
        currentNode = currentNode->next;
    }

    deleteList(list);

    return NULL;
}

void loadItemsToList(int k, char *directoryName)
{
    // basePath string that holds the directory name in memory
    char basePath[120];
    strcpy(basePath, directoryName);
    // use opendir to create a directoty stream for the specified directory
    DIR *directoryPointer = opendir(basePath);
    struct dirent *dirent;
    struct stat fileStats;

    // set global k variable
    globalK = k;

    if (directoryPointer == NULL)
    {
        printf("directory couldn't open");
        exit(1); // directory didn't open, end the program
    }

    // initialize a TList for tid's
    struct TList *tidList = createTList();

    // while loop over all of the files within the directory stream via the readdir function
    while ((dirent = readdir(directoryPointer)) != NULL)
    {
        // create new filePath string, initialized with the value within basePath, later concatenated with the file name
        char filePath[120];
        strcpy(filePath, basePath);
        strcat(filePath, "/");
        strcat(filePath, dirent->d_name);
        // use stat to fill the stat struct based on the file located at the given file path (file properties)
        if (stat(filePath, &fileStats) == 0)
        {
            // skip over any files starting with ".", ending with a "~", or are sub-directories using the posix wrapper S_ISREG
            if (dirent->d_name[0] != '.' && dirent->d_name[strlen(dirent->d_name) - 1] != '~' && S_ISREG(fileStats.st_mode))
            {
                // initalize the filePath string for the thread function
                char *filePathForThread = strdup(filePath);
                // initalize the pthread object
                pthread_t tid;

                int rc;
                // create and process the thread
                if ((rc = pthread_create(&tid, NULL, thr_func, filePathForThread)))
                {
                    printf("error during pthread_create");
                    exit(1);
                }

                // add the tid to a linked list
                struct TNode *tnode = createTNode(tid);
                insertTNode(tidList, tnode);
            }
        }
    }

    // add a while loop using a llist of their pid's to join the threads
    struct TNode *currentNode = tidList->head;
    while (currentNode != NULL)
    {
        pthread_join(currentNode->tid, NULL);
        currentNode = currentNode->next;
    }

    // delete tidList
    deleteTList(tidList);

    // close directory
    closedir(directoryPointer);
}

void writeListIntoFile(struct LList *list, char *fileName)
{
    FILE *ptr = fopen(fileName, "w");
    if (ptr == NULL)
    {
        exit(1); // file didn't open, end the program
    }
    struct Node *node = list->head;
    while (node != NULL)
    {
        fprintf(ptr, "%d\n", node->value);
        node = node->next;
    }

    fclose(ptr);
}

int main(int argc, char *argv[])
{
    // initialize the global values list
    globalList = createList();
    // load items from the input file into the globally linked list struct
    loadItemsToList(atoi(argv[1]), argv[2]);
    // write sorted global list items to a text file
    writeListIntoFile(globalList, argv[3]);
    // delete the global list and free memory
    deleteList(globalList);
    return 0;
}