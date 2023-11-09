//initialize reader to read the input file.
// StreamReader reader = new StreamReader("02_demo.txt");
StreamReader reader = new StreamReader("02_input.txt");

// Lookup tabel maken voor alle mogelijke spellen. Dat is een beperkte set, dus dat is sneller hardcoded.
Dictionary<string, int> games = new Dictionary<string, int>();

games.Add("A X",4); // opponent: rock, you: rock
games.Add("A Y",8); // opponent: rock, you: paper
games.Add("A Z",3); // opponent: rock, you: scissors
games.Add("B X",1); // opponent: paper, you: rock
games.Add("B Y",5); // opponent: paper, you: paper
games.Add("B Z",9); // opponent: paper, you: scissors
games.Add("C X",7); // opponent: scissors, you: rock
games.Add("C Y",2); // opponent: scissors, you: paper
games.Add("C Z",6); // opponent: scissors, you: scissors


Dictionary<string, int> games2 = new Dictionary<string, int>();

games2.Add("A X",3); // opponent: rock,     goal: lose,     you: sicssors
games2.Add("A Y",4); // opponent: rock,     goal: draw,     you: rock
games2.Add("A Z",8); // opponent: rock,     goal: win,      you: paper
games2.Add("B X",1); // opponent: paper,    goal: lose,     you: rock
games2.Add("B Y",5); // opponent: paper,    goal: draw,     you: paper
games2.Add("B Z",9); // opponent: paper,    goal: win,      you: scissors
games2.Add("C X",2); // opponent: scissors, goal: lose,     you: paper
games2.Add("C Y",6); // opponent: scissors, goal: draw,     you: scissors
games2.Add("C Z",7); // opponent: scissors, goal: win,      you: rock


string? line;
int totalScore = 0;
int totalScore2 = 0;

while ((line = reader.ReadLine()) != null) {
    totalScore += games[line];
     totalScore2 += games2[line];
}

Console.WriteLine("Total score part 1: {0}",totalScore);
Console.WriteLine("Total score part 2: {0}",totalScore2);

// wait for input before exiting
Console.WriteLine("Press enter to finish");
Console.ReadLine();

