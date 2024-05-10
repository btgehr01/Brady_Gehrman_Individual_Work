import matplotlib.pyplot as plt
import numpy as np

data = [0.21, 0.88, 0.37, 0.06, 0.98, 0.61, 0.89, 0.28, 0.70, 0.94, 0.46, 0.92, 0.34, 0.08,
0.79, 0.82, 0.36, 0.62, 0.27, 0.10]

plt.hist(data, bins=10, range=[0, 1], color='blue', alpha=0.7)

# Add labels and title
plt.title('20 Numbers Histogram')
plt.xlabel('Values')
plt.ylabel('Frequency')

# Save the plot
plt.savefig('20_num_histogram.png')

chi_squared_value = 0.0
frequencies = [2, 1, 3, 3, 1, 0, 3, 1, 3, 3]
ideal_frequencies = [2, 2, 2, 2, 2, 2, 2, 2, 2, 2]

for i, ideal in enumerate(ideal_frequencies):
    chi_squared_value = chi_squared_value + ((frequencies[i] - ideal)**2 / ideal)

print('chi_squared_value:', chi_squared_value)
print(sorted(data))

