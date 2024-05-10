function y = Naive_Exp(b,n,m)
y=1;
for i=1:n
y=mod(y*b,m);
end
