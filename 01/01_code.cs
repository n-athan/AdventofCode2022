//initialize reader to read the input file.
// StreamReader reader = new StreamReader("01_demo.txt");
StreamReader reader = new StreamReader("01_input.txt");


//initialize Calories List
List<int> cals = new List<int>();


// loop over each line in the file
int sum = 0;
string? line;
while ((line = reader.ReadLine()) != null)
{
    // if it is a blank line, we have counted all the calories one elf carries. Add the calorie sum to the list.
    if (string.IsNullOrWhiteSpace(line)) {
        // add sum to list
        cals.Add(sum);
        sum = 0;
    } else {
        // if the line is not empty, add the amount of calories to the temporary sum that the elf carries.
        sum += Convert.ToInt32(line);
    }
}
// add last sum to list
cals.Add(sum);

// convert list to array and sort it
int[] calsArray = cals.ToArray();
Array.Sort(calsArray);
Array.Reverse(calsArray);

// Deel 1 
Console.WriteLine(Convert.ToString(calsArray[0]));

// Deel 2
int top3sum = calsArray[0..3].Sum();
Console.WriteLine(Convert.ToString(top3sum));

// wait for input before exiting
Console.WriteLine("Press enter to finish");
Console.ReadLine();


// n-athan: how do i run 01_code.cs?

// GitHub Copilot: To run a C# file in Visual Studio Code, you need to follow these steps:

// 1. Install the .NET Core SDK if you haven't already. You can download it from the official Microsoft website.
// 2. Open the terminal in Visual Studio Code. You can do this by going to `View -> Terminal` or by pressing `Ctrl + ` `.
// 3. Navigate to the directory containing your `01_code.cs` file using the `cd` command.
// 4. Run the command `dotnet new console -o myApp` to create a new console application. This will create a new folder named `myApp` with a simple console application.
// 5. Replace the `Program.cs` file in the `myApp` folder with your `01_code.cs` file.
// 6. Navigate into the `myApp` directory with `cd myApp`.
// 7. Run the command `dotnet run` to compile and run your application.

// Please note that this is a general way to run C# files and might need adjustments based on your specific project setup.

// For your next steps, you might want to explore how to debug your C# code in Visual Studio Code, or how to add additional C# files to your project.