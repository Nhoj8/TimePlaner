using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// to do: 
/// make predicted location for extrinzoid based on velocity in 1 sec and show time projectil shot from ship will take to get to mouse. 
/// DONE make projectiels and exrtinzoids move when tiem reversed
/// DONE makes player not be able to move when time revesed
/// DONE need to make dependancies
/// when reEneabaling events need to check if all things that it depends on are also enabled, use refeerces for that i think
/// make spreading corruption blocks
/// diversify world gen(biomes like in the cool slime video)
/// make big map image
/// make extrinzoid mode change into an event or just make them not be angry if no friends nearby
/// DONE need to transfer all calculations from the draw phasde into the game phae ie: make th eblocks test for extrinzoids and corruption in there
///
/// </summary>

namespace CellOrganism
{
    public struct DoubleCoordinates
    {
        public float X;
        public float Y;

        public DoubleCoordinates(float X, float Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public static Point convertToPoint(DoubleCoordinates Dc1)
        {
            return new Point((int)Dc1.X, (int)Dc1.Y);
        }
        public static Vector2 convertToVector2(DoubleCoordinates Dc1)
        {
            return new Vector2((float)Dc1.X, (float)Dc1.Y);
        }
        public static DoubleCoordinates convertToDoubleCoordinates(Vector2 Dc1)
        {
            return new DoubleCoordinates(Dc1.X, Dc1.Y);
        }
        public static DoubleCoordinates operator +(DoubleCoordinates Dc1, DoubleCoordinates Dc2)
        {
            Dc1.X += Dc2.X;
            Dc1.Y += Dc2.Y;
            return Dc1;
        }
        public static DoubleCoordinates operator -(DoubleCoordinates Dc1, DoubleCoordinates Dc2)
        {
            Dc1.X -= Dc2.X;
            Dc1.Y -= Dc2.Y;
            return Dc1;
        }
        public static DoubleCoordinates operator *(DoubleCoordinates Dc1, DoubleCoordinates Dc2)
        {
            Dc1.X *= Dc2.X;
            Dc1.Y *= Dc2.Y;
            return Dc1;
        }
        public static DoubleCoordinates operator /(DoubleCoordinates Dc1, DoubleCoordinates Dc2)
        {
            Dc1.X /= Dc2.X;
            Dc1.Y /= Dc2.Y;
            return Dc1;
        }
        public static DoubleCoordinates operator +(DoubleCoordinates Dc1, Vector2 Dc2)
        {
            Dc1.X += Dc2.X;
            Dc1.Y += Dc2.Y;
            return Dc1;
        }
        public static DoubleCoordinates operator -(DoubleCoordinates Dc1, Vector2 Dc2)
        {
            Dc1.X -= Dc2.X;
            Dc1.Y -= Dc2.Y;
            return Dc1;
        }
        public static DoubleCoordinates operator +(DoubleCoordinates Dc1, Point Dc2)
        {
            Dc1.X += Dc2.X;
            Dc1.Y += Dc2.Y;
            return Dc1;
        }
        public static DoubleCoordinates operator /(DoubleCoordinates Dc1, int dv)
        {
            Dc1.X /=dv;
            Dc1.Y /=dv;
            return Dc1;
        }
        public static DoubleCoordinates operator *(DoubleCoordinates Dc1, double dv)
        {
            Dc1.X *= (float)dv;
            Dc1.Y *= (float)dv;
            return Dc1;
        }
        public static DoubleCoordinates operator /(DoubleCoordinates Dc1, double dv)
        {
            Dc1.X /= (float)dv;
            Dc1.Y /= (float)dv;
            return Dc1;
        }
        public static DoubleCoordinates operator *(DoubleCoordinates Dc1, int dv)
        {
            Dc1.X *= dv;
            Dc1.Y *= dv;
            return Dc1;
        }
        public static DoubleCoordinates operator *(DoubleCoordinates Dc1, Point Dc2)
        {
            Dc1.X *= Dc2.X;
            Dc1.Y *= Dc2.Y;
            return Dc1;
        }
        public static DoubleCoordinates operator *(DoubleCoordinates Dc1, WholeCoords Dc2)
        {
            Dc1.X *= Dc2.X;
            Dc1.Y *= Dc2.Y;
            return Dc1;
        }
        public static Point operator +(Point Dc1, DoubleCoordinates Dc2)
        {
            Dc1.X += (int)Dc2.X;
            Dc1.Y += (int)Dc2.Y;
            return Dc1;
        }
        public static bool operator ==(DoubleCoordinates wc1, DoubleCoordinates wc2)
        {
            return wc1.Equals(wc2); //(wc1.X == wc2.X && wc1.Y == wc2.Y);
        }

        public static bool operator !=(DoubleCoordinates wc1, DoubleCoordinates wc2)
        {
            return !wc1.Equals(wc2);
        }
    }
    public struct WholeCoords
    {
        public Int16 X;
        public Int16 Y;

