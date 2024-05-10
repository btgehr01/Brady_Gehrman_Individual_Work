//node class
class Node {
  constructor(id) {
    this.id = id;
    this.adjacentNodes = [];
    this.vistited = false;
    this.predecessor = undefined;
  }

  addAdjacent(node, weight) {
    this.adjacentNodes.push({ node, weight });
  }
  getAdjacents() {
    return this.adjacentNodes;
  }
}

//digrapgh class
class Digraph {
  constructor() {
    this.nodes = [];
  }
  //adds node to the digraph
  addNode(node) {
    this.nodes.push(node);
  }
  //adds an edge to the digraph, add this connection to the source nodes ajacency list
  addEdge(source, destination, weight) {
    source.addAdjacent(destination, weight);
  }
}

//create a digraph object
const digraph = new Digraph();
//create a node object for each node in the graph
const nodeS = new Node("S");
const nodeA = new Node("A");
const nodeB = new Node("B");
const nodeC = new Node("C");
const nodeD = new Node("D");
const nodeT = new Node("T");

//add the nodes to the digraph object
digraph.addNode(nodeS);
digraph.addNode(nodeA);
digraph.addNode(nodeB);
digraph.addNode(nodeC);
digraph.addNode(nodeD);
digraph.addNode(nodeT);

//add edges soruced by nodeS
digraph.addEdge(nodeS, nodeC, 4);
digraph.addEdge(nodeS, nodeA, 2);

//add edges sourced by nodeA
digraph.addEdge(nodeA, nodeS, 2);
digraph.addEdge(nodeA, nodeB, 8);
digraph.addEdge(nodeA, nodeC, 15);
digraph.addEdge(nodeA, nodeD, 5);

//add edges sourced by nodeB
digraph.addEdge(nodeB, nodeA, 8);
digraph.addEdge(nodeB, nodeC, 2);
digraph.addEdge(nodeB, nodeD, 8);
digraph.addEdge(nodeB, nodeT, 8);

//add edges sourced by nodeC
digraph.addEdge(nodeC, nodeS, 4);
digraph.addEdge(nodeC, nodeA, 15);
digraph.addEdge(nodeC, nodeB, 2);
digraph.addEdge(nodeC, nodeD, 2);

//add edges sourced by nodeD
digraph.addEdge(nodeD, nodeA, 5);
digraph.addEdge(nodeD, nodeB, 8);
digraph.addEdge(nodeD, nodeT, 11);

//add edges sourced by nodeT
digraph.addEdge(nodeT, nodeB, 8);
digraph.addEdge(nodeT, nodeD, 11);

//sorting function that is used to sort the array of node objects
const sortNodesReverseAlphabetically = (nodeObject1, nodeObject2) => {
  if (nodeObject1.node.id > nodeObject2.node.id) {
    return -1;
  } else if (nodeObject1.node.id < nodeObject2.node.id) {
    return 1;
  } else {
    return 0;
  }
};

const dfs = (startingNode, finalNodeID) => {
  //create an array that will be used as a stack
  var stack = [];
  //create a count variable, that will keep track of the number of visited nodes
  var count = 0;
  //push the starting node onto the stack and mark it as visited
  startingNode.vistited = true;
  stack.push(startingNode);

  //while loop that will run until the stack is empty
  while (stack.length !== 0) {
    //increment count
    count++;
    //store the node at the start of the stack, and pop it
    var currentNode = stack[stack.length - 1];
    /*
    create an ajacency list that gathers the current node's adjacentNodes list, 
    then filter the list to only have unvisited nodes and then sort it reverse alphabetically via node ID,  
    */
    var ajacentcyList = currentNode
      .getAdjacents()
      .filter((nodeObject) => !nodeObject.node.vistited)
      .sort(sortNodesReverseAlphabetically);
    //check to see that the ajacentcyList has atleast one element in it
    if (ajacentcyList.length > 0) {
      //log out the predecessor --(weight of edge)-> visited node
      console.log(
        `${currentNode.id}  --${ajacentcyList[0].weight}->  ${ajacentcyList[0].node.id}`
      );
      //set the visit property to true for the visited node
      ajacentcyList[0].node.vistited = true;
      //check to see if the visited node is the final node, if so, log out the node count and return true, symbolizing that the function has found the final node
      if (ajacentcyList[0].node.id === finalNodeID) {
        console.log(
          `DFS visited ${count} nodes before reaching the final node`
        );
        return true;
      }
      //push the visited node to the stack
      stack.push(ajacentcyList[0].node);
    } else {
      stack.pop();
    }
  }
  //return false if the final node is never encountered, symbolizing that the function hasn't found the final node
  return false;
};

//call the dfs function that is defined above
var startTime = new Date().getTime();
const foundTheFinalNode = dfs(nodeS, nodeT.id);
var endTime = new Date().getTime();
console.log("Execution time: ", endTime - startTime, " ms");
