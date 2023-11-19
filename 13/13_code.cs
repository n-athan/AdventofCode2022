public class Program
{

    public static bool ComparePair(List<object> a, List<object> b)
    {
        Console.WriteLine(a.Count);
        if (a.Count > b.Count)
        {
            // Console.WriteLine("a: {0}, b {1}", a.Count , b.Count);
            return false;
        }
        else
        {
            // Console.WriteLine("a: {0}, b {1}", a.Count , b.Count);
            // return true;
            for (int i = 0; i < a.Count; i++)
            {
                // TODO 
                // if (a[i] is int && ) {
                //     List<object> _a new
                // }
                Console.WriteLine(a);
                List<object> _a = a[i] as List<object>;
                List<object> _b = b[i] as List<object>;
                return ComparePair(_a, _b);
            }
        }
        return true;
    }

    // recursive function to change the input string to a List of ints/lists. 
    public static List<object> ParseLine(string line)
    {
        List<object> result = new List<object>();
        // exclude first and last character, they are always [ ]
        line = line.Substring(1, line.Length - 2);
        for (int i = 0; i < line.Length; i++)
        {
            // get nested array. match [ with ]
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
                result.Add(ParseLine(line.Substring(start, i - start + 1)));
            }
            else if (line[i] == ',' || line[i] == ']')
            {
                // do nothing
            }
            else
            {
                // add the number to the list
                // todo as int. 
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
        List<List<object>> parsedLines = new List<List<object>>();

        // read the input file
        while ((line = reader.ReadLine()) != null)
        {
            if (line == "")
            {
                continue;
            }
            // parse the line
            parsedLines.Add(ParseLine(line));
        }
        reader.Close();

        // compare parsedLines pairwise
        for (int i = 0; i < parsedLines.Count - 1; i += 2)
        {
            bool result = ComparePair(parsedLines[i], parsedLines[i + 1]);
            //log
            Console.WriteLine(result);
        }

        // Console.WriteLine("Total score part 1: {0}",);
        // Console.WriteLine("Total score part 2: {0}",);


        // wait for input before exiting
        Console.WriteLine("Press enter to finish");
        Console.ReadLine();
    }
}

