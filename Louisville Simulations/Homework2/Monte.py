

# Area of a Circle = A = pi * r^2
# Area of Bounding Square = (2 * r)^2
# A (circle) / A (Square) = pi/4 -> pi = 4 * n/N
# n = number of random points that reside in the circle
# N = total number of random points generated

# For my simulation the radius (r) will be 1 unit
# The circle will be centered on the origin,  the box will be a 2 x 2 box also centered on the origin

import random
import numpy as np
import matplotlib.pyplot as plt

N_values = [5, 10, 50, 100, 500, 1000, 5000, 10000, 20000, 50000, 100000]
pi_estimations = []

for N in N_values:
    n = 0
    count = N
    while count > 0:
        x = random.uniform(-1, 1)
        y = random.uniform(-1, 1)
        if x**2 + y**2 <= 1.0:
            n = n + 1
        count = count - 1 
    EstimationOfPi = 4.0 * n / N
    pi_estimations.append(EstimationOfPi)


plt.figure(figsize=(6, 6))
plt.title('Monte Carlo Estimation of π verus N')
plt.scatter(N_values, pi_estimations, color='blue')
plt.axhline(y=np.pi, color='red', linestyle='--', label='True π')
plt.xlabel('N')
plt.ylabel('Pi_Estimated')

# Save the plot
plt.savefig('monte_carlo_pi_estimation.png')














