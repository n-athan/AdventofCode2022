public class NodeTests
{
    [Test]
    public void TestParse()
    {
        string file = @"..\..\..\..\Source\07_demo.txt";

        Node root = Program.ParseInputFile(file);

        // Test parsing of demo file

        // root node is a dir with 4 children
        Assert.That(root.Name, Is.EqualTo("/"));
        Assert.That(root.Children.Count, Is.EqualTo(4));

        // first child is a dir 'a' with 4 children. Directories have no size yet after parsing.
        Assert.That(root.Children[0].Name, Is.EqualTo("a"));
        Assert.That(root.Children[0].Children.Count, Is.EqualTo(4));
        Assert.That(root.Children[0].Size, Is.EqualTo(0));

        // second child is a file 'b.txt' with a specific size. Files have no children.
        Assert.That(root.Children[1].Name, Is.EqualTo("b.txt"));
        Assert.That(root.Children[1].Size, Is.EqualTo(14848514));
        Assert.That(root.Children[1].Children.Count, Is.EqualTo(0));
    }

    [Test]
    public void TestSize()
    {
        string file = @"..\..\..\..\Source\07_demo.txt";
        Node root = Program.ParseInputFile(file);
        Node.AnalyzeLeaves(root); 

        // Part 1
        int totalSize = Node.GetTotalSize(root);
        Assert.That(totalSize, Is.EqualTo(95437));

        // Part 2
        int freeSpace = 70000000 - root.Size;
        int spaceNeeded = 30000000 - freeSpace;
        List<Node> dirs = Node.GetDirectoriesOfSize(root, spaceNeeded);
        Node smallestDir = dirs.OrderBy(node => node.Size).First();
        Assert.That(smallestDir.Size, Is.EqualTo(24933642));
    }
}