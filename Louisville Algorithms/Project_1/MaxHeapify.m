function A=MaxHeapify(A, i, N)
l = 2 * i;
r = l + 1;
if (l <= N) && (A(l) > A(i))
    k = l;
else
    k = i;
end
if (r <= N) && (A(r) > A(k))
    k = r;
end
if k ~= i
    t = A(i);
    A(i) = A(k);
    A(k) = t;
    A=MaxHeapify(A,k,N);
end
end



