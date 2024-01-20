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
        
        Console.WriteLine("Total score part 1: {0}",part1(valves, distanceMatrix));
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

    public static int part1(Dictionary<string,Valve> valves, int[,] distanceMatrix)
    {
        // functie nodig om te bepalen hoeveel water er verschil zit tussen alle dichte kleppen, afstand*flowRate. 
        // om te bepalen welke we als eerste naar toe gaan. 
        int minutesRemaining = 30;
        Valve current = valves["AA"];

        // TODO onderstaande is in de goede richting, maar nog niet helemaal goed.
        // todo recursieve functie die per keuze voor openable klep de score berekent en de beste keuze teruggeeft.

        while (minutesRemaining > 0)
        {
            List<Valve> openableValves = Valve.getOpenableValves(valves);
            if (openableValves.Count == 0) { break; }
            
            foreach (var valve in openableValves)
            {
                int distance = distanceMatrix[current.Index,valve.Index];
                valve.getMaxPressureReleased(minutesRemaining-(distance+1));
            }
            // go to the valve with the most potential.
            openableValves = openableValves.OrderByDescending(v => v.MaxPressureReleased).ToList();
            Console.WriteLine("Openable valves name and potential: {0}",string.Join(", ",openableValves.Select(v => v.Name + ":" + v.MaxPressureReleased)));

            // check if we have enough time to get there and open the valve.
            minutesRemaining -= distanceMatrix[current.Index,openableValves[0].Index] +1; // -1 for the time it takes to open valve.
            if (minutesRemaining <= 0) { break; }

            // update location and open valve
            current = openableValves[0];
            current.OpenValve(minutesRemaining);
        }

        int score = valves.Values.Sum(v => v.MaxPressureReleased);
        return score;
    }

    public static int part2()
    {
        int score = 0;
        return score;
    }    
}

