public class Program
{
    public static bool IsUnique(string str)
    {
        // maak een hash van de string. Dat verwijderd duplicaten, wan t die zijn in een hashset niet toegestaan. 
        HashSet<char> hash = new HashSet<char>(str);
        // als de lengte van de hash gelijk is aan de lengte van de string, dan zijn er geen duplicaten.
        return hash.Count == str.Length;
    }

    public static void Main(string[] args)
    {
        string file = "06_demo.txt";
        if (args.Length == 0) {
            Console.WriteLine("No input file specified, running demo input: 06_demo.txt");
        } else {
            file = args[0];
        }
        //initialize reader to read the input file.
        StreamReader reader = new StreamReader(file);

        // initialize variables
        string? packet = reader.ReadLine();
        reader.Close();
        if (packet == null)
        {
            Console.WriteLine("No input found");
            return;
        }
        else
        {   // deel 1
            // Copilot chat gevraagd naar algoritmes voor het oplossen van deze puzzel.
            // Sliding window approach gebruikt. 

            int packageMarkerIndex = -1;
            for (int i = 0; i < packet.Length - 4; i++)
            {
                if (IsUnique(packet.Substring(i, 4)))
                {
                    packageMarkerIndex = i + 4;
                    break;
                }
            }

            // deel 2
            int messageMarkerIndex = -1;
            for (int i = 0; i < packet.Length - 14; i++)
            {
                if (IsUnique(packet.Substring(i, 14)))
                {
                    messageMarkerIndex = i + 14;
                    break;
                }
            }

            Console.WriteLine("Total score part 1: {0}", packageMarkerIndex);
            Console.WriteLine("Total score part 2: {0}", messageMarkerIndex);

        }

        // wait for input before exiting
        Console.WriteLine("Press enter to finish");
        Console.ReadLine();
    }


}