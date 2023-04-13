using System;
using System.IO;
using System.Data;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Framework;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Content;
//using System.Drawing;
//using System.Windows.Forms;
using System.Globalization;
using System.Linq;
//using CellGame;
using System.Collections.ObjectModel;

namespace CellOrganism
{

    public class Game1 : Game
    {

        public static GraphicsDeviceManager _graphics;
        //private GraphicsDeviceManager _graphics;
        //public static Microsoft.Xna.Framework.Graphics.SpriteBatch _spriteBatch;
        private Microsoft.Xna.Framework.Graphics.SpriteBatch _spriteBatch;
        //Collisions Collisions = new Collisions();

        public static System.Random rnd = new System.Random();
        public static Rectangle GraphicsRectangle;




        public static Texture2D[] Blocks8x8;

        public static Int16[,] world;
        public static int blocksize, loadingareaX, loadingareaY;//BLOCK RANGE THE PLAYER CAN SEE IN ALL DIRECTIONS
        public string WorldName;
        bool Loadworld;
        public static Texture2D[] Blocks;
        public static Vector2 middleposition;// = new Vector2(20, 20);
        //Rectangle[,] blockrectangle;
        public static int width = 2048, height = 1536; //BLOCK SIZE OF THE MAP
        public static string seed;

        

        public static Texture2D Player;
        public static Vector2 playerOffSet;
        float thrusterStrength = (float).1;

        int playersize;


        public static Hotbar PlayerHotbar = new Hotbar();
        
        Storage Storage = new Storage();

        public static Color colorclear = Color.White;
        ColorRGB Rainbow;

        MouseState mouseState;
        Point MousePosition, MousePositionInRelationToMap;
        WholeCoords SelectedBlock;
        public static Boolean Paused = true;
            bool Infomode = false, screensizechanged = false, MouseLeftPressed = false, MoveOneFrame = false, ChatOn = true, ButtonSelected; 

        KeyBoardKey NewMonsters = new KeyBoardKey(), KillMonsters = new KeyBoardKey();
        KeyBoardKey InfoKey = new KeyBoardKey(), EscapeKey = new KeyBoardKey(), DebugButton1 = new KeyBoardKey(), DebugButton2 = new KeyBoardKey(), ChatButton = new KeyBoardKey(), OpenInventory = new KeyBoardKey();
        KeyBoardKey Shoot = new KeyBoardKey(), Switch = new KeyBoardKey();
        KeyBoardKey timeMoveForward = new KeyBoardKey(), timeUndo = new KeyBoardKey(); //, MoveLeft = new KeyBoardKey();
        KeyBoardKey moveRight = new KeyBoardKey(), moveLeft = new KeyBoardKey(), moveDown = new KeyBoardKey(), moveUp = new KeyBoardKey(), ToggleMap = new KeyBoardKey();
        int SelectedBlockType = 0;

        public static Texture2D[] Letters;
        public static Color[] LetterColors;
        String[] ColorString;
        Text Coordinates = new Text(), Extratext = new Text(), Info = new Text();

        const int EXTRINZOID = 3;
        float[] FrameDisplayTime = new float[60];
        float FPS;
        int currentFrame;


        public static Chat Chat = new Chat();

        bool MapEnabled;
        Color[] miniMapColors;
        int minimapsize;
        Texture2D Map;
        Texture2D pixel;

        public static Texture2D circle, circletransparent;
        Button[] Buttons;
        const int PLAYBUTTON = 0;
        const int CREATENEWWORLDBUTTON = 1;
        const int DEBUGBUTTON = 2;
        const int SETTINGSBUTTON =3;
        const int RESETBUTTON= 4;
        const int NEWWORLDBUTTON = 5;
        const int SAVEWORLDBUTTON = 6;
        const int LOADWORLDBUTTON = 7;
        const int WORLD1BUTTON = 8;
        //Position Anchors
        Vector2[] PositionAnchors = new Vector2[9];
        const int TOPLEFT = 0;
        const int TOPMIDDLE = 1;
        const int TOPRIGHT = 2;
        const int MIDDLELEFT = 3;
        const int MIDDLEMIDDLE = 4;
        const int MIDDLERIGHT = 5;
        const int BOTTOMLEFT = 6;
        const int BOTTOMMIDDLE = 7;
        const int BOTTOMRIGHT = 8;

        Boolean MainMenuInitilised = false;
        const int MAINMENU = 0;
        const int LOADING = 1;
        const int GAME = 2;
        const int DEBUGLOADING = 3;
        const int DEBUG = 4;
        int CurrentlyDisplaying = MAINMENU;



        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            //_graphics.ToggleFullScreen();
            _graphics.PreferredBackBufferWidth = 1280;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = 720;   // set this value to the desired height of your window
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        protected override void Initialize()
        {
            
            // TODO: Add your initialization logic here


            //playerposition = new Vector2(-playersize / 2, -playersize);
            NewMonsters.NewKeyBoardKey();
            NewMonsters.Key = Keys.N;
            KillMonsters.NewKeyBoardKey();
            KillMonsters.Key = Keys.B;
            InfoKey.Key = Keys.F3;
            EscapeKey.Key = Keys.Escape;
            DebugButton1.NewKeyBoardKey();
            DebugButton1.Key = Keys.C;
            DebugButton2.NewKeyBoardKey();
            DebugButton2.Key = Keys.V;
            Shoot.Key = Keys.Space;
            ChatButton.Key = Keys.OemQuestion;
            OpenInventory.Key = Keys.E;
            timeMoveForward.Key = Keys.Right;
            timeUndo.Key = Keys.Left;
            Switch.Key = Keys.L;
            moveRight.Key = Keys.D;
            moveRight.activateOnStateChange = true;
            moveLeft.Key = Keys.A;
            moveLeft.activateOnStateChange = true;
            moveUp.Key = Keys.W;
            moveUp.activateOnStateChange = true;
            moveDown.Key = Keys.S;
            moveDown.activateOnStateChange = true;
            ToggleMap.Key = Keys.M;

            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
            _spriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(GraphicsDevice);

            Blocks8x8 = new Texture2D[6];
            Blocks8x8[1] = Content.Load<Texture2D>("Blocks/Stone");
            Blocks8x8[2] = Content.Load<Texture2D>("Blocks/Dirt");
            Blocks8x8[3] = Content.Load<Texture2D>("Blocks/Clay");
            Blocks8x8[4] = Content.Load<Texture2D>("Blocks/Ore");
            Blocks8x8[5] = Content.Load<Texture2D>("Blocks/Corruption");

            Letters = new Texture2D[128];
            for (int i = 48; i < 58; i++)
            {
                Letters[i] = Content.Load<Texture2D>("Numbers/" + (i - 48).ToString());
            }
            for (int i = 65; i < 91; i++)
            {
                Letters[i] = Content.Load<Texture2D>("Letters/Letters" + ((Char)i).ToString());
            }
            for (int i = 97; i < 123; i++)
            {
                Letters[i] = Content.Load<Texture2D>("Letters/Letters_" + ((Char)i).ToString());
            }
            //MyExtension().run;

            string test = "";
            for (int i = 32; i < 129; i++)
            {
                test += (i.ToString() + "'" + ((char)i).ToString() + "', ");

            }
            System.Windows.Forms.MessageBox.Show(test);

            circle = SpriteBatchExtensions.Resize(GraphicsDevice, Getborders(CreateCircle(16), 1, Color.Black, 0, false), 2, 2);
            circletransparent = Getborders(CreateCircle(16), 1, Color.White,0, true);

            miniMapColors = new Color[7];
            miniMapColors[0] = Color.White;
            miniMapColors[1] = Color.DarkGray;
            miniMapColors[2] = Color.SaddleBrown;
            miniMapColors[3] = Color.Brown; 
             miniMapColors[4] = Color.Green;
            miniMapColors[5] = Color.Black;
            miniMapColors[6] = Color.Black;
            pixel = new Texture2D(this.GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[1] { Color.White });

            LetterColors = new Color[21];
            LetterColors[0] = Color.White;
            LetterColors[1] = Color.Red;
            LetterColors[2] = Color.Yellow;
            LetterColors[3] = Color.Lime;
            LetterColors[4] = Color.Cyan;
            LetterColors[5] = Color.Blue;
            LetterColors[6] = Color.Magenta;
            LetterColors[7] = Color.Black;
            LetterColors[8] = Color.DarkGray;
            LetterColors[9] = Color.DarkRed;
            LetterColors[10] = Color.DarkOrange;
            LetterColors[11] = Color.Green;
            LetterColors[12] = Color.DarkCyan;
            LetterColors[13] = Color.DarkBlue;
            LetterColors[14] = Color.DarkMagenta;
            LetterColors[15] = Color.DimGray;

            ColorString = new string[21];
            ColorString[0] = "Ωa";
            ColorString[1] = "Ωb";
            ColorString[2] = "Ωc";
            ColorString[3] = "Ωd";
            ColorString[4] = "Ωe";
            ColorString[5] = "Ωf";
            ColorString[6] = "Ωg";
            ColorString[7] = "Ωh";
            ColorString[8] = "Ωi";
            ColorString[9] = "Ωj";
            ColorString[10] = "Ωk";
            ColorString[11] = "Ωl";
            ColorString[12] = "Ωm";
            ColorString[13] = "Ωn";
            ColorString[14] = "Ωo";
            ColorString[15] = "Ωp";
            //ColorRGB Rainbow = new ColorRGB();
            Rainbow.R = 255;
            Rainbow.G = 0;
            Rainbow.B = 0;
            Rainbow.A = 255;
            LetterColors[20] = Rainbow.col;
            ColorString[20] = "Ωz";
            InitializeMainMenu();
            RedoScreenVars();
        }

