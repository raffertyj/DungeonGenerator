
namespace DungeonGenerator;

internal class DungeonCell
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int RoomIndex { get; set; }

    public int[] AttachedCells { get; } //nesw-0123

    public DungeonCell(int x, int y, int width, int height, int index)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        AttachedCells = new int[4];
        for (int i = 0; i < AttachedCells.Length; i++)
        {
            AttachedCells[i] = -1;
        }
        RoomIndex = index;
    }

    public void SetAttachedCell(int index, int val)
    {
        AttachedCells[index] = val;
    }

    public void PrintCell()
    {
        Console.WriteLine($"x:{X}\ty:{Y}\tw:{Width}\th:{Height}");
    }
}
