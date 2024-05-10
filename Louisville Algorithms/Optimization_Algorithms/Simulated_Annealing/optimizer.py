import numpy as np

class Schedule():
    def __init__(self, method="exponential", alpha=0.95, starting_temp=100):
        self.method = method
        self.alpha = alpha
        self.T_0 = starting_temp

    def cool(self, k):
        """
        Implements several cooling methods using format from:
        http://what-when-how.com/artificial-intelligence/a-comparison-of-cooling-schedules-for-simulated-annealing-artificial-intelligence/
        """
        if self.method == "exponential":
            # Assumes alpha is a constant 0.8 <= alpha <= 0.9
            return self.T_0*(self.alpha**k)
        
        if self.method == "logarithmic":
            # Assumes alpha is a constant greater than 1
            return  self.T_0/(1 + self.alpha * np.log(1+k))
        
        if self.method == "linear":
            # Assumes alpha is a constant greater than zero
            return self.T_0/(1+self.alpha*k)
        
        else:
            raise NotImplementedError(f"There is no cooling schedule matching {self.method}")

def simulated_annealing(problem, cooling_schedule, stop_temp=1e-5, max_iter=np.inf, random_state=None, verbose=True):
    """The simulated annealing algorithm, a version of stochastic hill climbing
    where some downhill moves are allowed. Downhill moves are accepted readily
    early in the annealing schedule and then less often as time goes on. The
    schedule input determines the value of the temperature T as a function of
    time.
    
    Parameters
    ----------
    problem : Problem
        An optimization problem, already initialized to a random starting state.
        The Problem class interface must implement a callable method
        "neighborhood()" which returns states in the neighborhood of the current
        state, and a callable function "evaluate()" which returns a fitness
        score for the state. (See the `TravelingSalesmanProblem` class for more details.)
        
    cooling_schedule : Schedule
        Instance of Schedule class initialized containing starting temp, alpha, and specified
        cooling method to use.

    stop_criteria : float
        Threshold for the temperature to determine when to stop the search.
        
    random_state : int
        The seed for the pseudorandom number generator. Passing "None" or no argument
        will result in wholely random behaviour each run. Passing any other integer value
        will produce the same behaviour and final convergence each run. 
        
    Returns
    -------
    Problem
        An approximate *best* found solution state of the optimization problem
        
    See Also
    --------
    Adapted (with modifications) from AIMA simulated_annealing() pseudocode
        https://github.com/aimacode/aima-pseudocode/blob/master/md/Simulated-Annealing.md
    """
    rand_gen = np.random.default_rng(random_state) # Makes our results reproducible with the same seed
    
    current = problem
    best_solution = current
    best_cost = current.evaluate()
    
    T_k = cooling_schedule.T_0
    k = 0
    
    while k < max_iter and T_k > stop_temp:
        T_k = cooling_schedule.cool(k)
        
        next_state = rand_gen.choice(current.get_neighbors())
        
        # Find costs for determining the Δ
        current_cost = current.evaluate()
        next_cost = next_state.evaluate()
        
        if next_cost < current_cost: # Case where f(j) <= f(i) 
            current = next_state
            
            # If this is the best move we have found so far, record it
            if next_cost < best_cost:
                best_solution = next_state
                best_cost = next_cost
            
        else:
            # With some probability, accept a worse move
            prob = np.exp((current_cost - next_cost)/T_k)
            R = rand_gen.uniform(0,1)
            if prob > R:
                current = next_state
        
        k += 1
    
    if verbose:
        print(f"Terminated with temperature T={T_k:.2g}\nAt iteration #{k:,}.")
        
    return best_solution

# Hill Climbing algorithm
def hill_climbing(problem, max_iter=1e10, min_delta=0):
    iterations = 0
    current = problem # FYI, current will always be the best path we have seen (greedy)
    delta = np.inf
    
    # While there is still a notable change from the last state to the next, repeat the above from the new state
    while delta > min_delta and iterations < max_iter:
        neighborhood = current.get_neighbors()
        best_neighbor_idx = np.argmin([x.evaluate() for x in neighborhood]) # Find the closest neighbour
        next_state = neighborhood[best_neighbor_idx] # Move to the next state
        delta = current.evaluate() - next_state.evaluate() 
        current = next_state
        iterations += 1
    return current