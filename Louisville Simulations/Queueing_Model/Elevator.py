import numpy as np
import random
from queue import Queue


# Six workers arrive per minute on average
arrivalRate = 6
# We except a worker to arrive every 0.1667 minutes (1 / arrival rate)
meanInterArrivalTime = 1.0 / arrivalRate
# Simulation time is between 8:00am and 9:00am
totalSimulationTime = 60

# Define Elevator Constants
elevatorCapacity = 12
elevatorTravelTimeMatrix = np.array([
    [0, 1.00, 1.50, 1.75],
    [1.00, 0, 0.50, 0.75],
    [1.50, 0.50, 0, 0.50],
    [1.75, 0.50, 0.25, 0]
    ])

# Define Stair Walking Probabilties
probWalkSecond = 0.50
probWalkThird = 0.33
probWalkFourth = 0.10

# Simulate worker arrival times using numpy's random class
arrivalTimes = np.cumsum(np.random.exponential(scale=meanInterArrivalTime, size=totalSimulationTime * arrivalRate))

# Simulate floor selection for each worker (all equally probable)
destinationFloors = np.random.randint(2, 5, size=len(arrivalTimes)) 

# Instantiate an elevator list (will hold three different arrays that represent the three different destination floors)
elevatorList = [[], [], []]
# Instantiate a waiting queue (will hold people objects)
waitingQueue = Queue()
# Instantiate destimation arrival list
destinationArrivalList = []
# Instantiate a walk list
walkList = [[], [], []]
# Specific time checks
time_checks = [30, 45, 60]


def numberOfWrokersOnElevator(elevatorList):
    return len(elevatorList[0]) + len(elevatorList[1]) + len(elevatorList[2])

def nextTimeElevatorWillDepart(elevatorList):
    jouneyTime = 0.00
    elevatorStopTime = 0.50
    if len(elevatorList[0]) > 0 and len(elevatorList[1]) > 0 and len(elevatorList[2]) > 0:
        jouneyTime = elevatorTravelTimeMatrix[0][1] + elevatorTravelTimeMatrix[1][2] + elevatorTravelTimeMatrix[2][3] + elevatorTravelTimeMatrix[3][0] + (3 * elevatorStopTime)
    elif len(elevatorList[0]) > 0 and len(elevatorList[1]):
        jouneyTime = elevatorTravelTimeMatrix[0][1] + elevatorTravelTimeMatrix[1][2] + elevatorTravelTimeMatrix[2][0] + (2 * elevatorStopTime)
    elif len(elevatorList[0]) > 0 and len(elevatorList[2]):
        jouneyTime = elevatorTravelTimeMatrix[0][1] + elevatorTravelTimeMatrix[1][3] + elevatorTravelTimeMatrix[3][0] + (2 * elevatorStopTime)
    elif len(elevatorList[1]) > 0 and len(elevatorList[2]):
        jouneyTime = elevatorTravelTimeMatrix[0][2] + elevatorTravelTimeMatrix[2][3] + elevatorTravelTimeMatrix[3][0] + (2 * elevatorStopTime)
    elif len(elevatorList[0]) > 0:
        jouneyTime = elevatorTravelTimeMatrix[0][1] + elevatorTravelTimeMatrix[1][0] + (1 * elevatorStopTime)
    elif len(elevatorList[1]) > 0:
        jouneyTime = elevatorTravelTimeMatrix[0][2] + elevatorTravelTimeMatrix[2][0] + (1 * elevatorStopTime)
    elif len(elevatorList[2]) > 0:
        jouneyTime = elevatorTravelTimeMatrix[0][3] + elevatorTravelTimeMatrix[3][0] + (1 * elevatorStopTime)
    return jouneyTime + elevatorStopTime

def updatewaitingQueue(currentTime):
    global waitingQueue
    updatedWaitingQueue = Queue()
    while not waitingQueue.empty():
        # fetch worker from front of queue
        worker = waitingQueue.get()
        # update worker wait time
        worker["waitTime"] =  currentTime - worker["arrival"]
        updatedWaitingQueue.put(worker)
    waitingQueue = updatedWaitingQueue

def doesWorkerdecideToWalk(destinationFloor):
    walkProbability = random.random()
    if destinationFloor == 2:
        return walkProbability <= probWalkSecond
    elif destinationFloor == 3:
        return walkProbability <= probWalkThird
    else:
        return walkProbability <= probWalkFourth


