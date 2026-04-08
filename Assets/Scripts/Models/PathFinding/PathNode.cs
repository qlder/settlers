public class PathNode
{
    public int X;
    public int Z;

    public int GCost;
    public int HCost;

    public int FCost
    {
        get { return GCost + HCost; }
    }

    public PathNode Parent;

    public PathNode(int x, int z)
    {
        X = x;
        Z = z;
        GCost = int.MaxValue;
        HCost = 0;
        Parent = null;
    }
}