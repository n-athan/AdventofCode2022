public class Program
{

    // public static bool ComparePair(int)

    // recursive function to change the input string to a List of ints/lists. 
    public static List<object> ParseLine(string line)
    {
        List<object> result = new List<object>();
        // exclude first and last character, they are always [ ]
        line = line.Substring(1, line.Length - 2);
        for (int i = 0; i < line.Length; i++)
        {
            if (line[i] == '[')
            {
                int start = i;
                int count = 1;
                while (count > 0)
                {
                    i++;
                    if (line[i] == '[')
                    {
                        count++;
                    }
                    else if (line[i] == ']')
                    {
                        count--;
                    }
                }
                // Console.WriteLine("Running on Substring: {0}", line.Substring(start,i-start+1));
                result.Add(ParseLine(line.Substring(start,i-start+1)));
            }
            else if (line[i] == ',' || line[i] == ']')
            {
                // do nothing
            }
            else
            {
                // add the number to the list
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
        // foreach (List<object> item in parsedLine)
        // {
        //     Console.WriteLine(item.Count);
        // }

        // Console.WriteLine("Total score part 1: {0}",);
        // Console.WriteLine("Total score part 2: {0}",);


        // wait for input before exiting
        Console.WriteLine("Press enter to finish");
        Console.ReadLine();
    }
}

