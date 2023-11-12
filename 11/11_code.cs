public class Monkey
{
    public List<int> Items { get; set; }
    public Func<int, int> Operation { get; set; }
    public Func<int, int> Test { get; }
    public int itemsInspected { get; set; }

    public Monkey(List<int> items, Func<int, int> operation, Func<int, int> test)
    {
        Items = items;
        Operation = operation;
        Test = test;
        itemsInspected = 0;
    }

    public void inspectItems()
    {
        for (int i = 0; i < Items.Count; i++)
        {
            // inspect item
            int afterInspection = Operation(Items[i]);
            // recuce worry, divide by 3 and round down to nearest integer
            int reducedWorry = afterInspection / 3;
            // update item
            Items[i] = reducedWorry;
        }
        itemsInspected += Items.Count;
    }

    public void throwItems(List<Monkey> monkeys)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            int item = Items[i];
            int newMonkey = Test(item);
            //add item to other monkey
            monkeys[newMonkey].Items.Add(item);
        }

        //remove all items from monkey
        Items.Clear();
    }

    public void printItems(int index)
    {
        Console.WriteLine("Monkey {1} has items: {0}. Total items inspected: {2}", string.Join(", ", Items), index, itemsInspected);
    }

}

public class Program
{

    static List<Monkey> monkeys = new List<Monkey>();
    // initialize variables
    static string? line;
    static int monkeyCount;
    static List<int> items = new List<int>();
    static Func<int, int> operation = (item => item);
    static int divisor;
    static int onTrue;
    static int onFalse;
    static Func<int, int> test = (item => item);

    public static void Main(string[] args)
    {
        string file = "11_demo.txt";
        if (args.Length == 0)
        {
            Console.WriteLine("No input file specified, running demo input: 11_demo.txt");
        }
        else
        {
            file = args[0];
        }
        //initialize reader to read the input file.
        StreamReader reader = new StreamReader(file);

        // read the input file and create Monkeys
        while ((line = reader.ReadLine()) != null)
        {
            // switch statement on the start of the lines
            switch (line)
            {
                case string s when s.StartsWith("Monkey"):
                    monkeyCount = int.Parse(line.Split(" ")[1].Split(":")[0]);
                    break;
                case string s when s.StartsWith("  Starting items:"):
                    var temp = line.Split("  Starting items: ")[1].Split(", ").ToList();
                    items = temp.Select(x => int.Parse(x)).ToList();
                    break;
                case string s when s.StartsWith("  Operation:"):
                    var operationString = line.Split("  Operation: ")[1];
                    List<string> operationList = operationString.Split(" ").ToList().Skip(2).ToList();

                    switch (operationList[1])
                    {
                        case "*":
                            operation = operationList[2] == "old"
                                ? (item => item * item)
                                : (item => item * int.Parse(operationList[2]));
                            break;
                        case "-":
                            operation = operationList[2] == "old"
                                ? (item => item - item)
                                : (item => item - int.Parse(operationList[2]));
                            break;
                        case "+":
                            operation = operationList[2] == "old"
                                ? (item => item + item)
                                : (item => item + int.Parse(operationList[2]));
                            break;
                        case "/":
                            operation = operationList[2] == "old"
                                ? (item => item / item)
                                : (item => item / int.Parse(operationList[2]));
                            break;
                        default:
                            break;
                    }
                    break;
                case string s when s.StartsWith("  Test: "):
                    divisor = int.Parse(line.Split("  Test: ")[1].Split(" ").Last());
                    break;
                case string s when s.StartsWith("    If true: "):
                    onTrue = int.Parse(line.Split("    If true: ")[1].Split(" ").Last());
                    break;
                case string s when s.StartsWith("    If false: "):
                    onFalse = int.Parse(line.Split("    If false: ")[1].Split(" ").Last());
                    // when not creating a new local variable, the reference of the variable is used in the lambda expression
                    // which would make all the Test functions the same. 
                    int localdivisor = divisor;
                    int localtrue = onTrue;
                    int localfalse = onFalse;
                    test = (item => item % localdivisor == 0 ? localtrue : localfalse);

                    break;
                // case empty line, we collected all the info for this monkey
                case string s when s == "":
                    monkeys.Add(new Monkey(items, operation, test));
                    break;
                default:
                    // do something else
                    break;
            }
        }
        // last monkey
        monkeys.Add(new Monkey(items, operation, test));
        reader.Close();

        // Part 1
        int rounds = 20;

        while (rounds > 0)
        {
            for (int m = 0; m < monkeys.Count; m++)
            {
                monkeys[m].inspectItems();
                monkeys[m].throwItems(monkeys);
            }
            rounds--;
        }

        // print Monkey item statistics
        for (int m = 0; m < monkeys.Count; m++)
        {
            monkeys[m].printItems(m);
        }

        List<Monkey>sorted = monkeys.OrderByDescending(x => x.itemsInspected).ToList();
        int monkeyBusiness = sorted[0].itemsInspected * sorted[1].itemsInspected; 

        Console.WriteLine("Total monkeybusiness part 1: {0}", monkeyBusiness);
        // Console.WriteLine("Total score part 2: {0}",);


        // wait for input before exiting
        Console.WriteLine("Press enter to finish");
        Console.ReadLine();
    }
}

