function l = LCS(x,y)
m = length(x);
n = length(y);
l = zeros(m+1,n+1);
for i = 2:m+1
    i1 = i - 1; 
    for j= 2:n+1
        j1 = j - 1;
        if x(i1) == y(j1)
            l(i,j) = l(i-1,j-1) + 1;
        else
            l(i,j) = max(l(i,j-1),l(i-1,j));
        end
    end
end

