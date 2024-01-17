using System.Text.RegularExpressions;

public class Signal
{
    public (int x, int y) position { get; set; }
    public (int x, int y) beacon { get; set; }
    public int distance { get; set; }

    public Signal((int x, int y) position, (int x, int y) beacon)
    {
        this.position = position;
        this.beacon = beacon;
        this.distance = Math.Abs(position.x - beacon.x) + Math.Abs(position.y - beacon.y);
    }
}

public class Map
{
    public List<List<string>> map { get; set; }
    public int[] boundaries { get; set; }
    public int rowLimit { get; set; }
    public int focusRow { get; set; }

    public Map(List<Signal> signals, int focusRow, int rowLimit)
    {
        this.boundaries = findMapBoundaries(signals, focusRow, rowLimit);
        Console.WriteLine("map boundaries: {0}", string.Join(", ", this.boundaries));
        Console.WriteLine("initializing map");
        this.map = initializeMap(this.boundaries);
        Console.WriteLine("adding signal ranges to map");
        addSignalRangestoMap(signals);
        Console.WriteLine("adding signals to map");
        addSignalsToMap(signals);
    }

    public static int[] findMapBoundaries(List<Signal> signals, int focusRow, int rowLimit)
    {
        int minX = signals.Min(s => s.position.x - s.distance);
        int maxX = signals.Max(s => s.position.x + s.distance);
        int minY = focusRow - rowLimit;
        int maxY = focusRow + rowLimit;
        return new int[4] { minX, maxX, minY, maxY };
    }

    public bool isInRange((int x, int y) position)
    {
        return position.x >= this.boundaries[0] && position.x <= this.boundaries[1]
            && position.y >= this.boundaries[2] && position.y <= this.boundaries[3];
    }

    public static List<List<string>> initializeMap(int[] boundaries)
    {
        List<List<string>> map = new List<List<string>>();
        for (int y = boundaries[2]; y <= boundaries[3]; y++)
        {
            List<string> row = new List<string>();
            for (int x = boundaries[0]; x <= boundaries[1]; x++)
            {
                row.Add(".");
            }
            map.Add(row);
        }
        return map;
    }

    public void addSignalRangestoMap(List<Signal> signals)
    {
        foreach (Signal signal in signals)
        {
            int y = signal.position.y;
            int x = signal.position.x - this.boundaries[0];
            for (int i = y - signal.distance; i <= y + signal.distance; i++)
            {
                if (i >= this.boundaries[2] && i <= this.boundaries[3])
                {
                    int offset = signal.distance - Math.Abs(i - y);
                    for (int j = x - offset; j <= x + offset; j++)
                    {
                        this.map[i - this.boundaries[2]][j] = "#";
                    }
                }
            }
        }
    }

    public void addSignalsToMap(List<Signal> signals)
    {
        foreach (Signal signal in signals)
        {
            if (isInRange(signal.position))
            {
                this.map[signal.position.y - this.boundaries[2]][signal.position.x - this.boundaries[0]] = "S";
            }
            if (isInRange(signal.beacon))
            {
                this.map[signal.beacon.y - this.boundaries[2]][signal.beacon.x - this.boundaries[0]] = "B";
            }
        }
    }

    public void drawMap()
    {
        foreach (List<string> row in this.map)
        {
            foreach (string sign in row)
            {
                Console.Write(sign);
            }
            Console.WriteLine();
        }
    }
}

public class Program
{
    public static List<Signal> readInput(string[] lines)
    {
        List<Signal> signals = new List<Signal>();
        string pattern = @"[x|y]=(-?\d+)";

        foreach (string line in lines)
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
        return signals;
    }

    public static void Main(string[] args)
    {
        string file = "15_demo.txt";
        if (args.Length == 0)
        {
            Console.WriteLine("No input file specified, running demo input: 15_demo.txt");
        }
        else
        {
            file = args[0];
        }

        // read the input file
        string[] lines = File.ReadAllLines(file);
        List<Signal> signals = readInput(lines);
        Console.WriteLine("Number of signals: {0}", signals.Count);

        // signals that have a beacon further away then their y distance to 10. 
        // their 'no beacon' ranges overlap with the mystery row
        int mysteryRow = file == "15_demo.txt" ? 10 : 2000000;
        List<Signal> signalsFiltered = signals.Where(s => Math.Abs(mysteryRow - s.position.y) <= s.distance).ToList();
        Console.WriteLine("Number of signals near mystery row: {0}", signalsFiltered.Count);

        // create a map with the signals
        Map map = new Map(signalsFiltered, mysteryRow, 1);
        if (file == "15_demo.txt") { map.drawMap(); }

        // count the number of points in the specified row that are in the signalranges
        int index = mysteryRow - map.boundaries[2];
        int count = map.map[index].Count(s => s == "#");

        Console.WriteLine("Total score part 1: {0}", count);


        // wait for input before exiting
        Console.WriteLine("Press enter to finish");
        Console.ReadLine();
    }
}

