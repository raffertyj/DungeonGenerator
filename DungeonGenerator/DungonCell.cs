
namespace DungeonGenerator;

internal class DungeonCell
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int RoomIndex { get; set; }

    private int[] AttachedCells { get; } //nesw-0123

    public DungeonCell(int x, int y, int width, int height, int idx)
    {
        this.X = x;
        this.Y = y;
        this.Width = width;
        this.Height = height;
        AttachedCells = new int[4];
        for (int i = 0; i < AttachedCells.Length; i++)
        {
            AttachedCells[i] = -1;
        }
        this.RoomIndex = idx;
    }

    public int GetAttachedCell(int i)
    {
        return AttachedCells[i];
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
