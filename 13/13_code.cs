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
    public static int ComparePair(Packet a, Packet b)
    {
        if (a.isInt && b.isInt)
        {
            if (a.value == b.value)
            {
                //Console.WriteLine("Equal ints {0}, {1}", a.value, b.value);
                return 0;
            }
            else if (a.value > b.value)
            {
                //Console.WriteLine("Left greater {0}, {1}", a.value, b.value);
                return -1;
            }
            else
            {
                //Console.WriteLine("Right greater {0}, {1}", a.value, b.value);
                return 1;
            }
        }
        else if (!a.isInt && !b.isInt)
        {
            if (a.list is null || b.list is null)
            {
                //Console.WriteLine("Null list, a b list");
                return -1;
            }
            else
            {
                int result = 0;
                int i = 0;
                while (result == 0 && i < a.list.Count)
                {
                    if (i == b.list.Count) {
                        //Console.WriteLine("Left longer");
                        result = -1;
                        break;
                    } else {
                        result = ComparePair(a.list[i], b.list[i]);
                        i++;
                    }
                }
                return result;
            }
        }
        else if (a.isInt && !b.isInt)
        {
            if (b.list is null)
            {
                //Console.WriteLine("Null list b, a int");
                return -1;
            } else if (b.list.Count == 0) 
            {
                //Console.WriteLine("Empty list b, a int");
                return -1;
            }
            else
            {
                //Console.WriteLine("Left int, right list");
                return ComparePair(a, b.list[0]);
            }
        } 
        else if (!a.isInt && b.isInt)
        {
            //Console.WriteLine("a list count {0}", a.list.Count);
            if (a.list is null)
            {
                //Console.WriteLine("Null list a, b int");
                return -1;
            }
            else if (a.list.Count == 0) 
            {
                //Console.WriteLine("Empty list a, b int");
                return 1;
            } else
            {
                //Console.WriteLine("Left list, right int");
                return ComparePair(a.list[0], b);
            }
        }
        else
        {
            //Console.WriteLine("Should not happen");
            return 0;
        }
        return 1; 
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
            int result = ComparePair(parsedLines[i], parsedLines[i + 1]);
            // Console.WriteLine("Index: {0}, result {1}", (i) / 2 + 1, result);
            if (result >= 0)
            {
                score+= (i)/2+1;
            }
        }

        Console.WriteLine("Total score part 1: {0}",score);
        // Console.WriteLine("Total score part 2: {0}",);


        // wait for input before exiting
        Console.WriteLine("Press enter to finish");
        Console.ReadLine();
    }
}

