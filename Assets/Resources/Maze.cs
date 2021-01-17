using System;
using System.Collections;
using System.Collections.Generic;

public class Maze {

    public static Random rand;
    public int[][] grid;
    Pather p;

    public Maze (int x, int y) {

        int gladeSize = 9;
        rand = new Random ();
        grid = new int[x][];

        for (int i = 0; i < grid.Length; i++) {
            grid[i] = new int[y];
        }
        
        int start = grid.Length / 2 + gladeSize / 2 + 2;
        int sx = start;
        int sy = grid.Length / 2;
        makeGlade (grid, gladeSize, gladeSize);

        grid[sy][sx] = 1;
        p = new Pather (grid, sx, sy);

        while (!(p.complete && p.shootsComplete)) {
            p.step ();
        }

    }

    void makeGlade (int[][] data, int xSize, int ySize) {
        int xstart = data[0].Length / 2 - xSize / 2;
        int ystart = data.Length / 2 - ySize / 2;
        for (int y = ystart - 1; y < ystart + ySize + 1; y++) {
            for (int x = xstart - 1; x < xstart + xSize + 1; x++) {
                data[y][x] = 0;
            }
        }

        for (int y = ystart; y < ystart + ySize; y++) {
            for (int x = xstart; x < xstart + xSize; x++) {
                data[y][x] = 1;
            }
        }

        // remove cells for glade entrances
         data[data.Length/2 + ySize/2 + 1][data[0].Length/2] = 2;
        // data[data.Length/2 + ySize/2 + 2][data[0].Length/2] = 1;
         data[data.Length/2 - ySize/2 - 1][data[0].Length/2] = 2;
        // data[data.Length/2 - ySize/2 - 2][data[0].Length/2] = 1;
    }

    public class Pather {

        List<Vec2> stack;
        List<Pather> shoots;
        int xPos, yPos;
        public bool complete = false, shootsComplete = true;
        int[][] grid;
        int shootChance = 80;

        public Pather (int[][] g, int x, int y) {
            xPos = x;
            yPos = y;
            grid = g;
            stack = new List<Vec2> ();
            shoots = new List<Pather> ();
        }

        public void step () {

            if (!complete) {
                bool wallFlag = false;

                if (!isDead ()) {
                    int d = Maze.rand.Next (0, 4);
                    if (d == 0) { // left
                        wallFlag = isWall (xPos - 2, yPos);
                        if (wallFlag) {
                            grid[yPos][xPos - 1] = 1;
                            grid[yPos][xPos - 2] = 1;
                            randShoot ();
                            stack.Add (new Vec2 (xPos, yPos));
                            xPos -= 2;
                        }
                    } else if (d == 1) { // up
                        wallFlag = isWall (xPos, yPos - 2);
                        if (wallFlag) {
                            grid[yPos - 1][xPos] = 1;
                            grid[yPos - 2][xPos] = 1;
                            randShoot ();
                            stack.Add (new Vec2 (xPos, yPos));
                            yPos -= 2;
                        }
                    } else if (d == 2) { // right
                        wallFlag = isWall (xPos + 2, yPos);
                        if (wallFlag) {
                            grid[yPos][xPos + 1] = 1;
                            grid[yPos][xPos + 2] = 1;
                            randShoot ();
                            stack.Add (new Vec2 (xPos, yPos));
                            xPos += 2;
                        }
                    } else if (d == 3) { // d == 3 // down
                        wallFlag = isWall (xPos, yPos + 2);
                        if (wallFlag) {
                            grid[yPos + 1][xPos] = 1;
                            grid[yPos + 2][xPos] = 1;
                            randShoot ();
                            stack.Add (new Vec2 (xPos, yPos));
                            yPos += 2;
                        }
                    }
                } else if (stack.Count == 0) {
                    complete = true;
                } else {
                    grid[yPos][xPos] = 1;
                    xPos = stack[stack.Count - 1].x;
                    yPos = stack[stack.Count - 1].y;
                    stack.RemoveAt (stack.Count - 1);
                }
            }

            if (shoots.Count != 0) {
                shootsComplete = false;
                for (int i = 0; i < shoots.Count; i++) {
                    if (shoots[i].complete && shoots[i].shootsComplete) {
                        shoots.RemoveAt (i);
                        i--;
                    } else {
                        shoots[i].step ();
                    }
                }
            } else {
                shootsComplete = true;
            }
        }
        public int dinc (int d) {
            if (d < 3) {
                d++;
                return d;
            } else {
                return 0;
            }
        }

        public bool isDead () {
            if (
                isWall (xPos - 2, yPos) ||
                isWall (xPos + 2, yPos) ||
                isWall (xPos, yPos - 2) ||
                isWall (xPos, yPos + 2)
            ) {
                return false;
            }
            return true;
        }
        public void generateShoots () {
            while (!isDead ()) {
                Pather p = new Pather (grid, xPos, yPos);
                p.step ();
                shoots.Add (p);
            }
        }

        public void randShoot () {
            int i = Maze.rand.Next (0, 100);
            if (!isDead () && i < shootChance) {
                Pather p = new Pather (grid, xPos, yPos);
                p.step ();
                shoots.Add (p);
            }
        }

        public void generateShoot () {
            if (!isDead ()) {
                Pather p = new Pather (grid, xPos, yPos);
                p.step ();
                shoots.Add (p);
            }
        }

        public bool isWall (int x, int y) {
            if (x < 0 || x >= grid[0].Length || y < 0 || y >= grid.Length) {
                return false;
            } else if (grid[y][x] == 0) {
                return true;
            }
            return false;
        }
    }
}