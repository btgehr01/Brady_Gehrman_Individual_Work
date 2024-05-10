class Node {
  constructor(parent, stateArray, depth) {
    this.stateArray = stateArray;
    this.parent = parent;
    this.depth = depth;
  }
  getStateArray() {
    return this.stateArray;
  }
  getParent() {
    return this.parent;
  }
  getDepth() {
    return this.depth;
  }
}

//Global variables:
const goalState = [1, 2, 3, 4, 5, 6, 7, 8, 0];
let openListPriorityQueue = [];
let closedList = [];
let solved = false;
let numSearch = 0;
const moveOptions = {
  0: [1, 3],
  1: [0, 2, 4],
  2: [1, 5],
  3: [4, 0, 6],
  4: [3, 5, 1, 7],
  5: [4, 2, 8],
  6: [7, 3],
  7: [6, 8, 4],
  8: [7, 5],
};

const sortFunctionForOpenList = (node1, node2) => {
  const cost1 = getManhattanCost(node1.getStateArray()) + node1.getDepth();
  const cost2 = getManhattanCost(node2.getStateArray()) + node2.getDepth();
  if (cost1 < cost2) {
    return -1;
  } else if (cost1 > cost2) {
    return 1;
  } else {
    return 0;
  }
};

const getEmptyTileLocation = (stateArray) => {
  return stateArray.findIndex((element) => element === 0);
};

const swapTiles = (stateArray, i0, i1) => {
  //swap the tiles and return the new state
  const newStateArray = [].concat(stateArray);
  var temp = newStateArray[i0];
  newStateArray[i0] = newStateArray[i1];
  newStateArray[i1] = temp;
  return newStateArray;
};

const printBlankOrValueForTile = (valueInTile) => {
  if (valueInTile !== 0) {
    return valueInTile;
  } else {
    return " ";
  }
};

const printCurrentState = (stateArray) => {
  //print out the state table
  console.log(" ");
  console.log(
    " " +
      printBlankOrValueForTile(stateArray[0]) +
      " | " +
      printBlankOrValueForTile(stateArray[1]) +
      " | " +
      printBlankOrValueForTile(stateArray[2])
  );
  console.log("-----------");
  console.log(
    " " +
      printBlankOrValueForTile(stateArray[3]) +
      " | " +
      printBlankOrValueForTile(stateArray[4]) +
      " | " +
      printBlankOrValueForTile(stateArray[5])
  );
  console.log("-----------");
  console.log(
    " " +
      printBlankOrValueForTile(stateArray[6]) +
      " | " +
      printBlankOrValueForTile(stateArray[7]) +
      " | " +
      printBlankOrValueForTile(stateArray[8])
  );
  console.log(" ");
};

const getManhattanCost = (stateArray) => {
  var totalCost = 0;
  //loop through each element within the puzzle and increment the cost variable based on each elements Manhattan cost
  for (let i = 0; i < stateArray.length; i++) {
    var valueInTile = stateArray[i];
    if (valueInTile !== 0) {
      //decrement valueInTile so that it will match its goal's index of the array
      valueInTile--;
      //calculate the current tile's goal row and column
      var currentColumn = i % 3;
      var currentRow = Math.floor(i / 3);
      //calculate the valueInTile's goal row and column
      var goalColumn = valueInTile % 3;
      var goalRow = Math.floor(valueInTile / 3);
    } else {
      //calculate the current tile's goal row and column
      var currentColumn = i % 3;
      var currentRow = Math.floor(i / 3);
      var goalColumn = 2;
      var goalRow = 2;
    }
    //calculate the manhattanDistance
    var manhattanDistance =
      Math.abs(currentRow - goalRow) + Math.abs(currentColumn - goalColumn);
    //increment the total cost
    totalCost = totalCost + manhattanDistance;
  }
  return totalCost;
};

const getMoveOptions = (indexOfBlankSpace) => {
  return moveOptions[indexOfBlankSpace];
};

const isStateWithinClosedList = (puzzleState) => {
  //loop through the closed list and see if any state is equal to the input puzzleState array
  let isTheStateWithinClosedList = false;
  closedList.forEach((state) => {
    if (state.every((tile, i) => puzzleState[i] === tile)) {
      isTheStateWithinClosedList = true;
    }
  });
  return isTheStateWithinClosedList;
};

const expandNode = (node) => {
  //check to see if the current node is in the goal state [1, 2, 3, 4, 5, 6, 7, 8, 0], if so, set the solved global boolean to true
  if (node.getStateArray().every((tile, i) => goalState[i] === tile)) {
    solved = true;
    return;
  }
  //find the empty slot in the state array (index that contains a 0)
  const indexOfEmptySlot = getEmptyTileLocation(node.getStateArray());
  //get the move options for the current state array, through using the move options dictionary
  const neighbours = getMoveOptions(indexOfEmptySlot);
  //loop through each of the move options and find the new puzzle state if the move was simulated
  neighbours.forEach((neighbour) => {
    const puzzleState = swapTiles(
      node.getStateArray(),
      indexOfEmptySlot,
      neighbour
    );
    //check to make sure the new state array is not already within the closed list
    if (!isStateWithinClosedList(puzzleState)) {
      //create a new node and add it to the open list
      const newNode = new Node(node, puzzleState, node.getDepth() + 1);
      openListPriorityQueue.push(newNode);
    }
  });
};

const selectNextNode = () => {
  numSearch++;
  //sort the openlist based on the heuristic function, lowest (manhattan cost + depth)
  openListPriorityQueue.sort(sortFunctionForOpenList);
  //pull the first node out of the queue
  const nextNodeToExplore = openListPriorityQueue.shift();
  //push the stateArray of the removed node into the closed list, so we don't recalculate already expanded states
  closedList.push(nextNodeToExplore.getStateArray());
  return nextNodeToExplore;
};

const printSuccessfulSearchPath = (finalNode) => {
  let count = 1;
  console.log("Successful Search Path:");
  //print the final node's state
  printCurrentState(finalNode.getStateArray());
  finalNode = finalNode.getParent();
  //print all of the final node's parent's states
  while (finalNode !== null) {
    count++;
    console.log("     ^");
    console.log("     |");
    printCurrentState(finalNode.getStateArray());
    finalNode = finalNode.getParent();
  }
  console.log("nodes in search path", count);
};

const aStarSearch = (initialState) => {
  //create a root node for the initial state, with a null parent, and 0 depth
  const root = new Node(null, initialState, 0);
  //push the new root node onto the open list
  openListPriorityQueue.push(root);
  let nextNode = null;
  //while not solved select the next node to search and expand it
  while (!solved) {
    nextNode = selectNextNode();
    expandNode(nextNode);
  }
  //after the process is solved print the search path in the console
  printSuccessfulSearchPath(nextNode);
  console.log("numSearch", numSearch);
};

//set the initial state
const initialState = [5, 4, 1, 0, 2, 8, 3, 6, 7];
aStarSearch(initialState);
