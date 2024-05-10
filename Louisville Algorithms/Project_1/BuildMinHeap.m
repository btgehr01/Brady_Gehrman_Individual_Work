function A=BuildMinHeap(A,N)
for i = floor(N/2):-1:1
    A=MinHeapify(A,i,N);
end