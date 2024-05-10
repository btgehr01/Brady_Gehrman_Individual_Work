function x = InsertionSort(x)
n = length(x);
for j = 2 : n
    temp=x(j);
    i = j - 1;
    while ((i > 0) && (x(i) > temp))
        x(i+1) = x(i);
        i = i - 1;
    end
    x(i + 1) = temp;
end
end

