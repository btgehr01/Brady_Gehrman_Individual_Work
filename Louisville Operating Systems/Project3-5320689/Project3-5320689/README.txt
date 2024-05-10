CSE 420 Project 3
Group:
Brady Gehrman 5320689
Christopher Moore 5344781

Performance Data

@Test 1: files 1-3
Trial 1:                      
* seq:     24 ms
* par_t:   22 ms
* par_p:   18 ms

speed-up for par_t: 1.09
speed-up for par_p: 1.33

Trial 2:
* seq:    22 ms
* par_t:  20 ms
* par_p:  21 ms

speed-up for par_t: 1.10
speed-up for par_p: 1.05

Trial 3:
* seq:     34 ms
* par_t:   25 ms
* par_p:   26 ms

speed-up for par_t: 1.36
speed-up for par_p: 1.31

average speed-up for par_t: 1.18
average speed-up for par_p: 1.23


@Test 2: files 2-10
Trial 1:                      
* seq:     15026 ms
* par_t:   10961 ms
* par_p:   10856 ms

speed-up for par_t: 1.37
speed-up for par_p: 1.38

Trial 2:
* seq:     18458 ms
* par_t:   13522 ms
* par_p:   13450 ms

speed-up for par_t: 1.37
speed-up for par_p: 1.37

Trial 3:
* seq:     18530 ms
* par_t:   13845 ms
* par_p:   14197 ms

speed-up for par_t: 1.34
speed-up for par_p: 1.31

average speed-up for par_t: 1.36
average speed-up for par_p: 1.35


@Test 3: files 4-100
Trial 1:                      
* seq:     34523 ms
* par_t:   19819 ms
* par_p:   19939 ms

speed-up for par_t: 1.74
speed-up for par_p: 1.73

Trial 2:
* seq:     37200 ms
* par_t:   23415 ms
* par_p:   24523 ms

speed-up for par_t: 1.59
speed-up for par_p: 1.52

Trial 3:
* seq:     39760 ms
* par_t:   26464 ms
* par_p:   25962 ms

speed-up for par_t: 1.50
speed-up for par_p: 1.53

average speed-up for par_t: 1.61
average speed-up for par_p: 1.59


@Test 4: files 8-1000
Trial 1:                      
* seq:     68064 ms
* par_t:   39584 ms
* par_p:   39954 ms

speed-up for par_t: 1.72
speed-up for par_p: 1.70

Trial 2:
* seq:     73281 ms
* par_t:   49450 ms
* par_p:   52968 ms

speed-up for par_t: 1.48
speed-up for par_p: 1.38

Trial 3:
* seq:     80583 ms
* par_t:   53798 ms
* par_p:   54537 ms

speed-up for par_t: 1.50
speed-up for par_p: 1.49

average speed-up for par_t: 1.57
average speed-up for par_p: 1.52

Hardware Specs: 4 CPU cores and 8 logical processors (2 threads per core)


1. Performance Analysis Paragraph:

Individually, the average speed-up values for each of the four tests make sense because each of those values is greater than 1, indicating that our parallel-based solution was in fact faster than
the sequential versions. On a more global level, the average speed-up values increased as the number of files visited increased, this makes sense because as the number of files needed to be read through increases,
the more files that can be read through using parallelism via threads or multiprocessing increases, causing the program to better utilize the processors within the system and therefore increase the ratio of 
sequential execution time versus parallel execution time. 

My system has 4 CPU cores and 8 logical processors. This means I have the ability to use threading and multi-processing in full effect within my system. If you look closely, you can notice that my speed-up times for test 1
are relatively small compared to the ones gathered for test 3 or test 4. This is because my system wasn't being used to its full capacity for the 3 files that it had to read through for that test For the 3 files in test 1,
3 threads were made for par_t, and 3 processes were made for par_p, and because my system has access to 8 core processors only using three of them at a time wouldn't provide as much speed-up as a program that utilized tens or
hundreds of threads or processes at a time would.

2. Did you take any measures against the lock contention and IPC communication costs mentioned above?

There was noticeable lock contention for the parallelism program that used threads. To alleviate this issue we decided to follow the suggestion mentioned within the project,
where each thread has its own locally linked list that is updated with the values contained within its own individually assigned file, these locally linked lists are
then used to fill the global linked list with the help of mutex locks or pipes to ensure stable data synchronization. 
The same measure was taken with the processes for parallelism programming using multi-processing.


3. Did you fully implement the project as described? If not, what parts are not implemented at all
or not implemented as following the specified description?

par_t: implemented as described.

par_p:
When implementing communication between the child and parent processes, I decided to use pipes instead of message queues

The pipe(int fd[2]) function takes in one argument: an integer array of size 2.
The function will open a one-directional data channel with two file descriptors:
    fd[0] ==> read end of Pipe
    fd[1] ==> write end of Pipe

Before forking, an array of file descriptors is created that will be used by both child and parent processes.
    the parent process will iterate through the list and read from each pipe
    each child process will write to a specified pipe according to the order in which they were created

4. Structure of your messages. Which size did you use for your messages and why?

par_p
pipes use the read() and write() function for communication
the child processes each have their own list for their given files;
Instead of sending nodes through the pipes, the value of the node is passed through.

the second argument of both functions is an address to a section of memory
the third argument of both functions is an integer for the size of the memory section for the second arg

the address to value is passed to the functions as well as sizeof(int)


