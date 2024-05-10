function s = KMP_Matcher(T,P)
%This string matcher requires Prefix.m function
n = length(T);
m = length(P);
Pi=Prefix(P);
j = 0;
k = 1;
for i = 1 : n
    while (j > 0) && (P(j+1) ~= T(i))
        j = Pi(j);
    end
    if (P(j+1) == T(i))
        j = j + 1;
    end
    if (j == m)
        s(k) = i - m;
        k = k + 1;
        j = Pi(j);
    end
end
end