        protected override void UnloadContent()
        {

        }
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

            if (screensizechanged)
                RedoScreenVars();

            mouseState = Mouse.GetState();
            MousePosition = new Point(mouseState.X, mouseState.Y);   


            if (CurrentlyDisplaying == MAINMENU)
            {
                if (!MainMenuInitilised)
                    InitializeMainMenu();

                if (mouseState.LeftButton == ButtonState.Released && MouseLeftPressed)
                    MouseLeftPressed = false;
                if (mouseState.LeftButton == ButtonState.Pressed && !MouseLeftPressed)
                {
                    MouseLeftPressed = true;
                    for (int b = 0; b < Buttons.Count(); b++)
                    {
                        if (Buttons[b].Selected)
                        {
                            ButtonClick(b);
                        }
                    }
                }

            }

            else if (CurrentlyDisplaying == LOADING)
            {
                int numberofblocks = 0;
                if (Loadworld)
                {
                    (WorldName, width, height) = Storage.ReadWorld();
                }
                else
                {

                    WorldName = "Bruh";
                    double randomFillPercent = 50;
                    int smoothIndex = 5;
                    int CompressionIndex = 4;
                    WorldGen mapGen = new WorldGen();
                    (world, seed, numberofblocks) = mapGen.Start(width, height, seed, randomFillPercent, smoothIndex, CompressionIndex);


                }
                Time.blockIDs = new blockID[0];
                Array.Resize(ref Time.blockIDs, numberofblocks);
                Map = new Texture2D(_graphics.GraphicsDevice, width, height);
                Color[] colorData = new Color[width * height];
                int numberOfBlocksSoFar = 0;
                for (int x = 0; x < width; x++)
                    for (int y = 0; y < height; y++)
                    {
                        if (world[x, y] != 0)
                        {
                            Time.blockIDs[numberOfBlocksSoFar].Originalcoords = new WholeCoords(x, y);
                            Time.blockIDs[numberOfBlocksSoFar].CurrentCoords = new WholeCoords(x, y);
                            Time.blockdata[x, y].ID = numberOfBlocksSoFar;
                            numberOfBlocksSoFar += 1;
                            int index = y * width + x;
                            colorData[index] = miniMapColors[world[x, y]];
                        }

                    }

                Map.SetData(colorData);
                blocksize = 8;
                playersize = blocksize;
                playerOffSet = new Vector2(-playersize/2, -playersize/2);

                
                RedoScreenVars();




                CurrentlyDisplaying = GAME;
                Blocks = new Texture2D[Blocks8x8.Length];
                if (blocksize == 8)
                    for (int i = 1; i < Blocks.Length; i++)
                        Blocks[i] = Blocks8x8[i];
                else
                    for (int i = 1; i < Blocks.Length; i++)
                        Blocks[i] = SpriteBatchExtensions.Resize(GraphicsDevice, Blocks8x8[i], Convert.ToDouble(blocksize)/8);
                
                loadingareaX = GraphicsRectangle.Width / blocksize / 2 + 2;
                loadingareaY = GraphicsRectangle.Height / blocksize / 2 + blocksize / 8 * 2;

                Player = Blocks[3];
                minimapsize = 75;

                Time.joinWorld(new DoubleCoordinates( width * blocksize / 2, height * blocksize / 2));
                PlayerHotbar.InitializeInv(GraphicsDevice);
                Buttons[SETTINGSBUTTON].visible = true;//SETTINGS BUTTON
                Chat.Initialize();
                Chat.NewLine("You Joined The World", Time.time);
            }
            else if (CurrentlyDisplaying == GAME)
            {
                if (EscapeKey.CheckKeyPress())
                {
                    PauseGame();

                }

                if (InfoKey.CheckKeyPress())
                    Infomode = !Infomode;
                if(OpenInventory.CheckKeyPress())
                {
                    PlayerHotbar.Open();
                }


                if (ChatButton.CheckKeyPress())
                    ChatOn = !ChatOn;
                //Exit();

                if (DebugButton1.CheckKeyPress())
                {

                    Time.newTravel();
                }

                if (DebugButton2.CheckKeyPress())
                {
                    Time.newChangeTimeDirection();
                }
                if (timeUndo.CheckKeyPress())
                {
                    if (!Time.timeUndo && !Time.timeRedo)
                    {
                        Time.timeOfUndo = Time.time;
                        Time.posOfUndo = Time.instances[Time.currantinstance].position;
                        Time.velOfUndo = Time.instances[Time.currantinstance].velocity;
                    }

                    Time.time += Time.instances[Time.currantinstance].timeReversed != Time.timeUndo ? 1 : -1;
                    Time.timeRedo = Time.timeUndo;
                    Time.timeUndo = !Time.timeUndo;
                }
                if (Paused == false || MoveOneFrame)
                {
                    MoveOneFrame = false;
                    //if (numberOfMonsters < maxMonsters)
                    //{
                    //    SummonMonster();
                    //}


                    //MoveMonster();
                    //DoubleCoordinates Acceleration = Time.instances[Time.currantinstance].acceleration;
                    if (!Time.timeUndo)
                    {
                        bool moveRightChanged = moveRight.CheckKeyPress();
                        bool moveLeftChanged = moveLeft.CheckKeyPress();
                        bool moveDownChanged = moveDown.CheckKeyPress();
                        bool moveUpChanged = moveUp.CheckKeyPress();
                        if (moveRightChanged || moveLeftChanged || moveDownChanged || moveUpChanged)
                        {
                            DoubleCoordinates Acceleration = new DoubleCoordinates(0, 0);
                            if (moveRight.Pressed)
                                Acceleration.X += thrusterStrength / Time.instances[Time.currantinstance].inventory.NumberOfInvItems;
                            if (moveLeft.Pressed)
                                Acceleration.X -= thrusterStrength / Time.instances[Time.currantinstance].inventory.NumberOfInvItems;
                            if (moveDown.Pressed)
                                Acceleration.Y += thrusterStrength / Time.instances[Time.currantinstance].inventory.NumberOfInvItems;
                            if (moveUp.Pressed)
                                Acceleration.Y -= thrusterStrength / Time.instances[Time.currantinstance].inventory.NumberOfInvItems;
                            if (Acceleration != Time.instances[Time.currantinstance].acceleration)
                            {
                                if (Time.timeRedo)
                                    Time.newTimeLine(Time.timeOfUndo, Time.posOfUndo, Time.velOfUndo);
                                Time.newAcceleration(Acceleration);
                            }

                            

                        }
                        if (Shoot.CheckKeyPress() && Time.instances[Time.currantinstance].blast.image == null)
                        {
                            int proVelocity = 20;
                            DoubleCoordinates ProPosition = DoubleCoordinates.convertToDoubleCoordinates(MousePosition.ToVector2() - middleposition);
                            //distanceFromPlayer * proVelocity/Math.Sqrt(distanceFromPlayer.X * distanceFromPlayer.X + distanceFromPlayer.Y * distanceFromPlayer.Y);
                            Time.newCreateProjectile(Time.instances[Time.currantinstance].position, ProPosition * proVelocity / Math.Sqrt(ProPosition.X * ProPosition.X + ProPosition.Y * ProPosition.Y), Time.currantinstance);
                        }
                    }

                    Time.advanceTime();


                    Point Coords = new Point((int)Time.instances[Time.currantinstance].position.X, (int)Time.instances[Time.currantinstance].position.Y);
                    for (int x = Coords.X / blocksize - loadingareaX; x <= Coords.X / blocksize + loadingareaX; x++)
                        for (int y = Coords.Y / blocksize - loadingareaY; y < Coords.Y / blocksize + loadingareaY; y++)
                            if ((x > -1) && (x < width) && (y > -1) && (y < height))
                                if (world[x, y] > 0)
                                {
                                    if (world[x, y] == 6)
                                    {
                                            Time.newCorruption(new WholeCoords(x, y));
                                    }
                                    else if (world[x, y] == EXTRINZOID)
                                    {
                                            Time.newExtrinzoid(new DoubleCoordinates(x * blocksize, y * blocksize));

                                    }
                                }





                }
                else
                {

                    if (timeMoveForward.CheckKeyPress())
                        MoveOneFrame = true;

                }

                if (ToggleMap.CheckKeyPress())
                {
                    MapEnabled = !MapEnabled;
                }



                PlayerHotbar.CheckSelected(MousePosition);


                if (mouseState.LeftButton == ButtonState.Released && MouseLeftPressed)// this can cause block placment at the start of the game
                {
                    MouseLeftPressed = false;
                    PlayerHotbar.ClickCoolDown = false;
                }
                if (mouseState.LeftButton == ButtonState.Pressed && !PlayerHotbar.ClickCoolDown && this.IsActive)
                {
                    if (ButtonSelected)
                    {
                        if (!MouseLeftPressed)
                        {
                            for (int b = 0; b < Buttons.Count(); b++)
                            {
                                if (Buttons[b].Selected)
                                {
                                    ButtonClick(b);
                                    return;
                                }
                            }
                        }

                    }
                    else
                        PlayerHotbar.Click(MousePosition, MouseLeftPressed, SelectedBlock);

                    MouseLeftPressed = true;
                }
                if (MousePosition.X > 0 && MousePosition.X < GraphicsRectangle.Width && MousePosition.Y > 0 && MousePosition.Y < GraphicsRectangle.Height)
                {
                    SelectedBlock = new WholeCoords((short)((MousePosition.X + Time.instances[Time.currantinstance].position.X - GraphicsRectangle.Width / 2 + blocksize/2) / blocksize), (short)((MousePosition.Y + Time.instances[Time.currantinstance].position.Y - GraphicsRectangle.Height / 2 + blocksize / 2) / blocksize));
                    if ((SelectedBlock.X > -1) && (SelectedBlock.X < width) && (SelectedBlock.Y > -1) && (SelectedBlock.Y < height))
                        SelectedBlockType = world[SelectedBlock.X, SelectedBlock.Y];
                    else
                        SelectedBlockType = 0;
                }

            }
            else if (CurrentlyDisplaying == DEBUG)
            {
                
                
            }
            else if (CurrentlyDisplaying == DEBUGLOADING)
            {
                
            }
            ButtonSelected = false;
            for (int b = 0; b < Buttons.Count(); b++)
            {
                if (Buttons[b].visible)
                {
                    if (Buttons[b].Rectangle.Contains(MousePosition))
                    {
                        Buttons[b].Selected = true;
                        ButtonSelected = true;
                    }
                    else
                        Buttons[b].Selected = false;
                }
            }

