function s = Naive_Matcher(T, P)
n = length(T);
m = length(P);
k = 1;
for i = 1 : n - m + 1
    if P(1:m) == T(i:i+m-1)
        s(k) = i-1;
        k = k +1;
    end
end

