using System.Linq;

//initialize reader to read the input file.
// StreamReader reader = new StreamReader("03_demo.txt");
StreamReader reader = new StreamReader("03_input.txt");

// initialize variables
string? line;
int half;
int doublePackedIndex;
int priority;
int prioritySum = 0;
int elfCounter = 0;
string[] group = new string[3];
int badgeSum = 0;

// read the input file
while ((line = reader.ReadLine()) != null)
{
    //Part 1
    // find the first character that is packed twice, by checking the characters in the first compartment, one by one, against the second compartment. 
    half = line.Length / 2;
    for (int i = 0; i < half; i++)
    {
        // if an item is not found after index 'half', the .IndexOf returns -1. This means it is not packed twice.
        if ((doublePackedIndex = line.IndexOf(line[i], half)) > -1)
        {
            // Console.WriteLine("Found double packed character {0} at index {1} in string {2}", line[doublePackedIndex], doublePackedIndex, line);

            // calculate the priority of the double packed item
            if ((priority = (int)line[doublePackedIndex]) < 97)
            {
                prioritySum += priority - 38;
            }
            else
            {
                prioritySum += priority - 96;
            }

            // Every bag has one double packed item, so stop after the first one is found.
            break;
        }
    }

    // Part 2
    // group the input in groups of 3, each group is a rucksack/string array of Elves with the same badge. 
    group[elfCounter] = line;

    // if we counted 3 elves, we can look for the badge.
    if (elfCounter == 2)
    {
        // met de intersect functie van de char array kan je de overeenkomstige karakters vinden.
        // find the common characters in the 3 rucksacks.
        char[] commonChars = group[0].ToCharArray().Intersect(group[1].ToCharArray()).ToArray();
        commonChars = commonChars.Intersect(group[2].ToCharArray()).ToArray();

        // we know there will only be one item in commonChars, so we can just take the first item.
        if ((priority = (int)commonChars[0]) < 97)
        {
            badgeSum += priority - 38;
        }
        else
        {
            badgeSum += priority - 96;
        }
    }

    // update elfcounter
    elfCounter++;
    elfCounter %= 3;
}

Console.WriteLine("Total score part 1: {0}", prioritySum);
Console.WriteLine("Total score part 1: {0}", badgeSum);


// wait for input before exiting
Console.WriteLine("Press enter to finish");
Console.ReadLine();

