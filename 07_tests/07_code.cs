public class Node
{
    // properties of nodes. Keep file/dir Name, Size, Parent and Children
    public string Name { get; set; }
    public List<Node> Children { get; set; }
    public Node? Parent { get; set; }
    // directories have size 0, files have size > 0. Later directory size will be calculated as sum of children.
    public int Size { get; set; }

    // root Node, no parent
    public Node(string name)
    {
        Name = name;
        Children = new List<Node>();
        Parent = null;
        Size = 0;
    }

    // directories
    public Node(string name, Node parent)
    {
        Name = name;
        Children = new List<Node>();
        Parent = parent;
        Size = 0;
    }

    //files
    public Node(string name, Node parent, int size)
    {
        Name = name;
        Children = new List<Node>();
        Parent = parent;
        Size = size;
    }

    // recursive function to analyze, calculate and add the size of all children to their parent. 
    public static void AnalyzeLeaves(Node node)
    {
        // Output the name and size of the current node
        // Console.WriteLine("{0} {1}", node.Name, node.Size); // for debugging

        // If this node has children, analyze them
        foreach (Node child in node.Children)
        {
            AnalyzeLeaves(child);
        }

        // Add the size of this node to its parent
        if (node.Parent != null)
        {
            node.Parent.Size += node.Size;
        }
    }

    // recursive function to add all the sizes of the children to the total size, if the size of the node is < 100000. 
    // this will contain 'duplicates', but that is requested in the puzzle. 
    public static int GetTotalSize(Node node)
    {
        int totalSize = 0;

        if (node.Size < 100000 && node.Children.Count > 0)
        {
            totalSize = node.Size;
        }

        foreach (Node child in node.Children)
        {
            totalSize += GetTotalSize(child);
        }

        return totalSize;
    }

    public static List<Node> GetDirectoriesOfSize(Node node, int minimumSize) {
        List<Node> directories = new List<Node>();
        if (node.Size >= minimumSize && node.Children.Count > 0) {
            directories.Add(node);
        }
        foreach (Node child in node.Children)
        {
            directories.AddRange(GetDirectoriesOfSize(child, minimumSize));
        }
        return directories;
    }

}

public class Program
{
    public static Node ParseInputFile(string path)
    {        
        //initialize reader to read the input file.
        StreamReader reader = new StreamReader(path);
        string? line;
        Node root = new Node("/");
        Node? dir = root;

        ////////////////////
        // read the input file and create a tree
        ////////////////////
        
        while ((line = reader.ReadLine()) != null)
        {
            // change directory (node)
            if (line.Substring(0, 4) == "$ cd")
            {
                string location = line.Substring(5);

                if (location == "..")
                {
                    dir = dir.Parent;
                }
                else if (location == "/")
                {
                    dir = root;
                }
                else
                {
                    // Check if child node already exists
                    Node? existingNode = dir.Children.FirstOrDefault(child => child.Name == location);

                    if (existingNode == null)
                    {
                        // Child node does not exist, so add it
                        Node newNode = new Node(location, dir);
                        dir.Children.Add(newNode);
                        dir = newNode;
                    }
                    else
                    {
                        // Child node already exists, so use it as the current directory
                        dir = existingNode;
                    }
                }
            }
            // list directory (node). Do nothing, as the tree is already created.
            else if (line.Substring(0, 4) == "$ ls")
            {
                continue;
            }
            // when a directory is listed. Check if it already exists, if not, create it, as a node.
            else if (line.Substring(0, 3) == "dir")
            {
                string location = line.Substring(4);
                // Check if child node already exists
                Node? existingNode = dir.Children.FirstOrDefault(child => child.Name == location);
                if (existingNode == null)
                {
                    // Child node does not exist, so add it
                    Node newNode = new Node(location, dir);
                    dir.Children.Add(newNode);
                }
            }
            // when a file is listed. Check if it already exists, if not, create it as a (leaf) node.
            else
            {
                string[] fileItem = line.Split(" ");

                // Check if child node already exists
                Node? existingNode = dir.Children.FirstOrDefault(child => child.Name == fileItem[1]);
                if (existingNode == null)
                {
                    // Child node does not exist, so add it
                    Node newNode = new Node(fileItem[1], dir, Int32.Parse(fileItem[0]));
                    dir.Children.Add(newNode);
                }

            }
        }
        reader.Close();
        return root;
    }

    public static void Main(string[] args)
    {
        string file = "07_demo.txt";
        if (args.Length == 0)
        {
            Console.WriteLine("No input file specified, running demo input: 07_demo.txt");
        }
        else
        {
            file = args[0];
        }

        Node root = ParseInputFile(file);        

        ////////////////////
        // use the root tree to calculate the different puzzle answer.
        ////////////////////

        // part 1
        Node.AnalyzeLeaves(root); 
        int totalSize = Node.GetTotalSize(root);

        // part 2
        int freeSpace = 70000000 - root.Size;
        int spaceNeeded = 30000000 - freeSpace;
        List<Node> dirs = Node.GetDirectoriesOfSize(root, spaceNeeded);
        // get node with the smallest size, 
        Node smallestDir = dirs.OrderBy(node => node.Size).First();

        Console.WriteLine("Total score part 1: {0}", totalSize);
        Console.WriteLine("Total score part 2: {0}", smallestDir.Size);

        // wait for input before exiting
        Console.WriteLine("Press enter to finish");
        Console.ReadLine();
    }
}

