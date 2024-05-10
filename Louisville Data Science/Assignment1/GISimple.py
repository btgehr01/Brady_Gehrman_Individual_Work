import random

def  GISimple(impSpd = (1,9), golemSpd = (3,5), headStart = 5, exitPosition = 50):
    golemPosition = 0
    impPosition = headStart
    impIsCaptured = False
    timeInSeconds = 0

    while impPosition < exitPosition and not impIsCaptured:
        # increment the time
        timeInSeconds = timeInSeconds + 1
    
        # increment the imp
        impPosition = impPosition + random.randint(impSpd[0], impSpd[1])

        #check to see if the imp escaped, if not continue
        if impPosition < exitPosition:
            # increment the golem
            golemPosition = golemPosition + random.randint(golemSpd[0], golemSpd[1])

            # check to see if the imp was captured by the golem
            if golemPosition >= impPosition:
                impIsCaptured = True
    
    if impIsCaptured:
        return False
    else:
        return True




    


