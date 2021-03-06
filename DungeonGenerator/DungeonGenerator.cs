
using System.Text;

namespace DungeonGenerator;

internal class DungeonGenerator
{
    protected Random rand;
    protected List<DungeonCell> map;
    protected int roomLimit;
    public int mapWidth, mapHeight;
    public string[,] mapArray;

    public DungeonGenerator(int width, int height)
    {
        rand = new Random();
        map = new List<DungeonCell>();
        mapWidth = width;
        mapHeight = height;
        mapArray = new string[width, height];

        InitializeMap();
    }

    private void InitializeMap()
    {
        roomLimit = rand.Next((int)(mapWidth * .2f)) + (int)(mapWidth * .1f);

        for (var i = 0; i < mapHeight; i++)
        {
            for (var j = 0; j < mapWidth; j++)
            {
                mapArray[j, i] = " ";
            }
        }
    }

    public void GenerateEmptyMap()
    {
        int generationAttemptCount;
        var retry = 0;
        for (generationAttemptCount = 0; map.Count < roomLimit; generationAttemptCount++)
        {
            BuildOutMap();

            if (generationAttemptCount - (retry * 100000) > 100000)
            {
                retry++;
                InitializeMap();
                Console.WriteLine($"Failed to generate map a after {generationAttemptCount} iterations with size {roomLimit}, retry #{retry}.");
            }

            if (retry > 100)
            {
                Console.WriteLine("Failure to generate map.");
                break;
            }
        }
        Console.WriteLine($"rooms: {roomLimit}, attempts: {generationAttemptCount} avg/room: {generationAttemptCount / roomLimit}");
        FillMapArray();
    }

    private void BuildOutMap()
    {
        while (map.Count <= 0)
        {
            var x = rand.Next(mapWidth);
            var y = rand.Next(mapHeight);
            var w = rand.Next(10) + 15;
            var h = rand.Next(10) + 15;

            // make sure the room is within map bounds
            if (x < 0 || x > mapWidth || y < 0 || y > mapHeight || x + w > mapWidth || y + h > mapHeight)
            {
                continue;
            }
            var newRoom = new DungeonCell(x, y, w, h, map.Count);
            map.Add(newRoom);
            return;
        }

        var tempIndex = rand.Next((map.Count - 1) == 0 ? 1 : map.Count - 1);
        var tempDungeonCell = map[tempIndex];

        // all 4 walls already have a room attached to it
        if (tempDungeonCell.AttachedCells[0] != -1 &&
            tempDungeonCell.AttachedCells[1] != -1 &&
            tempDungeonCell.AttachedCells[2] != -1 &&
            tempDungeonCell.AttachedCells[3] != -1)
        {
            return;
        }
        else
        {
            var whichWall = rand.Next(4);

            // search for an empty wall
            while (tempDungeonCell.AttachedCells[whichWall] != -1)
            {
                whichWall = rand.Next(4);
            }
            int newX, newY;

            // set up new room
            var newWidth = rand.Next(19) + 10;
            var newHeight = rand.Next(19) + 10;

            var corridorLength = rand.Next(20) + 1;

            switch (whichWall)
            {
                case 0:
                    newY = tempDungeonCell.Y - corridorLength - newHeight;
                    newX = rand.Next(tempDungeonCell.Width - 2) + tempDungeonCell.X - 1;
                    break;
                case 1:
                    newX = tempDungeonCell.X + tempDungeonCell.Width + corridorLength;
                    newY = rand.Next(tempDungeonCell.Height - 2) + tempDungeonCell.Y - 1;
                    break;
                case 2:
                    newY = tempDungeonCell.Y + tempDungeonCell.Height + corridorLength;
                    newX = rand.Next(tempDungeonCell.Width - 2) + tempDungeonCell.X - 1;
                    break;
                case 3:
                    newX = tempDungeonCell.X - corridorLength - newWidth;
                    newY = rand.Next(tempDungeonCell.Height - 2) + tempDungeonCell.Y - 1;
                    break;
                default:
                    Console.WriteLine("ERROR");
                    newY = -100;
                    newX = -100;
                    break;
            }

            // check if new room is within game map bounds
            if (newX < 0 || newX > mapWidth || newY < 0 || newY > mapHeight ||
                newWidth + newX > mapWidth || newHeight + newY > mapHeight)
            {
                return;
            }

            // check if new room collides with any other room
            for (var i = 0; i < map.Count; i++)
            {
                if (!(newX > map[i].X + map[i].Width || newX + newWidth < map[i].X ||
                newY > map[i].Y + map[i].Height || newY + newHeight < map[i].Y))
                {
                    return;
                }
            }

            // add new room to map
            var newRoom = new DungeonCell(newX, newY, newWidth, newHeight, map.Count);
            // set the empty wall to the index of a new room
            tempDungeonCell.SetAttachedCell(whichWall, map.Count);
            // set the wall of the new room to be the temp room
            newRoom.SetAttachedCell(GetOppositeWall(whichWall), tempIndex);

            // add new room to the map array
            map.Add(newRoom);
        }
    }

