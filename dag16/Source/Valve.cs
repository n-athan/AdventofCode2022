using NLog;

public class Valve
{
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public string Name { get; set; }
    public int FlowRate { get; set; }
    public List<string> Tunnels { get; set; }
    public int PressureReleased { get; set; }
    public bool Open { get; set; }
    public int Index { get; set;}

    public Valve(string name, int flowRate, string tunnels)
    {
        Name = name;
        FlowRate = flowRate;
        Tunnels = tunnels.Split(", ").ToList();
        PressureReleased = 0;
        Open = false;
        Index = 0;
    }

    public void OpenValve(int minutesRemaining)
    {
        Open = true;
        PressureReleased += FlowRate * minutesRemaining;
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

    public static List<Valve> GetShortestPath(Dictionary<string,Valve> valves, string start, string end, int[,] previousMatrix)
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
            Valve valve = valves.Values.FirstOrDefault(v => v.Index == prev);
            path.Add(valve);
            end = valve.Name;
        }
        path.Reverse();
        return path;
    }

}