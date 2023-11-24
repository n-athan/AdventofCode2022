// polymorfisme
public abstract class Operation
{
    public abstract int Calculate(int a, int b);

    // omdat de Calculate functie abstract is, wordt in deze helper de Calculate van de subclass gebruikt. 
    // maar de functie ShowAnswer is niet abstract, dus hoeft zelf niet te worden overschreven in de subclasses. 
    public void ShowAnswer(int a, int b) {
        Console.WriteLine(Calculate(a,b));
    }
}

public class Addition : Operation
{
    override public int Calculate(int a, int b)
    {
        return a + b;
    }
}

public class Multiplication : Operation
{
    override public int Calculate(int a, int b)
    {
        return a * b;
    }

}

public class Program
{

    public static void Main()
    {
        // List<Operation> ops = new List<Operation>();
        // var ops = new List<Operation>();
        // List<Operation> ops = new ();

        List<Operation> ops = new() {new Addition(), new Multiplication()};

        ops.ForEach(o => o.ShowAnswer(3,4));
    }

}