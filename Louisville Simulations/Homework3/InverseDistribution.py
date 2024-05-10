import random
import matplotlib.pyplot as plt

def generate():
    u = random.random()
    if u <= 0.5:
        return (2 * u) ** 0.5
    else:
        return (4 - (8 - (8 * u)) ** 0.5) / 2

# Generate a large number of samples
num_samples = 100000
samples = [round(generate(), 2) for _ in range(num_samples)]

# Create a histogram
plt.hist(samples, bins=40, density=True, color='blue', edgecolor='black')

# Add labels and title
plt.title('Histogram of Samples from Triangle Distribution')
plt.xlabel('Value')
plt.ylabel('Occurances')

# Show the plot
plt.show()
plt.savefig('TriangleDist.png')
