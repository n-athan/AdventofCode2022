using System.Text.RegularExpressions;

public class Range {
    public int start {get; set;}
    public int end {get; set;}

    public Range(int start, int end) {
        this.start = start;
        this.end = end;
    }

    public bool isInRange(int value) {
        return value >= start && value <= end;
    }

    public bool isInRange(Range range) {
        return isInRange(range.start) || isInRange(range.end);
    }

    public void Extend(Range range) {
        if (range.start < start) {
            start = range.start;
        }
        if (range.end > end) {
            end = range.end;
        }
    }
}

public class Signal {
    public (int x, int y) position {get; set;}
    public (int x, int y) beacon {get; set;}
    public int distance {get; set;}

    public Signal((int x, int y) position, (int x, int y) beacon) {
        this.position = position;
        this.beacon = beacon;
        this.distance = Math.Abs(position.x - beacon.x) + Math.Abs(position.y - beacon.y);
    }
}

public class Program
{

    public static void Main(string[] args)
    {
        string file = "15_demo.txt";
        if (args.Length == 0) {
            Console.WriteLine("No input file specified, running demo input: 15_demo.txt");
        } else {
            file = args[0];
        }
        //initialize reader to read the input file.
        StreamReader reader = new StreamReader(file);

        // initialize variables
        string? line;
        List<List<Range>> map = new List<List<Range>>();
        List<Signal> signals = new List<Signal>();

        // read the input file
        string pattern = @"[x|y]=(-?\d+)"; 
        while ((line = reader.ReadLine()) != null)
        {
            MatchCollection matches = Regex.Matches(line, pattern);
            List<int> numbers = new List<int>();
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                // groups[1] now contains the number after 'x=' or 'y='
                numbers.Add(int.Parse(groups[1].Value));
            }
            signals.Add(new Signal((numbers[0], numbers[1]), (numbers[2], numbers[3])));
        }
        reader.Close();

        // find the boundaries of the map
        int minX = signals.Min(s => s.position.x - s.distance);
        int maxX = signals.Max(s => s.position.x + s.distance);
        int minY = signals.Min(s => s.position.y - s.distance);
        int maxY = signals.Max(s => s.position.y + s.distance);
        Console.WriteLine("Map boundaries: {0}, {1}, {2}, {3}", minX, maxX, minY, maxY);

        // initialize the map with the right amount of rows. 
        // map[0] is the row with y = minY
        for (int y = minY; y <= maxY; y++) {
            List<Range> row = new List<Range>();
            map.Add(row);
        }

        // map[10].Add(new Range(minX, maxX));
        // add the signalranges to the map
        foreach (Signal s in signals){
            int y = s.position.y;
            int x = s.position.x;
            for (int i = y-s.distance; i <= y+s.distance; i++) {
                int offset = s.distance - Math.Abs(i-y);
                map[i-minY].Add(new Range(x-offset, x+offset));
            }
        }

        // merge overlapping ranges
        foreach (List<Range> row in map) {
            for (int i = 0; i < row.Count; i++) {
                for (int j = i+1; j < row.Count; j++) {
                    if (row[i].isInRange(row[j])) {
                        row[i].Extend(row[j]);
                        row.RemoveAt(j);
                        j--;
                    }
                }
            }
        }

        //draw the map
        foreach (List<Range> row in map) {
            for (int x = minX; x <= maxX; x++) {
                // check if the point is in any range in the row
                bool inRange = false;
                foreach (Range range in row) {
                    if (range.isInRange(x)) {
                        inRange = true;
                        break;
                    }
                }
                if (inRange) {
                    Console.Write("#");
                } else {
                    Console.Write(".");
                }
            }
            Console.WriteLine();
        }

        // Console.WriteLine("Total score part 1: {0}",);
        // Console.WriteLine("Total score part 2: {0}",);


        // wait for input before exiting
        Console.WriteLine("Press enter to finish");
        Console.ReadLine();
    }
}

