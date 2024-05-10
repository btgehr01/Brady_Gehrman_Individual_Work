function Pi = Prefix(P)
m = length(P);
for i = 1 : m
    Pi(i) = 0;
end
k = 0;
for i = 2 : m
    while (k > 0) && (P(k+1) ~= P(i))
        k = Pi(k);
    end
    if P(k+1) == P(i)
        k = k + 1;
    end
    Pi(i) = k;
end
end

