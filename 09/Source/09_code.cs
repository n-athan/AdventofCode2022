public class Program
{

    static HashSet<Tuple<int, int>> visitedPositions = new HashSet<Tuple<int, int>>();
    static int[] positionHead = new int[2];
    static int[] positionTail = new int[2];
    static int[][] positionKnots = new int[10][];
    static HashSet<Tuple<int, int>> visitedPositionsLastKnot = new HashSet<Tuple<int, int>>();

    public static void moveTail()
    {
        int distanceVert = positionHead[1] - positionTail[1];
        int distanceHor = positionHead[0] - positionTail[0];


        if ((distanceVert * distanceVert > 1 && distanceHor * distanceHor == 1)
        || (distanceVert * distanceVert == 1 && distanceHor * distanceHor > 1))  // diagonal
        {
            positionTail[0] += Math.Sign(distanceHor);
            positionTail[1] += Math.Sign(distanceVert);
        }
        else if (distanceVert * distanceVert > 1) // vertical
        {
            positionTail[1] += Math.Sign(distanceVert);
        }
        else if (distanceHor * distanceHor > 1) // horizontal
        {
            positionTail[0] += Math.Sign(distanceHor);
        }

        // save visited position
        if (!visitedPositions.Contains(new Tuple<int, int>(positionTail[0], positionTail[1])))
        {
            visitedPositions.Add(new Tuple<int, int>(positionTail[0], positionTail[1]));
        }
    }

    public static void moveHead(string direction, int steps)
    {

        for (int i = 0; i < steps; i++)
        {
            switch (direction)
            {
                case "R":
                    positionHead[0] += 1;
                    break;
                case "L":
                    positionHead[0] -= 1;
                    break;
                case "U":
                    positionHead[1] += 1;
                    break;
                case "D":
                    positionHead[1] -= 1;
                    break;
                default:
                    break;
            }

            moveTail();
        }
    }

    // for part 2
    public static void moveKnot(string direction, int steps)
    {
        for (int i = 0; i < steps; i++)
        {
            //move head i.e. positionKnots[0]
            switch (direction)
            {
                case "R":
                    positionKnots[0][0] += 1;
                    break;
                case "L":
                    positionKnots[0][0] -= 1;
                    break;
                case "U":
                    positionKnots[0][1] += 1;
                    break;
                case "D":
                    positionKnots[0][1] -= 1;
                    break;
                default:
                    break;
            }

            // move each knot one after the other
            for (int j = 1; j < positionKnots.Length; j++)
            {
                int distanceHorz = positionKnots[j - 1][0] - positionKnots[j][0];
                int distanceVert = positionKnots[j - 1][1] - positionKnots[j][1];

                if ((distanceVert * distanceVert > 1 && distanceHorz * distanceHorz >= 1)
                || (distanceVert * distanceVert >= 1 && distanceHorz * distanceHorz > 1))  // diagonal
                {
                    positionKnots[j][0] += Math.Sign(distanceHorz);
                    positionKnots[j][1] += Math.Sign(distanceVert);
                    // Console.WriteLine("Knot {0} moved diagonally to {1}, {2}", j, positionKnots[j][0], positionKnots[j][1]);
                }
                else if (distanceVert * distanceVert > 1) // vertical
                {
                    positionKnots[j][1] += Math.Sign(distanceVert);
                    // Console.WriteLine("Knot {0} moved vertically to {1}, {2}", j, positionKnots[j][0], positionKnots[j][1]);
                }
                else if (distanceHorz * distanceHorz > 1) // horizontal
                {
                    positionKnots[j][0] += Math.Sign(distanceHorz);
                    // Console.WriteLine("Knot {0} moved horizontally to {1}, {2}", j, positionKnots[j][0], positionKnots[j][1]);
                }
                // else { Console.WriteLine("Knot {0} did not move", j); }
                // Console.ReadLine();                
            }

            if (!visitedPositionsLastKnot.Contains(new Tuple<int, int>(positionKnots[9][0], positionKnots[9][1])))
            {
                visitedPositionsLastKnot.Add(new Tuple<int, int>(positionKnots[9][0], positionKnots[9][1]));
            }

        }
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
        //initialize reader to read the input file.
        StreamReader reader = new StreamReader(file);
        string? line;

        // initialize the visited positions
        visitedPositions.Add(new Tuple<int, int>(0, 0));

        for (int i = 0; i < positionKnots.Length; i++)
        {
            positionKnots[i] = new int[2];
        }

        visitedPositionsLastKnot.Add(new Tuple<int, int>(0, 0));

        // read the input file
        while ((line = reader.ReadLine()) != null)
        {
            //Part 1
            string[] directions = line.Split(' ');
            moveHead(directions[0], int.Parse(directions[1]));

            // Part 2
            moveKnot(directions[0], int.Parse(directions[1]));
        }

        // show visited positions
        // foreach (Tuple<int, int> position in visitedPositionsLastKnot)
        // {
        //     Console.WriteLine("Visited position: {0}, {1}", position.Item1, position.Item2);
            
        // Console.ReadLine();
        // }

        Console.WriteLine("Total score part 1: {0}", visitedPositions.Count);
        Console.WriteLine("Total score part 2: {0}", visitedPositionsLastKnot.Count);


        // wait for input before exiting
        Console.WriteLine("Press enter to finish");
        Console.ReadLine();
    }
}

