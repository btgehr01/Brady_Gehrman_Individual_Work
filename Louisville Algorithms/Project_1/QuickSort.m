function x = QuickSort(x)
n=length(x);
if n < 2 
    return;
end
x1=[];
x2=[];
%pivot is the first element x(1)
for i = 2 : n
    if x(i) < x(1)
        x1(end + 1)=x(i);
    else
        x2(end + 1)=x(i);
    end
end
x=[QuickSort(x1) x(1) QuickSort(x2)];
