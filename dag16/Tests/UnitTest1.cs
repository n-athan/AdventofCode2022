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
Valve JJ has flow rate=21; tunnel leads to valve II
";

    public static string[] Lines = demo.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

    // public static List<>  = Program.readInput(Lines);

    [TestCase()]
    public void TestParser()
    {
        var lines = Lines;
        Assert.That(Lines.Length, Is.EqualTo(10));
    }

    // [Test]
    // public void part1()
    // {
    //     var lines = Lines;

    //     Assert.That(, Is.EqualTo(1651));
    // }

    // [Test]
    // public void part2()
    // {
    //     var lines = Lines;
    //     Assert.That(, Is.EqualTo());
    // }
}