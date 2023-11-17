public class Program
{

    // public static bool ComparePair(int)

    // recursive function to change the input string to a List of ints/lists. 
    public static List<object> ParseLine(string line)
    {
        List<object> result = new List<object>();
        // exclude first and last character, they are always [ ]
        line = line.Substring(1, line.Length - 2);
        Console.WriteLine("input trimmed: {0}", line);
        for (int i = 0; i < line.Length; i++)
        {
            Console.WriteLine("first char: {0}", line.Substring(i, 1));
            if (line[i] == '[')
            {
                // find closing bracket
                // Console.WriteLine(line.Substring(i, line.IndexOf("]") - i));
                // Console.WriteLine("first char: {0}", line.Substring(i));
                int j = i;
                int matchingBracketIndex = line.Substring(i).IndexOf("]")+i;
                // log j an dmatchingBracketIndex
                Console.WriteLine("start j: {0}, matchingBracketIndex: {1}", j, matchingBracketIndex);
                while (j > 0)
                {
                    j = line.Substring(j, matchingBracketIndex - j).IndexOf("[");
                    if (matchingBracketIndex < line.Length - 1)
                    {
                        matchingBracketIndex = line.Substring(matchingBracketIndex + 1).IndexOf("]");
                    }
                    Console.WriteLine("temp j: {0}, matchingBracketIndex: {1}", j, matchingBracketIndex);

                }
                // call ParseLine on the Substring
                Console.WriteLine("Running on Substring: {0}", line.Substring(i, matchingBracketIndex - i + 1));
                result.Add(ParseLine(line.Substring(i, matchingBracketIndex - i + 1)));
                i = matchingBracketIndex;
            }
            else if (line[i] == ',' || line[i] == ']')
            {
                // do nothing
            }
            else
            {
                // add the number to the list
                Console.WriteLine("added: {0}", line[i]);
                result.Add(line[i]);
            }
        }
        return result;
    }

    public static void Main(string[] args)
    {
        string file = "13_demo.txt";
        if (args.Length == 0)
        {
            Console.WriteLine("No input file specified, running demo input: 13_demo.txt");
        }
        else
        {
            file = args[0];
        }
        //initialize reader to read the input file.
        StreamReader reader = new StreamReader(file);
        string? line;

        // initialize variables
        List<object> parsedLine = new List<object>();

        // read the input file
        while ((line = reader.ReadLine()) != null)
        {
            if (line == "")
            {
                continue;
            }
            // parse the line
            parsedLine.Add(ParseLine(line));
        }
        reader.Close();

        // show the number of items in each element of parsedLine
        foreach (List<object> item in parsedLine)
        {
            Console.WriteLine(item.Count);
        }

        // Console.WriteLine("Total score part 1: {0}",);
        // Console.WriteLine("Total score part 2: {0}",);


        // wait for input before exiting
        Console.WriteLine("Press enter to finish");
        Console.ReadLine();
    }
}

