import numpy as np
import random
from queue import Queue
import matplotlib.pyplot as plt


# Six workers arrive per minute on average
arrivalRate = 6
# We except a worker to arrive every 0.1667 minutes (1 / arrival rate)
meanInterArrivalTime = 1.0 / arrivalRate
# Simulation time is between 8:00am and 9:00am
totalSimulationTime = 120

# Define Elevator Constant
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

# Specific time checks
time_checks = [15, 30, 45, 60]

AverageWaitTimeArray = []
AverageWalkers = [[], [], []]
AverageTimeLastWorkerBoardsElevator = []
AverageNumberOfWorkersInLineAtTimes = [[], [], [], []]


numberOfSimulations = 1

for _ in range(numberOfSimulations):

    # Instantiate an elevator list (will hold three different arrays that represent the three different destination floors)
    elevatorList = [[], [], []]
    # Instantiate a waiting queue (will hold people objects)
    waitingQueue = Queue()
    # Instantiate destimation arrival list (will hold all workers that took the elevator to their destinations)
    destinationArrivalList = []
    # Instantiate a walk list (will hold the )
    walkList = [[], [], []]
    # variable to hold time that last work enters the elevator
    timeLastWorkerBoardsElevator = None

    # Simulation Output Variables
    averageWaitTime = []
    timesForAverageWaitTime = []

    num_walk_to_floor2 = 0
    num_walk_to_floor3 = 0
    num_walk_to_floor4 = 0

    walkListForPlot = [[], [], []]
    timesForWalkList = []

    numberOfWorkersInQueue = []
    timesList = []

    def numberOfWorkersOnElevator(elevatorList):
        return len(elevatorList[0]) + len(elevatorList[1]) + len(elevatorList[2])

    def getTimeUntilElevatorWillDepart(elevatorList):
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
        currentTotalWaitTime = 0.0
        while not waitingQueue.empty():
            # fetch worker from front of queue
            worker = waitingQueue.get()
            # update worker wait time
            worker["waitTime"] =  currentTime - worker["arrival"]
            currentTotalWaitTime += currentTime - worker["arrival"]
            updatedWaitingQueue.put(worker)
        waitingQueue = updatedWaitingQueue
        averageWaitTime.append(currentTotalWaitTime/waitingQueue.qsize())
        timesForAverageWaitTime.append(currentTime)


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

    def updateWalkPlot(destinationFloor, currentTime):
        global num_walk_to_floor2
        global num_walk_to_floor3
        global num_walk_to_floor4
        global walkListForPlot
        global timesForWalkList
        if destinationFloor == 2:
            num_walk_to_floor2 += 1
        elif destinationFloor == 3:
            num_walk_to_floor3 += 1
        else:
            num_walk_to_floor4 += 1

        walkListForPlot[0].append(num_walk_to_floor2)
        walkListForPlot[1].append(num_walk_to_floor3)
        walkListForPlot[2].append(num_walk_to_floor4)
        timesForWalkList.append(currentTime)

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
                updateWalkPlot(destinationFloor, arrivalTime)
            else:
                #add new worker to the waitingQueue
                waitingQueue.put({
                "arrival": arrivalTime,
                "destinationFloor": destinationFloor,
                "waitTime": 0.0,
                })

    # Looping variables
    elevatorIsAtGroundFloor = True
    nextElevatorDepartureTime = None

    currentTime = 0.00

    while currentTime < totalSimulationTime or not waitingQueue.empty():

        # compute when next worker arrives
        inter_arrival_time = np.random.exponential(scale=meanInterArrivalTime)
        # compute arrival's floor destination
        destinationFloor = np.random.randint(2, 5)
        # increment currentTime 
        currentTime += inter_arrival_time

        numberOfWorkersInQueue.append(waitingQueue.qsize())
        timesList.append(currentTime)

        # Initialize nextElevatorDepartureTime if null (only for the first arrival)
        if nextElevatorDepartureTime is None:
            nextElevatorDepartureTime = 0.5 + currentTime

        # Print out queue status if the time is right
        for i, time in enumerate(time_checks):
            # if last arrival iteration was the one of the minute marks, print queue status
            if(currentTime >= time and currentTime - inter_arrival_time < time):
                AverageNumberOfWorkersInLineAtTimes[i].append(waitingQueue.qsize())
                print(str(time) + " minute-mark: " + str(waitingQueue.qsize()) + " worker(s) in queue")

        # update nextElevatorDepartureTime
        nextElevatorDepartureTime = nextElevatorDepartureTime - inter_arrival_time

        # set elevatorIsAtGroundFloor and clear elevatorList if elevator is waiting at ground floor, and it hasn't been set already
        if nextElevatorDepartureTime <= 0.50 and not elevatorIsAtGroundFloor:
            elevatorIsAtGroundFloor = True
            # empty the elevator list
            elevatorList = [[], [], []]

        if currentTime < totalSimulationTime:
            # workers are still arriving, continue the process

            if elevatorIsAtGroundFloor:
                # Load up the elevator with the next worker(s) in the waitingQueue
                if not waitingQueue.empty():
                    # fill the elevatorList with workers in the queue
                    while not waitingQueue.empty() and numberOfWorkersOnElevator(elevatorList) < 12:
                        worker = waitingQueue.get()
                        # add worker to the elevatorList
                        elevatorList[worker["destinationFloor"] - 2].append(worker)
                    # check to see if the queue was emptied
                    if waitingQueue.empty():
                        # if the current arrived worker can board the elevator before it departs, add the recently arrived worker onto the elevator if room
                        if nextElevatorDepartureTime >= 0.00 and numberOfWorkersOnElevator(elevatorList) < 12:
                            elevatorList[destinationFloor - 2].append({
                            "arrival": currentTime,
                            "destinationFloor": destinationFloor,
                            "waitTime": 0.0,
                            })
                        else:
                            # add arrived worker into the system
                            addArrivedWorker(currentTime, destinationFloor)
                    else:
                        addArrivedWorker(currentTime, destinationFloor)
                        # update wait times for the workers still stuck inside of the waitingQueue
                        updatewaitingQueue(currentTime)
                # the waitingQueue is empty, add arrived worker to the elevatorList if they arrived in time
                elif nextElevatorDepartureTime >= 0.00 and numberOfWorkersOnElevator(elevatorList) < 12:
                        elevatorList[destinationFloor - 2].append({
                        "arrival": currentTime,
                        "destinationFloor": destinationFloor,
                        "waitTime": 0.0,
                        })
                else:
                    # add arrived worker into the system
                    addArrivedWorker(currentTime, destinationFloor)
                if nextElevatorDepartureTime <= 0.00:
                    # start elevator's journey and set nextElevatorDepartureTime
                    elevatorIsAtGroundFloor = False
                    nextElevatorDepartureTime = getTimeUntilElevatorWillDepart(elevatorList) 
                    updateDestinationArrivalList(elevatorList)
            else:
                addArrivedWorker(currentTime, destinationFloor)  
                # update each worker's waitTime in the queue
                updatewaitingQueue(currentTime)
        elif not waitingQueue.empty():
            print("emptying queue of size: " + str(waitingQueue.qsize()))
            # empty the remaining queue after the last arrival
            while not waitingQueue.empty():
                # update the current time to next departure time
                currentTime = currentTime + nextElevatorDepartureTime
                # fill the elevatorList with workers in the queue
                while not waitingQueue.empty() and numberOfWorkersOnElevator(elevatorList) < 12:
                    worker = waitingQueue.get()
                    elevatorList[worker["destinationFloor"] - 2].append(worker)

                if waitingQueue.empty():
                    # all workers gone from the quue, and are riding on elevator
                    timeLastWorkerBoardsElevator = currentTime
                else:
                    # some workers are still in the queue, update their wait times and the nextElevatorDepartureTime 
                    updatewaitingQueue(currentTime)
                    nextElevatorDepartureTime = getTimeUntilElevatorWillDepart(elevatorList)
                # update destination list for current workers on elevator, and reset the elevator list
                numberOfWorkersInQueue.append(waitingQueue.qsize())
                timesList.append(currentTime)
                updateDestinationArrivalList(elevatorList)
                elevatorList = [[], [], []]
        else:
            timeLastWorkerBoardsElevator = currentTime



    numberOfWorkersWhoWaitedForElevator = len(destinationArrivalList)
    totalWaitTime = sum([worker["waitTime"] for worker in destinationArrivalList])

    AverageWaitTimeArray.append(round(totalWaitTime / numberOfWorkersWhoWaitedForElevator, 2))

    print("Average Wait Time for a Worker: " + str(round(totalWaitTime/ numberOfWorkersWhoWaitedForElevator, 2)) + " minutes")
    print("Number of Workers that used the elevator: " + str(numberOfWorkersWhoWaitedForElevator))

    for i,floorList in enumerate(walkList):
        AverageWalkers[i].append(len(floorList))
        print(str(len(floorList)) + " workers walked to floor " + str(i + 2) + ".")

    AverageTimeLastWorkerBoardsElevator.append(timeLastWorkerBoardsElevator)
    print("Time last worker boarded the elevator: " + str(timeLastWorkerBoardsElevator))

    # Create Plots
    plt.figure()
    plt.plot(timesForAverageWaitTime, averageWaitTime)

    # Add labels and title
    plt.xlabel('Time (min)')
    plt.ylabel('Average Wait Time (min)')
    plt.title('Plot of Average Wait Time for a Worker')

    plt.savefig('AverageWaitTime.png')

    plt.figure() 
    plt.plot(timesForWalkList, walkListForPlot[0], label='Floor 2')
    plt.plot(timesForWalkList, walkListForPlot[1], label='Floor 3')
    plt.plot(timesForWalkList, walkListForPlot[2], label='Floor 4')

    # Add labels and title
    plt.xlabel('time (min)')
    plt.ylabel('# of Workers')
    plt.title('Number of Workers that Walked to Different Floors')

    plt.legend()

    plt.savefig('Walkers.png')

    plt.figure()
    plt.plot(timesList, numberOfWorkersInQueue)

    # Add labels and title
    plt.xlabel('Time (min)')
    plt.ylabel('Number of Workers in queue')
    plt.title('Plot of Queue Size over time')

    plt.savefig('QueueSize.png')

print("")
print("Simulation Trial Stats:")
print("")
print("Trials AverageWaitTime: " + str(round(sum(AverageWaitTimeArray) / len(AverageWaitTimeArray), 2)) + " minutes")
for i, floorList in enumerate(AverageWalkers):
    print("Average of " + str(round(sum(floorList)/len(floorList), 2)) + " workers walked to floor " + str(i + 2) + ".")
print("Average time last worker boards the elevator: " + str(round(sum(AverageTimeLastWorkerBoardsElevator)/ len(AverageTimeLastWorkerBoardsElevator), 2)))
for i, timeStamp in enumerate(AverageNumberOfWorkersInLineAtTimes):
    if i == 0:
        print("Average # of workers in queue at 15 minute-mark: " + str(round(sum(timeStamp)/ len(timeStamp),2)))
    elif i == 1:
        print("Average # of workers in queue at 30 minute-mark: " + str(round(sum(timeStamp)/ len(timeStamp),2)))
    elif i == 2:
        print("Average # of workers in queue at 45 minute-mark: " + str(round(sum(timeStamp)/ len(timeStamp),2)))
    else:
        print("Average # of workers in queue at 60 minute-mark: " + str(round(sum(timeStamp)/ len(timeStamp),2)))


