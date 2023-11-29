using System.Text.RegularExpressions;

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
        List<List<string>> map = new List<List<string>>();
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
            List<string> row = new List<string>();
            for (int x = minX; x <= maxX; x++) {
                row.Add(".");
            }
            map.Add(row);
        }

        // add the signalranges to the map
        foreach (Signal s in signals){
            int y = s.position.y;
            int x = s.position.x-minX;
            for (int i = y-s.distance; i <= y+s.distance; i++) {
                int offset = s.distance - Math.Abs(i-y);
                for (int j = x-offset; j <= x+offset; j++) {
                    map[i-minY][j] = "#";
                }
            }
        }

        // add the sensors and beacons to the map
        foreach (Signal s in signals){
            map[s.position.y-minY][s.position.x-minX] = "S";
            map[s.beacon.y-minY][s.beacon.x-minX] = "B";
        }

        //draw the map
        if (file == "15_demo.txt") {
            foreach (List<string> row in map) {
                foreach (string sign in row) {                
                    Console.Write(sign);
                }
                Console.WriteLine();
            }
        }

        // count the number of points in the specified row that are in the signalranges
        int index = file == "15_demo.txt" ? 10-minY : 2000000-minY;
        int count = map[index].Count(s => s == "#");

        Console.WriteLine("Total score part 1: {0}", count);
        // Console.WriteLine("Total score part 2: {0}",);


        // wait for input before exiting
        Console.WriteLine("Press enter to finish");
        Console.ReadLine();
    }
}

