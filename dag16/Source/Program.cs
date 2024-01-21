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

        // sla op in welke volgorde de kleppen open zijn gegaan en hoeveel flow er maximaal is geweest
        Dictionary<string[],int> maxFlows = new Dictionary<string[],int>();
        
        Console.WriteLine("Total score part 1: {0}",part1(valves, distanceMatrix, maxFlows));
        Console.WriteLine("Total score part 2: {0}",part2(maxFlows, valves, distanceMatrix));


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

    public static int part1(Dictionary<string,Valve> valves, int[,] distanceMatrix, Dictionary<string[],int> maxFlows)
    {
        // Learned about Queues in: https://www.reddit.com/r/adventofcode/comments/zo21au/comment/j0nz8df/
        // elk queueitem is een mogelijke optie op een bepaald moment. We lopen daarmee alle redelijke paden af.
        Queue<State> queue = new();

        // begin bij klep AA, met 1 operator (de mens)
        List<(Valve valve, int timeLeft)> valveOperators = new() { (valves["AA"], 30) };
        State state = new(valveOperators, 0, Array.Empty<string>());
        queue.Enqueue(state);
        
        // kijk naar alle kleppen die open kunnen, zet ze in de queue. 
        // ga dan de queue af en kijk of er nog meer kleppen open kunnen (tijd genoeg).
        // sla de kleppen op die open zijn gegaan en de totale flow die er is geweest.
        // aan het einde kijken welke volgorde het meest efficient is geweest.
        while (queue.Count > 0)
        {
            var queueItem = queue.Dequeue();

            State.ProcessState(queueItem,queue,maxFlows,valves,distanceMatrix);
        }

        int score = maxFlows.Values.Max();
        Console.WriteLine(" Max Flow Key: {0}",string.Join(", ",maxFlows.Where(v => v.Value == score).Select(v => string.Join(", ",v.Key))));
        return score;
    }

    public static int part2(Dictionary<string[],int> maxFlows, Dictionary<string,Valve> Valves, int[,] distanceMatrix){
        // vind de keys zonder overlappende kleppen en welk paar de meeste flow heeft gehad
        // dit duur te lang, hoe te optimaliseren?

        // disjoint key pairs in maxFlows (key x, key y, sum of totalFlow)
        var disjointKeyPairs = maxFlows.Keys.SelectMany((x, i) => maxFlows.Keys.Skip(i + 1),
            (x, y) => new { x, y })
            .Where(pair => !pair.x.Intersect(pair.y).Any())
            .Select(pair => new { pair.x, pair.y, maxFlow = maxFlows[pair.x] + maxFlows[pair.y] })
            .OrderByDescending(pair => pair.maxFlow)
            .First();

        int maxFlowx = Valve.getPressureReleasedFromPath(disjointKeyPairs.x, 26, distanceMatrix, Valves);
        int maxFlowy = Valve.getPressureReleasedFromPath(disjointKeyPairs.y, 26, distanceMatrix, Valves);
        int maxFlow = maxFlowx + maxFlowy;

        
        // Console.WriteLine(" Max Flow Key: {0}",string.Join(", ",maxFlowPerPair.Where(pair => pair.maxFlow == maxFlow).Select(pair => string.Join(", ",pair.pair.x.Concat(pair.pair.y)))));
        return maxFlow;
    }

    public static int part2slow(Dictionary<string,Valve> valves, int[,] distanceMatrix)
    {
        // werkte alleen op de demo data. Was te langzaam voor de echte data.

        Queue<State> queue = new();
        Dictionary<string[],int> maxFlows = new Dictionary<string[],int>();

        // begin bij klep AA, met 2 operators (de mens en de olifant)
        List<(Valve valve, int timeLeft)> valveOperators = new() { (valves["AA"], 26), (valves["AA"], 26) };
        State state = new(valveOperators, 0, Array.Empty<string>());
        queue.Enqueue(state);
        
        while (queue.Count > 0)
        {
            var queueItem = queue.Dequeue();

            State.ProcessState(queueItem,queue,maxFlows,valves,distanceMatrix);
        }

        int score = maxFlows.Values.Max();
        var options = maxFlows.Where(v => v.Value == score);

        foreach (var maxFlow in options)
        {
            Console.WriteLine(" Max Flow Key: {0}",string.Join(", ",maxFlow.Key));
        }
        return score;
    }  
}

