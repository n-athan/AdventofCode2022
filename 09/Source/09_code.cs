public class Knot
{
    public int x { get; set; }
    public int y { get; set; }
    public HashSet<Tuple<int, int>> visitedPositions = new HashSet<Tuple<int, int>>();

    public Knot(int x, int y)
    {
        this.x = x;
        this.y = y;
        this.visitedPositions.Add(new Tuple<int, int>(x, y));
    }

    public static Knot[] makeRope(int length)
    {
        Knot[] rope = new Knot[length];
        for (int i = 0; i < length; i++)
        {
            rope[i] = new Knot(0, 0);
        }
        return rope;
    }

    public void savePosition()
    {
        Tuple<int, int> position = new Tuple<int, int>(this.x, this.y);
        if (!this.visitedPositions.Contains(position))
        {
            this.visitedPositions.Add(position);
        }
    }

    public static void moveKnots(Knot[] rope, string direction, int steps)
    {
        for (int i = 0; i < steps; i++)
        {
            //move head i.e. knot 0
            switch (direction)
            {
                case "R":
                    rope[0].x += 1;
                    break;
                case "L":
                    rope[0].x -= 1;
                    break;
                case "U":
                    rope[0].y += 1;
                    break;
                case "D":
                    rope[0].y -= 1;
                    break;
                default:
                    break;
            }
            rope[0].savePosition();

            // move each knot one after the other
            for (int j = 1; j < rope.Length; j++)
            {
                int distanceHorz = rope[j - 1].x - rope[j].x;
                int distanceVert = rope[j - 1].y - rope[j].y;

                if ((distanceVert * distanceVert > 1 && distanceHorz * distanceHorz >= 1)
                    || (distanceVert * distanceVert >= 1 && distanceHorz * distanceHorz > 1))  // diagonal
                {
                    rope[j].x += Math.Sign(distanceHorz);
                    rope[j].y += Math.Sign(distanceVert);
                    // Console.WriteLine("Knot {0} moved diagonally to {1}, {2}", j, rope[j].x, rope[j].y);
                }
                else if (distanceVert * distanceVert > 1) // vertical
                {
                    rope[j].y += Math.Sign(distanceVert);
                    // Console.WriteLine("Knot {0} moved vertically to {1}, {2}", j, rope[j].x, rope[j].y);
                }
                else if (distanceHorz * distanceHorz > 1) // horizontal
                {
                    rope[j].x += Math.Sign(distanceHorz);
                    // Console.WriteLine("Knot {0} moved horizontally to {1}, {2}", j, rope[j].x, rope[j].y);
                }
                // else { Console.WriteLine("Knot {0} did not move", j); }
                // Console.ReadLine();     
                
                rope[j].savePosition();   
            }
        }
    }
}

public class Program
{
    public static List<string[]> readInput(string file)
    {
        // read the input file
        string[] lines = File.ReadAllLines(file);

        List<string[]> instructions = new List<string[]>();

        // read the input file and fill the treeGrid.
        foreach (string line in lines)
        {
            instructions.Add(line.Split(' '));
        }
        return instructions;
    }

    public static void Main(string[] args)
    {
        string file = "09_demo.txt";
        if (args.Length == 0)
        {
            Console.WriteLine("No input file specified, running demo input: 09_demo.txt");
        }
        else
        {
            file = args[0];
        }

        // initialize the ropes with knots
        Knot[] rope1 = Knot.makeRope(2);
        Knot[] rope2 = Knot.makeRope(10);

        // read the input file
        List<string[]> instructions = readInput(file);

        foreach (string[] instruction in instructions)
        {            
            //Part 1
            Knot.moveKnots(rope1, instruction[0], int.Parse(instruction[1]));

            // Part 2
            Knot.moveKnots(rope2, instruction[0], int.Parse(instruction[1]));
        }

        // get the number of visited positions of the last Knot in the rope1
        Console.WriteLine("Total score part 1: {0}", rope1[rope1.Length -1].visitedPositions.Count);
        Console.WriteLine("Total score part 2: {0}", rope2[rope2.Length -1].visitedPositions.Count);

        // wait for input before exiting
        Console.WriteLine("Press enter to finish");
        Console.ReadLine();
    }
}

