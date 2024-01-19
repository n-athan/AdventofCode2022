namespace Tests;

public class Tests
{
    public string[] Lines()
    {
        string[] lines = @"R 4
U 4
L 3
D 1
R 4
D 1
L 5
R 2".Split("\r\n");
        lines.ToList().ForEach(line => line.Trim());
        lines = lines.ToArray();
        return lines;
    }

    public List<string[]> Instructions()
    {
        var lines = Lines();
        List<string[]> instructions = Program.readInput(lines);
        return instructions;
    }

    [Test]
    public void TestParse()
    {
        var instructions = Instructions();
        Assert.That(instructions.Count, Is.EqualTo(8));
        Assert.That(instructions[0].Length, Is.EqualTo(2));
        Assert.That(instructions[0][0], Is.EqualTo("R"));
        Assert.That(instructions[0][1], Is.EqualTo("4"));
        Assert.That(instructions[1][0], Is.EqualTo("U"));
        Assert.That(instructions[1][1], Is.EqualTo("4"));
    }

    [Test]
    public void TestRope()
    {
        Knot[] rope1 = Knot.makeRope(2);
        Assert.That(rope1.Length, Is.EqualTo(2));
        Assert.That(rope1[0].x, Is.EqualTo(0));
        Assert.That(rope1[0].y, Is.EqualTo(0));

        Knot[] rope2 = Knot.makeRope(10);
        Assert.That(rope2.Length, Is.EqualTo(10));
        Assert.That(rope2[9].x, Is.EqualTo(0));
        Assert.That(rope2[9].y, Is.EqualTo(0));
    }

    [Test]
    public void TestPart1()
    {
        var instructions = Instructions();
        Knot[] rope = Knot.makeRope(2);
        foreach (string[] instruction in instructions)
        {            
            Knot.moveKnots(rope, instruction[0], int.Parse(instruction[1]));
        }
        Assert.That(rope[rope.Length - 1].visitedPositions.Count, Is.EqualTo(13));
    }

    [Test]
    public void TestPart2()
    {
        var instructions = Instructions();
        Knot[] rope = Knot.makeRope(10);
        foreach (string[] instruction in instructions)
        {            
            Knot.moveKnots(rope, instruction[0], int.Parse(instruction[1]));
        }
        Assert.That(rope[rope.Length - 1].visitedPositions.Count, Is.EqualTo(1));
    }
}