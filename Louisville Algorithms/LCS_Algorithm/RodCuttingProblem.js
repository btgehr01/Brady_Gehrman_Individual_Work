const main = (priceArray, lengthOfRod) => {
  let maxPrice = [];
  maxPrice[0] = 0;
  //for loop to fill in the maxPrice array
  for (let i = 1; i < lengthOfRod + 1; i++) {
    let maxValue = 0;
    //for loop to find the maxValue for rod of length i
    for (let j = 0; j < i; j++) {
      //calculate each rod cut value for each combination of 2 lengths
      const priceOfCombination = priceArray[j + 1] + maxPrice[i - (j + 1)];
      //sets maxValue if a more optimized cut is found
      maxValue = Math.max(maxValue, priceOfCombination);
    }
    maxPrice[i] = maxValue;
  }
  console.log(maxPrice);
  return maxPrice;
};

//call main function
main([0, 1, 5, 8, 9, 10, 17, 17, 20, 24, 30], 10);
//price array from rod of size 0 - n as input
