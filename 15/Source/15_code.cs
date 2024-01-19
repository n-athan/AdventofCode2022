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

    public HashSet<(int x, int y)> getOuterEdge((int min, int max) limit)
    {
        HashSet<(int x, int y)> outerEdge = new HashSet<(int x, int y)>();
        int x = this.position.x;
        int y = this.position.y;
        int distance = this.distance;

        for (int i = 0; i <= distance+1; i++)
        {
            int offset = distance - i;
            if (isInLimit((x - offset -1, y - i), limit)) { outerEdge.Add((x - offset -1, y - i)); }
            if (isInLimit((x - offset -1, y + i), limit)) { outerEdge.Add((x - offset -1, y + i)); }
            if (isInLimit((x + offset + 1, y + i), limit)) { outerEdge.Add((x + offset +1, y + i)); }
            if (isInLimit((x + offset + 1, y - i), limit)) { outerEdge.Add((x + offset +1, y - i)); }
        }

        return outerEdge;
    }

    public static bool isInLimit((int x, int y) point,(int min, int max) limit)
    {
        return point.x >= limit.min && point.x <= limit.max && point.y >= limit.min && point.y <= limit.max;
    }

    public bool isInRange((int x, int y) point)
    {
        int distanceToPoint = Math.Abs(point.x - this.position.x) + Math.Abs(point.y - this.position.y);

        return distanceToPoint <= this.distance;
    }
}

public class Map
{
    public char[,] map { get; set; }
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

    public static char[,] initializeMap(int[] boundaries)
    {
        int rows = boundaries[3] - boundaries[2] + 1;
        int cols = boundaries[1] - boundaries[0] + 1;

        char[,] map = new char[rows, cols];

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
                            this.map[i - this.boundaries[2], j] = '#';
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
                this.map[signal.position.y - this.boundaries[2], signal.position.x - this.boundaries[0]] = 'S';
            }
            if (isInRange(signal.beacon))
            {
                this.map[signal.beacon.y - this.boundaries[2], signal.beacon.x - this.boundaries[0]] = 'B';
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

    public int countItemsInRow(int rowIndex, char itemToCount)
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

    public int findItemInRow(int rowIndex, char itemToFind)
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
        int count = map.countItemsInRow(index, '#');

        return count;
    }

    // part 2
    public static long findTuningFrequency(List<Signal> signals, (int min, int max) limit, bool debug = false)
    {   // er is precies 1 punt dat buiten alle ranges valt.
        // dat ligt daarom op de rand van een aantal ranges.
        // beperk de zoekruimte tot de rand van de ranges.

        // set van alle punten die mogelijk de juiste zijn.
        HashSet<(int, int)> possibleLocations = new HashSet<(int, int)>();
        foreach (Signal signal in signals)
        {
            // vind alle punten die net buiten het bereik van de sensor liggen.
            HashSet<(int, int)> signalRange = signal.getOuterEdge(limit);
            // voeg deze toe aan de set van mogelijke punten.
            possibleLocations.UnionWith(signalRange);
        }

        Console.WriteLine("number of possible locations: {0}", possibleLocations.Count);

        // voor elke mogelijke locatie, kijk of deze aan alle voorwaarden voldoet.
        foreach ((int x, int y) location in possibleLocations)
        {
            // check of de locatie in de range van een sensor ligt.
            // dan is dit niet de juiste locatie.
            bool inRange = false;
            foreach (Signal signal in signals)
            {
                if (signal.isInRange(location))
                {
                    inRange = true;
                    break;
                } 
            }

            // als de locatie niet in de range van alle sensors ligt, dan is dit de juiste locatie.
            if (!inRange)
            {
                Console.WriteLine("found location: {0},{1}", location.x, location.y);
                long xL = location.x;
                long yL = location.y;
                long frequency = xL*4000000 + yL;
                return frequency;
            } 
        }

        return -1;        
    }

    public static void Main(string[] args)
    {
        string file = "15_input.txt";

        // read the input file
        string[] lines = File.ReadAllLines(file);
        List<Signal> signals = readInput(lines);

        int count = findBeaconsinRow(signals, 2000000);
        Console.WriteLine("Number of signals near mystery row: {0}", count);

        long frequency = findTuningFrequency(signals, (0, 4000000));

        Console.WriteLine("Tuning frequency: {0}", frequency);

        // wait for input before exiting
        Console.WriteLine("Press enter to finish");
        Console.ReadLine();
    }
}

