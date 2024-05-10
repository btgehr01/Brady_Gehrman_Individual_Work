const calculateLongestSubsequence = (index1, index2, sequence1, sequence2) => {
  if (sequence1.length === index1 || sequence2.length === index2) {
    //end of one or both of the strings, return 0 because no additional length was added down this path
    return 0;
  } else if (sequence1[index1] === sequence2[index2]) {
    //specified indexes i,j of string1, string2 have an equal character, add 1 to the count and continue with next indexes of both strings
    return (
      1 +
      calculateLongestSubsequence(index1 + 1, index2 + 1, sequence1, sequence2)
    );
  } else {
    //index i,j of string1,string2 don't match return the max count/size of the subsequences found within the next possible positions fot both strings
    return Math.max(
      calculateLongestSubsequence(index1, index2 + 1, sequence1, sequence2),
      calculateLongestSubsequence(index1 + 1, index2, sequence1, sequence2)
    );
  }
};

//start calculateLongestSubsequence at index i=0 and j=0
const longestSubsequence = calculateLongestSubsequence(
  0,
  0,
  "abcdbab",
  "bdcaba"
);
console.log(longestSubsequence);
