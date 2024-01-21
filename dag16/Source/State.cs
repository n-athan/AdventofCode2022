using NLog;

public class State{

    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public (Valve valve, int timeLeft) Human { get; set; }
    public (Valve valve, int timeLeft) Elephant { get; set; }
    public int TotalFlow = 0;
    public string[] OpenedValves = {};

    public State((Valve valve, int timeLeft) human, int totalFlow, string[] openedValves)
    {
        Human = human;
        TotalFlow = totalFlow;
        OpenedValves = openedValves;
    }

    public State((Valve valve, int timeLeft) human, (Valve valve, int timeLeft) elephant, int totalFlow, string[] openedValves)
    {
        Human = human;
        Elephant = elephant;
        TotalFlow = totalFlow;
        OpenedValves = openedValves;
    }
}