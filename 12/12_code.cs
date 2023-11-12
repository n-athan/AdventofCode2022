public class Location
{
    public int x { get; set; }
    public int y { get; set; }
    public string? elevation { get; set; }
    public int index { get; set; }
    public List<Location>? reachableNeighbors { get; set; }

    public Location(int _x, int _y, string? _elevation, int _index)
    {
        x = _x;
        y = _y;
        elevation = _elevation;
        index = _index;
        reachableNeighbors = new List<Location>();
    }

    public void AddNeighbors(int width, int height, List<Location> locations)
    {
        int _x = x - 1;
        int _y = y - 1;
        while (_x <= x + 1)
        {
            while (_y <= y + 1)
            {
                if (_x >= 0 && _x < width && _y >= 0 && _y < height)
                {
                    if (_x != x || _y != y)
                    {
                        if (string.Compare(locations[_x + _y * width].elevation,elevation) <= 1 )
                        {
                            Console.WriteLine("Adding neighbor {0} to {1}", locations[_x + _y * width].index, index);
                            reachableNeighbors.Add(locations[_x + _y * width]);
                        }
                    }
                }
                _y++;
            }
            _x++;
        }
    }

}

public class Program
{

    public static void Main(string[] args)
    {
        string file = "12_demo.txt";
        if (args.Length == 0)
        {
            Console.WriteLine("No input file specified, running demo input: 12_demo.txt");
        }
        else
        {
            file = args[0];
        }
        //initialize reader to read the input file.
        StreamReader reader = new StreamReader(file);

        // initialize variables
        string? line;
        List<string> map = new List<string>();

        // read the input file
        while ((line = reader.ReadLine()) != null)
        {
            map.Add(line);
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
                string elevation = map[y][x].ToString();
                if (elevation == "S")
                {
                    elevation = "a";
                    start = (x, y);
                }
                else if (elevation == "E")
                {
                    elevation = "z";
                    end = (x, y);
                }
                locations.Add(new Location(x, y, elevation, x + y * map[0].Length));
            }
        }

        // add neighbors to each location
        // TODO dit gaat mis. Verkeerder buren worden toegevoegd, zie rondom 0,0. 
        for (int i = 0; i < locations.Count; i++)
        {
            locations[i].AddNeighbors(map[0].Length, map.Count, locations);
        }

        // locations[1].reachableNeighbors.ForEach(x => Console.WriteLine(x.index));
        // show all locations[0]  infor
        Console.WriteLine("Location {0}, x: {1}, y: {2}", locations[0].index, locations[0].x, locations[0].y);

        // Console.WriteLine("Total score part 1: {0}",);
        // Console.WriteLine("Total score part 2: {0}",);


        // wait for input before exiting
        Console.WriteLine("Press enter to finish");
        Console.ReadLine();
    }
}