            DoLettersOnScreen();



            // Last thing, do not move
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SlateGray);

            _spriteBatch.Begin();


            if (CurrentlyDisplaying == MAINMENU)
            {


            }
            else if (CurrentlyDisplaying == LOADING)
            {
                GraphicsDevice.Clear(Color.Gray);
            }
            else if (CurrentlyDisplaying == GAME)
            {
                Vector2 position = new Vector2((float)Time.instances[Time.currantinstance].position.X, (float)Time.instances[Time.currantinstance].position.Y);
                Point Coords = new Point((int)Time.instances[Time.currantinstance].position.X, (int)Time.instances[Time.currantinstance].position.Y);

                Color blockcolor;
                for (int x = Coords.X / blocksize - loadingareaX; x <= Coords.X / blocksize + loadingareaX; x++)
                    for (int y = Coords.Y / blocksize - loadingareaY; y < Coords.Y / blocksize + loadingareaY; y++)
                        if ((x > -1) && (x < width) && (y > -1) && (y < height))
                            if (world[x, y] > 0)
                            {
                                bool candraw = true;
                                for(int e = 0; e < Time.extrinzoidInstances.Length; e++)//will make slow when their are multiple
                                {
                                    if (Time.extrinzoidInstances[e].exsists)
                                    {
                                        if ((x == Math.Floor(Time.extrinzoidInstances[e].position.X/blocksize) || x == Math.Ceiling(Time.extrinzoidInstances[e].position.X / blocksize)) && (y == Math.Floor(Time.extrinzoidInstances[e].position.Y / blocksize) || y == Math.Ceiling(Time.extrinzoidInstances[e].position.Y / blocksize)))
                                        {
                                            candraw = false;
                                            break;
                                        }
                                    }
                                }
                                if (world[x, y] == 6)
                                {
                                }
                                else if (world[x, y] == EXTRINZOID)
                                {   
                                }
                                else if (candraw)
                                {
                                    if (x == SelectedBlock.X && y == SelectedBlock.Y)
                                        blockcolor = Color.LightGray;
                                    else
                                        blockcolor = colorclear;
                                    _spriteBatch.Draw(Blocks[world[x, y]], new Vector2(x * blocksize, y * blocksize) + playerOffSet - position + middleposition, blockcolor);
                                }

                            }
                //for(int i = 0; i < Time.instances.Length; i++)
                //if(Time.extrinzoidInstances.Length > 0)
                for (int e = 0; e < Time.extrinzoidInstances.Length; e++)
                    if (Time.extrinzoidInstances[e].exsists)
                    {
                        _spriteBatch.Draw(Blocks[2], DoubleCoordinates.convertToVector2(Time.extrinzoidInstances[e].position) - position + middleposition + playerOffSet, colorclear);
                        if (Time.extrinzoidInstances[e].blast.image != null)
                            _spriteBatch.Draw(Time.extrinzoidInstances[e].blast.image, DoubleCoordinates.convertToVector2(Time.extrinzoidInstances[e].blast.position) - position + middleposition + playerOffSet, Color.Red);
                    }


                for (int i = 0; i < (Time.instances.Length); i++)//-1 so does not draw player
                    if (Time.instances[i].exsists)
                    {
                        _spriteBatch.Draw(Player, new Vector2((float)Time.instances[i].position.X, (float)Time.instances[i].position.Y) - position + middleposition + playerOffSet, colorclear);
                        if (Time.instances[i].blast.image != null)
                            _spriteBatch.Draw(Time.instances[i].blast.image, DoubleCoordinates.convertToVector2(Time.instances[i].blast.position) - position + middleposition + playerOffSet, Color.Red);
                    }


                FadeDraw.draw(_spriteBatch, middleposition);
                //plqayer goes here
                //_spriteBatch.Draw(Player, middleposition + playerOffSet, colorclear);
                //if (Time.instances[Time.currantinstance].blast.image != null)
                //    _spriteBatch.Draw(Time.instances[Time.currantinstance].blast.image, DoubleCoordinates.convertToVector2(Time.instances[Time.currantinstance].blast.position) - position + middleposition + playerOffSet, Color.Red);


                Color mappixelcolor;
                for (int x = Coords.X / blocksize - minimapsize; x < Coords.X / blocksize + minimapsize; x++)
                    for (int y = Coords.Y / blocksize - minimapsize; y < Coords.Y / blocksize + minimapsize; y++)
                    {
                        if ((x > -1) && (x < width) && (y > -1) && (y < height))
                            mappixelcolor = miniMapColors[world[x, y]];
                        else
                            mappixelcolor = miniMapColors[0];
                        if (x - (Coords.X / blocksize - minimapsize) >= minimapsize - playersize / blocksize / 2 && y - (Coords.Y / blocksize - minimapsize) < minimapsize + playersize / blocksize / 2 && y - (Coords.Y / blocksize - minimapsize) >= minimapsize - playersize / blocksize / 2 && x - (Coords.X / blocksize - minimapsize) < minimapsize + playersize / blocksize / 2)
                            mappixelcolor = Color.Black;
                        _spriteBatch.Draw(pixel, new Vector2(x - (Coords.X / blocksize - minimapsize) + GraphicsRectangle.Width - 50 - minimapsize * 2, y - (Coords.Y / blocksize - minimapsize) + GraphicsRectangle.Height - 50 - minimapsize * 2), mappixelcolor);
                    }
                if (MapEnabled)
                    _spriteBatch.Draw(Map, middleposition - new Vector2(Map.Width/4, Map.Height / 4), null, colorclear, (float)0,new Vector2(0,0),(Single)0.5,new SpriteEffects(),0);


                Coordinates.draw(_spriteBatch, GraphicsRectangle.Width - 50 - minimapsize - Coordinates.size.Width / 2, GraphicsRectangle.Height - 50 + 5);

                RectangleSprite.DrawRectangle(_spriteBatch, new Rectangle(GraphicsRectangle.Width - 50 - minimapsize - Coordinates.size.Width / 2, GraphicsRectangle.Height - 50 + 5, Coordinates.size.Width, Coordinates.size.Height), Color.Red, 1);
                ref Inventory Inv = ref Time.instances[Time.currantinstance].inventory;


                for (int b = 0; b < PlayerHotbar.NumberOfInvSlots; b++)
                    if (b < 10 || PlayerHotbar.InventoryOpen)
                    {
                        _spriteBatch.DrawRoundedRect(PlayerHotbar.Slots[b].Rectangle, circle, circle.Width / 2, (b == PlayerHotbar.Selected || b == PlayerHotbar.mouseOnSlot) ? Color.Red : Color.OrangeRed);
                        if (PlayerHotbar.Slots[b].InvReference != -1)
                            _spriteBatch.Draw(Inventory.ItemsProperties[Inv.Items[PlayerHotbar.Slots[b].InvReference].number].image, new Vector2(PlayerHotbar.Slots[b].Rectangle.X + PlayerHotbar.Slots[b].Rectangle.Width / 2 - Inventory.ItemsProperties[Inv.Items[PlayerHotbar.Slots[b].InvReference].number].image.Width / 2, PlayerHotbar.Slots[b].Rectangle.Y + PlayerHotbar.Slots[b].Rectangle.Height / 2 - Inventory.ItemsProperties[Inv.Items[PlayerHotbar.Slots[b].InvReference].number].image.Height / 2), colorclear);
                    }
                if (PlayerHotbar.InventoryOpen)
                {
                    PlayerHotbar.DoStorageSlots();
                    _spriteBatch.DrawRoundedRect(PlayerHotbar.StorageRectangle, circle, circle.Width / 2, Color.OrangeRed * (float)0.5);
                    for (int b = 0; b < PlayerHotbar.NumberOfStorageSlots; b++)
                    {

                        if (PlayerHotbar.StorageSlots[b].InvReference != -1)
                            _spriteBatch.Draw(Inventory.ItemsProperties[Inv.Items[PlayerHotbar.StorageSlots[b].InvReference].number].image, new Vector2(PlayerHotbar.StorageSlots[b].Rectangle.X + PlayerHotbar.StorageSlots[b].Rectangle.Width / 2 - Inventory.ItemsProperties[Inv.Items[PlayerHotbar.StorageSlots[b].InvReference].number].image.Width / 2, PlayerHotbar.StorageSlots[b].Rectangle.Y + PlayerHotbar.StorageSlots[b].Rectangle.Height / 2 - Inventory.ItemsProperties[Inv.Items[PlayerHotbar.StorageSlots[b].InvReference].number].image.Height / 2), colorclear);
                    }
                    _spriteBatch.DrawRoundedRect(PlayerHotbar.SettingRectangle, circle, circle.Width / 2, Color.OrangeRed * (float)0.5);
                    for (int b = 0; b < PlayerHotbar.SettingSlots.Length; b++)
                    {

                        Texture2D SettingImage = PlayerHotbar.SettingSlots[b].Selected ? PlayerHotbar.SettingSlots[b].IconSelected : PlayerHotbar.SettingSlots[b].Icon;
                            _spriteBatch.Draw(SettingImage, new Vector2(PlayerHotbar.SettingSlots[b].Rectangle.X + PlayerHotbar.SettingSlots[b].Rectangle.Width / 2 - SettingImage.Width / 2, PlayerHotbar.SettingSlots[b].Rectangle.Y + PlayerHotbar.SettingSlots[b].Rectangle.Height / 2 - SettingImage.Height / 2), colorclear);
                    }
                }


                if (PlayerHotbar.Slots[PlayerHotbar.HOLDINGSLOT].InvReference != -1)
                    _spriteBatch.Draw(Inventory.ItemsProperties[Inv.Items[PlayerHotbar.Slots[PlayerHotbar.HOLDINGSLOT].InvReference].number].image, new Vector2(MousePosition.X, MousePosition.Y), colorclear);


                if (PlayerHotbar.InventoryOpen && PlayerHotbar.Slots[PlayerHotbar.HOLDINGSLOT].InvReference == -1)
                    if (PlayerHotbar.mouseOnSlot >= 0 && PlayerHotbar.Slots[PlayerHotbar.mouseOnSlot].InvReference != -1)
                    {
                        PlayerHotbar.SelectedSlotText.text.text = new string("BlockID: " + Inv.Items[PlayerHotbar.Slots[PlayerHotbar.mouseOnSlot].InvReference].acquireryData.ID.ToString());
                        if (Inv.Items[PlayerHotbar.Slots[PlayerHotbar.mouseOnSlot].InvReference].acquireryData.ID != -1)
                            PlayerHotbar.SelectedSlotText.text.text += "\nOriginal Coords: " + Time.blockIDs[Inv.Items[PlayerHotbar.Slots[PlayerHotbar.mouseOnSlot].InvReference].acquireryData.ID].Originalcoords.X.ToString() + ", " + Time.blockIDs[Inv.Items[PlayerHotbar.Slots[PlayerHotbar.mouseOnSlot].InvReference].acquireryData.ID].Originalcoords.Y.ToString() + "\nAcquiered at:" +
                                "\n Time: " + Time.instances[Inv.Items[PlayerHotbar.Slots[PlayerHotbar.mouseOnSlot].InvReference].acquireryData.reference.instance].Events[Inv.Items[PlayerHotbar.Slots[PlayerHotbar.mouseOnSlot].InvReference].acquireryData.reference.eventnum].Time.ToString() +
                                "\n Instance: " + Inv.Items[PlayerHotbar.Slots[PlayerHotbar.mouseOnSlot].InvReference].acquireryData.reference.instance.ToString();
                        PlayerHotbar.SelectedSlotText.draw(_spriteBatch, MousePosition.X, MousePosition.Y);
                    }
                    else if (PlayerHotbar.mouseOnStorageSlot >= 0 && PlayerHotbar.StorageSlots[PlayerHotbar.mouseOnStorageSlot].InvReference != -1)
                    {
                        PlayerHotbar.SelectedSlotText.text.text = new string("BlockID: " + Inv.Items[PlayerHotbar.StorageSlots[PlayerHotbar.mouseOnStorageSlot].InvReference].acquireryData.ID.ToString());
                        if (Inv.Items[PlayerHotbar.StorageSlots[PlayerHotbar.mouseOnStorageSlot].InvReference].acquireryData.ID != -1)
                            PlayerHotbar.SelectedSlotText.text.text += "\nOriginal Coords: " + Time.blockIDs[Inv.Items[PlayerHotbar.StorageSlots[PlayerHotbar.mouseOnStorageSlot].InvReference].acquireryData.ID].Originalcoords.X.ToString() + ", " + Time.blockIDs[Inv.Items[PlayerHotbar.StorageSlots[PlayerHotbar.mouseOnStorageSlot].InvReference].acquireryData.ID].Originalcoords.Y.ToString() + "\nAcquiered at:" +
                                "\n Time: " + Time.instances[Inv.Items[PlayerHotbar.StorageSlots[PlayerHotbar.mouseOnStorageSlot].InvReference].acquireryData.reference.instance].Events[Inv.Items[PlayerHotbar.StorageSlots[PlayerHotbar.mouseOnStorageSlot].InvReference].acquireryData.reference.eventnum].Time.ToString() +
                                "\n Instance: " + Inv.Items[PlayerHotbar.StorageSlots[PlayerHotbar.mouseOnStorageSlot].InvReference].acquireryData.reference.instance.ToString() +
                                "\n Event: " + Inv.Items[PlayerHotbar.StorageSlots[PlayerHotbar.mouseOnStorageSlot].InvReference].acquireryData.reference.eventnum.ToString();

                        PlayerHotbar.SelectedSlotText.draw(_spriteBatch, MousePosition.X, MousePosition.Y);
                    }
                    else if (PlayerHotbar.mouseOnSettingSlot >= 0)
                    {//need to use data from previeus instancese
                        PlayerHotbar.SelectedSlotText.text.text = PlayerHotbar.SettingSlots[PlayerHotbar.mouseOnSettingSlot].Text;

                        PlayerHotbar.SelectedSlotText.draw(_spriteBatch, MousePosition.X, MousePosition.Y);
                    }


            }
            else if (CurrentlyDisplaying == DEBUGLOADING)
            {

            }
            else if (CurrentlyDisplaying == DEBUG)
            {
               




            }

