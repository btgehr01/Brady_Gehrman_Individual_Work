
# 𝑑ℎ1⁄𝑑t = (𝐹1 − 𝐹2)⁄𝐴1; 𝑑h2⁄𝑑t = (𝐹2 − 𝐹0)⁄𝐴2

# 2. ∆ℎ1 = 𝐹1 − 𝐹2⁄𝐴1; ∆ℎ2 = 𝐹2 − 𝐹0⁄𝐴2
# 3. ℎ1(𝑡 + 1) = ℎ1(𝑡) + ∆ℎ1; ℎ2(𝑡 + 1) = ℎ2(𝑡) + ∆ℎ2


import matplotlib.pyplot as plt

def simulate(A1= 1.0, A2= 2.0, F0= 0.02, F1= 0.01, F2= 0.01, h1= 0.0, h2= 0.0):
    h1_array = [h1]
    h2_array = [h2]
    simulation_time = 20
    for t in range(1, simulation_time):
        # update h1
        delta_h1 = (F1 - F2) / A1
        # tank_1 will fill over time
        if delta_h1 > 0:
            h1 += delta_h1
        # tank_1 will empty over time
        else:
            # delta_h1 doesn't empty tank_1, give tank_2 a full delta_h1 amount of water
            if h1 + delta_h1 > 0:
                h1 += delta_h1
            # delta_h1 empties tank_1, give tank_2 the rest of water in tank_1
            else:
                # F2 = F1
                h1 = 0
        h1_array.append(h1)


        # update h2
        delta_h2 = (F2 - F0) / A2
        # tank_2 will fill over time
        if delta_h2 > 0:
            h2 += delta_h2
        # tank_2 will empty over time
        else:
            # delta_h2 doesn't empty tank_2, subtract lost amount
            if h2 + delta_h2 > 0:
                h2 += delta_h2
            # delta_h2 empties tank_2, take tank 2 to zero
            else:
                h2 = 0
                
        h2_array.append(h2)

        


    # Print the results
    print(h1_array)
    print(h2_array)
    # Plot the results
    plt.plot(range(simulation_time), h1_array, label='h1')
    plt.plot(range(simulation_time), h2_array, label='h2')
    plt.xlabel('Time')
    plt.ylabel('Height')
    plt.title('Coupled Tank Simulation')
    plt.legend()
    plt.show()
    plt.savefig('ChangesInHeight.png')

# simulate(A1= 6.0, A2= 3.0, F0= 0.1, F1= 0.5, F2= 0.3, h1= 4.0, h2= 3.0)
simulate(A1= 1.0, A2= 2.0, F0= 0.02, F1= 0.01, F2= 0.01, h1= 2, h2= 1)

