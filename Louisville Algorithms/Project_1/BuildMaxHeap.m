function A=BuildMaxHeap(A,N)
for i = floor(N/2):-1:1
    A=MaxHeapify(A,i,N);
end