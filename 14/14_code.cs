public class Material {
    public (int x, int y) position;
    public string elementSign = " ";
    public string elementName = "NA";
    public bool passable;
    public bool inRest;

    virtual public string Show() {
        return elementSign;
    }
}

public class Air : Material {

    public Air(int x, int y) {
        position = (x, y);
        elementSign = ".";
        elementName = "Air";
        inRest = true;
        passable = true;
    }

}

public class Sand : Material {

    public Sand(int x, int y) {
        position = (x, y);
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

}

public class Rock : Material {
    
        public Rock(int x, int y) {
            position = (x, y);
            elementSign = "#";
            elementName = "Rock";
            inRest = true;
            passable = false;
        }
}

public class Program
{

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
            (int x, int y) start = (-1,-1);
            foreach (string node in nodes) {
                int _x = int.Parse(node.Split(",")[0]);
                int _y = int.Parse(node.Split(",")[1]);
                if (start == (-1,-1)) {
                    start = (_x,_y);
                } 
                // check if materials[_y] already exists
                if (materials.Count < _y) {
                    for (int i = materials.Count; i <= _y ; i++) {
                        // Console.WriteLine("Adding row {0} to materials",i);
                        //initialize the rows with 1000 instances of Air
                        List<Material> row = new List<Material>();
                        for (int j = 0; j < 1000; j++ ){
                            row.Add(new Air(j,i));
                        }
                        materials.Add(row);
                    }
                }
                // Console.WriteLine("Adding {0} to materials[{1}][{2}]",node,_y,_x);
                materials[_y][_x] = new Rock(_x,_y);
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

        // draw the materials
        foreach (List<Material> row in materials) {
            foreach (Material material in row) {
                Console.Write(material.Show());
            }
            Console.WriteLine();
        }

        // Console.WriteLine("Total score part 1: {0}",);
        // Console.WriteLine("Total score part 2: {0}",);


        // wait for input before exiting
        Console.WriteLine("Press enter to finish");
        Console.ReadLine();
    }
}

