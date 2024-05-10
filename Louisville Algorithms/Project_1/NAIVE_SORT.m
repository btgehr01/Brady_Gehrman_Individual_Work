function y=NAIVE_SORT(x)
n = length(x);
N=FACT(n);
p=perms(1:n);
for i = 1:N
    y=x(p(i,:));
    if isSorted(y)
        break;
    end;
end;