        public WholeCoords(Int16 X, Int16 Y)
        {
            this.X = X;
            this.Y = Y;
        }
        public WholeCoords(int X, int Y)
        {
            this.X = (short)X;
            this.Y = (short)Y;
        }
        public static Point convertToPoint(WholeCoords Dc1)
        {
            return new Point((int)Dc1.X, (int)Dc1.Y);
        }
        public WholeCoords(Point coords)
        { 
            this.X = (short)coords.X;
            this.Y = (short)coords.Y;
        }
        public WholeCoords(DoubleCoordinates coords)
        {
            this.X = (short)coords.X;
            this.Y = (short)coords.Y;
        }
        public static WholeCoords operator *(WholeCoords Dc1, int dv)
        {
            Dc1.X *= (short)dv;
            Dc1.Y *= (short)dv;
            return Dc1;
        }
        public static DoubleCoordinates operator -(WholeCoords Dc1, DoubleCoordinates Dc2)
        {
            Dc2.X = (float)Dc1.X - Dc2.X;
            Dc2.Y = (float)Dc1.Y - Dc2.Y;
            return Dc2;
        }

        public static bool operator ==(WholeCoords wc1, WholeCoords wc2)
        {
            return wc1.Equals(wc2); //(wc1.X == wc2.X && wc1.Y == wc2.Y);
        }

        public static bool operator !=(WholeCoords wc1, WholeCoords wc2)
        {
            return !wc1.Equals(wc2);
        }
    }
    public class Text
    {
        public string text;
        public char[] chars;
        public Vector2[] coords;
        public int[] color;
        public Rectangle size;
        public int NumberOfLines;
        public int[] LinePerCharecter;


