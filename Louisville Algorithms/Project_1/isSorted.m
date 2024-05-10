function s=isSorted(x)
n=length(x);
s=logical(1);
for i=1:n-1
    if x(i) > x(i+1)
        s=logical(0);
    end;
end;