def updateDestinationArrivalList(elevatorList):
    global destinationArrivalList
    for floorDestinations in elevatorList:
        for worker in floorDestinations:
            destinationArrivalList.append(worker)

def addArrivedWorker(arrivalTime, destinationFloor):
    # add arrived worker to waitingQueue or walkList
    global waitingQueue
    global walkList
    if waitingQueue.qsize() < 12:
            # add new worker to the waiting queue
            waitingQueue.put({
            "arrival": arrivalTime,
            "destinationFloor": destinationFloor,
            "waitTime": 0.0,
            })
    else:
        # decide if the new worker wants to walk instead, since queue is full
        if doesWorkerdecideToWalk(destinationFloor):
            #add new worker to walk list, don't add them to the waitingQueue
            walkList[destinationFloor - 2].append({
            "arrival": arrivalTime,
            "destinationFloor": destinationFloor,
            "waitTime": 0.0,
            })
        else:
            #add new worker to the waitingQueue
            waitingQueue.put({
            "arrival": arrivalTime,
            "destinationFloor": destinationFloor,
            "waitTime": 0.0,
            })

# Looping variables
elevatorIsAtGroundFloor = True
previousArrivalTime = 0.00
nextElevatorDepartureTime = arrivalTimes[0]

# Iterate through arrival times to simulate the elevator simulation
for i, arrivalTime in enumerate(arrivalTimes):
    for time in time_checks:
        # if last arrival iteration was the 30 minute mark
        if(arrivalTime >= time and previousArrivalTime < time):
            print(str(time) + " minute-mark: " + str(waitingQueue.qsize()) + " worker(s) in queue")


    # update nextElevatorDepartureTime
    nextElevatorDepartureTime = nextElevatorDepartureTime - (arrivalTime - previousArrivalTime)
    if nextElevatorDepartureTime < 0.50:
        elevatorIsAtGroundFloor = True

    previousArrivalTime = arrivalTime

    if elevatorIsAtGroundFloor:
        # Load up the elevator with the next worker(s) in the waitingQueue
        if not waitingQueue.empty():
            # fill the elevatorList with workers in the queue
            while not waitingQueue.empty() and numberOfWrokersOnElevator(elevatorList) < 12:
                worker = waitingQueue.get()
                elevatorList[worker["destinationFloor"] - 2].append(worker)
            # check to see if the queue was emptied
            if waitingQueue.empty():
                # if the current arrived worker can board the elevator before it departs, add the recently arrived worker onto the elevator if room
                if nextElevatorDepartureTime > 0.00 and numberOfWrokersOnElevator(elevatorList) < 12:
                    elevatorList[destinationFloors[i] - 2].append({
                    "arrival": arrivalTime,
                    "destinationFloor": destinationFloors[i],
                    "waitTime": 0.0,
                    })
                else:
                    # add arrived worker into the system
                    addArrivedWorker(arrivalTime, destinationFloors[i])
            else:
                # update wait times for the workers still stuck inside of the waitingQueue
                updatewaitingQueue(arrivalTime)
        # the waitingQueue is empty, add arrived worker to the elevatorList if they arrived in time
        elif nextElevatorDepartureTime > 0.00:
                elevatorList[destinationFloors[i] - 2].append({
                "arrival": arrivalTime,
                "destinationFloor": destinationFloors[i],
                "waitTime": 0.0,
                })
        else:
            # add arrived worker into the system
            addArrivedWorker(arrivalTime, destinationFloors[i])
        if nextElevatorDepartureTime <= 0.00:
            # start elevator's journey and set nextElevatorDepartureTime
            elevatorIsAtGroundFloor = False
            nextElevatorDepartureTime = nextTimeElevatorWillDepart(elevatorList) 
            updateDestinationArrivalList(elevatorList)
    else:
        addArrivedWorker(arrivalTime, destinationFloors[i])  
        # update each worker's waitTime
        updatewaitingQueue(arrivalTime) 


numberOfWorkersWhoWaitedForElevator = len(destinationArrivalList)
totalWaitTime = sum([worker["waitTime"] for worker in destinationArrivalList])

print("Average Wait Time For A Worker: " + str(round(totalWaitTime/ numberOfWorkersWhoWaitedForElevator, 2)) + " minutes")

for i,floorList in enumerate(walkList):
    print(str(len(floorList)) + " workers walked to floor " + str(i + 2) + ".")


            
        

