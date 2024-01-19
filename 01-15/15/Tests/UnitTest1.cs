namespace Tests;

public class Tests
{
    public static string demo = @"Sensor at x=2, y=18: closest beacon is at x=-2, y=15
Sensor at x=9, y=16: closest beacon is at x=10, y=16
Sensor at x=13, y=2: closest beacon is at x=15, y=3
Sensor at x=12, y=14: closest beacon is at x=10, y=16
Sensor at x=10, y=20: closest beacon is at x=10, y=16
Sensor at x=14, y=17: closest beacon is at x=10, y=16
Sensor at x=8, y=7: closest beacon is at x=2, y=10
Sensor at x=2, y=0: closest beacon is at x=2, y=10
Sensor at x=0, y=11: closest beacon is at x=2, y=10
Sensor at x=20, y=14: closest beacon is at x=25, y=17
Sensor at x=17, y=20: closest beacon is at x=21, y=22
Sensor at x=16, y=7: closest beacon is at x=15, y=3
Sensor at x=14, y=3: closest beacon is at x=15, y=3
Sensor at x=20, y=1: closest beacon is at x=15, y=3";

    public static string[] Lines = demo.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

    public static List<Signal> Signals = Program.readInput(Lines);

    [TestCase(0,2,18,-2,15)]
    [TestCase(1,9,16,10,16)]
    [TestCase(2,13,2,15,3)]
    [TestCase(3,12,14,10,16)]
    [TestCase(4,10,20,10,16)]
    [TestCase(5,14,17,10,16)]
    [TestCase(6,8,7,2,10)]
    [TestCase(7,2,0,2,10)]
    [TestCase(8,0,11,2,10)]
    [TestCase(9,20,14,25,17)]
    [TestCase(10,17,20,21,22)]
    [TestCase(11,16,7,15,3)]
    [TestCase(12,14,3,15,3)]
    [TestCase(13,20,1,15,3)]
    public void TestSignalParser(int index, int sx, int sy, int bx, int by)
    {
        var lines = Lines;
        var signals = Signals;
        Assert.That(signals[index].position.x, Is.EqualTo(sx));
        Assert.That(signals[index].position.y, Is.EqualTo(sy));
        Assert.That(signals[index].beacon.x, Is.EqualTo(bx));
        Assert.That(signals[index].beacon.y, Is.EqualTo(by));
    }

    [Test]
    public void part1()
    {
        var lines = Lines;
        var signals = Signals;
        var count = Program.findBeaconsinRow(signals, 10, false);
        Assert.That(count, Is.EqualTo(26));
    }

    [TestCase(0,13)]
    [TestCase(1,8)]
    [TestCase(2,13)]
    [TestCase(6,32)]
    public void TestOuterEdge(int index,int expected)
    {
        var lines = Lines;
        var signals = Signals;
        HashSet<(int,int)> edgePoints = signals[index].getOuterEdge((0,20));
        Assert.That(edgePoints.Count, Is.EqualTo(expected));
    }

    [TestCase(14,11,true)]
    [TestCase(-4,12,false)]
    public void TestIsInLimit(int x,int y,bool expected)
    {
        Assert.That(Signal.isInLimit((x,y), (0,20)), Is.EqualTo(expected));
    }

    [TestCase(0,0,16,true)]
    [TestCase(0,0,50,false)]
    [TestCase(6,-1,7,true)]
    [TestCase(6,-2,7,false)]
    public void TestIsInRange(int index,int x,int y,bool expected)
    {
        var lines = Lines;
        var signals = Signals;
        Assert.That(signals[index].isInRange((x,y)), Is.EqualTo(expected));
    }

    [Test]
    public void part2()
    {
        var lines = Lines;
        var signals = Signals;
        var frequency = Program.findTuningFrequency(signals, (0,20), false);
        Assert.That(frequency, Is.EqualTo(56000011));
    }
}