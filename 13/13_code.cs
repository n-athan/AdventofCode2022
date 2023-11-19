public class Packet {
    public bool isInt { get; set; }
    public int? value { get; set; }
    public List<Packet>? list { get; set; }

    public Packet(int _value) {
        isInt = true;
        value = _value;
    }

    public Packet(List<Packet> _list) {
        isInt = false;
        list = _list;
    }
}

public class Program
{
    public static bool ComparePair(Packet a, Packet b)
    {
        if (a.isInt && b.isInt)
        {
            return a.value <= b.value;
        }
        else if (!a.isInt && !b.isInt)
        {
            if (a.list is null || b.list is null || (a.list.Count > b.list.Count))
            {
                return false;
            }
            else
            {
                bool result = true;
                for (int i = 0; i < a.list.Count; i++)
                {
                    result = result && ComparePair(a.list[i], b.list[i]);
                }
                return result;
            }
        }
        else if (a.isInt && !b.isInt)
        {
            if (b.list is null || b.list.Count != 1)
            {
                return false;
            }
            else
            {
                return ComparePair(a, b.list[0]);
            }
        } 
        else if (!a.isInt && b.isInt)
        {
            if (a.list is null || a.list.Count != 1)
            {
                return false;
            }
            else
            {
                return ComparePair(a.list[0], b);
            }
        }
        else
        {
            return false;
        }
    }

    // recursive function to change the input string to a List of ints/lists. 
    public static Packet ParseLine(string line)
    {
        Packet result = new Packet(new List<Packet>());
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
                result.list.Add(ParseLine(line.Substring(start, i - start + 1)));
            }
            else if (line[i] == ',' || line[i] == ']')
            {
                // do nothing
            }
            else
            {
                // add the number to the list
                // todo as int. 
                result.list.Add(new Packet(int.Parse(line[i].ToString())));
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
        List<Packet> parsedLines = new List<Packet>();

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
        int score = 0;
        for (int i = 0; i < parsedLines.Count - 1; i += 2)
        {
            bool result = ComparePair(parsedLines[i], parsedLines[i + 1]);
            //log
            Console.WriteLine(result);
            if (result)
            {
                score+= (i)/2+1;
                Console.WriteLine("Index: {0}", (i) / 2 + 1);
            }
        }

        Console.WriteLine("Total score part 1: {0}",score);
        // Console.WriteLine("Total score part 2: {0}",);


        // wait for input before exiting
        Console.WriteLine("Press enter to finish");
        Console.ReadLine();
    }
}