        public void DoText(string textword = "", int startindex = 0)
        {
            (chars, coords, color, size, LinePerCharecter, NumberOfLines) = Getcoordinateschar(text.Substring(startindex) + textword);
        }
        (Char[], Vector2[], int[], Rectangle, int[], int) Getcoordinateschar(string data)
        {
            char[] chararrayprivate = data.ToCharArray(), chararray;
            chararray = chararrayprivate;
            int length = 0, newlength = 0, height = 16;
            //string test = "", test2 = "";
            Vector2[] CoordinatesString = new Vector2[chararrayprivate.Length];
            int[] ColorString = new int[chararrayprivate.Length];
            int[] numberofchararray = new int[chararrayprivate.Length];
            int[] LinesPerCharecter = new int[chararrayprivate.Length];
            int newLine = 0, charectersinpreviousline = 0, TotalWeirdChar = 0, SpacesPerLine = 0, WeirdCharPerLine = 0;
            Boolean ColorChar = false;
            int ColorOfLetter = 0;
            //System.Windows.Forms.MessageBox.Show((index).ToString());
            for (int i = 0; i < chararrayprivate.Length; i++)
            {
                char c;
                c = chararrayprivate[i];
                numberofchararray[i] = c;
                chararray[i - newLine - TotalWeirdChar] = chararrayprivate[i];
                //test2 += i.ToString() + "'" + numberofchararray[i].ToString() + "', ";
                if (ColorChar)
                {
                    if (numberofchararray[i] > 96 && numberofchararray[i] < 122)
                        ColorOfLetter = numberofchararray[i] - 97;
                    else if (numberofchararray[i] == 122)//z
                        ColorOfLetter = 20;
                    TotalWeirdChar += 1;
                    WeirdCharPerLine += 1;
                    ColorChar = false;
                }
                else if (numberofchararray[i] == 10)//enter or newline
                {
                    //System.Array.Resize(ref charectersperline, newLine + 1);
                    //charectersperline[newLine] = i + 1 - charectersinpreviousline;
                    newLine += 1;
                    charectersinpreviousline = i + 1;
                    SpacesPerLine = 0;
                    WeirdCharPerLine = 0;
                    height += 16;
                    length = length < newlength ? newlength : length;
                    newlength = 0;
                }
                else if (numberofchararray[i] == 32) //space
                {
                    SpacesPerLine += 1;
                    TotalWeirdChar += 1;
                    newlength += 8;
                }
                else if (numberofchararray[i] == 937) //Ω
                {
                    ColorChar = true;
                    TotalWeirdChar += 1;
                    WeirdCharPerLine += 1;
                }
                else
                {

                    if (numberofchararray[i] > 128)
                    {
                        System.Windows.Forms.MessageBox.Show(((char)numberofchararray[i]).ToString());
                        WeirdCharPerLine += 1;
                        TotalWeirdChar += 1;
                    }
                    else if (Game1.Letters[numberofchararray[i]] == null)
                    {
                        numberofchararray[i] = Game1.rnd.Next(97, 123);
                        chararray[i - newLine - TotalWeirdChar] = (char)numberofchararray[i];
                        //System.Windows.Forms.MessageBox.Show(chararray[i - newLine - TotalWeirdChar].ToString());
                    }
                    newlength += 16;
                    CoordinatesString[i - newLine - TotalWeirdChar] = new Vector2((i - SpacesPerLine - WeirdCharPerLine) * 16 - charectersinpreviousline * 16 + SpacesPerLine * 8, 0 + newLine * 16);
                    ColorString[i - newLine - TotalWeirdChar] = ColorOfLetter;
                    LinesPerCharecter[i - newLine - TotalWeirdChar] = newLine;
                    //test += (i.ToString() + "'" + ((char)c).ToString() + "', ");
                }

            }
            //System.Array.Resize(ref charectersperline, newLine + 1);
            //charectersperline[newLine] = chararrayprivate.Length - charectersinpreviousline;
            length = length < newlength ? newlength : length;
            //System.Windows.Forms.MessageBox.Show(test2);
            System.Array.Resize(ref chararray, chararrayprivate.Length - newLine - TotalWeirdChar);
            Rectangle size = new Rectangle(0, 0, length, height);
            return (chararray, CoordinatesString, ColorString, size, LinesPerCharecter, newLine + 1);
        }

        public void draw(SpriteBatch _spriteBatch, int X = 0, int Y = 0)
        {
            for (int i = 0; i < chars.Length; i++)
                _spriteBatch.Draw(Game1.Letters[chars[i]], coords[i] + new Vector2(size.X + X, size.Y + Y), Game1.LetterColors[color[i]]);
        }
    }

    public class Label
    {
        public Rectangle rectangle;
        public Point DistanceFromText = new Point(20,20);
        public Color color;
        public Text text = new Text();

        public void draw(SpriteBatch _spriteBatch, int X = 0, int Y = 0)
        {
            text.DoText();
            //System.Windows.Forms.MessageBox.Show(Inventory.SelectedSlotText.size.X.ToString());
            _spriteBatch.DrawRoundedRect(new Rectangle(X + rectangle.X, Y + rectangle.Y, text.size.Width + DistanceFromText.X * 2, text.size.Height + DistanceFromText.Y*2), Game1.circle, Game1.circle.Width / 2, Color.Red);

            text.draw(_spriteBatch, X + rectangle.X + DistanceFromText.X, Y + rectangle.Y + DistanceFromText.Y);

        }
    }
}