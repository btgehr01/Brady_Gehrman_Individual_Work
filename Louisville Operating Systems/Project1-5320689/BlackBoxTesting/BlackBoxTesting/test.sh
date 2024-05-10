#!/bin/sh

rm *.txt
make clean
make

./empsort files/in_easy.txt student_out_easy.txt

if diff -w student_out_easy.txt files/correct_out_easy.txt; then
    echo Success---------------------EASY--------------------------Success
else
    echo Fail------------------------EASY--------------------------Fail
fi

./empsort files/in_medium.txt student_out_medium.txt

if diff -w student_out_medium.txt files/correct_out_medium.txt; then
    echo Success---------------------MEDIUM------------------------Success
else
    echo Fail------------------------MEDIUM------------------------Fail
fi

./empsort files/in_hard.txt student_out_hard.txt

if diff -w student_out_hard.txt files/correct_out_hard.txt; then
    echo Success---------------------HARD--------------------------Success
else
    echo Fail------------------------HARD--------------------------Fail
fi
