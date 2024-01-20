using NLog;

public class Program
{
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public static void Main(string[] args)
    {
        string file = "dag16_input.txt";

        // read the input file
        string[] lines = File.ReadAllLines(file);
        List<REPLACE> replace = readInput(lines);

        
        Console.WriteLine("Total score part 1: {0}",part1());
        Console.WriteLine("Total score part 2: {0}",part2());


        // wait for input before exiting
        Console.WriteLine("Press enter to finish");
        Console.ReadLine();
    }

    public static List<REPLACE> readInput(string[] lines)
    {
        List<REPLACE> replace = new List<REPLACE>();
        
        return replace;
    }

    public static int part1()
    {
        int score = 0;
        return score;
    }

    public static int part2()
    {
        int score = 0;
        return score;
    }    
}

