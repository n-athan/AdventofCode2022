using NLog;

public class State{

    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public List<(Valve valve, int timeLeft)> ValveOperators { get; set; }
    public int TotalFlow = 0;
    public string[] OpenedValves = {};

    public State(List<(Valve valve, int timeLeft)> valveOperators, int totalFlow, string[] openedValves)
    {
        ValveOperators = valveOperators;
        TotalFlow = totalFlow;
        OpenedValves = openedValves;
    }

    public static void ProcessState(State state, Queue<State> queue, Dictionary<string[],int> maxFlow, Dictionary<string,Valve> valves, int[,] distanceMatrix) {
        
        List<List<Valve>> openableValves = new();
        for (int i = 0; i < state.ValveOperators.Count; i++)
        {
            (Valve valve, int timeLeft) = state.ValveOperators[i];
            openableValves.Add(Valve.getOpenableValves(valves, valve, timeLeft, distanceMatrix, state.OpenedValves));
        }
        if (openableValves.Count == 0) return;

        // we willen an alle operators de kleppen kunnen openen, alle mogelijke combinaties als volgende stap hebben. 
        // dus als de mens naar A en B kan en de Olifant naar A, C en D, dan willen we de volgende combinaties:
        // A en C, A en D, B en A, B en C, B en D. Die moeten als volgende stap in de queue.

        // dit was uiteindelijk te langzaam, dus ik dit wordt niet gebruikt in deel 2. 
        var combinations = State.GetCombinations(openableValves)
            .Where(c => c.Distinct().Count() == c.Count())
            .ToList();

        foreach (var combination in combinations){
            var combList = combination.ToList();

            int totalFlow = state.TotalFlow;
            string[] valvesOpened = state.OpenedValves;
            var operators = new List<(Valve,int)>(state.ValveOperators); // new list, zodat het geen reference is naar de oude list

            for (int i = 0; i < combList.Count; i++)
            {
                Valve valve = combList[i];
                // hoeveel tijd is er nog over als we naar deze klep gaan
                int time = state.ValveOperators[i].timeLeft - (distanceMatrix[state.ValveOperators[i].valve.Index, valve.Index] +1);
                operators[i] = (valve, time);

                // hoeveel flow is er maximaal is als we naar deze klep gaan
                valve.getMaxPressureReleased(time);
                totalFlow += valve.MaxPressureReleased;

                // voeg klep toe aan de lijst van kleppen die open zijn gegaan
                valvesOpened = valvesOpened.Append(valve.Name).ToArray();
            }
            // zet de state in de queue, om te kijken of hierna nog meer kleppen open kunnen
            queue.Enqueue(new State(operators, totalFlow, valvesOpened));
            
            // sla op welke kleppen er open zijn gegaan en hoeveel flow er maximaal is geweest
            maxFlow.Add(valvesOpened, totalFlow);
        }
    }

    // via copilot, onduidelijk waarom dit niet List<List<Valve>> is. 
    private static IEnumerable<IEnumerable<Valve>> GetCombinations(List<List<Valve>> lists)
    {
        if (lists.Count == 1)
            return lists[0].Select(t => new Valve[] { t });
        else
        {
            var subCombinations = GetCombinations(lists.Skip(1).ToList());
            return lists[0].SelectMany(t =>
                subCombinations.Select(c => new Valve[] { t }.Concat(c)));
        }
    }
}