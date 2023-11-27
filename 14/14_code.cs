public class Material {
    public string elementSign = " ";
    public string elementName = "NA";
    public bool passable;
    public bool inRest;

    virtual public string Show() {
        return elementSign;
    }
}

public class Air : Material {

    public Air() {
        elementSign = ".";
        elementName = "Air";
        inRest = true;
        passable = true;
    }

}

public class Sand : Material {

    public Sand() {
        elementSign = "+";
        elementName = "Sand";
        inRest = false;
        passable = false;
    }

    override public string Show() {
        if (inRest) {
            return "o";            
        } else {
            return elementSign;
        }
    }

    public void Move(List<List<Material>> materials, (int x, int y) position, int left) {
        int _x = position.x;
        int _y = position.y;

        if (_y == materials.Count-1) {
            // the sand is at the bottom, remove it
            materials[_y][_x] = new Air();
        } 

        // TODO sand moving out the map.

        if (materials[_y+1][_x].passable) {
            // move the sand down
            materials[_y+1][_x] = this;
            materials[_y][_x] = new Air();
        } else {
            // check if the sand can move left
            if (materials[_y+1][_x-1].passable) {
                // move the sand left
                materials[_y+1][_x-1] = this;
                materials[_y][_x] = new Air();
            } else {
                // check if the sand can move right
                if (materials[_y+1][_x+1].passable) {
                    // move the sand right
                    materials[_y+1][_x+1] = this;
                    materials[_y][_x] = new Air();
                } else {                    
                    // the sand is in rest
                    inRest = true;
                    Program.SpawnSand(materials, (500-left,0));
                }
            }
        }
    }
}

public class Rock : Material {
    
        public Rock() {
            elementSign = "#";
            elementName = "Rock";
            inRest = true;
            passable = false;
        }
}

public class Program
{
    public static void drawState(List<List<Material>> materials) {
        foreach (List<Material> row in materials) {
            foreach (Material material in row) {
                Console.Write(material.Show());
            }
            Console.WriteLine();
        }
    }

    public static void Update(List<List<Material>> materials, int left) {
        // update the state of the materials, i.e. the falling sand
        // foreach (List<Material> row in materials) {
        for (int i = materials.Count-1; i >= 0; i--) {
            for(int j = 0; j < materials[i].Count; j++) {
                if (materials[i][j].elementName == "Sand") {
                    Sand sand = (Sand)materials[i][j];
                    if (!sand.inRest) {
                        sand.Move(materials,(j,i),left);
                    }
                }
            }
        }
        // SpawnSand(materials, (500-left,0));
    }

    public static void SpawnSand(List<List<Material>> materials, (int x, int y) sandhole) {
        // spawn sand at the top of the sandhole
        materials[sandhole.y][sandhole.x] = new Sand();
    }

    public static void Main(string[] args)
    {
        string file = "14_demo.txt";
        if (args.Length == 0) {
            Console.WriteLine("No input file specified, running demo input: 14_demo.txt");
        } else {
            file = args[0];
        }
        //initialize reader to read the input file.
        StreamReader reader = new StreamReader(file);

        // initialize variables
        string? line;
        List<List<Material>> materials = new List<List<Material>>();
        int left = 1000;
        int right = 0;

        // read the input file
        while ((line = reader.ReadLine()) != null)
        {
            //Part 1
            string[] nodes = line.Split(" -> ");
            (int x, int y) prev = (-1,-1);
            foreach (string node in nodes) {
                int _x = int.Parse(node.Split(",")[0]);
                int _y = int.Parse(node.Split(",")[1]);
                if (prev == (-1,-1)) {
                    prev = (_x,_y);
                } 
                // check if row materials[_y] already exists
                if (materials.Count < _y) {
                    for (int i = materials.Count; i <= _y ; i++) {
                        //initialize the rows with 1000 instances of Air
                        List<Material> row = new List<Material>();
                        for (int j = 0; j < 1000; j++ ){
                            row.Add(new Air());
                        }
                        materials.Add(row);
                    }
                }

                // add the location as a Rock.
                materials[_y][_x] = new Rock();

                // add rocks in between the previous and current location
                if (prev.y == _y) {
                    // horizontal line
                    if (prev.x < _x) {
                        for (int i = prev.x; i <= _x; i++) {
                            materials[_y][i] = new Rock();
                        }
                    } else {
                        for (int i = _x; i <= prev.x; i++) {
                            materials[_y][i] = new Rock();
                        }
                    }
                } else {
                    // vertical line
                    if (prev.y < _y) {
                        for (int i = prev.y; i <= _y; i++) {
                            materials[i][_x] = new Rock();
                        }
                    } else {
                        for (int i = _y; i <= prev.y; i++) {
                            materials[i][_x] = new Rock();
                        }
                    }
                }

                // update previous location
                prev = (_x,_y);

                // update left and right, to remove the empty columns later
                if (_x < left) {
                    left = _x;
                }
                if (_x > right) {
                    right = _x;
                }
            }
            // Part 2

        }
        reader.Close();


        // remove the last 1000 - right columns
        Console.WriteLine("Left: {0}, Right: {1}",left,right);
        foreach (List<Material> row in materials) {
            row.RemoveRange(right+1,row.Count-(right+1));
        }
        // remove the first 1000 - left columns
        foreach (List<Material> row in materials) {
            row.RemoveRange(0,left);
        }


        SpawnSand(materials, (500-left,0));

        // draw the state (animated)
        // todo stop in end state.
        for (int i = 0; i < 200; i++)
        {
            Update(materials, left);
            Console.Clear(); // Clear the console
            drawState(materials);
            System.Threading.Thread.Sleep(50); // Wait for half a second
        }

        // Console.WriteLine("Total score part 1: {0}",);
        // Console.WriteLine("Total score part 2: {0}",);


        // wait for input before exiting
        Console.WriteLine("Press enter to finish");
        Console.ReadLine();
    }
}

