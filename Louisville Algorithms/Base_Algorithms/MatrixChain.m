function [M, K] = MatrixChain(p)
n = size(p, 2) - 1;
M = zeros(n);
K = zeros(n);
for l = 1 : n - 1
    for i = 1 : n - l
        j = i + l;
        m1 = inf;
        kk = inf;
        for k = i : j - 1
            x = M(i,k) + M(k+1,j) + p(i) * p(k + 1) * p(j+1);
            if (m1 >= x)
                m1 = x;
                kk = k;
            end
        end
        M(i, j) = m1;
        K(i, j) = kk;
    end
end

