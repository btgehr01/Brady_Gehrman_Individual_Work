#!/bin/sh
# This is a comment

rm *.txt
make clean
make

./sched results/in.txt sout_fcfs.txt FCFS
./sched results/in.txt sout_fcfs_100.txt FCFS 100
./sched results/in.txt sout_rr4.txt RR 4
./sched results/in.txt sout_rr2_100.txt RR 2 100
./sched results/in.txt sout_rr1.txt RR 1

if diff -w sout_fcfs.txt results/correct_fcfs.txt; then
    echo Test 1 - Success--------------------FCFS--------------------------------Success
else
    echo Test 1 - Fail-----------------------FCFS--------------------------------Fail
fi

if diff -w sout_fcfs_100.txt results/correct_fcfs_100.txt; then
    echo Test 2 - Success--------------------FCFS-100----------------------------Success
else
    echo Test 2 - Fail-----------------------FCFS-100----------------------------Fail
fi

if diff -w sout_rr4.txt results/correct_rr4.txt; then
    echo Test 3 - Success--------------------RR4---------------------------------Success
else
    echo Test 3 - Fail-----------------------RR4---------------------------------Fail
fi

if diff -w sout_rr2_100.txt results/correct_rr2_100.txt; then
    echo Test 4 - Success--------------------RR2-100-----------------------------Success
else
    echo Test 4 - Fail-----------------------RR2-100-----------------------------Fail
fi

if diff -w sout_rr1.txt results/correct_rr1.txt; then
    echo Test 5 - Success--------------------RR1---------------------------------Success
else
    echo Test 5 - Fail-----------------------RR1---------------------------------Fail
fi
