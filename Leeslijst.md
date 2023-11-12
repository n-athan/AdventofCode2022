# vragen over C# onderdelen. 
[ ] `using` voor de Streamreader. Wanneer wordt de reader dan opgeruimd? Wanneer is het nuttig i.t.t. de gewone garbage collector?
[ ] Error handling en try/catch/finally
[ ] Wanneer zou je File.ReadLines of File.ReadAllLines gebruiken, in plaats van Streamreader. 
[ ] Wat betekent `static` en waar is het wel of niet nodig/gebruikelijk?
[ ] Wanneer update een functie een (globale) variable en wanneer alleen een lokale variabele? i.e. Value Types en Reference Types.

## Functies als Class property
```csharp
public class Monkey
{
    public Func<int, int, int> Operation { get; set; }

    public Monkey(Func<int, int, int> operation)
    {
        Operation = operation;
    }
}

public class Program
{
    static void Main(string[] args)
    {
        Monkey monkey = new Monkey((a, b) => a + b);
        int result = monkey.Operation(2, 3);  // result is 5
    }
}
```

The Func<int, int, int> delegate type in C# represents a function that takes two int parameters and returns an int. The last type parameter is always the return type, and the preceding type parameters are the input types.

If you see a Func with three int type parameters like Func<int, int, int, int>, it represents a function that takes three int parameters and returns an int. The number of type parameters in Func minus one gives you the number of input parameters to the function, as the last type parameter is always the return type.