public class Program
{

    public static void Main(string[] args)
    {
        string file = "AoCTemplate_demo.txt";
        if (args.Length == 0) {
            Console.WriteLine("No input file specified, running demo input: AoCTemplate_demo.txt");
        } else {
            file = args[0];
        }
        //initialize reader to read the input file.
        StreamReader reader = new StreamReader(file);

        // initialize variables
        string? line;

        // read the input file
        while ((line = reader.ReadLine()) != null)
        {
            //Part 1


            // Part 2

        }

        // Console.WriteLine("Total score part 1: {0}",);
        // Console.WriteLine("Total score part 2: {0}",);


        // wait for input before exiting
        Console.WriteLine("Press enter to finish");
        Console.ReadLine();
    }
}

