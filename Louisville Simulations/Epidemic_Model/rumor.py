import random
import matplotlib.pyplot as plt


# number of comference attendees (N is an even integer 2 ≤ N ≤ 1,000,000)
N_array = [1000, 10000]

pair_talk_time = 1

# 1 attendees starts a rumor in each conference room
number_of_satellite_rooms = 4

number_of_minutes = 61

likelihood_of_spreading_rumor = 0.5

# after a person has heard the rumor 2 times, they will stop spreading the rumor

alerted_threshold = 2

population = []

def initializeRoomArray(number_of_rooms, N):
    rooms = []

    base_size = N // number_of_rooms

    # Generate random even sizes for each room
    for _ in range(number_of_rooms):
        room_size = room_size = base_size + random.randint(-40, 40)
        if room_size % 2 != 0:
            room_size += 1
        rooms.append(room_size)

    # Adjust the last room size to ensure the total sum equals N
    total_size = sum(rooms)
    rooms[-1] += N - total_size

    return rooms




def initializePopulation(room_array):
    global population
    population = [[] for _ in range(len(room_array))]
    for i, roomSize in enumerate(room_array):
        for _ in range(roomSize):
            if _ == 0:
                population[i].append(
                    {
                        "times_exposed": 1,
                        "infected": True,
                        "stagnated": False, 
                        "roomNumber": i,
                    }
                )
            else:
                population[i].append(
                    {
                        "times_exposed": 0,
                        "infected": False,
                        "stagnated": False,
                        "roomNumber": i, 
                    }
                )


def findPercentageExposed(population, N):
    number_exposed = 0
    for room in population:
        for employee in room:
            if employee["infected"] == True:
                number_exposed = number_exposed + 1

    return 100 * (number_exposed / N)

def generate_random_except(N, undesired_numbers):
    random_number = random.randint(0, N - 1)
    while random_number in undesired_numbers:
        random_number = random.randint(0, N - 1)
    return random_number

def check_probability(probability):
    return random.random() < probability

def plot_infected(infected_array, N):

    x_values = list(range(len(infected_array)))

    # Plotting the data
    plt.plot(x_values, infected_array)

    # Adding labels and title
    plt.xlabel('Minute')
    plt.ylabel('Heard Rumor (%)')
    plt.title('Plot of Total Employees Who Have Heard the Rumor (' + N + ' Employees)')

    plt.savefig('infected_' + N + '.png')
    plt.show()





for N in N_array:
    print("")
    print("New Simulation: " + str(N) + " employees") 
    print("")
    roomArray = initializeRoomArray(number_of_satellite_rooms, N)
    print(roomArray)
    initializePopulation(roomArray)
    print(len(population[0]))
    print(len(population[1]))
    print(len(population[2]))
    print(len(population[3]))
    infected_per_minute_array = []
    for minute in range(number_of_minutes):
        percentExposed = findPercentageExposed(population, N)
        print(f'minute {minute}: {percentExposed}%')
        infected_per_minute_array.append(percentExposed)
        for roomNum, group in enumerate(population):
            pairedEmployeeIndexes = set()
            for index, employee in enumerate(group):
                if employee["stagnated"] == False and employee["infected"]:
                    pairedEmployeeIndexes.add(index)
                    # pick a random individual to pair up with
                    random_number_1 = generate_random_except(len(group), pairedEmployeeIndexes)
                    pairedEmployeeIndexes.add(random_number_1)
                    targeted_employees = [population[roomNum][random_number_1]]
                    for emp in targeted_employees:
                        if check_probability(likelihood_of_spreading_rumor):
                            emp["infected"] = True
                            emp["times_exposed"] = emp["times_exposed"] + 1
                            if emp["times_exposed"] == alerted_threshold:
                                emp["stagnated"] = True



    plot_infected(infected_per_minute_array, str(N))
