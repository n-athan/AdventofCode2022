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

## Difference break and continue
In programming, break and continue are two control flow statements that are used in loops.

break: This statement is used to completely exit the loop prematurely. It stops the execution of the loop immediately, and program control resumes at the next statement following the loop.

continue: This statement is used to skip the rest of the current loop iteration and immediately start the next iteration. It stops the execution of the current iteration and jumps to the next iteration of the loop.

Here's an example in C# to illustrate the difference:
```c#
for (int i = 0; i < 10; i++)
{
    if (i == 5)
    {
        break;  // This will exit the loop completely. The loop will not iterate for i = 6, 7, 8, 9.
    }
    Console.WriteLine(i);
}

for (int i = 0; i < 10; i++)
{
    if (i == 5)
    {
        continue;  // This will skip the rest of the loop for i = 5 and immediately continue with i = 6.
    }
    Console.WriteLine(i);
}
```
In the first loop, the break statement causes the loop to exit completely when i equals 5. In the second loop, the continue statement causes the loop to skip the rest of the iteration when i equals 5 and immediately continue with the next iteration.