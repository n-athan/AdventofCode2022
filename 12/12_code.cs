public class Location
{
    public int x { get; set; }
    public int y { get; set; }
    public char elevation { get; set; }
    public int distance { get; set; }
    public List<Location> neighbours { get; set; } = new List<Location>();
    public bool visited { get; set; }

    public Location(int _x, int _y, char _elevation, int _distance)
    {
        x = _x;
        y = _y;
        elevation = _elevation;
        distance = _distance;
        visited = false;
    }

    public void checkNeighbour(int _x, int _y, int width, int height, List<Location> locations)
    {
        // check if the location is within the map
        if (_x >= 0 && _x <= width - 1 && _y >= 0 && _y <= height - 1)
        {
            // check if the location can be reached
            // neighbour can be reached, when elevation is lower, equeal or one higher than current location
            Location? neighbour = locations.Find(loc => loc.x == _x && loc.y == _y && loc.elevation >= this.elevation - 1);
            if (neighbour != null)
            {
                this.neighbours.Add(neighbour);
            }
        }
    }

    public void CheckNeighbours(int width, int height, List<Location> locations)
    {
        // loop over all neighbours
        checkNeighbour(this.x - 1, this.y, width, height, locations);
        checkNeighbour(this.x + 1, this.y, width, height, locations);
        checkNeighbour(this.x, this.y - 1, width, height, locations);
        checkNeighbour(this.x, this.y + 1, width, height, locations);
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        string file = "12_demo.txt";
        int part = 1;
        if (args.Length == 0)
        {
            Console.WriteLine("No input file specified, running demo input: 12_demo.txt");
        }
        else if (args.Length == 1)
        {
            part = int.Parse(args[0]);
        }
        else if (args.Length == 2)
        {
            part = int.Parse(args[0]);
            file = args[1];
        }
        //initialize reader to read the input file.
        StreamReader reader = new StreamReader(file);

        // initialize variables
        string? line;
        List<char[]> map = new List<char[]>();

        // read the input file
        while ((line = reader.ReadLine()) != null)
        {
            map.Add(line.ToCharArray());
        }
        reader.Close();

        // create a list of locations
        (int x, int y) start = new(0, 0);
        (int x, int y) end = new(0, 0);
        // initialize the list with the correct size
        List<Location> locations = new List<Location>(map.Count * map[0].Length);

        for (int y = 0; y < map.Count; y++)
        {
            for (int x = 0; x < map[y].Length; x++)
            {
                char elevation = map[y][x];
                if (elevation == 'S')
                {
                    elevation = 'a';
                    start = (x, y);
                }
                else if (elevation == 'E')
                {
                    elevation = 'z';
                    end = (x, y);
                }
                locations.Add(new Location(x, y, elevation, 1000));
            }
        }
        // create Neighbours
        locations.ForEach(x => x.CheckNeighbours(map[0].Length, map.Count, locations));

        // https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm#Algorithm

        // for part 2 we go backwards. So from end to start. Shortest path stays shortest path.
        // select the end location and set distance to 0
        Location current = locations.Find(x => x.x == end.x && x.y == end.y);
        current.distance = 0;

        List<Location> candidates = new List<Location>();
        if (part == 2)
        { 
            // limit set of possible canditates with elevation 'a' to those that have a neighbour that is 'b'. 
           candidates = locations.Where(loc => loc.elevation == 'a' && loc.neighbours.Any(nb => nb.elevation == 'b')).ToList();
        } else
        {
            // limit set of candidates to start location
            candidates = locations.Where(loc => loc.x == start.x && loc.y == start.y).ToList();
        }
        Console.WriteLine("Candidates: {0}", candidates.Count);

        while (!(candidates.Contains(current)))
        {
            // select the unvisited location with the smallest distance
            current = locations.Find(loc => loc.visited == false && loc.distance == locations.Where(loc => loc.visited == false).Min(loc => loc.distance));

            // if the distance is 1000, there is no path, this means that the current location is not connected to the start location
            if (current.distance == 1000)
            {
                Console.WriteLine("No path found");
                break;
            }

            // loop over all neighbours, if the distance is greater than the current distance + 1, update the distance
            current.neighbours.ForEach(neighbour =>
            {
                if (neighbour.distance > current.distance + 1)
                {
                    neighbour.distance = current.distance + 1;
                }
            });
            current.visited = true;
        }

        int shortestPath = locations.Find(x => x.x == current.x && x.y == current.y).distance;
        Console.WriteLine("Length shortest path: {0}", shortestPath);

        // wait for input before exiting
        Console.WriteLine("Press enter to finish");
        Console.ReadLine();
    }
}