            Extratext.draw(_spriteBatch, 50, GraphicsRectangle.Height - 50);

            for (int b = 0; b < Buttons.Count(); b++)
                if (Buttons[b].visible)
                {
                    Color LetterColor = colorclear;
                    if (Buttons[b].TextOnly == false)
                    {
                        _spriteBatch.DrawRoundedRect(Buttons[b].Rectangle, // The coordinates of the Rectangle to be drawn
circle, // Texture for the whole rounded rectangle
circle.Width / 2, // Distance from the edges of the texture to the "middle" patch
Buttons[b].Selected ? Color.Red : Color.OrangeRed);
                    }
                    else
                    {
                        LetterColor = Buttons[b].Selected ? Color.Red : colorclear;
                    }
                    for (int i = 0; i < Buttons[b].text.chars.Length; i++)
                    {
                        if (LetterColor == colorclear)
                            LetterColor = LetterColors[Buttons[b].text.color[i]];
                        _spriteBatch.Draw(Letters[Buttons[b].text.chars[i]], Buttons[b].text.coords[i] + new Vector2(Buttons[b].Rectangle.X + Buttons[b].Rectangle.Width / 2 - Buttons[b].text.size.Width / 2, Buttons[b].Rectangle.Y + Buttons[b].Rectangle.Height / 2 - Buttons[b].text.size.Height / 2), LetterColor);
                    }
                        

                }



