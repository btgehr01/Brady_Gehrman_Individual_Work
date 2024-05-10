import numpy as np
import matplotlib.pyplot as plt

# Parameters
A1 = 2  # Area of tank 1 (m^2)
A2 = 3  # Area of tank 2 (m^2)
F1 = 1  # Inflow rate to tank 1 (m^3/s)
F2 = 0.8  # Flow rate from tank 1 to tank 2 (m^3/s)
F0 = 0.5  # Outflow rate from tank 2 (m^3/s)

# Initial conditions
h1_0 = 1  # Initial height of tank 1 (m)
h2_0 = 0.5  # Initial height of tank 2 (m)

# Time parameters
dt = 0.1  # Time step (s)
t_end = 10  # Simulation end time (s)
num_steps = int(t_end / dt)  # Number of time steps

# Arrays to store results
time = np.linspace(0, t_end, num_steps)
h1 = np.zeros(num_steps)
h2 = np.zeros(num_steps)

# Set initial heights
h1[0] = h1_0
h2[0] = h2_0

# Euler's method to solve differential equations
for i in range(1, num_steps):
    # Calculate derivative terms
    dh1_dt = (F1 - F2) / A1
    dh2_dt = (F2 - F0) / A2
    
    # Update heights using Euler's method
    h1[i] = max(h1[i-1] + dh1_dt * dt, 0)  # Ensure height doesn't go below 0
    h2[i] = max(h2[i-1] + dh2_dt * dt, 0)  # Ensure height doesn't go below 0

# Plot results
plt.plot(time, h1, label='Tank 1')
plt.plot(time, h2, label='Tank 2')
plt.xlabel('Time (s)')
plt.ylabel('Height (m)')
plt.title('Coupled Tank Simulation with Emptying')
plt.legend()
plt.grid(True)
plt.show()
