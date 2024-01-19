public class Program
{
    static Dictionary<int, (int X, int signalStrengthDuringCycle)> state = new Dictionary<int, (int, int)>();

    public static int runCycle(int cycle, string instruction)
    {
        if (instruction == "noop")
        {
            int x = state[cycle - 1].X;
            state.Add(cycle, (x, cycle * x));
            cycle++;
        }
        else
        {
            int x = state[cycle - 1].X;
            int newX = state[cycle - 1].X + int.Parse(instruction.Split(" ")[1]);
            state.Add(cycle, (x, cycle * x));
            state.Add(cycle + 1, (newX, (cycle + 1) * x));
            cycle += 2;
        }
        return cycle;
    }

    public static void Main(string[] args)
    {
        string file = "10_demo.txt";
        if (args.Length == 0)
        {
            Console.WriteLine("No input file specified, running demo input: 10_demo.txt");
        }
        else
        {
            file = args[0];
        }
        //initialize reader to read the input file.
        StreamReader reader = new StreamReader(file);

        // initialize variables
        string? line;

        // starting state is cycle 0, X = 1
        state.Add(0, (1, 0));
        int cycleCount = 1;

        // read the input file
        while ((line = reader.ReadLine()) != null)
        {
            //Part 1
            cycleCount = runCycle(cycleCount, line);

        }

        int sumSignalStrength = 0;
        for (int i = 20; i < 221; i += 40)
        {
            sumSignalStrength += state[i].signalStrengthDuringCycle;
        }

        // part 2 
        string output = "";
        int rowOffset = 0;
        for (int i = 0; i < cycleCount - 1; i++)
        {
            if (i % 40 == 0 && i != 0)
            {
                output += "\n";
                rowOffset += 40;
            }
            if (state[i].X >= i - 1 - rowOffset && state[i].X <= i + 1 - rowOffset)
            {
                output += "#";
            }
            else
            {
                output += ".";
            }
        }

        Console.WriteLine("Total score part 1: {0}", sumSignalStrength);
        Console.WriteLine("Graphic part 2: \n{0}", output);


        // wait for input before exiting
        Console.WriteLine("Press enter to finish");
        Console.ReadLine();
    }
}