    private int GetOppositeWall(int val)
    {
        var oppositeWallLookupList = new List<int>() { 2, 3, 0, 1 };
        return oppositeWallLookupList[val];
    }

    private void FillMapArray()
    {
        for (var i = 0; i < map.Count; i++)
        {
            for (var j = 0; j < map[i].Height; j++)
            {
                for (var k = 0; k < map[i].Width; k++)
                {
                    // set actual walls
                    if (j == 0)
                    {
                        mapArray[map[i].X + k, map[i].Y + j] = "W"; //wall
                    }
                    else if (k == 0)
                    {
                        mapArray[map[i].X + k, map[i].Y + j] = "A"; //wall
                    }
                    else if (j == map[i].Height - 1)
                    {
                        mapArray[map[i].X + k, map[i].Y + j] = "X"; //wall
                    }
                    else if (k == map[i].Width - 1)
                    {
                        mapArray[map[i].X + k, map[i].Y + j] = "D"; //wall
                    }
                    else // set floor
                    {
                        mapArray[map[i].X + k, map[i].Y + j] = "_"; //floor
                    }

                    // label center of rooms with i
                    if (k == map[i].Width / 2 && j == map[i].Height / 2)
                    {
                        mapArray[map[i].X + k, map[i].Y + j] = "_";
                        //mapArray[map[i].X + k, map[i].Y + j] = i.ToString().Substring(0, 1);//Integer.toString(i).substring(0,1);
                    }
                    if (k == (map[i].Width / 2) + 1 && j == map[i].Height / 2 && i > 9)
                    {
                        mapArray[map[i].X + k, map[i].Y + j] = "_";
                        //mapArray[map[i].X + k, map[i].Y + j] = i.ToString().Substring(1, 1);//Integer.toString(i).substring(1,2)
                    }
                }
            }

            // set corner pieces as different chars
            mapArray[map[i].X, map[i].Y] = "Q";
            mapArray[map[i].X + map[i].Width - 1, map[i].Y] = "E";
            mapArray[map[i].X, map[i].Y + map[i].Height - 1] = "Z";
            mapArray[map[i].X + map[i].Width - 1, map[i].Y + map[i].Height - 1] = "C";

            DungeonCell mainRoom = map[i];

            // make corridors
            if (mainRoom.AttachedCells[0] != -1 && mainRoom.AttachedCells[0] > i)
            {
                var attachedRoomN = map[mainRoom.AttachedCells[0]];

                //0,0 is top right
                var corridorLength = mainRoom.Y - (attachedRoomN.Y + attachedRoomN.Height);
                var foundX = FindX(mainRoom, attachedRoomN, 0);
                while (corridorLength > 0)
                {
                    mapArray[foundX - 1, mainRoom.Y - corridorLength] = "A";
                    mapArray[foundX, mainRoom.Y - corridorLength] = "|";
                    mapArray[foundX + 1, mainRoom.Y - corridorLength] = "D";
                    corridorLength--;
                }
            }
            if (mainRoom.AttachedCells[1] != -1 && mainRoom.AttachedCells[1] > i)
            {
                var attachedRoomE = map[mainRoom.AttachedCells[1]];

                var corridorLength = attachedRoomE.X - (mainRoom.X + mainRoom.Width);
                var foundX = FindX(mainRoom, attachedRoomE, 1);
                while (corridorLength > 0)
                {
                    mapArray[mainRoom.X + mainRoom.Width - 1 + corridorLength, foundX - 1] = "W";
                    mapArray[mainRoom.X + mainRoom.Width - 1 + corridorLength, foundX] = "-";
                    mapArray[mainRoom.X + mainRoom.Width - 1 + corridorLength, foundX + 1] = "X";
                    corridorLength--;
                }
            }
            if (mainRoom.AttachedCells[2] != -1 && mainRoom.AttachedCells[2] > i)
            {
                var attachedRoomS = map[mainRoom.AttachedCells[2]];

                var corridorLength = attachedRoomS.Y - (mainRoom.Y + mainRoom.Height);
                var foundX = FindX(mainRoom, attachedRoomS, 0);
                while (corridorLength > 0)
                {
                    mapArray[foundX - 1, mainRoom.Y + mainRoom.Height - 1 + corridorLength] = "A";
                    mapArray[foundX, mainRoom.Y + mainRoom.Height - 1 + corridorLength] = "|";
                    mapArray[foundX + 1, mainRoom.Y + mainRoom.Height - 1 + corridorLength] = "D";
                    corridorLength--;

                }
            }
            if (mainRoom.AttachedCells[3] != -1 && mainRoom.AttachedCells[3] > i)
            {
                var attachedRoomW = map[mainRoom.AttachedCells[3]];

                var corridorLength = mainRoom.X - (attachedRoomW.X + attachedRoomW.Width);
                var foundX = FindX(mainRoom, attachedRoomW, 1);
                while (corridorLength > 0)
                {
                    mapArray[mainRoom.X - corridorLength, foundX - 1] = "W";
                    mapArray[mainRoom.X - corridorLength, foundX] = "-";
                    mapArray[mainRoom.X - corridorLength, foundX + 1] = "X";
                    corridorLength--;
                }
            }
        }
        
        // the below code once again goes over the array and turns the door signal into a hole in the walls
        for (var i = 1; i < mapWidth - 1; i++)
        {
            for (var j = 1; j < mapHeight - 1; j++)
            {
                // walls are wdxa
                if (mapArray[i, j].Equals("|")) // NS
                {
                    mapArray[i, j] = "#";//hallway

                    if (mapArray[i, j - 1].Equals("X"))
                    {
                        mapArray[i, j - 1] = "I"; //wall
                    }
                    if (mapArray[i, j + 1].Equals("W"))
                    {
                        mapArray[i, j + 1] = "K"; //wall
                    }
                }
                if (mapArray[i, j].Equals("-")) // EW
                {
                    mapArray[i, j] = "^";//hallway

                    if (mapArray[i - 1, j].Equals("D"))
                    {
                        mapArray[i - 1, j] = "J";
                    }
                    if (mapArray[i + 1, j].Equals("A"))
                    {
                        mapArray[i + 1, j] = "M";
                    }
                }
            }
        }
    }

