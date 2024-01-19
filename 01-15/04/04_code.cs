//initialize reader to read the input file.
// StreamReader reader = new StreamReader("04_demo.txt");
StreamReader reader = new StreamReader("04_input.txt");

// initialize variables
string? line;
int sillyAssignmentCount = 0; 
int overlapAssignmentCount = 0;

// read the input file
while ((line = reader.ReadLine()) != null)
{
    //Part 1
    // split the line into indexes
    string[] words = line.Split(new char[] {',', '-'});  
    int[] locations = Array.ConvertAll(words, int.Parse);

    // Method 1 Compare starts and ends
    // if ((locations[0] <= locations[2] && locations[3] <= locations[1])
    //     || (locations[2] <= locations[0] && locations[1] <= locations[3])){
    //     sillyAssignmentCount++;
    // }

    // Method 2 Artithmetic
    // verschillen (start-start en end-end) zijn altijd van ongelijk teken, of 0. Dus vermenigvuldiging is altijd negatief of 0.
    if ((locations[0]-locations[2])*(locations[1]-locations[3]) <= 0){
        sillyAssignmentCount++;
    }

    // Part 2
    // Method 1 Compare starts and ends
    if ((locations[0] >= locations[2] && locations[0] <= locations[3])
        || (locations[2] >= locations[0] && locations[2] <= locations[1])){
        overlapAssignmentCount++;
    }
    
}

Console.WriteLine("Total score part 1: {0}", sillyAssignmentCount);
Console.WriteLine("Total score part 2: {0}", overlapAssignmentCount);


// wait for input before exiting
Console.WriteLine("Press enter to finish");
Console.ReadLine();

