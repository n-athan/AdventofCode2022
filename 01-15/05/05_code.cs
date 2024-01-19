using System.IO;
using System.Text.RegularExpressions;

//initialize reader to read the input file.
// StreamReader reader = new StreamReader("05_demo.txt");
StreamReader reader = new StreamReader("05_input.txt");

// initialize variables
string? line;

// split the file into stacks and instructions
string pattern = @"^ \d";
List<string> stackStart = new List<string>();
int stackCount = 0;
List<string> instructions = new List<string>();

while ((line = reader.ReadLine()) != null)
{
    if (Regex.IsMatch(line, pattern))
    {
        stackCount = int.Parse(line.Substring(line.Length - 2).Trim());
    }
    else if (stackCount == 0)
    {
        stackStart.Add(line);
    }
    else if (Regex.IsMatch(line, "move"))
    {
        instructions.Add(line);
    }
}

reader.Close();

// create stacks as proper objects, not string List.
Stack<string>[] stacks = new Stack<string>[stackCount];
for (int i = 0; i < stackCount; i++)
{
    stacks[i] = new Stack<string>();
    for (int j = stackStart.Count - 1; j >= 0; j--)
    {
        string crate = stackStart[j].Substring(1 + (4 * i), 1);
        if (crate == " ") { continue; }
        stacks[i].Push(crate);
    }
}

// process instructions
foreach (string instruction in instructions)
{
    string[] words = instruction.Split(" ");
    for (int i = 0; i < int.Parse(words[1]); i++)
    {
        int stackFrom = int.Parse(words[3]);
        int stackTo = int.Parse(words[5]);
        string crate = stacks[stackFrom - 1].Pop();
        stacks[stackTo - 1].Push(crate);
    }
}

// show results
string result = "";
for (int i = 0; i < stackCount; i++)
{
    result += stacks[i].Pop();
}
Console.WriteLine("Result part 1: {0}", result);

//////////////////
/// Part 2
/// Reinitialize stacks
//////////////////

stacks = new Stack<string>[stackCount];
for (int i = 0; i < stackCount; i++)
{
    stacks[i] = new Stack<string>();
    for (int j = stackStart.Count - 1; j >= 0; j--)
    {
        string crate = stackStart[j].Substring(1 + (4 * i), 1);
        if (crate == " ") { continue; }
        stacks[i].Push(crate);
    }
}

// process instructions
foreach (string instruction in instructions)
{
    string[] words = instruction.Split(" ");
    Stack<string> tempStack = new Stack<string>();
    for (int i = 0; i < int.Parse(words[1]); i++)
    {
        int stackFrom = int.Parse(words[3]);
        tempStack.Push(stacks[stackFrom - 1].Pop());
    }
    for (int i = 0; i < int.Parse(words[1]); i++)
    {
        int stackTo = int.Parse(words[5]);
        stacks[stackTo - 1].Push(tempStack.Pop());
    }
}

// show results
result = "";
for (int i = 0; i < stackCount; i++)
{
    result += stacks[i].Pop();
}
Console.WriteLine("Result part 2: {0}", result);


// wait for input before exiting
Console.WriteLine("Press enter to finish");
Console.ReadLine();

