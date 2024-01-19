abstract public class Packet {
    abstract public List<Packet> list { get; set;}
    abstract public int value { get; set;}
    abstract public string? basestring { get; set;}

}

public class ListPacket : Packet {
    
    public override List<Packet> list { get; set;}
    public override int value { get; set;}
    public override string? basestring { get; set;}

    public ListPacket(List<Packet> list) {
        this.list = list;
    }

    public ListPacket(List<Packet> list, string basestring) {
        this.list = list;
        this.basestring = basestring;
    }
}

public class IntPacket : Packet {
    
    public override int value { get; set;}
    public override List<Packet> list { get; set;}    
    public override string? basestring { get; set;}

    public IntPacket(int value) {
        this.value = value;
        this.list = new List<Packet>();
    }
}


public class Program
{
    public static int ComparePair(Packet a, Packet b)
    {
        if (typeof(IntPacket) == a.GetType() && typeof(IntPacket) == b.GetType())
        {
            // Console.WriteLine("Left int {0}, right int {1}", a.value, b.value);
            if (a.value == b.value)
            {
                // Console.WriteLine("Equal ints {0}, {1}", a.value, b.value);
                return 0;
            }
            else if (a.value > b.value)
            {
                // Console.WriteLine("Left greater {0}, {1}", a.value, b.value);
                return -1;
            }
            else
            {
                // Console.WriteLine("Right greater {0}, {1}", a.value, b.value);
                return 1;
            }
        }
        else if (typeof(ListPacket) == a.GetType() && typeof(ListPacket) == b.GetType())
        {
           // Console.WriteLine("Left list count: {0}, right list count:{1}", a.list.Count, b.list.Count);
            if (a.list.Count == 0)
                {
                   // Console.WriteLine("Empty list a, b list");
                    return 1;
                }
            int result = 0;
            int i = 0;
            while (result == 0 && i < a.list.Count)
            {
                if (i == b.list.Count) {
                    // Console.WriteLine("Left longer");
                    result = -1;
                    break;
                } else {
                    result = ComparePair(a.list[i], b.list[i]);
                    i++;
                }
            }
            return result;            
        }
        else if (a.GetType() != b.GetType())
        {
            if (b.GetType() == typeof(ListPacket)) {
                if (b.list.Count == 0)
                {
                    // Console.WriteLine("Empty list b, a int");
                    return -1;
                }
                else
                {
                    // Console.WriteLine("Left int, right list");
                     int r = ComparePair(a, b.list[0]);
                    if (r == 0) {
                        return 1;
                    } else {
                        return r;
                    }
                }
            } else {
                if (a.list.Count == 0)
                {
                    // Console.WriteLine("Empty list a, b int");
                    return 1;
                }
                else
                {
                    // Console.WriteLine("Left list, right int");
                    return ComparePair(a.list[0], new ListPacket(new List<Packet>(){b}));
                }
            }            
        } 
        else
        {
           // Console.WriteLine("Should not happen");
            return 0;
        }
    }

    // recursive function to change the input string to a List of ints/lists. 
    public static ListPacket ParseLine(string line)
    {
        ListPacket result = new ListPacket(new List<Packet>(), line);
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
                result.list.Add(ParseLine(line.Substring(start, i - start + 1)));
            }
            else if (line[i] == ',' || line[i] == ']')
            {
                // do nothing
            }
            else
            {
                // add the number to the list, can be multidecimal. End is a comma.
                int start = i;
                while (i < line.Length && line[i] != ',' && line[i] != ']')
                {
                    i++;
                }           
                result.list.Add(new IntPacket(int.Parse(line.Substring(start, i - start))));
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
        List<ListPacket> parsedLines = new List<ListPacket>();

        // read the input file
        while ((line = reader.ReadLine()) != null)
        {
            if (line == "")
            {
                continue;
            }
            // parse the line
            var parsedLine = ParseLine(line);
            parsedLines.Add(parsedLine);
        }
        reader.Close();
        Console.WriteLine("Parsed {0} lines", parsedLines.Count);

        // compare parsedLines pairwise
        int score = 0;
        for (int i = 0; i < parsedLines.Count - 1; i += 2)
        {
            int result = ComparePair(parsedLines[i], parsedLines[i + 1]);
            // Console.WriteLine("Index: {0}, result {1}", (i) / 2 + 1, result);
            if (result >= 0)
            {
                // Console.WriteLine("{0}", (i)/2+1);
                score+= (i)/2+1;
            }
        }

        Console.WriteLine("Total score part 1: {0}",score);

        // part 2
        // add dividers
        List<string> extralines = new List<string>(){"[[2]]","[[6]]"};
        foreach (var l in extralines)
        {
            var parsedLine = ParseLine(l);
            parsedLines.Add(parsedLine);
        }

        // sorting of all lines
        parsedLines.Sort((a, b) => ComparePair(a, b));
        parsedLines.Reverse();

        for (int i = 0; i < parsedLines.Count; i++){
            Console.WriteLine(parsedLines[i].basestring);
        }
        int d2 = parsedLines.IndexOf(parsedLines.Find(x => x.basestring == "[[2]]")) + 1;
        int d6 = parsedLines.IndexOf(parsedLines.Find(x => x.basestring == "[[6]]")) + 1;

        int decoderkey = d2 * d6;

        Console.WriteLine("Total score part 2: {0}", decoderkey);

        // wait for input before exiting
        Console.WriteLine("Press enter to finish");
        Console.ReadLine();
    }
}

