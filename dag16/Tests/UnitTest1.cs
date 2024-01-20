namespace Tests;

public class Tests
{
    public static string demo = @"Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
Valve BB has flow rate=13; tunnels lead to valves CC, AA
Valve CC has flow rate=2; tunnels lead to valves DD, BB
Valve DD has flow rate=20; tunnels lead to valves CC, AA, EE
Valve EE has flow rate=3; tunnels lead to valves FF, DD
Valve FF has flow rate=0; tunnels lead to valves EE, GG
Valve GG has flow rate=0; tunnels lead to valves FF, HH
Valve HH has flow rate=22; tunnel leads to valve GG
Valve II has flow rate=0; tunnels lead to valves AA, JJ
Valve JJ has flow rate=21; tunnel leads to valve II";

    public static string[] Lines = demo.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

    public static Dictionary<string,Valve> Valves = Program.readInput(Lines);

    public static List<int[,]> getDistanceMatrix = Program.GetDistanceMatrix(Valves);    

    [Test, Order(1)]
    public void TestParser()
    {
        var lines = Lines;
        var valves = Valves;
        Assert.That(Lines.Length, Is.EqualTo(10));
        Assert.That(Valves.Count, Is.EqualTo(10));
    }

    [Test, Order(2)]
    [TestCase("AA",0, new string[] { "DD", "II", "BB" }, 0, false)]
    [TestCase("BB",13, new string[] { "CC", "AA" }, 0, false)]
    [TestCase("CC",2, new string[] { "DD", "BB" }, 0, false)]
    [TestCase("JJ",21, new string[] { "II" }, 0, false)]
    public void TestValve(string name, int flowRate, string[] tunnels, int pressureReleased, bool open)
    {
        var valve = Valves[name];
        Assert.That(valve.Name, Is.EqualTo(name));
        Assert.That(valve.FlowRate, Is.EqualTo(flowRate));
        Assert.That(valve.Tunnels, Is.EqualTo(tunnels.ToList()));
        Assert.That(valve.MaxPressureReleased, Is.EqualTo(pressureReleased));
        Assert.That(valve.Open, Is.EqualTo(open));
    }

    [Test, Order(3)]
    [TestCase("AA", 30, 0, true)]
    [TestCase("BB", 30, 390, true)]
    [TestCase("HH", 15, 330, true)]
    [TestCase("JJ", 0, 0, true)]
    public void TestOpenValve(string name, int minutesRemaining, int expectedPressureReleased, bool expectedOpen)
    {
        var valve = Valves[name];
        valve.OpenValve(minutesRemaining);
        Assert.That(valve.MaxPressureReleased, Is.EqualTo(expectedPressureReleased));
        Assert.That(valve.Open, Is.EqualTo(expectedOpen));
        valve.CloseValve();
    }

    [Test, Order(5)]
    [TestCase("AA", 0)]
    [TestCase("BB", 1)]
    [TestCase("CC", 2)]
    [TestCase("JJ", 9)]
    public void TestSetIndex(string name, int expectedIndex)
    {
        var valve = Valves[name];
        Assert.That(valve.Index, Is.EqualTo(expectedIndex));
    }

    [Test, Order(6)]
    [TestCase("AA", "DD", 1)]
    [TestCase("DD", "GG", 3)]
    [TestCase("AA", "GG", 4)]
    [TestCase("BB", "BB", 0)]
    [TestCase("BB", "DD", 2)]
    [TestCase("AA", "JJ", 2)]
    public void TestGetDistanceMatrix(string start, string end, int expectedDistance)
    {
        var getDistanceMatrix = Tests.getDistanceMatrix;
        var distanceMatrix = getDistanceMatrix[0];
        var previousMatrix = getDistanceMatrix[1];
        Assert.That(distanceMatrix[Valves[start].Index, Valves[end].Index], Is.EqualTo(expectedDistance));
    }

    [Test, Order(7)]
    [TestCase("AA", "DD", 2)]
    [TestCase("DD", "GG", 4)]
    public void TestGetShortestPathLength(string start, string end, int pathCount)
    {
        var previousMatrix = getDistanceMatrix[1];
        var path = Valve.GetShortestPath(Valves, start, end, previousMatrix);
        Assert.That(path.Count, Is.EqualTo(pathCount));
    }

    [Test, Order(8)]
    [TestCase("AA", "DD", new string[] { "AA", "DD" })]
    [TestCase("DD", "GG", new string[] { "DD", "EE", "FF", "GG" })]
    public void TestGetShortestPath(string start, string end, string[] expectedPath)
    {
        var previousMatrix = getDistanceMatrix[1];
        var path = Valve.GetShortestPath(Valves, start, end, previousMatrix);
        Assert.That(path.Select(v => v.Name), Is.EqualTo(expectedPath));
    }

    [Test, Order(9)]
    public void TestGetOpenableValves()
    {
        var openableValves = Valve.getOpenableValves(Valves);
        Assert.That(openableValves.Count, Is.EqualTo(6));
        Assert.That(openableValves.Select(v => v.Name), Is.EqualTo(new string[] { "BB", "CC", "DD", "EE", "HH", "JJ" }));
    }

    [Test, Order(10)]
    public void part1()
    {
        var lines = Lines;
        int score = Program.part1(Valves, getDistanceMatrix[0]);
        Assert.That(score, Is.EqualTo(1651));
    }

    // [Test]
    // public void part2()
    // {
    //     var lines = Lines;
    //     Assert.That(, Is.EqualTo());
    // }
}