    private int FindX(DungeonCell main, DungeonCell dc, int mode)
    {
        // top and bottom
        var placement = 0;
        if (mode == 0)
        {
            var min = main.X + 1;
            var max = main.X + main.Width - 2;
            if (dc.X + 1 > min)
            {
                min = dc.X + 1;
            }
            if (dc.X + dc.Width - 2 < max)
            {
                max = dc.X + dc.Width - 2;
            }
            placement = rand.Next(max - min) + min;
        }
        // left right
        else if (mode == 1)
        {
            var min = main.Y + 1;
            var max = main.Y + main.Height - 2;
            if (dc.Y + 1 > min)
            {
                min = dc.Y + 1;
            }
            if (dc.Y + dc.Height - 2 < max)
            {
                max = dc.Y + dc.Height - 2;
            }
            placement = rand.Next(max - min) + min;
        }
        return placement;
    }

    public void PrintMap()
    {
        var sb = new StringBuilder();
        for (var i = 0; i < mapHeight; i++)
        {
            for (var j = 0; j < mapWidth; j++)
            {
                _ = sb.Append(mapArray[j, i]);
            }
            sb.Append('\n');
        }

        //File.WriteAllText("output.txt", sb.ToString());

        Console.WriteLine(sb.ToString());
    }
}
