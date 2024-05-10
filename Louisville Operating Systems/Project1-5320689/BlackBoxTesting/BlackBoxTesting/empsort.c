#include <stdio.h>
#include <stdlib.h>
#include <string.h>

struct Node
{
    char *firstName;
    char *lastName;
    char *employeeId;
    int age;
    int luckyNumber;
    struct Node *next;
    struct Node *prev;
};

struct LList
{
    struct Node *head;
    struct Node *tail;
};

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
        free(currentNode->firstName);
        free(currentNode->lastName);
        free(currentNode->employeeId);
        tmp = currentNode;
        currentNode = currentNode->next;
        free(tmp);
    }
    // free up memory for the list
    free(list);
}

struct Node *createNode(char *firstName, char *lastName, char *employeeId, int age, int luckyNumber)
{
    struct Node *node = malloc(sizeof(struct Node));
    if (node == NULL)
    {
        exit(1); // exit the program bc there isn't enough memory to allocate
    }
    else
    {
        node->firstName = strdup(firstName);
        node->lastName = strdup(lastName);
        node->employeeId = strdup(employeeId);
        node->age = age;
        node->luckyNumber = luckyNumber;
        node->next = NULL;
        node->prev = NULL;
    }
    return node;
}

int shouldPassUpNode(struct Node *nodeToAdd, struct Node *currentNode)
{
    if (nodeToAdd->age < currentNode->age)
    {
        return 1;
    }
    else if (nodeToAdd->age > currentNode->age)
    {
        return 0;
    }
    else
    {
        if (nodeToAdd->luckyNumber > currentNode->luckyNumber)
        {
            return 1;
        }
        else if (nodeToAdd->luckyNumber < currentNode->luckyNumber)
        {
            return 0;
        }
        else
        {
            if (strcmp(nodeToAdd->employeeId, currentNode->employeeId) < 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}

void insert_sorted(struct LList *list, struct Node *node)
{
    // if list is empty, just add node by updating the list's head and tail properties
    if (list->head == NULL && list->tail == NULL)
    {
        list->head = node;
        list->tail = node;
    }
    // else compare with the nodes in the list to find the right spot for the node
    else
    {
        struct Node *nodeInList = list->head;
        int insertedNode = 0;
        while (!insertedNode)
        {

            // compare the node we are looking to insert with the current nodeInList
            int shouldContinue = shouldPassUpNode(node, nodeInList);

            // node should be add somewhere after the current nodeInList
            if (shouldContinue)
            {
                // increment nodeInList
                nodeInList = nodeInList->next;
                // check if the node is being added after the last node in the list, making it the new list->tail
                if (nodeInList == NULL)
                {
                    // add node as the new end of the list, and signal we inserted the node
                    list->tail->next = node;
                    node->prev = list->tail;
                    list->tail = node;
                    insertedNode = 1;
                }
            }
            // node should be added before the current nodeInList
            else
            {
                // check if the node is being added before the first node in the list, making it the new list->head
                if (nodeInList == list->head)
                {
                    list->head->prev = node;
                    node->next = list->head;
                    list->head = node;
                }
                else
                {
                    // add node into the list before the currentNode and after the nodeInList -> prev
                    // update node before currentNode
                    nodeInList->prev->next = node;
                    node->prev = nodeInList->prev;
                    // update currentNode
                    nodeInList->prev = node;
                    node->next = nodeInList;
                }
                // signal we inserted the node
                insertedNode = 1;
            }
        }
    }
}

void loadItemsToList(struct LList *list, char *fileName)
{
    char *firstName;
    char *lastName;
    char employeeId[9];
    int age;
    int luckyNumber;
    FILE *textFile = fopen(fileName, "r");
    if (textFile == NULL)
    {
        exit(1); // file didn't open, end the program
    }
    // while loop that loops over the lines in a file
    while (fscanf(textFile, "%9s %ms %ms %d %d", employeeId, &firstName, &lastName, &age, &luckyNumber) == 5)
    {
        // create new node using a line from the file
        struct Node *newNode = createNode(firstName, lastName, employeeId, age, luckyNumber);
        // add node to the list using insert_sorted
        insert_sorted(list, newNode);
        // free the buffer that scanf allocated for the %ms variables
        free(firstName);
        free(lastName);
    }
    // close the file
    fclose(textFile);
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
        fprintf(ptr, "%d,%d,%s,%s,%s\n", node->age, node->luckyNumber, node->employeeId, node->firstName, node->lastName);
        node = node->next;
    }

    fclose(ptr);
}

int main(int argc, char *argv[])
{
    // initialize linked list
    struct LList *list = createList();
    // load items from the input file into the linked list struct
    loadItemsToList(list, argv[1]);
    // write sorted list items to a text file
    writeListIntoFile(list, argv[2]);
    // delete the list and free memory
    deleteList(list);
    return 0;
}