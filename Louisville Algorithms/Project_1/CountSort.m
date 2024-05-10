function y = CountSort(x)
%CountSort performs linear time sorting algorithm on array x of positive
%integers greater than or equal to 0
k = max(x) +1;
n = length(x);
for i = 1:k
    c(i)=0;
end
for i = 1:n
    ii = x(i)+1;
    c(ii)=c(ii)+1;
end
for i = 2:k
    c(i)=c(i)+c(i-1);
end
for i = 1:n
    ii = x(i)+1;
    y(c(ii))=ii-1;
    c(ii)=c(ii)-1;
end
end

