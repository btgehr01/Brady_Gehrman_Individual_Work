#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <sys/stat.h>
#include <sys/types.h>
#include <dirent.h>

typedef struct node
{
    int value;
    struct node *next;
    struct node *prev;
} Node;

typedef struct list
{
    int size;
    Node *head;
    Node *tail;
} List;

List *createList()
{
    // allocate memory for a new LLink struct
    List *list = malloc(sizeof(List));
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

void deleteList(List *list)
{
    // start at the head of the list
    Node *currentNode = list->head;
    Node *tmp = NULL;
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

void popListTail(List *list)
{
    Node *tmp = NULL;
    tmp = list->tail;
    list->tail = list->tail->prev;
    list->tail->next = NULL;
    free(tmp);
    list->size = list->size - 1;
}

int insert_sorted(List *list, Node *node)
{
    if (list->head != NULL && list->tail != NULL) // non-empty list
    {
        Node *curr = list->head;
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

void loadItemsToList(List *list, int k, char *directoryName)
{
    int value;
    // basePath string that holds the directory name in memory
    char basePath[120];
    strcpy(basePath, directoryName);
    // use opendir to create a directoty stream for the specified directory
    DIR *directoryPointer = opendir(basePath);
    struct dirent *dirent;
    FILE *textFile;
    struct stat fileStats;
    if (directoryPointer == NULL)
    {
        printf("directory couldn't open");
        exit(1); // directory didn't open, end the program
    }
    // while loop over all of the files within the directory stream via the readdir function
    while ((dirent = readdir(directoryPointer)) != NULL)
    {
        // create new filePath string, initialized with the value within basePath, later concatenated with the file name
        char filePath[120];
        strcpy(filePath, basePath);
        strcat(filePath, "/");
        strcat(filePath, dirent->d_name);
        // skip over any files starting with ".", ending with a "~", or are sub-directories (TODO)
        if (stat(filePath, &fileStats) == 0)
        {
            if (dirent->d_name[0] != '.' && dirent->d_name[strlen(dirent->d_name) - 1] != '~' && S_ISREG(fileStats.st_mode))
            {
                // open the file given by the filePath
                textFile = fopen(filePath, "r");
                if (textFile == NULL)
                {
                    printf("couldn't open file");
                    exit(1); // file didn't open, end the program
                }
                // while loop that loops over the lines in a file
                while (fscanf(textFile, "%d", &value) == 1)
                {
                    if (list->size >= k) // Check values against those in list
                    {
                        // if value is greater than the smallest value in the list, look to add the value into the list
                        if (value > list->tail->value)
                        {
                            // create new node using a line from the file
                            Node *newNode = createNode(value);
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
                        Node *newNode = createNode(value);
                        // add node to the list using insert_sorted
                        int inserted = insert_sorted(list, newNode);
                        if (!inserted)
                            free(newNode);
                    }
                    // list is full, check to see if the value from the file should replace a value in the list
                }
                // close the file
                fclose(textFile);
            }
        }
    }
    // close directory
    closedir(directoryPointer);
}

void writeListIntoFile(List *list, char *fileName)
{
    FILE *ptr = fopen(fileName, "w");
    if (ptr == NULL)
    {
        exit(1); // file didn't open, end the program
    }
    Node *node = list->head;
    while (node != NULL)
    {
        fprintf(ptr, "%d\n", node->value);
        node = node->next;
    }

    fclose(ptr);
}

int main(int argc, char *argv[])
{
    // initialize linked list
    List *list = createList();
    // load items from the input file into the linked list struct
    loadItemsToList(list, atoi(argv[1]), argv[2]);
    // write sorted list items to a text file
    writeListIntoFile(list, argv[3]);
    // delete the list and free memory
    deleteList(list);
    return 0;
}