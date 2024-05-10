a. Brady Gehrman 5320689 Project 2
b.

FCFS:
Average Waiting Time: 37,316.890625 ms
Average Turn Around Time: 37,327.402344 ms

RR with quantum = 4:
Average Waiting Time: 49,463.183594 ms
Average Turn Around Time: 49,473.671875 ms

RR with quantum = 1:
Average Waiting Time: 51,203.605469 ms
Average Turn Around Time: 51,214.031250 ms

The FCFS algorithm is an example of a non-preemptive algorithm, while RR is an example of a preemptive algorithm.
I would expect the RR algorithm to have a lower average waiting time than the FCFS algorithm, 
and I would expect RR to have a higher average turnaround time than that of the FCFS algorithm. 
As we can see from the values I collected the FCFS algorithm did have a lower turnaround time than either of the RR algorithm outputs, this was expected.
However, the FCFS algorithm showed that it has a lower average waiting time than either of the other RR algorithms, this is unexpected but I'm sure can be justified.
If we take a look at the input file that contains all of the processes, we can notice that each process has a burst duration of 20ms or less,
this means that we won't experience as much convoy effect when it comes to running the FCFS algorithm because there aren't huge differences in burst durations between processes.
This could conclude why we don't experience as much delay or waiting time between processes for FCFS compared to the RR algorithms. 
I would also like to add that there are also 10,000 processes within the input file, and if these processes don't finish within the first or even second (small) quantum bursts,
then the waiting time will greatly increase because of the circular nature of the RR algorithm, this happens for multiple processes during RR simulation, which could explain the higher average waiting time.

