import random
import matplotlib.pyplot as plt


# number of employees (N is an even integer 2 ≤ N ≤ 1,000,000)
N_array = [100, 1000, 10000]

number_of_simulation_days = 41

alerted_threshold = 2

population = []

def initializePopulation(N):
    global population
    population = []
    for _ in range(N):
        if _ == 0:
            population.append(
                {
                    "times_exposed": 1,
                    "infected": True,
                    "alerted": False, 
                }
            )
        else:
            population.append(
                {
                    "times_exposed": 0,
                    "infected": False,
                    "alerted": False, 
                }
            )


def findPercentageExposed(population):
    number_exposed = 0
    for employee in population:
        if employee["infected"] == True:
            number_exposed = number_exposed + 1

    return 100 * (number_exposed / len(population))

def generate_random_except(N, undesired_numbers):
    random_number = random.randint(0, N - 1)
    while random_number in undesired_numbers:
        random_number = random.randint(0, N - 1)
    return random_number

def check_probability(probability):
    return random.random() < probability

def plot_infected(infected_array, N):

    x_values = list(range(len(infected_array)))

    plt.figure()
    # Plotting the data
    plt.plot(x_values, infected_array)

    # Adding labels and title
    plt.xlabel('Day')
    plt.ylabel('Infected (%)')
    plt.title('Plot of Total Employees Infected (' + N + ' Employees)')

    plt.savefig('infected_' + N + '.png')
    plt.show()





for N in N_array:
    print("")
    print("New Simulation: " + str(N) + " employees") 
    print("")
    initializePopulation(N)
    infected_per_day_array = []
    for day in range(number_of_simulation_days):
        print(f'day {day}: {findPercentageExposed(population)}%')
        infected_per_day_array.append(findPercentageExposed(population))
        for index, employee in enumerate(population):
            if employee["alerted"] == False and employee["infected"]:
                # pick 3 people to send an email to
                random_number_1 = generate_random_except(N, [index])
                random_number_2 = generate_random_except(N, [index, random_number_1])
                random_number_3 = generate_random_except(N, [index, random_number_1, random_number_2])
                targeted_employees = [population[random_number_1], population[random_number_2], population[random_number_3]]
                for emp in targeted_employees:
                    if check_probability(0.4):
                        emp["infected"] = True
                        emp["times_exposed"] = emp["times_exposed"] + 1
                        if emp["times_exposed"] == alerted_threshold:
                            emp["alerted"] = True



    plot_infected(infected_per_day_array, str(N))
