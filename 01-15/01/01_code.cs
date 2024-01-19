//initialize reader to read the input file.
// StreamReader reader = new StreamReader("01_demo.txt");
StreamReader reader = new StreamReader("01_input.txt");

//initialize Calories List
List<int> cals = new List<int>();

// loop over each line in the file
int sum = 0;
string? line;
while ((line = reader.ReadLine()) != null)
{
    // if it is a blank line, we have counted all the calories one elf carries. Add the calorie sum to the list.
    if (string.IsNullOrWhiteSpace(line)) {
        // add sum to list
        cals.Add(sum);
        sum = 0;
    } else {
        // if the line is not empty, add the amount of calories to the temporary sum that the elf carries.
        sum += Convert.ToInt32(line);
    }
}
// add last sum to list
cals.Add(sum);

// convert list to array and sort it
int[] calsArray = cals.ToArray();
Array.Sort(calsArray);
Array.Reverse(calsArray);

// Deel 1 
Console.WriteLine(calsArray[0]);

// Deel 2
int top3sum = calsArray[0..3].Sum();
Console.WriteLine(top3sum);

// wait for input before exiting
Console.WriteLine("Press enter to finish");
Console.ReadLine();