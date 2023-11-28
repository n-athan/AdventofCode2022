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

    public bool Move(List<List<Material>> materials, (int x, int y) position, int left) {
        int _x = position.x;
        int _y = position.y;

        if (_y == materials.Count-1) {
            // the sand is at the bottom, remove it (falls forever)
            materials[_y][_x] = new Air();
            Program.SpawnSand(materials, (501-left,0));
            // all posible sand is in rest, finished part 1
            return true;
        } 

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
                    if (_x == 501-left && _y == 0) {
                        // the sand is in rest at the top, finished part 2
                        return true;
                    }
                    Program.SpawnSand(materials, (501-left,0));
                }
            }
        }
        return false;
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

    public static bool Update(List<List<Material>> materials, int left) {
        // update the state of the materials, i.e. the falling sand
        for (int i = materials.Count-1; i >= 0; i--) {
            for(int j = 0; j < materials[i].Count; j++) {
                if (materials[i][j].elementName == "Sand") {
                    Sand sand = (Sand)materials[i][j];
                    if (!sand.inRest) {
                        return sand.Move(materials,(j,i),left);
                    }
                }
            }
        }
        return false;
    }

    public static void SpawnSand(List<List<Material>> materials, (int x, int y) sandhole) {
        // spawn sand at the top of the sandhole
        materials[sandhole.y][sandhole.x] = new Sand();
    }

    public static void Main(string[] args)
    {
        string file = "14_demo.txt";
        int part = 1;
        if (args.Length == 0)
        {
            Console.WriteLine("No input file specified, running demo input: 14_demo.txt");
        }
        else if (args.Length == 1)
        {
            part = int.Parse(args[0]);
        }
        else if (args.Length == 2)
        {
            part = int.Parse(args[0]);
            file = args[1];
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
                if (materials.Count <= _y) {
                    for (int i = materials.Count; i <= _y ; i++) {
                        //initialize the rows with 1000 instances of Air
                        // Console.WriteLine("Adding row {0}",i);
                        List<Material> row = new List<Material>();
                        for (int j = 0; j < 1000; j++ ){
                            row.Add(new Air());
                        }
                        materials.Add(row);
                    }
                }

                // add the location as a Rock.
                // Console.WriteLine("Adding rock at {0},{1}",_x,_y);
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
        }
        reader.Close();

        // add floor for part 2
        if (part == 2) {
            // add air layer
            List<Material> airlayer = new List<Material>();
            for (int i = 0; i < 1000; i++ ){
                airlayer.Add(new Air());
            }
            materials.Add(airlayer);
            // add floor
            List<Material> floor = new List<Material>();
            for (int i = 0; i < 1000; i++ ){
                floor.Add(new Rock());
            }
            materials.Add(floor);

            // the sand will form a triangle, so the left and right columns are at the same distance from the center. Distance is dependent on the height.
            left = 500 - (materials.Count-1);
            right = 500 + (materials.Count-1);
        }

        // remove the last 1000 - right columns
        Console.WriteLine("Left: {0}, Right: {1}",left,right);
        foreach (List<Material> row in materials) {
            row.RemoveRange(right+2,row.Count-(right+2));
        }
        // remove the first 1000 - left columns
        foreach (List<Material> row in materials) {
            row.RemoveRange(0,left-1);
        }


        SpawnSand(materials, (501-left,0));

        bool finished = false;
        while (!finished)
        {
            finished = Update(materials, left);
            // draw the state (animated)
            if (file == "14_demo.txt") {
                Console.Clear(); // Clear the console
                drawState(materials);
                System.Threading.Thread.Sleep(50); // Wait for half a second
            }
        }

        int sandInRest = materials.Sum(row => row.Count(material => material.elementName == "Sand" && material.inRest));

        Console.WriteLine("Total score: {0}", sandInRest);


        // wait for input before exiting
        Console.WriteLine("Press enter to finish");
        Console.ReadLine();
    }
}

