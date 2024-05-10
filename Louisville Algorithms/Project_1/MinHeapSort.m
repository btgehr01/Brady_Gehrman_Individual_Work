function A=MinHeapSort(A)
N=length(A);
A=BuildMinHeap(A,N);
for i = N:-1:2
    t=A(1);
    A(1) = A(i);
    A(i) = t;
    N=N-1;
    A=MinHeapify(A,1,N);
end