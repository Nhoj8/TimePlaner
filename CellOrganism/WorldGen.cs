using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.CodeDom.Compiler;
using Microsoft.VisualBasic;
using System.Windows.Forms;
using System.Diagnostics.Eventing.Reader;
using System.Windows.Forms.VisualStyles;
using System.Data.OleDb;
using Microsoft.Xna.Framework;

namespace CellOrganism
{

    class WorldGen
    {




        public Int16[,] world;
        //Int16[,] Compressedworld;
        long seedint = 0;
        int numberOfBlocks = 0;
        double OreFillPercent = 1;
        System.Random rnd;
        public (Int16[,], string, int) Start(int width, int height, string seed, double randomFillPercent, int smoothIndex, int CompressionIndex)
        {
            GenerateWorld(width, height, seed, randomFillPercent, smoothIndex, CompressionIndex);
            return (world, seedint.ToString(), numberOfBlocks);
        }
        void GenerateWorld(int width, int height, string seed, double randomFillPercent, int smoothIndex, int CompressionIndex)
        {
            world = new Int16[width, height];
            short[,] Compressedworld = new Int16[width / (int)Math.Pow(2, CompressionIndex), height / (int)Math.Pow(2,CompressionIndex)];
            Compressedworld = RandomFillWorld(Compressedworld, seed, randomFillPercent);
            for (int c = 0; c < CompressionIndex; c++)
            {
                Smoothworld(ref Compressedworld);
                Smoothworld(ref Compressedworld);
                Compressedworld = SmoothUpworld(Compressedworld, 2);
            }
            world = Compressedworld;
            for (int i = 0; i < smoothIndex; i++)
            {
                Smoothworld(ref world);
            }
            FinalSmoothworld(width, height);
            // world[width / 2, height / 2] = 2; //MIDDLE BOLCK
        }

        short[,] RandomFillWorld(short[,] world, string seed, double randomFillPercent)
        {


            if (seed == null || seed == "")
            {
                //seedint = "".GetHashCode();

                seedint = DateAndTime.TimeString.GetHashCode();
            }
            else
            {
                if (!long.TryParse(seed, out seedint))
                {
                    char[] char_array = seed.ToCharArray();
                    long oldseedint = 0;

                    for (int i = 0; i < char_array.Length; i++)
                    {
                        int index;
                        index = (char_array[i]);
                        long power = Convert.ToInt32(Math.Pow(2, i));
                        seedint += index * power;
                        if (seedint > int.MaxValue)
                        {
                            seedint = oldseedint;
                            break;
                        }
                        oldseedint = seedint;
                    }
                }
            }




            //MessageBox.Show(seedint.ToString());   
            rnd = new System.Random(Convert.ToInt32(seedint));

            //MessageBox.Show(rnd.Next(0 ,100).ToString());
            for (int x = 0; x < world.GetLength(0); x++)
            {
                for (int y = 0; y < world.GetLength(1); y++)
                {
                    //if ((x > width/2 -10 && x < width/2 + 10) && (y > height/2 - 10 && y < height/2 + 10))
                    //{
                    //    world[x, y] = 0;
                    //}
                    //if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                    //{
                    //    world[x, y] = 1;
                    //}
                    //else
                    world[x, y] = (short)(rnd.Next(0, 100) < randomFillPercent ? (rnd.Next(0, 100) < OreFillPercent ? 4 : 1) : 0);

                }
            }
            //world[width / 2, height / 2] = 3; MIDDLE BOLCK Actually no its not cuz smooth
            return world;
        }

