using NLog;
using System.Text.RegularExpressions;

public class Program
{
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public static void Main(string[] args)
    {
        string file = "dag16_input.txt";

        // read the input file
        string[] lines = File.ReadAllLines(file);
        Dictionary<string,Valve> valves = readInput(lines);
        Valve.SetIndex(valves);

        // calculate the distance matrix
        List<int[,]> getDistanceMatrix = GetDistanceMatrix(valves);
        int[,] distanceMatrix = getDistanceMatrix[0];
        int[,] previousMatrix = getDistanceMatrix[1];
        
        Console.WriteLine("Total score part 1: {0}",part1());
        Console.WriteLine("Total score part 2: {0}",part2());


        // wait for input before exiting
        Console.WriteLine("Press enter to finish");
        Console.ReadLine();
    }

    public static Dictionary<string,Valve> readInput(string[] lines)
    {
        Dictionary<string,Valve> valves = new Dictionary<string,Valve>();
        
        foreach (string line in lines)
        {
            // Regex kan match groepen benoemen met (?<naam>patroon)
            string pattern = @"Valve (?<name>\w+) has flow rate=(?<flowRate>\d+); tunnels? leads? to valves? (?<tunnels>.*)";
            Match match = Regex.Match(line, pattern);

            string name = match.Groups["name"].Value;
            int flowRate = int.Parse(match.Groups["flowRate"].Value);
            string tunnels = match.Groups["tunnels"].Value;

            valves.Add(name, new Valve(name, flowRate, tunnels));
        }

        Valve.SetIndex(valves);

        return valves;
    }

    // https://en.wikipedia.org/wiki/Floyd%E2%80%93Warshall_algorithm
    public static List<int[,]> GetDistanceMatrix(Dictionary<string,Valve> valves)
    {
        // let dist be a Array of minimum distances initialized to infinity 
        // let next be a Array of vertex indices initialized to null (for path reconstruction)
        int[,] distanceMatrix = new int[valves.Count,valves.Count];
        int[,] previousMatrix = new int[valves.Count,valves.Count];
        for (int i = 0; i < valves.Count; i++)
        {
            for (int j = 0; j < valves.Count; j++)
            {
                distanceMatrix[i,j] = 999999;
                previousMatrix[i,j] = -1;
            }
        }


        // for each edge (u,v) do:
        foreach (var valve in valves)
        {
            foreach (var tunnel in valve.Value.Tunnels)
            {
                int valveIndex = valve.Value.Index;
                int tunnelIndex = valves[tunnel].Index;
                distanceMatrix[valveIndex,tunnelIndex] = 1;
                previousMatrix[valveIndex,tunnelIndex] = valveIndex;
            }
        }

        // for each vertex v do:
        foreach (var valve in valves) {
            int valveIndex = valve.Value.Index;
            distanceMatrix[valveIndex,valveIndex] = 0;
            previousMatrix[valveIndex,valveIndex] = valveIndex;
        }

        // for k from 1 to |V| do:
        for (int k = 0; k < valves.Count; k++) {
            // for i from 1 to |V| do:
            for (int i = 0; i < valves.Count; i++) {
                // for j from 1 to |V| do:
                for (int j = 0; j < valves.Count; j++) {
                    // if dist[i, j] > dist[i, k] + dist[k, j] then:
                    // set the value of dist[i, j] to dist[i, k] + dist[k, j]
                    if (distanceMatrix[i,j] > distanceMatrix[i,k] + distanceMatrix[k,j]) 
                    {
                        distanceMatrix[i,j] = distanceMatrix[i,k] + distanceMatrix[k,j];
                        previousMatrix[i,j] = previousMatrix[k,j];
                    }
                }
            }
        }
        
        return new List<int[,]>{distanceMatrix, previousMatrix};
    }

    public static int part1()
    {

        int score = 0;
        return score;
    }

    public static int part2()
    {
        int score = 0;
        return score;
    }    
}

