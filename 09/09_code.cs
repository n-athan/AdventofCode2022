public class Program
{

    static HashSet<Tuple<int, int>> visitedPositions = new HashSet<Tuple<int, int>>();
    static int[] positionHead = new int[2];
    static int[] positionTail = new int[2];

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

        // read the input file
        while ((line = reader.ReadLine()) != null)
        {
            //Part 1
            string[] directions = line.Split(' ');
            moveHead(directions[0], int.Parse(directions[1]));

            // Part 2

        }

        // show visited positions
        // foreach (Tuple<int, int> position in visitedPositions)
        // {
        //     Console.WriteLine("Visited position: {0}, {1}", position.Item1, position.Item2);
        // }

        Console.WriteLine("Total score part 1: {0}", visitedPositions.Count);
        // Console.WriteLine("Total score part 2: {0}",);


        // wait for input before exiting
        Console.WriteLine("Press enter to finish");
        Console.ReadLine();
    }
}