        short[,] SmoothUpworld(short[,] Compressedworld, int compressAmount)
        {
            short[,] NewCompressedworld = new short[Compressedworld.GetLength(0) * compressAmount, Compressedworld.GetLength(1) * compressAmount];
            for (int x = 0; x < NewCompressedworld.GetLength(0); x++)
            {
                for (int y = 0; y < NewCompressedworld.GetLength(1); y++)
                {
                    int offsetX = 0;
                    if (x != 0 && x != NewCompressedworld.GetLength(0) - 1)
                        offsetX = rnd.Next(-1, 1);
                    int offsetY = 0;
                    if (y != 0 && y != NewCompressedworld.GetLength(1) - 1)
                        offsetY = rnd.Next(-1, 1);

                    NewCompressedworld[x, y] = Compressedworld[(int)Math.Floor((double)((x + offsetX) / compressAmount)), (int)Math.Floor((double)((y + offsetY) / compressAmount))];

                }
            }
            return NewCompressedworld;
        }
        void Smoothworld(ref short[,] world)
        {

            for (int x = 0; x < world.GetLength(0); x++)
            {
                for (int y = 0; y < world.GetLength(1); y++)
                {
                    int adjacentblocks;
                    (adjacentblocks) = Getadjacentblockamount(ref world, x, y);
                    if (adjacentblocks > 4)
                    {
                        if (world[x, y] == 0)
                            world[x, y] = 1;
                    }
                    else if (adjacentblocks < 4)
                        world[x, y] = 0;
                    //else
                    //    world[x, y] = 2;

                }
            }
        }
        void FinalSmoothworld(int width, int height)
        {
            short[,] newworld = new short[width, height];
            numberOfBlocks = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if(x == 0 || x == width - 1 || y == 0 || y == height - 1)
                    {
                        newworld[x, y] = 1;
                        numberOfBlocks += 1;
                    }
                    else if (Math.Sqrt((x - width / 2) * (x - width / 2) + (y - height / 2) * (y - height / 2)) < 9)
                    {
                        newworld[x, y] = 0;
                    }
                    else
                    {
                        int[] blockcount;
                        (blockcount) = Getadjacentblockcount(ref world, x, y);
                        if (world[x, y] != 0)
                        {
                            int oregen = rnd.Next(0, 100000);
                            if (world[x, y] == 4)
                                newworld[x, y] = 4;
                            else if (blockcount[4] > 0 && oregen < 50000)
                                newworld[x, y] = 4;
                            else if (blockcount[0] > 0)
                                newworld[x, y] = 2;
                            else if (blockcount[1] > 7 && oregen < 1)
                                newworld[x, y] = 6;
                            else if (blockcount[1] > 7 && oregen < 10)
                                newworld[x, y] = 3;
                            else
                                newworld[x, y] = 1;
                            numberOfBlocks += 1;
                        }
                    }
                    

                    //else if (adjacentblocks < 4)
                    //    world[x, y] = 0;
                    //else
                    //    world[x, y] = 2;

                }
            }
            world = newworld;
        }

        public static int[] Getadjacentblockcount(ref short[,] world, int gridX, int gridY)
        {

            int[] blockcount = new int[Game1.Blocks8x8.Length +1];
            for (int neighborX = gridX - 1; neighborX <= gridX + 1; neighborX++)
            {
                for (int neighborY = gridY - 1; neighborY <= gridY + 1; neighborY++)
                {
                    if (neighborX >= 0 && neighborX < world.GetLength(0) && neighborY >= 0 && neighborY < world.GetLength(1))
                    {
                        if (neighborX != gridX || neighborY != gridY)
                        {
                            blockcount[world[neighborX, neighborY]] += 1;
                        }
                    }


                }
            }
            //blockcount[world[gridX - 1, gridY - 1]] += 1;
            //blockcount[world[gridX - 1, gridY]] += 1;
            //blockcount[world[gridX - 1, gridY + 1]] += 1;
            //blockcount[world[gridX, gridY - 1]] += 1;
            //blockcount[world[gridX, gridY + 1]] += 1;
            //blockcount[world[gridX + 1, gridY - 1]] += 1;
            //blockcount[world[gridX + 1, gridY]] += 1;
            //blockcount[world[gridX + 1, gridY + 1]] += 1;

            return (blockcount);
        }
        int Getadjacentblockamount(ref short[,] world, int gridX, int gridY)
        {
            int wallcount = 0;

            for (int neighborX = gridX - 1; neighborX <= gridX + 1; neighborX++)
            {
                for (int neighborY = gridY - 1; neighborY <= gridY + 1; neighborY++)
                {
                    if (neighborX >= 0 && neighborX < world.GetLength(0) && neighborY >= 0 && neighborY < world.GetLength(1))
                    {
                        if (neighborX != gridX || neighborY != gridY)
                        {
                            if (world[neighborX, neighborY] != 0)
                            {
                                wallcount += 1;
                            }




                        }
                    }
                    else
                        wallcount += 2;

                }
            }
            return (wallcount);
        }


    }


}


