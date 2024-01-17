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
    // public List<List<string>> map { get; set; }
    public string[,] map { get; set; }
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

    public Map(List<Signal> signals, int focusRow, int rowLimit, int focusColumn, int columnLimit)
    {
        this.boundaries = findMapBoundaries(signals, focusRow, rowLimit, focusColumn, columnLimit);
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

    public static int[] findMapBoundaries(List<Signal> signals, int focusRow, int rowLimit, int focusColumn, int columnLimit)
    {
        int minX = focusColumn - columnLimit;
        int maxX = focusColumn + columnLimit;
        int minY = focusRow - rowLimit;
        int maxY = focusRow + rowLimit;
        return new int[4] { minX, maxX, minY, maxY };
    }

    public bool isInRange((int x, int y) position)
    {
        return position.x >= this.boundaries[0] && position.x <= this.boundaries[1]
            && position.y >= this.boundaries[2] && position.y <= this.boundaries[3];
    }

    public static string[,] initializeMap(int[] boundaries)
    {
        int rows = boundaries[3] - boundaries[2] + 1;
        int cols = boundaries[1] - boundaries[0] + 1;

        string[,] map = new string[rows, cols];

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
                        if (isInRange((j + this.boundaries[0], i)))
                        {
                            this.map[i - this.boundaries[2], j] = "#";
                        }
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
                this.map[signal.position.y - this.boundaries[2], signal.position.x - this.boundaries[0]] = "S";
            }
            if (isInRange(signal.beacon))
            {
                this.map[signal.beacon.y - this.boundaries[2], signal.beacon.x - this.boundaries[0]] = "B";
            }
        }
    }

    public void drawMap()
    {
        for (int i = 0; i < this.map.GetLength(0); i++)
        {
            for (int j = 0; j < this.map.GetLength(1); j++)
            {
                Console.Write(this.map[i, j]);
            }
            Console.WriteLine();
        }
    }

    public int countItemsInRow(int rowIndex, string itemToCount)
    {
        int count = 0;
        int rowLength = this.map.GetLength(1);

        for (int j = 0; j < rowLength; j++)
        {
            if (this.map[rowIndex, j] == itemToCount)
            {
                count++;
            }
        }
        return count;
    }

    public int findItemInRow(int rowIndex, string itemToFind)
    {
        int rowLength = this.map.GetLength(1);

        for (int j = 0; j < rowLength; j++)
        {
            if (map[rowIndex, j] == itemToFind)
            {
                return j;
            }
        }

        return -1; // Item not found
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

    // part 1
    public static int findBeaconsinRow(List<Signal> signals, int row, bool debug = false)
    {
        // signals that have a beacon further away then their y distance to the row we want to know. 
        // their 'no beacon' ranges overlap with the  row
        List<Signal> signalsFiltered = signals.Where(s => Math.Abs(row - s.position.y) <= s.distance).ToList();

        // create a map with the signals, only the  row.
        int limit = 0;
        if (debug) { limit = 15; }
        Map map = new Map(signalsFiltered, row, limit);

        if (debug) { map.drawMap(); }

        // count the number of points in the specified row that are in the signalranges
        int index = row - map.boundaries[2];
        int count = map.countItemsInRow(index, "#");

        return count;
    }

    // part 2
    public static int findTuningFrequency(List<Signal> signals, (int min, int max) limit, bool debug = false)
    {
        int mid = (limit.min + limit.max) / 2;
        int offset = limit.max - mid;
        int x = 0;
        int y = 0;
        Map map = new Map(signals, mid, offset, mid, offset);
        int rowLength = map.map.GetLength(1);

        if (debug) { map.drawMap(); }

        for (int i = limit.min; i <= limit.max; i++)
        {
            int ind = map.findItemInRow(i, null);
            if (ind != -1)
            {   
                x = ind + map.boundaries[0];
                y = i;
                break;
            }
        }
        int frequency = x * 4000000 + y;
        return frequency;
    }

    public static void Main(string[] args)
    {
        string file = "15_input.txt";

        // read the input file
        string[] lines = File.ReadAllLines(file);
        List<Signal> signals = readInput(lines);

        int count = findBeaconsinRow(signals, 2000000);
        Console.WriteLine("Number of signals near mystery row: {0}", count);

        int frequency = findTuningFrequency(signals, (0, 4000000));
        Console.WriteLine("Tuning frequency: {0}", frequency);

        // wait for input before exiting
        Console.WriteLine("Press enter to finish");
        Console.ReadLine();
    }
}