            if (Infomode)
                Info.draw(_spriteBatch, 25, 100);

            
            if (ChatOn)
            {
                DrawRectangle(new Rectangle(25, GraphicsRectangle.Height - 50 - Chat.Lines * 16, Chat.data.size.Width, Chat.data.size.Height), Color.Black * Chat.OpacityOfRectangle);
                for (int i = 0; i < Chat.data.chars.Length; i++)
                    _spriteBatch.Draw(Letters[Chat.data.chars[i]], Chat.data.coords[i] + new Vector2(25, GraphicsRectangle.Height - 50 - Chat.Lines * 16), LetterColors[Chat.data.color[i]] * (float)(Convert.ToDouble(Chat.TimeDisplayed[Chat.TotalLines - (Chat.Lines - Chat.data.LinePerCharecter[i])] + Chat.TimePerLine - Time.time)/ Chat.LengthOfFade));

            }

            _spriteBatch.End();

            FPS = 0;
            float CurrentlyDisplayTime = DateTime.Now.Minute * 60 + DateTime.Now.Second + (float)DateTime.Now.Millisecond / 1000;
            FrameDisplayTime[currentFrame] = CurrentlyDisplayTime - FrameDisplayTime[currentFrame];
            for (int i = 0; i < FrameDisplayTime.Length; i++)
            {

                FPS += FrameDisplayTime[i];
            }
            //int oldFrame = currentFrame;
            if (currentFrame >= 59)
                currentFrame = 0;
            else
                currentFrame += 1;
            FrameDisplayTime[currentFrame] = CurrentlyDisplayTime;


