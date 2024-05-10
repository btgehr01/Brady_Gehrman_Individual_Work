import networkx as nx
import matplotlib.pyplot as plt
import random

numberOfSamples = 1

def displayGraph(G):
    # Draw the graph with edge labels and weights
    pos = nx.circular_layout(G)
    nx.draw(G, pos, with_labels=True, node_color='lightblue', node_size=1500, font_size=12, arrows=True)
    nx.draw_networkx_edge_labels(G, pos, edge_labels={(u, v): d['weight'] for u, v, d in G.edges(data=True)})

    # Save the graph as an image
    plt.savefig("directedGraph.png")

    # Display the graph
    plt.show()

def dfs_traversal(graph, currentNode, path, currentCost, arrival_times):
    if currentNode == 7:
        global allTraversedPaths
        allTraversedPaths.append({
             "path": path + [currentNode],
             "cost": currentCost
        })
    max_arrival_time = max((arrival_times.get(neighbor, 0) for neighbor in graph.predecessors(currentNode)), default=0)
    arrival_time = max_arrival_time + graph.nodes[currentNode]['duration']
    arrival_times[currentNode] = arrival_time
    
    for neighbor, attr in graph[currentNode].items():
        edge_cost = attr['weight']
        if neighbor not in path:
            dfs_traversal(graph, neighbor, path + [currentNode], currentCost + edge_cost, arrival_times)

for _ in range(numberOfSamples):
    allTraversedPaths = []
    # Create a directed graph
    G = nx.DiGraph()

    # Add nodes
    G.add_nodes_from([1, 2, 3, 4, 5, 6, 7])

    # Add edges with uniform weight
    edges = [(1, 2, {'weight': random.randint(3, 5)}), (2, 3, {'weight': 6}), (2, 4, {'weight': random.randint(7, 9)}),
             (3, 4, {'weight': random.randint(5, 8)}), (4, 7, {'weight': 4}), (1, 5, {'weight': 6}),
             (5, 3, {'weight': 7}), (5, 4, {'weight': 9}), (5, 6, {'weight': random.randint(7, 10)}),
             (6, 7, {'weight': random.randint(8, 12)})]
    G.add_edges_from(edges)

    # Assign random durations to nodes
    for node in G.nodes:
        G.nodes[node]['duration'] = random.randint(1, 10)

    # Perform DFS traversal
    arrival_times = {}
    dfs_traversal(G, 1, [], 0, arrival_times)
    
    # Find the total time to reach node 7
    total_time = arrival_times[7]
    
    # Find the critical path
    critical_path = max(allTraversedPaths, key=lambda x: x['cost'])

    # Print results
    print("Total Time to Reach Node 7:", total_time)
    print("Critical Path:", critical_path['path'])
    print("Total Cost of Critical Path:", critical_path['cost'])
