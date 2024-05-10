function A=HeapSort(A)
N=length(A);
A=BuildMaxHeap(A,N);
for i = N:-1:2
    t=A(1);
    A(1) = A(i);
    A(i) = t;
    N=N-1;
    A=MaxHeapify(A,1,N);
end