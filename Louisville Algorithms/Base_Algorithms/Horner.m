function y = Horner(a,x)
%Horner evaluates the polynomial with coefficents a at point x
n = length(a);
y = a(n);
for i=n-1:-1:1
    y = y*x + a(i);
end