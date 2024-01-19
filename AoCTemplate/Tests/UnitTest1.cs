namespace Tests;

public class Tests
{
    public static string demo = @"
";

    public static string[] Lines = demo.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

    public static List<>  = Program.readInput(Lines);

    [TestCase()]
    public void TestParser()
    {
        var lines = Lines;
        Assert.That();
    }

    [Test]
    public void part1()
    {
        var lines = Lines;
        Assert.That(, Is.EqualTo());
    }

    // [Test]
    // public void part2()
    // {
    //     var lines = Lines;
    //     Assert.That(, Is.EqualTo());
    // }
}