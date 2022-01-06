# DungeonGenerator
A 2D map generator to be used in a game as a dungeon generator. Currently every x,y coord in the map is a character representing something different (NESW walls, corners, floors, corridors/walls, doors), this makes it very easy to iterate over the map and draw different textures to the screen in a game engine. Right now every room has 4 walls (naturally) and a maximum of one connected room on that wall. A random number of rooms are generated, but if too many failures occur while trying to draw the map we reset and start over. A failure is when rooms improperly overlap or we just cant fit an expected number of rooms.

# Usage
```
var mapWidth = 50;
var mapHeight = 50;
var dg = new DungeonGenerator(mapWidth, mapHeight);
dg.GenerateEmptyMap();
dg.PrintMap(); // show the example output below
```

# Example output from above example
rooms: 6, attempts: 269 avg/room: 44
```
                                    QWWWWWWWWWWWE
                                    A___________D
                                    A___________D
                                    A___________D
                                    A___________D
                                    A___________D
                                    A___________D
                                    A___________D
QWWWWWWWWWWE                        A___________D
A__________D QWWWWWWWWWWWWWWWWWWWE  A___________D
A__________D A___________________D  A___________D
A__________D A___________________D  ZXXXXXIXXXXXC
A__________D A___________________D        #
A__________D A___________________D QWWWWWWKWWWWWE
A__________D A___________________D A____________D
A__________D A___________________D A____________D
A__________D A___________________D A____________D
A__________D A___________________D A____________D
A__________J^M___________________D A____________D
A__________D A___________________J^M____________D
ZXXXXXXXXXXC A___________________D A____________D
             A___________________D A____________D
             ZXXIXXXXXXXXXXXXXXXXC A____________D
                #                  A____________D
        QWWWWWWWKWWWWWWWWWWWE      A____________D
        A___________________D      A____________D
        A___________________D      A____________D
        A___________________D      A____________D
        A___________________D      ZXXXXXXXXXXXXC
        A___________________D
        A___________________D
        A___________________D QWWWWWWWWWWWWE
        A___________________D A____________D
        A___________________D A____________D
        A___________________D A____________D
        A___________________J^M____________D
        A___________________D A____________D
        A___________________D A____________D
        A___________________D A____________D
        A___________________D A____________D
        ZXXXXXXXXXXXXXXXXXXXC A____________D
                              A____________D
                              A____________D
                              A____________D
                              A____________D
                              A____________D
                              ZXXXXXXXXXXXXC
```

#TODO
Currently this is random, make procedural by providing seed to random.
