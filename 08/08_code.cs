using System.Linq;

public class Program
{

    public static int checkView(List<int[]> treeGrid, int directionVert, int directionHorz, int row, int column)
    {
        int treeCounter = 0;
        int rowCounter = row + directionVert;
        int columnCounter = column + directionHorz;
        while (rowCounter >= 0 && rowCounter < treeGrid.Count && columnCounter >= 0 && columnCounter < treeGrid[rowCounter].Length)
        {
            if (treeGrid[rowCounter][columnCounter] < treeGrid[row][column])
            {
                treeCounter++;
            } else if (treeGrid[rowCounter][columnCounter] >= treeGrid[row][column])
            {
                treeCounter++;
                break;
            }
            rowCounter += directionVert;
            columnCounter += directionHorz;
        }
        return treeCounter;
    }

    public static void Main(string[] args)
    {
        string file = "08_demo.txt";
        if (args.Length == 0)
        {
            Console.WriteLine("No input file specified, running demo input: 08_demo.txt");
        }
        else
        {
            file = args[0];
        }
        //initialize reader to read the input file.
        StreamReader reader = new StreamReader(file);

        // initialize variables
        string? line;
        List<int[]> treeGrid = new List<int[]>(); // 2d array of ints

        // read the input file and fill the treeGrid.
        while ((line = reader.ReadLine()) != null)
        {
            int[] lineArray = new int[line.Length];
            for (int i = 0; i < line.Length; i++)
            {
                lineArray[i] = Int32.Parse(line.Substring(i, 1));
            }
            treeGrid.Add(lineArray);
        }

        treeGrid.ToArray();

        // part 1
        int visibleTrees = treeGrid.Count * treeGrid[0].Length;
        for (int row = 1; row < treeGrid.Count - 1; row++)
        {
            for (int column = 1; column < treeGrid[row].Length - 1; column++)
            {                
                // check visible from east
                int[] subArray = treeGrid[row].Skip(column + 1).ToArray();
                if (subArray.Max() >= treeGrid[row][column])
                {
                    // check visible from west
                    subArray = treeGrid[row].Take(column).ToArray();
                    if (subArray.Max() >= treeGrid[row][column])
                    {
                        // check visible from north
                        subArray = treeGrid.Select(x => x[column]).Take(row).ToArray();
                        if (subArray.Max() >= treeGrid[row][column])
                        {
                            // check visible from south
                            subArray = treeGrid.Select(x => x[column]).Skip(row + 1).ToArray();
                            if (subArray.Max() >= treeGrid[row][column])
                            {
                                visibleTrees--;
                            }
                        }
                    }
                }
            }
        }

        // part 2
        int highestScenicScore = 0;
        for (int row = 0; row < treeGrid.Count; row++)
        {
            for (int column = 0; column < treeGrid[row].Length; column++)
            {
                int south = checkView(treeGrid, 1, 0, row, column);
                int north = checkView(treeGrid, -1, 0, row, column);
                int west = checkView(treeGrid, 0, -1, row, column);
                int east = checkView(treeGrid, 0, 1, row, column);
                int scenicScore = east * west * north * south;      

                if (scenicScore > highestScenicScore)
                {
                    highestScenicScore = scenicScore;
                }
            }
        }
        

        Console.WriteLine("Total visible trees part 1: {0}", visibleTrees);
        Console.WriteLine("Total score part 2: {0}", highestScenicScore);


        // wait for input before exiting
        Console.WriteLine("Press enter to finish");
        Console.ReadLine();
    }
}