            base.Draw(gameTime);
        }

        private void Window_ClientSizeChanged(object sender, System.EventArgs e)
        {
            screensizechanged = true;
        }

        void DoLettersOnScreen()
        {





            Extratext.text = "Ωa0Ωb1Ωc2Ωd3Ωe4Ωf5Ωg6Ωh7\nΩi8Ωj9Ωk0Ωl1Ωm2Ωn3Ωo4Ωp5Ωz5  " + "Ωa" + Rainbow.R.ToString() + " " + Rainbow.G.ToString() + " " + Rainbow.B.ToString() + " " + Rainbow.A.ToString();
            Extratext.DoText();

            Chat.RefreshText(Time.time);
            Chat.data.DoText();


            if (CurrentlyDisplaying != MAINMENU)
            {
                if (CurrentlyDisplaying == GAME)
                {
                    Coordinates.text = "Ωg" + Time.instances[Time.currantinstance].position.X.ToString() + ", " + Time.instances[Time.currantinstance].position.Y.ToString();
                    Coordinates.DoText();
                }

                if (Infomode)
                {
                    Info.text = "WORLD:" +
                        "\n  Width: " + width.ToString() + ", Height: " + height.ToString() + " LoadingareaX: " + loadingareaX.ToString() + " LoadingareaY: " + loadingareaY.ToString() + " Seed: " + seed;
                    Info.text +=
                           "\n\nPLAYER:" +
                            "\n  ΩbPlayerVelocity X: " + Time.instances[Time.currantinstance].velocity.X + " Y: " + Time.instances[Time.currantinstance].velocity.Y;
                    Info.text += "\nFPS: " + (60 / FPS).ToString();

                    Info.text +=
                        "\n\nMOUSE:" +
                        "\n  Position X : " + MousePosition.X.ToString() + " Y: " + MousePosition.Y.ToString() +
                        "\n  Selected Block X : " + SelectedBlock.X.ToString() + " Y: " + SelectedBlock.Y.ToString() + " Type: " + SelectedBlockType.ToString();

                   

                    Info.text +=
                        "\n\nΩbTIME: " + Time.time.ToString();

                    Info.DoText();
                }
            }
                if (Rainbow.R == 255 && Rainbow.G == 0 && Rainbow.B > 0)
                    Rainbow.B -= 5;
                else if (Rainbow.R == 255 && Rainbow.G < 255 && Rainbow.B == 0)
                    Rainbow.G += 5;
                else if (Rainbow.R > 0 && Rainbow.G == 255 && Rainbow.B == 0)
                    Rainbow.R -= 5;
                else if (Rainbow.R == 0 && Rainbow.G == 255 && Rainbow.B < 255)
                    Rainbow.B += 5;
                else if (Rainbow.R == 0 && Rainbow.G > 0 && Rainbow.B == 255)
                    Rainbow.G -= 5;
                else if (Rainbow.R < 255 && Rainbow.G == 0 && Rainbow.B == 255)
                    Rainbow.R += 5;
                else
                {
                    Rainbow.R = 255;
                    Rainbow.A = 255;
                }
            LetterColors[20] = Rainbow.col;
        }
        void RedoScreenVars()
        {
            GraphicsRectangle = new Rectangle(this.GraphicsDevice.Viewport.X, this.GraphicsDevice.Viewport.Y, this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);
            middleposition = new Vector2(GraphicsRectangle.Width / 2, GraphicsRectangle.Height / 2);
            screensizechanged = false;
            if (CurrentlyDisplaying == GAME)
            {
                RedoLoadingarea();
            }
            int row = 0;
            for (int i = 0; i < PositionAnchors.Count(); i++)
            {
                PositionAnchors[i] = new Vector2((i - row * 3) * GraphicsDevice.Viewport.Width / 2, row * GraphicsDevice.Viewport.Height / 2);
                row += i - row * 3 == 3 ? 1 : 0;
            }
            //for (buttons)
            for (int b = 0; b < Buttons.Count(); b++)
                ResetButtonPosition(b);

        }
        void RedoLoadingarea()
        {
            loadingareaX = GraphicsRectangle.Width / blocksize / 2 + 1;
            loadingareaY = GraphicsRectangle.Height / blocksize / 2 + blocksize / 8 * 2;
        }

        void PauseGame()
        {
            Paused = !Paused;
            if (Paused)
                Buttons[SETTINGSBUTTON].visible = true;
            else
            {
                Buttons[SETTINGSBUTTON].visible = false;
                Buttons[RESETBUTTON].visible = false;
                Buttons[NEWWORLDBUTTON].visible = false;
            }
        }

        public struct Button
        {
            public Rectangle Rectangle;
            public Text text;
            public Boolean Selected;
            public Boolean visible;
            public Boolean TextOnly;
            public int PositionAnchor;
            public Vector2 Position;
        }

        void ButtonClick(int buttonNum)
        {
            Buttons[buttonNum].Selected = false;
            if (buttonNum == PLAYBUTTON)//PLAY BUTTON
            {
                Buttons[PLAYBUTTON].visible = false;
                Buttons[CREATENEWWORLDBUTTON].visible = true;
                Buttons[LOADWORLDBUTTON].visible = true;
                Buttons[DEBUGBUTTON].visible = true;
            }
            else if (buttonNum == CREATENEWWORLDBUTTON)//CREATE NEW WORLD BUTTON
            {
                Buttons[CREATENEWWORLDBUTTON].visible = false;
                Buttons[DEBUGBUTTON].visible = false;
                Buttons[LOADWORLDBUTTON].visible = false;
                CurrentlyDisplaying = LOADING;
                Loadworld = false;
            }
            else if (buttonNum == LOADWORLDBUTTON)//LOAD WORLD BUTTON
            {
                Buttons[CREATENEWWORLDBUTTON].visible = false;
                Buttons[DEBUGBUTTON].visible = false;
                Buttons[LOADWORLDBUTTON].visible = false;
                ButtonChangeText(WORLD1BUTTON, Storage.FindWorlds());
                Buttons[WORLD1BUTTON].visible = true;
            }
            else if (buttonNum == WORLD1BUTTON)//LOAD WORLD1 BUTTON
            {
                Buttons[WORLD1BUTTON].visible = false;

                Loadworld = true;
                CurrentlyDisplaying = LOADING;
            }
            else if (buttonNum == DEBUGBUTTON)//DEBUG BUTTON
            {
                Buttons[CREATENEWWORLDBUTTON].visible = false;
                Buttons[DEBUGBUTTON].visible = false;
                Buttons[LOADWORLDBUTTON].visible = false;
                CurrentlyDisplaying = DEBUGLOADING;

            }
            else if (buttonNum == SETTINGSBUTTON)//SETTINGS BUTTON
            {
                Buttons[NEWWORLDBUTTON].visible = !Buttons[NEWWORLDBUTTON].visible;
                Buttons[RESETBUTTON].visible = !Buttons[RESETBUTTON].visible;
                Buttons[SAVEWORLDBUTTON].visible = !Buttons[SAVEWORLDBUTTON].visible;

            }
            else if (buttonNum == RESETBUTTON)//RESET BUTTON
            {
                CurrentlyDisplaying = LOADING;
            }
            else if (buttonNum == NEWWORLDBUTTON)//NEWWORLD BUTTON
            {
                seed = "";
                CurrentlyDisplaying = LOADING;
            }
            else if (buttonNum == SAVEWORLDBUTTON)//SAVEWORLD BUTTON
            {
                Storage.SaveWorld(WorldName);
            }
        }
        
        void NewButton(int buttonNum, string ButtonText, int PositionAnchor, int RecX, int RecY, Boolean TextOnly = true, int RecWidth = 200, int RecHeight = 50, Boolean ButtonVisible = false)
        {
            Buttons[buttonNum].Position = new Vector2(RecX, RecY);
            Buttons[buttonNum].PositionAnchor = PositionAnchor;
            Buttons[buttonNum].TextOnly = TextOnly;
            Buttons[buttonNum].Rectangle = new Rectangle(RecX, RecY, RecWidth, RecHeight);
            Buttons[buttonNum].text = new Text();
            ButtonChangeText(buttonNum, ButtonText);
            Buttons[buttonNum].visible = ButtonVisible;
        }
        void ButtonChangeText(int buttonNum, string ButtonText)
        {
            Buttons[buttonNum].text.text = ButtonText;
            Buttons[buttonNum].text.DoText();
            if (Buttons[buttonNum].TextOnly)
            {
                int RecWidth = Buttons[buttonNum].text.size.Width;
                int RecHeight = Buttons[buttonNum].text.size.Height;
                Buttons[buttonNum].Rectangle.Size = new Point(RecWidth, RecHeight);
            }
        }
        void ResetButtonPosition(int buttonNum)
        {
            Vector2 PositionModifier = PositionAnchors[Buttons[buttonNum].PositionAnchor];
            if (Buttons[buttonNum].PositionAnchor == TOPRIGHT || Buttons[buttonNum].PositionAnchor == MIDDLERIGHT || Buttons[buttonNum].PositionAnchor == BOTTOMRIGHT)
                PositionModifier.X -= Buttons[buttonNum].Rectangle.Width;
            Buttons[buttonNum].Rectangle = new Rectangle(Convert.ToInt32(PositionModifier.X + Buttons[buttonNum].Position.X), Convert.ToInt32(PositionModifier.Y + Buttons[buttonNum].Position.Y), Buttons[buttonNum].Rectangle.Width, Buttons[buttonNum].Rectangle.Height);
        }

        void InitializeMainMenu()
        {
            Buttons = new Button[9];  //change when adding new button
            NewButton(PLAYBUTTON, "Play", TOPMIDDLE, -100, 200, false, 200, 50);
            NewButton(CREATENEWWORLDBUTTON, "Create New World", TOPMIDDLE, -150, 100, false, 300, 50);
            NewButton(LOADWORLDBUTTON, "Load World", TOPMIDDLE, -150, 200, false, 300, 50);
            NewButton(DEBUGBUTTON, "Debug", TOPMIDDLE, -150, 300, false, 300, 50);
            NewButton(WORLD1BUTTON, "World", TOPMIDDLE, -150, 200, false, 300, 100);
            NewButton(SETTINGSBUTTON, "Settings", TOPRIGHT, -20, 20);
            NewButton(RESETBUTTON, "Reset", TOPRIGHT, -20, 50);
            NewButton(NEWWORLDBUTTON, "New World", TOPRIGHT, -20, 70);
            NewButton(SAVEWORLDBUTTON, "Save World", TOPRIGHT, -20, 90);


            Buttons[PLAYBUTTON].visible = true;
            MainMenuInitilised = true;
        }

        Texture2D CreateCircle(int diameter)
        {
            Texture2D texture = new Texture2D(GraphicsDevice, diameter, diameter);
            Color[] colorData = new Color[diameter * diameter];

            float radius = diameter / 2f;
            float radiussq = radius * radius;

            for (int x = 0; x < diameter; x++)
            {
                for (int y = 0; y < diameter; y++)
                {
                    int index = x * diameter + y;
                    Vector2 pos = new Vector2(x - radius, y - radius);
                    Vector2 pos2 = new Vector2(x - radius + 1, y - radius);
                    Vector2 pos3 = new Vector2(x - radius, y - radius + 1);
                    Vector2 pos4 = new Vector2(x - radius + 1, y - radius + 1);
                    if (pos.LengthSquared() <= radiussq || pos2.LengthSquared() <= radiussq || pos3.LengthSquared() <= radiussq || pos4.LengthSquared() <= radiussq)
                    {
                            colorData[index] = Color.White;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }
            texture.SetData(colorData);
            return texture;
        }

        public static Texture2D Getborders(Texture2D texture, int bordersize, Color bordercolor, int bordersizeOverImage, bool onlyBorder = false)
        {
            Color actualbordercolor = bordercolor;
            if (onlyBorder) 
            {
                bordercolor = Color.Black;
            }

            //Texture2D newtexture;
            Color[] colorData = new Color[(texture.Width) * (texture.Height)];
            texture.GetData(colorData);
            for (int x = 0; x < texture.Width; x++)
                for (int y = 0; y < texture.Height; y++)
                {
                    int index = x * texture.Width + y;
                    if (colorData[index] != Color.Transparent)
                        for (int neighborX = x - bordersize; neighborX <= x + bordersize; neighborX++)
                            for (int neighborY = y - bordersize; neighborY <= y + bordersize; neighborY++)
                                if (neighborX != x || neighborY != y)
                                {
                                    int indexNeighbor = neighborX * texture.Width + neighborY;
                                    if (indexNeighbor < 0 || indexNeighbor >= texture.Width * texture.Height || neighborY < 0 || neighborY >= texture.Height)
                                        colorData[index] = bordercolor;
                                    else if (colorData[indexNeighbor] == Color.Transparent)
                                    {
                                        colorData[index] = bordercolor;
                                    }
                                }
                }
            if (onlyBorder)
                for (int x = 0; x < texture.Width; x++)
                    for (int y = 0; y < texture.Height; y++)
                    {
                        int index = x * texture.Width + y;
                        if (colorData[index] == Color.White)
                            colorData[index] = Color.Transparent;
                        else if (colorData[index] == Color.Black)
                            colorData[index] = actualbordercolor;
                    }


            Texture2D TextureFinished = new Texture2D(texture.GraphicsDevice, texture.Width, texture.Height);
            TextureFinished.SetData(colorData);
            return TextureFinished;
        }



        private static Texture2D rect;

        private void DrawRectangle(Rectangle coords, Color color)
        {
            if (rect == null)
            {
                rect = new Texture2D(GraphicsDevice, 1, 1);
                rect.SetData(new[] { Color.White });
            }
            _spriteBatch.Draw(rect, coords, color);
        }

        public struct ColorRGB
        {
            internal Microsoft.Xna.Framework.Color col;

            public byte R
            {
                get { return col.R; }
                set { col.R = value; }
            }
            public byte G
            {
                get { return col.G; }
                set { col.G = value; }
            }
            public byte B
            {
                get { return col.B; }
                set { col.B = value; }
            }
            public byte A
            {
                get { return col.A; }
                set { col.A = value; }
            }
        }


    }
    class KeyBoardKey
    {
        public bool Pressed;
        public bool Holdable;
        public bool activateOnStateChange = false;
        public int Continue;
        public int MaxContinueTime;
        public int MaxContinueTimeOriginal;
        public int InitialWaitTime;
        public int InitialWaitTimeOriginal;
        public int MinWaitTime;
        public Keys Key;
        public void NewKeyBoardKey(int thisMaxContinueTime = 10, int thisMinWaitTime = 5, int thisInitialWaitTime = 25)
        {
            MaxContinueTimeOriginal = thisMaxContinueTime;
            MaxContinueTime = MaxContinueTimeOriginal;
            MinWaitTime = thisMinWaitTime;
            InitialWaitTimeOriginal = thisInitialWaitTime - (MaxContinueTime + MinWaitTime);
            InitialWaitTime = InitialWaitTimeOriginal;
            Holdable = true;
        }
        public Boolean CheckKeyPress(Boolean Overide = false)
        {
            bool Canactivate = false;
            if (Keyboard.GetState().IsKeyUp(Key) && (Pressed == true))
            {
                Pressed = false;
                if (Holdable)
                {
                    Continue = 0;
                    MaxContinueTime = MaxContinueTimeOriginal;
                    InitialWaitTime = InitialWaitTimeOriginal;
                }
                if (activateOnStateChange)
                    Canactivate = true;
            }
            
            else if ((Keyboard.GetState().IsKeyDown(Key)))
            {
                if (!Overide)
                {
                    if (Holdable)
                    {
                        Continue += 1;
                        if (Pressed == false || Continue > (MaxContinueTime + MinWaitTime + InitialWaitTime))
                        {
                            Canactivate = true;
                            Continue = 0;
                            if (MaxContinueTime > 0)
                                MaxContinueTime -= 1;
                            if (Pressed == true)
                                InitialWaitTime = 0;
                        }
                    }
                    else if (Pressed == false)
                        Canactivate = true;
                    Pressed = true;
                }
                else
                    Canactivate = true;

            }
            return Canactivate;
        }
    }


    
    class RectangleSprite
    {
        static Texture2D _pointTexture;
        public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color, int lineWidth)
        {
            if (_pointTexture == null)
            {
                _pointTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                _pointTexture.SetData<Color>(new Color[] { Color.White });
            }

            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width + lineWidth, lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X + rectangle.Width, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width + lineWidth, lineWidth), color);
        }
    }

    public class Chat
    {
        public Text data = new Text();
        public const int LineLimit = 20;
        public const int TimePerLine = 150;
        //public int StartChar;
        public int TotalLines;
        public int Lines;
        public int[] TimeDisplayed;
        public string[] Texts;
        public int LengthOfFade = 25;
        public int maxLengthOfchat = 300;
        public float OpacityOfRectangle = 0.5F;


        public void Initialize()
        {
            TimeDisplayed = new int[maxLengthOfchat];
            Texts = new string[maxLengthOfchat];
        }

        public void NewLine(string Newtext, int TimeToDisplay)
        {

            if (TotalLines + 1 >= maxLengthOfchat)
                TotalLines = 0;
            Texts[TotalLines] = "Ωa" + Newtext;
            TimeDisplayed[TotalLines] = TimeToDisplay;
            TotalLines += 1;
        }
        public void RefreshText(int time)
        {
            Lines = 0;
            data.text = "";
            for (int i = TotalLines - 1; i >= 0; i--)
            {
                if (TimeDisplayed[i] <= time && time <= TimeDisplayed[i] + TimePerLine)
                {
                    string newline = "";
                    if (Lines > 0)
                        newline = "\n";
                    data.text = Texts[i] + newline +  data.text;
                    Lines += 1; 
                }
                else
                    break;
                if (Lines >= LineLimit)
                    break;
            }
            
        }
    }

    public static class SpriteBatchExtensions
    {
        public static void DrawRoundedRect(this SpriteBatch spriteBatch, Rectangle destinationRectangle, Texture2D texture, int border, Color color)
        {
            // Top left
            spriteBatch.Draw(
                texture,
                new Rectangle(destinationRectangle.Location, new Point(border)),
                new Rectangle(0, 0, border, border),
                color);

            // Top
            spriteBatch.Draw(
                texture,
                new Rectangle(destinationRectangle.Location + new Point(border, 0),
                    new Point(destinationRectangle.Width - border * 2, border)),
                new Rectangle(border, 0, texture.Width - border * 2, border),
                color);

            // Top right
            spriteBatch.Draw(
                texture,
                new Rectangle(destinationRectangle.Location + new Point(destinationRectangle.Width - border, 0), new Point(border)),
                new Rectangle(texture.Width - border, 0, border, border),
                color);

            // Middle left
            spriteBatch.Draw(
                texture,
                new Rectangle(destinationRectangle.Location + new Point(0, border), new Point(border, destinationRectangle.Height - border * 2)),
                new Rectangle(0, border, border, texture.Height - border * 2),
                color);

            // Middle
            spriteBatch.Draw(
                texture,
                new Rectangle(destinationRectangle.Location + new Point(border), destinationRectangle.Size - new Point(border * 2)),
                new Rectangle(border, border, texture.Width - border * 2, texture.Height - border * 2),
                color);

            // Middle right
            spriteBatch.Draw(
                texture,
                new Rectangle(destinationRectangle.Location + new Point(destinationRectangle.Width - border, border),
                    new Point(border, destinationRectangle.Height - border * 2)),
                new Rectangle(texture.Width - border, border, border, texture.Height - border * 2),
                color);

            // Bottom left
            spriteBatch.Draw(
                texture,
                new Rectangle(destinationRectangle.Location + new Point(0, destinationRectangle.Height - border), new Point(border)),
                new Rectangle(0, texture.Height - border, border, border),
                color);

            // Bottom
            spriteBatch.Draw(
                texture,
                new Rectangle(destinationRectangle.Location + new Point(border, destinationRectangle.Height - border),
                    new Point(destinationRectangle.Width - border * 2, border)),
                new Rectangle(border, texture.Height - border, texture.Width - border * 2, border),
                color);

            // Bottom right
            spriteBatch.Draw(
                texture,
                new Rectangle(destinationRectangle.Location + destinationRectangle.Size - new Point(border), new Point(border)),
                new Rectangle(texture.Width - border, texture.Height - border, border, border),
                color);
        }

        public static void DrawVertices(this SpriteBatch spriteBatch, Texture2D pixel, DoubleCoordinates[] TestVectrices, Vector2 position, Vector2 middleposition, Color color)
        {
            int next = 0;
            for (int current = 0; current < TestVectrices.Length; current++)
            {
                next = current + 1;
                if (next == TestVectrices.Length) next = 0;
                DoubleCoordinates vc = TestVectrices[current];    // c for "current"
                DoubleCoordinates vn = TestVectrices[next];       // n for "next"
                double pente = (vn.Y - vc.Y) / (vn.X - vc.X);
                double b = vc.Y - pente * vc.X;
                double Forvalue1 = vc.X;
                double Forvalue2 = vn.X;
                bool ForLoopY = false;

                if (Math.Abs(vc.Y - vn.Y) > Math.Abs(vc.X - vn.X))
                {
                    Forvalue1 = vc.Y;
                    Forvalue2 = vn.Y;
                    ForLoopY = true;
                }
                if (Forvalue1 > Forvalue2)
                {
                    double savedvalue = Forvalue1;
                    Forvalue1 = Forvalue2;
                    Forvalue2 = savedvalue;
                }
                for (int i = Convert.ToInt32(Forvalue1); i < Convert.ToInt32(Forvalue2); i++)
                {
                    if (ForLoopY)
                    {
                        //pente = (Forvalue2 - Forvalue1) / (vc.X  - vn.X );
                        int speciealy;
                        if (pente > 1000000 || pente < -1000000)
                            speciealy = Convert.ToInt32(vn.X);
                        else
                            speciealy = Convert.ToInt32((i - b) / pente);


                        spriteBatch.Draw(pixel, new Vector2(speciealy, i) - position + middleposition, color);
                    }
                    else
                    {
                        int speciealx;
                        if (pente > 1000000 || pente < -1000000)
                            speciealx = Convert.ToInt32(vn.Y);
                        else
                            speciealx = Convert.ToInt32(pente * i + b);


                        spriteBatch.Draw(pixel, new Vector2(i, speciealx) - position + middleposition, color);

                    }

                }

            }
        }

        public static Texture2D Resize(GraphicsDevice _GraphicsDevice, Texture2D texture, double ResizeamountX, double ResizeamountY = 0)
        {
            if (ResizeamountY == 0) ResizeamountY = ResizeamountX;
            Texture2D newtexture = new Texture2D(_GraphicsDevice, Convert.ToInt32(texture.Width * ResizeamountX), Convert.ToInt32(texture.Height * ResizeamountY));
            Color[] newcolorData = new Color[Convert.ToInt32(texture.Width * ResizeamountX) * Convert.ToInt32(texture.Height * ResizeamountY)];

            Color[] colorData = new Color[texture.Width * texture.Height];
            texture.GetData(colorData);
            for (int x = 0; x < texture.Height; x++)
                for (int y = 0; y < texture.Width; y++)
                {
                    int index = x * texture.Width + y;
                    for (int newx = Convert.ToInt32(x * ResizeamountY); newx < Convert.ToInt32(x * ResizeamountY + ResizeamountY); newx++)
                        for (int newy = Convert.ToInt32(y * ResizeamountX); newy < Convert.ToInt32(y * ResizeamountX + ResizeamountX); newy++)
                        {
                            int newindex = Convert.ToInt32(newx * newtexture.Width + newy);
                            newcolorData[newindex] = colorData[index];
                        }
                }

            newtexture.SetData(newcolorData);
            return newtexture;
        }
    }

    public static class CreateVertice
    {
        public static (Texture2D, Vector2) NewTexture(GraphicsDevice _GraphicsDevice, Point PointA, Point PointB)
        {
            double m = ((PointA.X - PointB.X) == 0 )? 1000 : (double)(PointA.Y - PointB.Y) / (double)(PointA.X - PointB.X);
            //int extraY = PointA.Y < PointB.Y ? PointA.Y : PointB.Y;
            //int extraX = PointA.X < PointB.X ? PointA.X : PointB.X;
            int width = Math.Abs(PointA.X - PointB.X) == 0 ? 1 : Math.Abs(PointA.X - PointB.X);
            int height = Math.Abs(PointA.Y - PointB.Y) == 0 ? 1 : Math.Abs(PointA.Y - PointB.Y);
            int b = m < 0 ? height -1: 0;
            Texture2D image = new Texture2D(_GraphicsDevice, width, height);
            Color[] colorData = new Color[image.Width * image.Height];
            if (height > width)
                for (int y = 0; y < image.Height; y++)
                {
                    //for(int x = -2; x < 2 + 1; x++)
                    //{
                    //    if((x + (int)((y - b) / m) >= 0 ) && (x + (int)((y - b) / m)) < width)
                    //        {
                            int index = y * image.Width + (int)((y - b) / m);// + x);
                    colorData[index] = Color.White;// * (float)((double)1/(1 + Math.Abs(x) ));

                    //        }

                    //}


                }
            else
                for (int x = 0; x < image.Width; x++)
                //for (int y = 0; y < image.Width; y++)
                {

                    int index = (int)((m * x + b)) * image.Width + x;
                    colorData[index] = Color.White;

                }


            image.SetData(colorData);
            Vector2 reletivepos = new Vector2(PointA.X > PointB.X ? width : 0, PointA.Y > PointB.Y ? height : 0);
            return (image, reletivepos);
        }
    }
    public static class FadeDraw
    {
        public static fadeDrawStruct[] items = new fadeDrawStruct[0];

        public static void newItem(Texture2D image, Vector2 position, int lastingTime, bool isPlayerDependant = true)
        {
            Array.Resize(ref items, items.Length + 1);
            items[items.Length - 1].texture = image;
            items[items.Length - 1].position = position;
            items[items.Length - 1].startTime = Time.time;
            items[items.Length - 1].endTime = Time.time + lastingTime;
            items[items.Length - 1].isPlayerDependant = isPlayerDependant;
        }
        public static void draw(SpriteBatch spriteBatch, Vector2 middlepos)
        {
            int done = 0;
            for (int i = 0; i < items.Length; i++)
            {
                if (done > 0)
                {
                    items[i - done] = items[i];
                }
                if (items[i - done].endTime > items[i - done].startTime && items[i - done].endTime > Time.time || items[i - done].endTime < items[i - done].startTime && items[i - done].endTime < Time.time)
                {
                    Vector2 playerpos = items[i - done].isPlayerDependant ? new Vector2((float)Time.instances[Time.currantinstance].position.X, (float)Time.instances[Time.currantinstance].position.Y) : new Vector2(0, 0);
                    spriteBatch.Draw(items[i - done].texture, items[i - done].position - playerpos + middlepos, Color.White * (float)((double)(items[i - done].endTime - Time.time) / (double)(items[i - done].endTime - items[i - done].startTime)));
                }
                else
                {
                    done += 1;
                }
            }
            if (done > 0)
                Array.Resize(ref items, items.Length - done);

        }
        public static void clear()
        {
            Array.Resize(ref items, 0);

        }
    }
    public struct fadeDrawStruct
    {
        public Texture2D texture;
        public Vector2 position;
        public int endTime;
        public int startTime;
        public bool isPlayerDependant;
    }


}




