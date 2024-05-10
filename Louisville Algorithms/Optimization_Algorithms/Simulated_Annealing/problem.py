from abc import ABC, abstractmethod
import copy
import numpy as np

class MyProblem(ABC):
    """Abstract class used to provide a skeleton for problem representation"""
    @abstractmethod
    def get_neighbors(self):
        pass
    
    @abstractmethod
    def evaluate(self):
        pass
    
def L2_dist(x, y):
    return ((x[0]-y[0])**2 + (x[1]-y[1])**2)**.5 
        
def L1_dist(x,y):
    return (abs(x[0]-y[0]) + abs(x[1]-y[1]))

class TravelingSalesmanProblem(MyProblem):
    """Representation of a traveling salesman optimization problem.  The goal
    is to find the shortest path that visits every city in a closed loop path.
    
    Students should only need to implement or modify the successors() and
    get_values() methods.
    
    Parameters
    ----------
    cities : list
        A list of cities specified by a tuple containing the name and the x, y
        location of the city on a grid. e.g., ("Atlanta", (585.6, 376.8))
    
    Attributes
    ----------
    names
    coords
    path : list
        The current path between cities as specified by the order of the city
        tuples in the list.
    """
    def __init__(self, cities, dist_measure=L1_dist):
        self.path = copy.deepcopy(cities)
        self.dist_measure = dist_measure
    
    def copy(self):
        """Return a copy of the current path state."""
        new_tsp = TravelingSalesmanProblem(self.path)
        return new_tsp
    
    @property
    def names(self):
        """Strip and return only the city name from each element of the
        path list. For example,
            [("Atlanta", (585.6, 376.8)), ...] -> ["Atlanta", ...]
        """
        names, _ = zip(*self.path)
        return names
    
    @property
    def coords(self):
        """Strip the city name from each element of the path list and return
        a list of tuples containing only pairs of xy coordinates for the
        cities. For example,
            [("Atlanta", (585.6, 376.8)), ...] -> [(585.6, 376.8), ...]
        """
        _, coords = zip(*self.path)
        return coords
    
    
    def get_neighbors(self):
        """Return a list of states in the neighborhood of the current state by
        switching the order in which any adjacent pair of cities is visited.
        
        For example, if the current list of cities (i.e., the path) is [A, B, C, D]
        then the neighbors will include [A, B, D, C], [A, C, B, D], [B, A, C, D],
        and [D, B, C, A]. (The order of successors does not matter.)
        
        In general, a path of N cities will have N neighbors (note that path wraps
        around the end of the list between the first and last cities).

        Returns
        -------
        list<Problem>
            A list of TravelingSalesmanProblem instances initialized with their list
            of cities set to one of the neighboring permutations of cities in the
            present state
        """
        neighbors = []
        for i in range(len(self.path)-1):
            new_path = self.copy()
            new_path.path[i], new_path.path[i+1] = new_path.path[i+1], new_path.path[i] # Swap two non-adjacent edges
            neighbors.append(new_path)
            
        last_path = self.copy()
        last_path.path[0], last_path.path[-1] = last_path.path[-1], last_path.path[0]
        neighbors.append(last_path)
        
        return neighbors 
    
        
    def evaluate(self):
        """Calculate the total length of the closed-circuit path of the current
        state by summing the distance between every pair of adjacent cities.  Since
        the default simulated annealing algorithm seeks to maximize the objective
        function, return -1x the path length. (Multiplying by -1 makes the smallest
        path the smallest negative number, which is the maximum value.)
        
        Returns
        -------
        float
            A floating point value with the total cost of the path given by visiting
            the cities in the order according to the self.cities list
        
        Notes
        -----
            (1) Remember to include the edge from the last city back to the
            first city
            
            (2) Remember to multiply the path length by -1 so that simulated
            annealing finds the shortest path
        """
        
        
        length = 0
        coords = self.coords
        for i in range(len(coords)-1):
            length += np.abs(self.dist_measure(coords[i], coords[i+1]))
            
        length += np.abs(self.dist_measure(coords[0], coords[-1])) # Return to the initial city
        return length
