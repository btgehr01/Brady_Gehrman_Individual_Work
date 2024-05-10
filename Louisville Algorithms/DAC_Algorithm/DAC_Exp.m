function y = DAC_Exp(b,n,m)
if n == 1
y = b;
else
y = DAC_Exp(b,n/2,m);
y=mod(y*y,m);
end
end
