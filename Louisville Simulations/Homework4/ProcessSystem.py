import networkx as nx
import matplotlib.pyplot as plt
import random
from collections import Counter

numberOfSamples = 100000
processExecutionTimes = []
critical_paths_data = {
    "[1, 2, 3, 4, 7]": 0,
    "[1, 2, 4, 7]": 0,
    "[1, 5, 3, 4, 7]": 0,
    "[1, 5, 4, 7]": 0,
    "[1, 5, 6, 7]": 0
}

def displayGraph(G):
     # Draw the graph with edge labels and weights
    pos = nx.circular_layout(G)
    nx.draw(G, pos, with_labels=True, node_color='lightblue', node_size=1500, font_size=12, arrows=True)
    nx.draw_networkx_edge_labels(G, pos, edge_labels={(u, v): d['weight'] for u, v, d in G.edges(data=True)})

    # Save the graph as an image
    plt.savefig("directedGraph.png")

    # Display the graph
    plt.show()
     

def dfs_traversal(graph, currentNode, path, currentCost):
    if currentNode == 7:
        global allTraversedPaths
        allTraversedPaths.append({
             "path": path + [currentNode],
             "cost": currentCost
        })
    for neighbor, attr in graph[currentNode].items():
            edge_cost = attr['weight']
            if neighbor not in path:
                dfs_traversal(graph, neighbor, path + [currentNode], currentCost + edge_cost)

# Function to find the critical path
def find_critical_path(traversed_paths):
    critical_path = max(traversed_paths, key=lambda x: x['cost'])
    return str(critical_path['path'])

for sample in range(numberOfSamples):
    allTraversedPaths = []
    # Create a directed graph
    G = nx.DiGraph()

    # Add nodes
    G.add_nodes_from([1, 2, 3, 4, 5, 6, 7])


    weight_1_2 = random.randint(3, 5)
    weight_2_4 = random.randint(7, 9)
    weight_3_4 = random.randint(5, 8)
    weight_5_6 = random.randint(7, 10)
    weight_6_7 = random.randint(8, 12)

    # Define edges with assigned weights
    edges = [(1, 2, {'weight': weight_1_2}), 
             (2, 3, {'weight': 6}), 
             (2, 4, {'weight': weight_2_4}),
             (3, 4, {'weight': weight_3_4}), 
             (4, 7, {'weight': 4}), 
             (1, 5, {'weight': 6}),
             (5, 3, {'weight': 7}), 
             (5, 4, {'weight': 9}), 
             (5, 6, {'weight': weight_5_6}),
             (6, 7, {'weight': weight_6_7})]
    G.add_edges_from(edges)

    node3Cost = max([weight_1_2 + 6, 6 + 7])
    node4Cost = max([weight_1_2 + weight_2_4, node3Cost + weight_3_4, 6 + 9])
    node6Cost = max([6 + weight_5_6])
    completion_time = max([node4Cost + 4, node6Cost + weight_6_7])
    processExecutionTimes.append(completion_time)

    print("Process completion_time for sample", sample + 1, ":", completion_time)
    
    dfs_traversal(G, 1, [], 0)

    # find critical path
    critical_path = find_critical_path(allTraversedPaths)

    critical_paths_data[critical_path] += 1

    print("Critical Path:", critical_path)

# Plotting critical paths count
paths = list(critical_paths_data.keys())
counts = list(critical_paths_data.values())

x = range(len(paths))
plt.figure(figsize=(10, 8))

# Increase bar width and set color
plt.bar(x, counts, width=0.5, color='blue')
plt.xticks(x, paths, rotation=45)
plt.xlabel('Critical Path')
plt.ylabel('Count')
plt.title('Count of Critical Paths: ' + str(numberOfSamples) + " samples")
plt.savefig("criticalPaths.png")
plt.show()

print("Average Process Execution Time:", sum(processExecutionTimes) / numberOfSamples)
