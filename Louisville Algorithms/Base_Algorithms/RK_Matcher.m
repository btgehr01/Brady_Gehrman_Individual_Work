function s = RK_Matcher(T,P,d,q)
%d : length of the aphabet; sigma. For example in the case of 8 bit acii
%d = 256.
%q : modulus that reduces the resolution of the string hash values. 
%select q to be a small prime number
n = length(T);
m = length(P);
h = mod(d^(m-1), q);
p = 0;
t(1) = 0;
k = 1;
for i = 1 : m
    p = mod(d*p + P(i), q);
    t(1) = mod(d*t(1) + T(i), q);
end
for i = 1 : n-m+1
    if t(i) == p
        if P(1:m) == T(i:i+m-1)
            s(k) = i-1;
            k = k +1;
        end
    end
    if i < n-m+1
        t(i+1) = mod(d*(t(i)-h*T(i))+T(i+m),q);
    end
end        
end

