using NLog;

public class Valve
{
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public string Name { get; set; }
    public int FlowRate { get; set; }
    public List<string> Tunnels { get; set; }
    public int MaxPressureReleased { get; set; }
    public bool Open { get; set; }
    public int Index { get; set;}

    public Valve(string name, int flowRate, string tunnels)
    {
        Name = name;
        FlowRate = flowRate;
        Tunnels = tunnels.Split(", ").ToList();
        MaxPressureReleased = 0;
        Open = false;
        Index = 0;
    }

    public void OpenValve(int minutesRemaining) // only for testing
    {
        Open = true;
        getMaxPressureReleased(minutesRemaining);
    }

    public void CloseValve() // only for testing
    {
        Open = false;
        MaxPressureReleased = 0;
    }

    public static void SetIndex(Dictionary<string,Valve> valves)
    {
        int index = 0;
        valves.OrderBy(v => v.Key);
        foreach (var valve in valves)
        {
            valve.Value.Index = index;
            index++;
        }
    }

    public static List<Valve>? GetShortestPath(Dictionary<string,Valve> valves, string start, string end, int[,] previousMatrix)
    {
        var previous = previousMatrix;
        var path = new List<Valve>();
        // if prev[u][v] = null then
        if (previous[valves[start].Index, valves[end].Index] == -1)
        {
            // no path between u and v
            return null;
        } else {
            path.Add(valves[end]);
        }
        // while u != v
        while (valves[start] != valves[end])
        {
            int prev = previous[valves[start].Index, valves[end].Index];
            
            // find element in Dictionary valves where the propery Index == v
            Valve? valve = valves.Values.FirstOrDefault(v => v.Index == prev);
            if (valve != null)
            {
                path.Add(valve);
                end = valve.Name;
            }
        }
        path.Reverse();
        return path;
    }

    public static List<Valve> getOpenableValves(Dictionary<string,Valve> valves, Valve currentValve, int minutesRemaining, int[,] distanceMatrix, string[] excludedValves)
    {
        var openableValves = new List<Valve>();
        foreach (var valve in valves)
        {
            if (valve.Value.FlowRate > 0 && !excludedValves.Contains(valve.Value.Name) && distanceMatrix[valve.Value.Index, currentValve.Index] <= minutesRemaining)
            {
                openableValves.Add(valve.Value);
            }
        }

        return openableValves;
    }

    public void getMaxPressureReleased(int minutesRemaining)
    {
        MaxPressureReleased = FlowRate * minutesRemaining;
    }

    public static int getPressureReleasedFromPath(string[] path, int minutesRemaining, int[,] distanceMatrix, Dictionary<string,Valve> valves)
    {
        int pressureReleased = 0;
        int currentValve = 0;
        foreach (var valveName in path)
        {
            Valve valve = valves[valveName];
            minutesRemaining -= distanceMatrix[valve.Index, currentValve] + 1;
            valve.getMaxPressureReleased(minutesRemaining);
            if (minutesRemaining < 0) break;
            pressureReleased += valve.MaxPressureReleased;
            currentValve = valve.Index;
        }
        return pressureReleased;
    }

}