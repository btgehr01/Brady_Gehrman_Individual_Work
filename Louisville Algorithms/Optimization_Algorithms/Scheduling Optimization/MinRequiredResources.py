from pyworkforce.scheduling import MinRequiredResources
from pprint import PrettyPrinter
import matplotlib.pyplot as plt
import numpy as np
import time

# Columns are an hour of the day (24 hours in a day -> 24 columns), rows are the days (2 days in this example -> 2 row vectors) (N_dp)
required_resources = [
    [9, 11, 17, 9, 7, 12, 5, 11, 8, 9, 18, 17, 8, 12, 16, 8, 7, 12, 11, 10, 13, 19, 16, 7],
    [13, 13, 12, 15, 18, 20, 13, 16, 17, 8, 13, 11, 6, 19, 11, 20, 19, 17, 10, 13, 14, 23, 16, 8],
    [8, 9, 10, 15, 12, 25, 14, 16, 17, 9, 20, 21, 6, 14, 11, 20, 13, 15, 10, 12, 18, 23, 19, 10],
    [5, 9, 10, 13, 11, 22, 12, 10, 11, 9, 24, 26, 2, 14, 11, 17, 12, 19, 11, 13, 24, 23, 19, 23],
    [4, 4, 5, 6, 10, 22, 24, 25, 23, 4, 24, 25, 4, 14, 16, 17, 18, 19, 11, 11, 24, 23, 25, 23],
    [24, 16, 19, 20, 20, 22, 24, 25, 23, 23, 24, 25, 25, 15, 18, 17, 18, 19, 11, 11, 13, 13, 12, 13],
    [24, 25, 25, 22, 15, 12, 15, 16, 5, 5, 6, 7, 24, 24, 25, 25, 22, 20, 12, 15, 14, 15, 18, 19],
    [9, 11, 17, 9, 7, 12, 5, 11, 8, 9, 18, 17, 8, 12, 16, 8, 7, 12, 11, 10, 13, 19, 16, 7],
    [13, 13, 12, 15, 18, 20, 13, 16, 17, 8, 13, 11, 6, 19, 11, 20, 19, 17, 10, 13, 14, 23, 16, 8],
    [8, 9, 10, 15, 12, 25, 14, 16, 17, 9, 20, 21, 6, 14, 11, 20, 13, 15, 10, 12, 18, 23, 19, 10],
    [5, 9, 10, 13, 11, 22, 12, 10, 11, 9, 24, 26, 2, 14, 11, 17, 12, 19, 11, 13, 24, 23, 19, 23],
    [4, 4, 5, 6, 10, 22, 24, 25, 23, 4, 24, 25, 4, 14, 16, 17, 18, 19, 11, 11, 24, 23, 25, 23],
    [24, 16, 19, 20, 20, 22, 24, 25, 23, 23, 24, 25, 25, 15, 18, 17, 18, 19, 11, 11, 13, 13, 12, 13],
    [24, 25, 25, 22, 15, 12, 15, 16, 5, 5, 6, 7, 24, 24, 25, 25, 22, 20, 12, 15, 14, 15, 18, 19],
]

# Each entry of a shift, is an hour of the day (24 hours in a day -> 24 columns) (E_sp)
shifts_coverage = {"Morning": [0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                   "Afternoon": [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0],
                   "Night": [1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1],
                   "Mixed": [0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0]}

# The cost of shifting a resource if each shift, if present, solver will minimize the total cost (C_s)
cost_dict = {"Morning": 1, "Afternoon": 1.2, "Night": 2, "Mixed": 1.5}

start_time = time.time()

scheduler = MinRequiredResources(num_days=14,  # S
                                 periods=24,  # P
                                 shifts_coverage=shifts_coverage,
                                 required_resources=required_resources,
                                 max_period_concurrency=27,  # gamma
                                 max_shift_concurrency=25)   # beta

end_time = time.time()

elapsed_time = end_time - start_time

print(f"Execution time: {elapsed_time:.5} seconds")

solution = scheduler.solve()
pp = PrettyPrinter(indent=2)
pp.pprint(solution)

# calculate a set of days from the solution object
days = sorted(list(set(assignment['day'] for assignment in solution['resources_shifts'])))

# calculate the total hours needed on the plot
hours_per_day = 24
total_hours = len(days) * hours_per_day

# create and initialize an object that will store the hour number and the allocated resources for that hour
resource_hours = {hour: 0 for hour in range(total_hours)}

# calculate allocated resources for each hour
for assignment in solution['resources_shifts']:
    # fetch shift coverage for the shift assignment
    shift_coverage = shifts_coverage.get(assignment['shift'], [0] * hours_per_day)
    for hour, coverage in enumerate(shift_coverage):
        #check to see if the shift_coverage index has a 1, indicating the hour is included within the shift
        if coverage == 1:
            # assign corresponding hour number the specified allocated resources
            resource_hours[assignment['day'] * hours_per_day + hour] += assignment['resources']

fig, ax1 = plt.subplots()

# plot allocated resources
ax1.plot(range(total_hours), list(resource_hours.values()), label='Scheduled Resources', color='blue')
ax1.set_xlabel('Hour')
ax1.set_ylabel('Resources')

# plot required resources
ax1.plot(range(total_hours), np.array(required_resources).flatten(), label='Required Resources', color='orange')

# show legends
ax1.legend(loc='upper left')

plt.title('Required vs Scheduled Agents (MRR)')
plt.grid(True)
plt.savefig('MinRequiredResources_14Day.png')