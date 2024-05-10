function [D,P]=FloydWarshall(D,P)
%
%
n=length(D);
for k = 1:n
    for i = 1:n
        for j= 1:n
            x=D(i,k)+D(k,j);
            if x < D(i,j)
                D(i,j)=x;
                P(i,j)=k;
            end
        end
    end
end

