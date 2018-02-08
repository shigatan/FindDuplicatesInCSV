# App allows to find rows in csv file with duplicate Values by specified column 

## Algorithm

#### Concerns
1. file size 
   - if file has small size => go though data and put data to hashmap to count occurrences of the target column values, output rows that we've met before
   - if file has large size => content of file might be not fitted to memory => external merge sort can be used to solve this problem
2. nature of input data
    - If values of key column are unique with high probability (like guids) =>  hashmap-based algorithm will be storing a key for each value, thus spanning a memory for the size of the whole input file.
    - If contents of key column are known to be limited to some known set of possible values => then even for large files, not fitted in memory, we can use hashmap
    

#### External merge sort
- Step1. divide file into chunks
- Step2. sort each chunk and save result of sorting into temp file
		 Sorting can be implemented using well known algorithm. I used Sorted Hash Map. Time complexity of insert entry to Sorted Hash Map will be O(log n). Extra space O(n).
- Step3. read limited piece of data from each chunk file. In my implementation each chunk has associsated queue and I read data to these queues.
- Step4. Compare first values in queues. Find queue with minimum value - target value
- Step5. Started from this queue push element to output while value are equal target value. When elements are finished grab new piece of data from file or move to next queue. 
- Step6. When all queues have no values to output (== targe value) go to step 4. 

### Tests:
- pass valid file 
- pass empty file
- pass non-existed file
- pass file without column
- pass large files
- pass file with unique value in column
- pass file with same value in column
