using System;
using System.Collections.Generic;
using System.Text;
using CellOrganism;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CellOrganism
{
    public static class Time
    {
        public static int time = 0;
        public static int currantinstance = 0;
        static Dependancies SpawnReference;
        public static blockdatastrc[,] blockdata = new blockdatastrc[Game1.width, Game1.height];
        public static blockID[] blockIDs = new blockID[0];
        public static Inventory starterInventory;
        public static InstanceData[] instances;
        public static extrinzoid[] extrinzoidInstances;
        static DoubleCoordinates startcoords;
        const sbyte BLOCKPLACE = -1;
        const sbyte BLOCKBREAK = 1;
        const sbyte SPAWN = -2;
        const sbyte ACCELERATION = 3;
        const sbyte TRAVEL = 2;
        const sbyte VELOCITYLOSS = 4;
        const sbyte EXTRINZOIDSPAWN = 5;
        const sbyte EXTRINZOIDKILL = -5;
        const sbyte CREATEPROJECTILE = 6;
        const sbyte DESTROYPROJECTILE = -6;
        const sbyte PLAYERKILL = 7;
        const sbyte PLAYERREVIVIE = -7;
        const sbyte CORRUPTIONSPREAD = 8;
        const sbyte CORRUPTIONCLEANSE = -8;
        public static bool timeUndo = false;
        public static bool timeRedo = false;
        public static int timeOfUndo = -1;
        public static DoubleCoordinates posOfUndo;
        public static DoubleCoordinates velOfUndo;


        public static void transformation()//craft
        {

        }
        public static void newBlockPlace(WholeCoords newCoords, int invreference)//0
        {
            ref EventData[] data = ref Time.instances[currantinstance].Events;
            ref int numberOfEvents = ref Time.instances[currantinstance].numberOfEvents;
            ref Inventory Inv = ref Time.instances[currantinstance].inventory;
            EventData newEvent = new EventData();
            newEvent.Time = time;
            newEvent.Reference = Inv.Items[invreference].acquireryData.reference;
            newEvent.type = BLOCKPLACE;
            newEvent.BlockID = Inv.Items[invreference].acquireryData.ID;
            newEvent.Coords = newCoords;
            AddElementToArray(ref data, ref numberOfEvents, newEvent);

            newDepandancie(ref instances[Inv.Items[invreference].acquireryData.reference.instance].Events[Inv.Items[invreference].acquireryData.reference.eventnum].dependancies, new Dependancies(currantinstance, numberOfEvents - 1));
            newDepandancie(ref instances[currantinstance].Events[0].dependancies, new Dependancies(currantinstance, numberOfEvents - 1));
            //blockPlace(numberOfEvents, currantinstance);
        }
        public static void newBlockBreak(WholeCoords SelectedBlock)//i need a minus and plusa version of each one
        {

            ref EventData[] data = ref Time.instances[currantinstance].Events;
            ref int numberOfEvents = ref Time.instances[currantinstance].numberOfEvents;
            EventData newEvent = new EventData();
            newEvent.Time = time;
            newEvent.Reference = SpawnReference;
            newEvent.type = BLOCKBREAK;
            newEvent.BlockID = Time.blockdata[SelectedBlock.X, SelectedBlock.Y].ID;
            newEvent.Coords = SelectedBlock;
            AddElementToArray(ref data, ref numberOfEvents, newEvent);
            //Array.Resize(ref data, numberOfEvents + 1);

            newDepandancie(ref blockIDs[blockdata[SelectedBlock.X, SelectedBlock.Y].ID].dependancies, new Dependancies(currantinstance, numberOfEvents -1));
            newDepandancie(ref instances[currantinstance].Events[0].dependancies, new Dependancies(currantinstance, numberOfEvents -1));
           
            //blockBreak(numberOfEvents, currantinstance);

        }
        public static void newAcceleration(DoubleCoordinates acceleration)
        {
            ref EventData[] data = ref Time.instances[currantinstance].Events;
            ref int numberOfEvents = ref Time.instances[currantinstance].numberOfEvents;

            //Array.Resize(ref data, numberOfEvents + 1);
            EventData newEvent = new EventData();
            newEvent.Time = time;
            newEvent.Reference = SpawnReference;
            newEvent.type = ACCELERATION;
            newEvent.auxDoubleCoords = instances[currantinstance].acceleration;
            newEvent.doubleCoords = acceleration;
            newDepandancie(ref instances[currantinstance].Events[0].dependancies, new Dependancies(currantinstance, numberOfEvents));
            AddElementToArray(ref data, ref numberOfEvents, newEvent);
            //TimeReversed? instances[instance].Events[numberofthetime].auxDoubleCoords : instances[instance].Events[numberofthetime].doubleCoords
            //changeAcceleration(numberOfEvents, currantinstance);
        }
        public static void newVelocityLoss(Point loss, DoubleCoordinates positionOffset)
        {
            ref EventData[] data = ref Time.instances[currantinstance].Events;
            ref int numberOfEvents = ref Time.instances[currantinstance].numberOfEvents;
            EventData newEvent = new EventData();
            //Array.Resize(ref data, numberOfEvents + 1);
            newEvent.Time = time;
            newEvent.Reference = SpawnReference;
            newEvent.type = VELOCITYLOSS;
            newEvent.doubleCoords = positionOffset;
            newEvent.auxDoubleCoords = instances[currantinstance].velocity;
            newEvent.Coords = new WholeCoords(loss);
            newDepandancie(ref instances[currantinstance].Events[0].dependancies, new Dependancies(currantinstance, numberOfEvents));
            AddElementToArray(ref data, ref numberOfEvents, newEvent);
            //changeVelocity(numberOfEvents, currantinstance);
        }

        public static void joinWorld(DoubleCoordinates SpawnCoords)
        {
            extrinzoidInstances = new extrinzoid[0];
            instances = new InstanceData[1];
            //Time.instances[currantinstance].Events = new EventData[0];
            newSpawn(SpawnCoords);
            
        }

        public static void newSpawn(DoubleCoordinates SpawnCoords, DoubleCoordinates velocity = default)//0
        {

            ref EventData[] data = ref Time.instances[currantinstance].Events;
            data = new EventData[1];
            ref int numberOfEvents = ref instances[currantinstance].numberOfEvents;
            numberOfEvents = 0;
            instances[currantinstance].position = SpawnCoords;//movementl

            EventData newEvent = new EventData();
            newEvent.Time = time;

            newEvent.type = instances[currantinstance].timeReversed ? TRAVEL : SPAWN;
            newEvent.doubleCoords = SpawnCoords;
            newEvent.auxDoubleCoords = velocity;
            AddElementToArray(ref data, ref numberOfEvents, newEvent);
            SpawnReference = new Dependancies(currantinstance, 0);

            //spawn(numberOfEvents, currantinstance);
            //advanceTime();


        }
        public static void newTravel()
        {

            ref EventData[] data = ref Time.instances[currantinstance].Events;
            ref int numberOfEvents = ref Time.instances[currantinstance].numberOfEvents;

            EventData newEvent = new EventData();
            newEvent.Time = time;
            newEvent.Reference = SpawnReference;
            newEvent.type = TRAVEL;
            AddElementToArray(ref data, ref numberOfEvents, newEvent);
            //newEvent.doubleCoords = ;
            //newEvent.doubleCoords);


            currantinstance += 1;
            Array.Resize(ref instances, currantinstance + 1);
            instances[currantinstance] = new InstanceData();

            instances[currantinstance - 1].finalInventory = instances[currantinstance - 1].inventory;
            instances[0].inventory = starterInventory;
            instances[currantinstance - 1].velocity = new DoubleCoordinates(0, 0);
            instances[currantinstance - 1].acceleration = new DoubleCoordinates(0, 0);
            for (int i = 0; i < instances.Length - 1; i++)
                instances[i + 1].inventory = instances[i].finalInventory;
            double randomFillPercent = 50;
            int smoothIndex = 5;
            int CompressionIndex = 2;
            //seed = "";
            WorldGen mapGen = new WorldGen();
            int numberofblocks;
            (Game1.world, Game1.seed, numberofblocks) = mapGen.Start(Game1.width, Game1.height, Game1.seed, randomFillPercent, smoothIndex, CompressionIndex);
            for (int i = 0; i < blockIDs.Length; i++)
                blockIDs[i].CurrentCoords = blockIDs[i].Originalcoords;

            FadeDraw.clear();
            time = 0;

            newSpawn(instances[currantinstance - 1].position);
            
        }
        public static void newChangeTimeDirection()//need to do somthing for when change time direction in the middle of an instance
        {
            if (Time.instances[currantinstance].acceleration != new DoubleCoordinates(0, 0))
                newAcceleration(new DoubleCoordinates(0, 0));
            ref EventData[] data = ref Time.instances[currantinstance].Events;
            ref int numberOfEvents = ref Time.instances[currantinstance].numberOfEvents;
            time += instances[currantinstance].timeReversed ? 1 : -1;
            EventData newEvent = new EventData();
            newEvent.Time = time;
            newEvent.Reference = SpawnReference;
            newEvent.type = instances[currantinstance].timeReversed ? SPAWN : TRAVEL;
            newEvent.doubleCoords = instances[currantinstance].position;
            newEvent.auxDoubleCoords = instances[currantinstance].velocity;
            AddElementToArray(ref data, ref numberOfEvents, newEvent);
            //newEvent.doubleCoords = ;
            //newEvent.doubleCoords);
            //newAcceleration(new DoubleCoordinates(0, 0));

            currantinstance += 1;
            Array.Resize(ref instances, currantinstance + 1);
            instances[currantinstance] = new InstanceData();
            
            instances[currantinstance].timeReversed = !instances[currantinstance - 1].timeReversed;
            instances[currantinstance - 1].finalInventory = instances[currantinstance - 1].inventory;
            instances[0].inventory = instances[currantinstance - 1].finalInventory;
            instances[currantinstance - 1].velocity = new DoubleCoordinates(0, 0);
            
            //instances[currantinstance - 1].acceleration = new DoubleCoordinates(0, 0);
            for (int i = 0; i < instances.Length - 1; i++)
                instances[i + 1].inventory = instances[i].finalInventory;
            newSpawn(instances[currantinstance - 1].position);
            //time += instances[currantinstance].timeReversed ? 1 :-1;
        }
        public static void newTimeLine(int timeOfSplitTravel, DoubleCoordinates position, DoubleCoordinates velocity)//
        {
            timeRedo = false;
            if (Time.instances[currantinstance].acceleration != new DoubleCoordinates(0, 0))
                newAcceleration(new DoubleCoordinates(0, 0));
            ref EventData[] data = ref Time.instances[currantinstance].Events;
            ref int numberOfEvents = ref Time.instances[currantinstance].numberOfEvents;
            //time += instances[currantinstance].timeReversed ? 1 : -1;
                        EventData newEvent = new EventData();
            newEvent.Time = timeOfSplitTravel;
            newEvent.Reference = SpawnReference;
            newEvent.type = instances[currantinstance].timeReversed ? SPAWN : TRAVEL;
            newEvent.doubleCoords = position;
            newEvent.auxDoubleCoords = velocity;
            AddElementToArray(ref data, ref numberOfEvents, newEvent);
            //newEvent.doubleCoords = ;
            //newEvent.doubleCoords);
            //newAcceleration(new DoubleCoordinates(0, 0));

            currantinstance += 1;
            Array.Resize(ref instances, currantinstance + 1);
            instances[currantinstance] = new InstanceData();

            instances[currantinstance].timeReversed = instances[currantinstance - 1].timeReversed;
            instances[currantinstance - 1].finalInventory = instances[currantinstance - 1].inventory;
            instances[currantinstance].inventory = instances[currantinstance - 1].finalInventory;
            
            //instances[0].inventory = instances[currantinstance - 1].finalInventory;
            //instances[currantinstance - 1].velocity = new DoubleCoordinates(0, 0);

            newSpawn(instances[currantinstance - 1].position, instances[currantinstance - 1].velocity);
        }

        public static void newplayerKill(int instanceKilled, int eventOfProjectile)
        {
            ref EventData[] data = ref Time.instances[currantinstance].Events;
            ref int numberOfEvents = ref Time.instances[currantinstance].numberOfEvents;

            EventData newEvent = new EventData();
            newEvent.Time = time;

            newEvent.type = PLAYERKILL;
            newEvent.doubleCoords = instances[instanceKilled].position;
            newEvent.auxDoubleCoords = instances[instanceKilled].velocity;
            newEvent.BlockID = instanceKilled;
            AddElementToArray(ref data, ref numberOfEvents, newEvent);
            newDepandancie(ref instances[currantinstance].Events[eventOfProjectile].dependancies, new Dependancies(currantinstance, numberOfEvents));

        }
        public static void playerKill(int numberOfEvents, int instance)
        {
            int instamcenum = instances[instance].Events[numberOfEvents].BlockID;
            instances[instamcenum].exsists = false;
            disableEvents(ref instances[instamcenum].Events[0].dependancies);
        }
        static void disableEvents(ref Dependancies[] depends)
        {
            for (int d = 0; d < depends.Length; d++)
            {
                if (depends[d].isExtrinzoid)
                {
                    extrinzoidInstances[depends[d].instance].Events[depends[d].eventnum].isDisabled = true;
                    if (extrinzoidInstances[depends[d].instance].Events[depends[d].eventnum].dependancies != null)
                        disableEvents(ref extrinzoidInstances[depends[d].instance].Events[depends[d].eventnum].dependancies);
                }
                else
                {
                    instances[depends[d].instance].Events[depends[d].eventnum].isDisabled = true;
                    if(instances[depends[d].instance].Events[depends[d].eventnum].dependancies != null)
                        disableEvents(ref instances[depends[d].instance].Events[depends[d].eventnum].dependancies);
                }


            }
        }
        static void enableEvents(ref Dependancies[] depends)
        {
            for (int d = 0; d < depends.Length; d++)
            {
                if (depends[d].isExtrinzoid)
                {
                    //for (int n = 0; n < extrinzoidInstances[depends[d].instance].Events[depends[d].numberOfEvents].dependancies.Length; n++)
                    //{ 
                    //if()
                    //}
                    //need to check if all things that it depends on are also enabled
                        extrinzoidInstances[depends[d].instance].Events[depends[d].eventnum].isDisabled = false;
                    if (extrinzoidInstances[depends[d].instance].Events[depends[d].eventnum].dependancies != null)
                        disableEvents(ref extrinzoidInstances[depends[d].instance].Events[depends[d].eventnum].dependancies);
                }
                else
                {
                    instances[depends[d].instance].Events[depends[d].eventnum].isDisabled = false;
                    if (instances[depends[d].instance].Events[depends[d].eventnum].dependancies != null)
                        disableEvents(ref instances[depends[d].instance].Events[depends[d].eventnum].dependancies);
                }


            }
        }
        public static void newplayerRevive(int instanceKilled, int eventOfProjectile)
        {

        }
        public static void playerRevive(int numberOfEvents, int instance)
        {
            int instamcenum = instances[instance].Events[numberOfEvents].BlockID;
            instances[instamcenum].exsists = true;
            instances[instamcenum].position = instances[instance].Events[numberOfEvents].doubleCoords;
            instances[instamcenum].velocity = instances[instance].Events[numberOfEvents].auxDoubleCoords;
            enableEvents(ref instances[instamcenum].Events[0].dependancies);

        }

        public static void newCorruption(WholeCoords Location)
        {
            //System.Windows.Forms.MessageBox.Show(blockdata[Location.X, Location.Y].ID.ToString());
            if (instances[blockIDs[blockdata[Location.X, Location.Y].ID].reference.instance].Events[blockIDs[blockdata[Location.X, Location.Y].ID].reference.eventnum].Time + Math.Abs(Math.Sin(blockdata[Location.X, Location.Y].ID.GetHashCode() + Location.Y.GetHashCode() + Location.X.GetHashCode())) * 60 < time)
                {
                int[] blockcount = WorldGen.Getadjacentblockcount(ref Game1.world, Location.X, Location.Y);
                //System.Diagnostics.Debug.WriteLine("corrupting " + Location.X.ToString() + ", " + Location.Y.ToString() + " Time: " + time.ToString());

                if (blockcount[5] < 8)
                {

                    if (Location.X + 1 < Game1.width)
                        newOne(Location.X + 1, Location.Y, blockcount[5] + blockcount[6]);
                    if (Location.X - 1 > -1)
                        newOne(Location.X - 1, Location.Y, blockcount[5] + blockcount[6]);
                    if (Location.Y + 1 < Game1.height)
                        newOne(Location.X, Location.Y + 1, blockcount[5] + blockcount[6]);
                    if (Location.Y - 1 > -1)
                        newOne(Location.X, Location.Y - 1, blockcount[5] + blockcount[6]);

                }
                Game1.world[Location.X, Location.Y] = 5;
            }
            

        }
        static void newOne(int neighborX, int neighborY, int blockcount)
        {
            if (Game1.world[neighborX, neighborY] == 1 || (Game1.world[neighborX, neighborY] == 2 && blockcount > 2) || (Game1.world[neighborX, neighborY] == 4 && blockcount > 3) || (Game1.world[neighborX, neighborY] == 0 && blockcount > 4))
            {
                ref EventData[] data = ref Time.instances[currantinstance].Events;
                ref int numberOfEvents = ref Time.instances[currantinstance].numberOfEvents;

                EventData newEvent = new EventData();
                newEvent.Time = time;
                newEvent.type = CORRUPTIONSPREAD;
                newEvent.Coords = new WholeCoords(neighborX, neighborY);
                newEvent.BlockID = Game1.world[neighborX, neighborY];
                newEvent.Reference = SpawnReference;

                newDepandancie(ref instances[currantinstance].Events[0].dependancies, new Dependancies(currantinstance, numberOfEvents));
                AddElementToArray(ref data, ref numberOfEvents, newEvent);
            }
        }
        public static void CorruptionSpread(int numberofthetime, int instance)
        {
            //Game1.Paused = true;
            Game1.world[instances[instance].Events[numberofthetime].Coords.X, instances[instance].Events[numberofthetime].Coords.Y] = 6;

            blockIDs[blockdata[instances[instance].Events[numberofthetime].Coords.X, instances[instance].Events[numberofthetime].Coords.Y].ID].reference = new Dependancies(instance, numberofthetime);
            
        }
        public static void CorruptionCleanse(int numberofthetime, int instance)
        {
            Game1.world[instances[instance].Events[numberofthetime].Coords.X, instances[instance].Events[numberofthetime].Coords.Y] = (short)instances[instance].Events[numberofthetime].BlockID;
        }

        public static void newExtrinzoid(DoubleCoordinates SpawnCoords)
        {
            ref EventData[] data = ref Time.instances[currantinstance].Events;
            ref int numberOfEvents = ref Time.instances[currantinstance].numberOfEvents;
            Array.Resize(ref extrinzoidInstances, extrinzoidInstances.Length + 1);
            //extrinzoidInstances[extrinzoidInstances.Length - 1] = new extrinzoid();
            extrinzoidInstances[extrinzoidInstances.Length - 1].Events = new EventData[0];
            extrinzoidInstances[extrinzoidInstances.Length - 1].inventory = new Inventory();
            extrinzoidInstances[extrinzoidInstances.Length - 1].inventory.Items = new Itemstrc[0];
            EventData newEvent = new EventData();
            newEvent.Time = time;
            newEvent.BlockID = extrinzoidInstances.Length - 1;

            newEvent.type = EXTRINZOIDSPAWN;
            newEvent.doubleCoords = SpawnCoords;
            newEvent.Reference = SpawnReference;
            newDepandancie(ref instances[currantinstance].Events[0].dependancies, new Dependancies(currantinstance, numberOfEvents));
            AddElementToArray(ref data, ref numberOfEvents, newEvent);
        }
        static void extrinzoidSpawn(int numberofthetime, int instance)
        {
            Game1.world[(int)instances[instance].Events[numberofthetime].doubleCoords.X / Game1.blocksize, (int)instances[instance].Events[numberofthetime].doubleCoords.Y / Game1.blocksize] = 1;
            extrinzoidInstances[instances[instance].Events[numberofthetime].BlockID].exsists = true;
            extrinzoidInstances[instances[instance].Events[numberofthetime].BlockID].position = instances[instance].Events[numberofthetime].doubleCoords;
        }
        public static void newExtrinzoidKill(int extrinzoidInstance)
        {
            ref EventData[] data = ref Time.instances[currantinstance].Events;
            ref int numberOfEvents = ref Time.instances[currantinstance].numberOfEvents;
            Array.Resize(ref extrinzoidInstances, extrinzoidInstances.Length + 1);
            //extrinzoidInstances[extrinzoidInstances.Length - 1] = new extrinzoid();
            EventData newEvent = new EventData();
            newEvent.Time = time;
            newEvent.BlockID = extrinzoidInstance;
            newEvent.type = EXTRINZOIDKILL;
            newEvent.doubleCoords = extrinzoidInstances[extrinzoidInstance].position;
            newEvent.Reference = SpawnReference;
            AddElementToArray(ref data, ref numberOfEvents, newEvent);
        }
        static void extrinzoidKill(int numberofthetime, int instance)
        {
            extrinzoidInstances[instances[instance].Events[numberofthetime].BlockID].exsists = false;
        }

        public static void Travel(int numberofthetime, int instance)
        {

            instances[instance].exsists = false;
            if (timeUndo == instances[currantinstance].timeReversed)
                FadeDraw.newItem(Game1.Player, new Vector2((float)instances[instance].position.X, (float)instances[instance].position.Y) + Game1.playerOffSet, 30);
            else
                FadeDraw.newItem(Game1.Player, new Vector2((float)instances[instance].position.X, (float)instances[instance].position.Y) + Game1.playerOffSet, -30);
            //if (instances[currantinstance].timeReversed)
            //{
            //    instances[instance].position = instances[instance].Events[numberofthetime].doubleCoords;
            //    instances[instance].exsists = true;
            //}


        }
        //break, place, craft, move
        //saved data: time,

        public static void LoseinvItem(int lostnum, int instance)
        {
            for (int i = 0; i < instances[instance].inventory.Items.Length; i++)
                instances[instance].inventory.Items[i] = instances[instance].inventory.Items[i + 1];
        }



        static void blockBreak(int numberofthetime, int instance)
        {
            ref EventData[] data = ref Time.instances[instance].Events;
            ref int numberOfEvents = ref Time.instances[instance].numberOfEvents;

            WholeCoords SelectedBlock = data[numberofthetime].Coords;

            ref Inventory Inv = ref Time.instances[instance].inventory;

            if (Game1.world[SelectedBlock.X, SelectedBlock.Y] != 0)
            {

                Array.Resize(ref Inv.Items, Inv.NumberOfInvItems + 1);
                Inv.Items[Inv.NumberOfInvItems].number = Game1.world[SelectedBlock.X, SelectedBlock.Y] + 1;
                var image = CreateVertice.NewTexture(Game1._graphics.GraphicsDevice, DoubleCoordinates.convertToPoint(instances[instance].position), WholeCoords.convertToPoint(SelectedBlock * Game1.blocksize));
                if(timeUndo == instances[currantinstance].timeReversed)        
                FadeDraw.newItem(image.Item1, DoubleCoordinates.convertToVector2(instances[instance].position) - image.Item2, 15);
                else
                    FadeDraw.newItem(image.Item1, DoubleCoordinates.convertToVector2(instances[instance].position) - image.Item2, -15);
                if (Inv.Items[Inv.NumberOfInvItems].number < 2)
                    System.Windows.Forms.MessageBox.Show("bruh you break nothing man");
                Inv.Items[Inv.NumberOfInvItems].acquireryData.ID = data[numberofthetime].BlockID;
                Inv.Items[Inv.NumberOfInvItems].acquireryData.reference = new Dependancies(instance,numberofthetime);
                //Inv.Items[Inv.NumberOfInvItems].acquireryData.Instance = currantinstance;
                
                Inv.NumberOfInvItems += 1;
                Game1.world[SelectedBlock.X, SelectedBlock.Y] = (short)(0);
                blockIDs[data[numberofthetime].BlockID].CurrentCoords = new WholeCoords(-1, -1);
            }


        }
        static void blockPlace(int numberofthetime, int instance)
        {
            ref EventData[] data = ref Time.instances[instance].Events;
            ref int numberOfEvents = ref Time.instances[instance].numberOfEvents;
            WholeCoords SelectedBlock = data[numberofthetime].Coords;

            ref Inventory Inv = ref Time.instances[instance].inventory;
            int InvReference = -1;
            for (int i = 0; i < Inv.Items.Length; i++)
                if (Inv.Items[i].acquireryData.ID == data[numberofthetime].BlockID)
                {
                    InvReference = i;
                    break;
                }

            if (InvReference != -1)
            {
                if (blockIDs[data[numberofthetime].BlockID].CurrentCoords != new WholeCoords(-1, -1))// so a block can only exsist in one place at a specific time
                    Game1.world[blockIDs[data[numberofthetime].BlockID].CurrentCoords.X, blockIDs[data[numberofthetime].BlockID].CurrentCoords.Y] = (short)0;

                blockIDs[data[numberofthetime].BlockID].CurrentCoords = SelectedBlock;
                blockIDs[data[numberofthetime].BlockID].reference = new Dependancies(instance, numberofthetime);
                blockdata[SelectedBlock.X, SelectedBlock.Y].ID = data[numberofthetime].BlockID;
                //}
                if (InvReference < 2)
                    System.Windows.Forms.MessageBox.Show("bruh you selected no itme");
                if (Inv.Items[InvReference].number < 2)
                    System.Windows.Forms.MessageBox.Show("bruh you itme is not exsiust");
                Game1.world[SelectedBlock.X, SelectedBlock.Y] = (short)(Inv.Items[InvReference].number - 1);

                if (instance == currantinstance && !timeUndo && !timeRedo)
                {
                    InvReference = Game1.PlayerHotbar.Slots[Game1.PlayerHotbar.SelectedUseSlot].InvReference;
                    int newReference = -1;
                    if (Game1.PlayerHotbar.SettingSlots[Hotbar.BLOCKCHAINING].Selected)
                        for (int i = 0; i < Inv.NumberOfInvItems; i++)
                        {
                            if (i != InvReference && Inv.Items[i].number == Inv.Items[InvReference].number)
                            {
                                newReference = i;
                                break;
                            }
                        }

                    int Removedreference = InvReference;
                    for (int i = 0; i < Game1.PlayerHotbar.Slots.Length; i++)
                    {
                        if (Game1.PlayerHotbar.Slots[i].InvReference == Removedreference)
                            Game1.PlayerHotbar.Slots[i].InvReference = newReference;
                        if (Game1.PlayerHotbar.Slots[i].InvReference > Removedreference)
                            Game1.PlayerHotbar.Slots[i].InvReference -= 1;
                    }
                }

                Inv.NumberOfInvItems -= 1;
                for (int i = InvReference; i < Inv.NumberOfInvItems; i++)
                {


                    Inv.Items[i] = Inv.Items[i + 1];
                }
                Array.Resize(ref Inv.Items, Inv.NumberOfInvItems);

            }
            

        }
        public static void spawn(int numberofthetime, int instance)
        {
            instances[instance].position = instances[instance].Events[numberofthetime].doubleCoords;
            instances[instance].velocity = instances[instance].Events[numberofthetime].auxDoubleCoords;
            instances[instance].acceleration = new DoubleCoordinates(0, 0);
            instances[instance].exsists = true;
        }
        public static void newCreateProjectile(DoubleCoordinates position, DoubleCoordinates velocity, int instance)
        {
            ref EventData[] data = ref Time.instances[instance].Events;
            ref int numberOfEvents = ref Time.instances[instance].numberOfEvents;

            EventData newEvent = new EventData();
            newEvent.Time = time;
            newEvent.Reference = SpawnReference;
            newEvent.type = CREATEPROJECTILE;
            newEvent.auxDoubleCoords = velocity;
            newEvent.doubleCoords = position;
            newDepandancie(ref instances[instance].Events[0].dependancies, new Dependancies(instance, numberOfEvents));
            AddElementToArray(ref data, ref numberOfEvents, newEvent);

        }
        static void CreateProjectile(int numberofthetime, int instance)
        {
            instances[instance].blast.position = instances[currantinstance].timeReversed != timeUndo ? instances[instance].Events[numberofthetime].doubleCoords: instances[instance].Events[numberofthetime].doubleCoords - Game1.playerOffSet;
            instances[instance].blast.velocity = instances[instance].Events[numberofthetime].auxDoubleCoords;
            var image = CreateVertice.NewTexture(Game1._graphics.GraphicsDevice, new Point(0, 0), DoubleCoordinates.convertToPoint(instances[instance].blast.velocity));
            if(instances[currantinstance].timeReversed == timeUndo)
                instances[instance].blast.position -= image.Item2;
            //else
            //    instances[instance].blast.position += image.Item2;
            instances[instance].blast.image = image.Item1;
            instances[instance].blast.EventCreated = numberofthetime;



        }
        public static void newDestroyProjectile(DoubleCoordinates position, DoubleCoordinates velocity, int instance, int EventCreated)
        {
            ref EventData[] data = ref Time.instances[instance].Events;
            ref int numberOfEvents = ref Time.instances[instance].numberOfEvents;

            EventData newEvent = new EventData();
            newEvent.Time = time;
            newEvent.Reference = SpawnReference;
            newEvent.type = DESTROYPROJECTILE;
            newEvent.auxDoubleCoords = velocity;
            newEvent.doubleCoords = position;
            newDepandancie(ref instances[instance].Events[EventCreated].dependancies, new Dependancies(instance, numberOfEvents));
            AddElementToArray(ref data, ref numberOfEvents, newEvent);

        }
        static void DestroyProjectile(int numberofthetime, int instance)
        {
            instances[instance].blast.image = null;

        }
        public static void changeAcceleration(int numberofthetime, int instance, bool iterateForward)
        {
            instances[instance].acceleration = !iterateForward ? instances[instance].Events[numberofthetime].auxDoubleCoords : instances[instance].Events[numberofthetime].doubleCoords;
            //Game1.Chat.NewLine("Acceleration changed", time);
            //System.Diagnostics.Debug.WriteLine("moving");
        }
        public static void changeVelocity(int numberofthetime, int instance, bool iterateForward)
        { 
            instances[instance].velocity = !iterateForward ? instances[instance].Events[numberofthetime].auxDoubleCoords : instances[instance].velocity * instances[instance].Events[numberofthetime].Coords;
            instances[instance].position += !iterateForward ? instances[instance].Events[numberofthetime].doubleCoords * -1  : instances[instance].Events[numberofthetime].doubleCoords;//
        }


        public static void advanceTime()
        {

            //SByte direction = instances[currantinstance].timeReversed ? (SByte)(-1) : (SByte)1;

            SByte direction = (instances[currantinstance].timeReversed != timeUndo) ? (SByte)(-1) : (SByte)1;
            for (int i = 0; i < (instances.Length); i++)
            {



                if ((instances[currantinstance].timeReversed == instances[i].timeReversed) != timeUndo)
                    if (instances[i].exsists)
                    {
                        if (instances[i].blast.image != null)
                        {
                            if(!timeRedo && i == currantinstance)
                            {
                                double BlockHits = Collisions.lineBlock(instances[i].blast.position, instances[i].blast.velocity, Game1.blocksize);
                                var ExtrinzoidHits = checkLineHitExtrinzoid(instances[i].blast.position, instances[i].blast.velocity);
                                var PlayerHits = checkLineHitAllPLayers(instances[i].blast.position, instances[i].blast.velocity);
                                if (BlockHits < 2 || ExtrinzoidHits.Item1 < 2 || PlayerHits.Item1 < 2)
                                {
                                    if (ExtrinzoidHits.Item1 < BlockHits && ExtrinzoidHits.Item1 < PlayerHits.Item1)
                                        newExtrinzoidKill(ExtrinzoidHits.Item2);
                                    else if (PlayerHits.Item1 < BlockHits && PlayerHits.Item1 < ExtrinzoidHits.Item1)
                                        newplayerKill(PlayerHits.Item2, instances[i].blast.EventCreated);
                                     newDestroyProjectile(instances[i].blast.position + instances[i].blast.velocity, instances[i].blast.velocity, i, instances[i].blast.EventCreated);
                                }
                                else
                                    instances[i].blast.position += instances[i].blast.velocity;
                            }
                            else
                                instances[i].blast.position += instances[i].blast.velocity;



                        }
                        instances[i].velocity += instances[i].acceleration;
                        if (i != currantinstance || timeUndo || timeRedo)
                            instances[i].position += instances[i].velocity;
                    }
            }
            if (!instances[currantinstance].timeReversed && !timeUndo)
            {
                for (int i = 0; i < (extrinzoidInstances.Length); i++)
                {
                    if (extrinzoidInstances[i].exsists)
                    {
                        extrinzoidInstances[i].velocity += extrinzoidInstances[i].acceleration;
                        extrinzoidInstances[i].position += extrinzoidInstances[i].velocity;
                        if (extrinzoidInstances[i].blast.image != null && (timeRedo))
                            extrinzoidInstances[i].blast.position += extrinzoidInstances[i].blast.velocity;
                    }

                }
                if (!timeRedo)
                    moveExtrinzoid();
            }


            if (!timeUndo && !timeRedo)
                {
                Point velocityloss;
                double timeofhit;
                bool hit;
                DoubleCoordinates newPos;
                (hit, timeofhit, newPos, velocityloss.X, velocityloss.Y) = Collisions.CollisionWithBlocks((instances[currantinstance].position), (instances[currantinstance].position + instances[currantinstance].velocity), Game1.world, Game1.blocksize, Game1.width, Game1.height); //+Game1.playerOffSet
                newPos -= instances[currantinstance].position + instances[currantinstance].velocity;
                //instances[currantinstance].position += newP
                //instances[currantinstance].position -= Game1.playerOffSet;
                if (hit)
                    newVelocityLoss(velocityloss, newPos);
                instances[currantinstance].position += instances[currantinstance].velocity;
            }


            for (int i = 0; i < (instances.Length); i++) // -1 so it doesnt include currant instance
            {
                bool iterateForward = (instances[currantinstance].timeReversed == instances[i].timeReversed) != timeUndo;
                for (int d = (!iterateForward) ? instances[i].numberOfEvents - 1 : 0; ((iterateForward && d < instances[i].numberOfEvents) || (!iterateForward && d > -1)); d += (!iterateForward) ? (-1) : 1)
                {
                    //System.Diagnostics.Debug.WriteLine("instance " + i.ToString() + " Event " + d.ToString());
                    if (instances[i].Events[d].Time == time && !instances[i].Events[d].isDisabled)
                    {
                        //System.Diagnostics.Debug.WriteLine(instances[i].Events[d].type.ToString());
                        if (instances[i].Events[d].type == BLOCKPLACE * direction)
                            blockPlace(d, i);
                        else if (instances[i].Events[d].type == BLOCKBREAK * direction)
                            blockBreak(d, i);
                        else if (instances[i].Events[d].type == ACCELERATION)
                            changeAcceleration(d, i, iterateForward);
                        else if (instances[i].Events[d].type == VELOCITYLOSS)
                            changeVelocity(d, i, iterateForward);
                        else if (instances[i].Events[d].type == SPAWN * direction)
                            spawn(d, i);
                        else if (instances[i].Events[d].type == TRAVEL * direction)
                            Travel(d, i);
                        else if (instances[i].Events[d].type == EXTRINZOIDSPAWN * direction)
                            extrinzoidSpawn(d, i);
                        else if (instances[i].Events[d].type == EXTRINZOIDKILL * direction)
                            extrinzoidKill(d, i);
                        else if (instances[i].Events[d].type == CREATEPROJECTILE * direction)
                            CreateProjectile(d, i);
                        else if (instances[i].Events[d].type == DESTROYPROJECTILE * direction)
                            DestroyProjectile(d, i);
                        else if (instances[i].Events[d].type == PLAYERKILL * direction)
                            playerKill(d, i);
                        else if (instances[i].Events[d].type == PLAYERREVIVIE * direction)
                            playerRevive(d, i);
                        else if (instances[i].Events[d].type == CORRUPTIONSPREAD * direction)
                            CorruptionSpread(d, i);
                        else if (instances[i].Events[d].type == CORRUPTIONCLEANSE * direction)
                            CorruptionCleanse(d, i);



                    }
                    else if (iterateForward && instances[i].Events[d].Time > time)
                    {
                        //System.Diagnostics.Debug.WriteLine("Broke " + instances[i].Events[d].Time.ToString() + " > " + time.ToString());
                        break;
                    }

                    else if (!iterateForward != timeUndo && instances[i].Events[d].Time < time)
                    {
                        //System.Diagnostics.Debug.WriteLine("Broke " + instances[i].Events[d].type.ToString() + " Time " + instances[i].Events[d].Time.ToString() + " < " + time.ToString());
                        break;
                    }


                }
        }

            for (int i = 0; i < (extrinzoidInstances.Length); i++) // -1 so it doesnt include currant instance
            {
                bool iterateForward = instances[currantinstance].timeReversed == timeUndo;
                if (extrinzoidInstances[i].exsists)
                    for (int d = (!iterateForward) ? extrinzoidInstances[i].numberOfEvents - 1 : 0; ((iterateForward && d < extrinzoidInstances[i].numberOfEvents) || (!iterateForward && d > -1)); d += (!iterateForward) ? (-1) : 1)
                    {

                        if (extrinzoidInstances[i].Events[d].Time == time)
                        {
                            if (extrinzoidInstances[i].Events[d].type == ACCELERATION)
                                ExtrinzoidChangeAcceleration(d, i, iterateForward);
                            else if (extrinzoidInstances[i].Events[d].type == CREATEPROJECTILE * direction)
                                CreateExtrinzoidProjectile(d, i);
                            else if (extrinzoidInstances[i].Events[d].type == DESTROYPROJECTILE * direction)
                                DestroyExtrinzoidProjectile(d, i);


                        }
                        else if (iterateForward && extrinzoidInstances[i].Events[d].Time > time)
                            break;
                        else if (!iterateForward && extrinzoidInstances[i].Events[d].Time < time)
                            break;

                    }
            }
            if (instances[currantinstance].timeReversed != timeUndo)
                for (int i = 0; i < (extrinzoidInstances.Length); i++)
                {
                    if (extrinzoidInstances[i].exsists)
                    {
                        extrinzoidInstances[i].position -= extrinzoidInstances[i].velocity;
                        extrinzoidInstances[i].velocity -= extrinzoidInstances[i].acceleration;
                        if (extrinzoidInstances[i].blast.image != null)
                            extrinzoidInstances[i].blast.position -= extrinzoidInstances[i].blast.velocity;
                    }
                }

            for (int i = 0; i < (instances.Length); i++)
            {


                if ((instances[currantinstance].timeReversed != instances[i].timeReversed) != timeUndo)
                    if (instances[i].exsists)
                    {

                        if (i != currantinstance || timeUndo || timeRedo)
                            instances[i].position -= instances[i].velocity;
                        instances[i].velocity -= instances[i].acceleration;

                        if (instances[i].blast.image != null)
                            instances[i].blast.position -= instances[i].blast.velocity;
                    }

            }

            //instances[i].velocity *= bruh;

            //SByte direction = instances[currantinstance].timeReversed ? (SByte)(-1) : (SByte)1;
            if (instances[currantinstance].timeReversed != timeUndo && time == 0)
            {
                Game1.Paused = true;
                //time.
            }
            else
                time += direction;

            if (time == timeOfUndo)
            {
                timeOfUndo = -1;
                timeRedo = false;
            }



        }

        static (double, int) checkLineHitExtrinzoid(DoubleCoordinates position, DoubleCoordinates velocity) 
        {
            DoubleCoordinates reletivepos = new DoubleCoordinates(velocity.X >= 0 ? velocity.X : 0, velocity.Y >= 0 ? velocity.Y : 0);
            double fastestTime = 2;
            int instanceHit = -1;
        for (int i = 0; i < extrinzoidInstances.Length; i++)
            {
                if (extrinzoidInstances[i].exsists)
                {
                    double newTime = Collisions.checkLineCollisions(position + reletivepos, velocity, Game1.blocksize, extrinzoidInstances[i].position);
                    if (newTime < fastestTime) 
                    {
                        instanceHit = i;
                        fastestTime = newTime;                    
                    }

                }
            }
            return (fastestTime, instanceHit);
        }
        static (double, int) checkLineHitAllPLayers(DoubleCoordinates position, DoubleCoordinates velocity)
        {
            DoubleCoordinates reletivepos = new DoubleCoordinates(velocity.X >= 0 ? velocity.X : 0, velocity.Y >= 0 ? velocity.Y : 0);
            double fastestTime = 2;
            int instanceHit = -1;
            for (int i = 0; i < instances.Length; i++)
            {
                if (instances[i].exsists)
                {
                    double newTime = Collisions.checkLineCollisions(position + reletivepos, velocity, Game1.blocksize, instances[i].position);
                    if (newTime < fastestTime)
                    {
                        instanceHit = i;
                        fastestTime = newTime;
                    }

                }
            }
            return (fastestTime, instanceHit);
        }
        static double checkLineHitPlayer(DoubleCoordinates position, DoubleCoordinates velocity)
        {
            DoubleCoordinates reletivepos = new DoubleCoordinates(velocity.X >= 0 ? velocity.X : 0, velocity.Y >= 0 ? velocity.Y : 0);
                 
            return Collisions.checkLineCollisions(position + reletivepos, velocity, Game1.blocksize, instances[currantinstance].position);
        }

        static void moveExtrinzoid()
        {
            WholeCoords upperRadius = new WholeCoords(1920, 1080);//1920,1080
            WholeCoords lowerRadius = new WholeCoords(1280, 720);//1280,720

            for (int i = 0; i < (extrinzoidInstances.Length); i++)
            {


                if (extrinzoidInstances[i].exsists)
                {

                    int numberOfAllies = 0;
                    for (int o = 0; o < (extrinzoidInstances.Length); o++)
                    {
                        if (o != i && extrinzoidInstances[o].exsists && Collisions.PointRect(extrinzoidInstances[o].position.X, extrinzoidInstances[o].position.Y, extrinzoidInstances[i].position.X - upperRadius.X / 2, extrinzoidInstances[i].position.Y - upperRadius.Y / 2, upperRadius.X, upperRadius.Y))
                        {
                            numberOfAllies += 1;
                            if (extrinzoidInstances[o].mode == 2)
                                extrinzoidInstances[i].mode = 2;
                        }
                    }
                    if (numberOfAllies >= 5)
                    {
                        if (extrinzoidInstances[i].mode != 2)
                            //Game1.Paused = true;
                            extrinzoidInstances[i].mode = 2;
                    }
                    //int currantinstance = Time.currantinstance;
                    //double olddistance = Math.Sqrt(upperRadius.X * upperRadius.X + upperRadius.Y * upperRadius.Y)/2;
                    //for (int p = 0; p < instances.Length; p++)
                    //{
                    //    if (instances[p].exsists)
                    //    {
                    //        DoubleCoordinates TotaldistanceFromPlayer = instances[p].position - extrinzoidInstances[i].position;
                    //        double thisdistance = Math.Sqrt(TotaldistanceFromPlayer.X * TotaldistanceFromPlayer.X + TotaldistanceFromPlayer.Y * TotaldistanceFromPlayer.Y);
                    //        if (thisdistance < olddistance)
                    //        {
                    //            currantinstance = p;
                    //            olddistance = thisdistance;
                    //        }

                    //    }

                    //}
                    float thrusterStrength = 1;
                    DoubleCoordinates trueposition = extrinzoidInstances[i].position + (extrinzoidInstances[i].velocity * new DoubleCoordinates(Math.Abs(extrinzoidInstances[i].velocity.X), Math.Abs(extrinzoidInstances[i].velocity.Y))) / (thrusterStrength / (extrinzoidInstances[i].inventory.Items.Length + 1) * 2);
                    DoubleCoordinates distanceFromPlayer = instances[currantinstance].position - trueposition;
                    //DoubleCoordinates Acceleration = new DoubleCoordinates(0, 0);
                    DoubleCoordinates Acceleration = extrinzoidInstances[i].acceleration;
                        if (Collisions.PointRect(Math.Round(extrinzoidInstances[i].position.X / Game1.blocksize), Math.Round(extrinzoidInstances[i].position.Y / Game1.blocksize), 0, 0, Game1.width - 1, Game1.height - 1))
                        {
                            if (Collisions.PointRect(distanceFromPlayer.X, distanceFromPlayer.Y, -upperRadius.X / 2, -upperRadius.Y / 2, upperRadius.X, upperRadius.Y))
                            {
                                if (extrinzoidInstances[i].mode == 2)
                                {
                                    if (Math.Sqrt(distanceFromPlayer.X * distanceFromPlayer.X + distanceFromPlayer.Y * distanceFromPlayer.Y) > 35 * Game1.blocksize)
                                    {
                                        if (Math.Abs(distanceFromPlayer.X) > Math.Abs(distanceFromPlayer.Y))
                                            Acceleration.X = distanceFromPlayer.X > 0 ? thrusterStrength / (extrinzoidInstances[i].inventory.Items.Length + 1) : -thrusterStrength / (extrinzoidInstances[i].inventory.Items.Length + 1);
                                        else
                                            Acceleration.Y = distanceFromPlayer.Y > 0 ? thrusterStrength / (extrinzoidInstances[i].inventory.Items.Length + 1) : -thrusterStrength / (extrinzoidInstances[i].inventory.Items.Length + 1);
                                    }
                                    else if (Math.Sqrt(distanceFromPlayer.X * distanceFromPlayer.X + distanceFromPlayer.Y * distanceFromPlayer.Y) < 5 * Game1.blocksize)
                                    {
                                        if (Math.Abs(distanceFromPlayer.X) < Math.Abs(distanceFromPlayer.Y))
                                            Acceleration.X = distanceFromPlayer.X < 0 ? thrusterStrength / (extrinzoidInstances[i].inventory.Items.Length + 1) : -thrusterStrength / (extrinzoidInstances[i].inventory.Items.Length + 1);
                                        else
                                            Acceleration.Y = distanceFromPlayer.Y < 0 ? thrusterStrength / (extrinzoidInstances[i].inventory.Items.Length + 1) : -thrusterStrength / (extrinzoidInstances[i].inventory.Items.Length + 1);
                                    }
                                    if(extrinzoidInstances[i].blast.image == null && Game1.world[(int)Math.Round(extrinzoidInstances[i].position.X / Game1.blocksize), (int)Math.Round(extrinzoidInstances[i].position.Y / Game1.blocksize)] != 1)
                                    {
                                        int proVelocity = 20;
                                        distanceFromPlayer = instances[currantinstance].position - extrinzoidInstances[i].position;
                                        float G = instances[currantinstance].velocity.X * distanceFromPlayer.Y - instances[currantinstance].velocity.Y * distanceFromPlayer.X;
                                        float A = (distanceFromPlayer.X * distanceFromPlayer.X + distanceFromPlayer.Y * distanceFromPlayer.Y);
                                        //DoubleCoordinates velocity = new DoubleCoordinates((distanceFromPlayer.Y * G + distanceFromPlayer.X * Math.Sqrt(G * G + proVelocity * proVelocity * A)) / A, (-distanceFromPlayer.X * G + distanceFromPlayer.Y * Math.Sqrt(G*G + proVelocity * proVelocity *A))/A);
                                        //velocity.X = Math.Sqrt(proVelocity * proVelocity - velocity.Y * velocity.Y);
                                        DoubleCoordinates velocity = (distanceFromPlayer * Math.Sqrt(- G * G + proVelocity * proVelocity * A) + new DoubleCoordinates(distanceFromPlayer.Y * G, -distanceFromPlayer.X * G))/A;
                                        //if(Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y) > 20)
                                            //Game1.Chat.NewLine(Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y).ToString(), time);
                                        newCreateExtrinzoidProjectile(extrinzoidInstances[i].position, velocity, i);//distanceFromPlayer * proVelocity/Math.Sqrt(distanceFromPlayer.X * distanceFromPlayer.X + distanceFromPlayer.Y * distanceFromPlayer.Y), i);
                                    }


                                }
                                else if (Collisions.PointRect(distanceFromPlayer.X, distanceFromPlayer.Y, -lowerRadius.X / 2, -lowerRadius.Y / 2, lowerRadius.X, lowerRadius.Y) && Game1.world[(int)Math.Round(extrinzoidInstances[i].position.X / Game1.blocksize), (int)Math.Round(extrinzoidInstances[i].position.Y / Game1.blocksize)] != 1)
                                {
                                    extrinzoidInstances[i].mode = 1;

                                }
                                else if (Game1.world[(int)Math.Round(extrinzoidInstances[i].position.X / Game1.blocksize), (int)Math.Round(extrinzoidInstances[i].position.Y / Game1.blocksize)] == 1)
                                {
                                    extrinzoidInstances[i].mode = 0;
                                    if (extrinzoidInstances[i].velocity.X != 0)
                                        Acceleration.X = extrinzoidInstances[i].velocity.X < 0 ? thrusterStrength / (extrinzoidInstances[i].inventory.Items.Length + 1) : -thrusterStrength / (extrinzoidInstances[i].inventory.Items.Length + 1);
                                    else if (extrinzoidInstances[i].position.X % Game1.blocksize != 0)
                                        Acceleration.X = ((int)Math.Round(extrinzoidInstances[i].position.X / Game1.blocksize)) * Game1.blocksize - extrinzoidInstances[i].position.X > 0 ? thrusterStrength / (extrinzoidInstances[i].inventory.Items.Length + 1) : -thrusterStrength / (extrinzoidInstances[i].inventory.Items.Length + 1);
                                    else
                                        Acceleration.X = 0;
                                    if (extrinzoidInstances[i].velocity.Y != 0)
                                        Acceleration.Y = extrinzoidInstances[i].velocity.Y < 0 ? thrusterStrength / (extrinzoidInstances[i].inventory.Items.Length + 1) : -thrusterStrength / (extrinzoidInstances[i].inventory.Items.Length + 1);
                                    else if (extrinzoidInstances[i].position.Y % Game1.blocksize != 0)
                                        Acceleration.Y = ((int)Math.Round(extrinzoidInstances[i].position.Y / Game1.blocksize)) * Game1.blocksize - extrinzoidInstances[i].position.Y > 0 ? thrusterStrength / (extrinzoidInstances[i].inventory.Items.Length + 1) : -thrusterStrength / (extrinzoidInstances[i].inventory.Items.Length + 1);
                                    else
                                        Acceleration.Y = 0;
                                }
                                else
                                    Acceleration = new DoubleCoordinates(0, 0);

                            }
                            else
                            {
                                extrinzoidInstances[i].mode = 0;
                                if (Math.Abs(distanceFromPlayer.X) > upperRadius.X / 2)
                                    Acceleration.X = distanceFromPlayer.X > 0 ? thrusterStrength / (extrinzoidInstances[i].inventory.Items.Length + 1) : -thrusterStrength / (extrinzoidInstances[i].inventory.Items.Length + 1);
                                else if (Math.Abs(distanceFromPlayer.Y) > upperRadius.Y / 2)
                                    Acceleration.Y = distanceFromPlayer.Y > 0 ? thrusterStrength / (extrinzoidInstances[i].inventory.Items.Length + 1) : -thrusterStrength / (extrinzoidInstances[i].inventory.Items.Length + 1);
                            }


                            if (extrinzoidInstances[i].mode == 1)
                            {
                                bool foundblock = false;
                                //int maxright = 1920 / 2 - (int)distanceFromPlayer.X < Game1.width - Math.Round(trueposition.X / Game1.blocksize) ? 1920 / 2 - (int)distanceFromPlayer.X : Game1.width - (int)Math.Round(trueposition.X / Game1.blocksize);
                                //int maxleft = 1920 / 2 + (int)distanceFromPlayer.X < Math.Round(trueposition.X / Game1.blocksize) ? 1920 / 2 + (int)distanceFromPlayer.X : (int)Math.Round(trueposition.X / Game1.blocksize);
                                //int maxup = 1080 / 2 - (int)distanceFromPlayer.Y < Game1.height - Math.Round(trueposition.Y / Game1.blocksize) ? 1080 / 2 - (int)distanceFromPlayer.Y : Game1.height - (int)Math.Round(trueposition.Y / Game1.blocksize);
                                //int maxdown = 1080 / 2 + (int)distanceFromPlayer.Y < Math.Round(trueposition.Y / Game1.blocksize) ? 1080 / 2 + (int)distanceFromPlayer.Y : (int)Math.Round(trueposition.Y / Game1.blocksize);
                                for (int b = 0; b < 25; b++)
                                {
                                    //if (b < maxright)
                                    //FadeDraw.newItem(Game1.Player, new Vector2((int)Math.Round(trueposition.X + Game1.blocksize * b), (int)Math.Round(trueposition.Y)) + Game1.playerOffSet, 2);
                                    //if (b < maxleft)
                                    //    FadeDraw.newItem(Game1.Player, new Vector2((int)Math.Round(trueposition.X - Game1.blocksize * b), (int)Math.Round(trueposition.Y)) + Game1.playerOffSet, 2);
                                    //if (b < maxup)
                                    //FadeDraw.newItem(Game1.Player, new Vector2((int)Math.Round(trueposition.X), (int)Math.Round(trueposition.Y + Game1.blocksize * b)) + Game1.playerOffSet, 5);
                                    //if (b < maxdown)
                                    //    FadeDraw.newItem(Game1.Player, new Vector2((int)Math.Round(trueposition.X), (int)Math.Round(trueposition.Y - Game1.blocksize * b)) + Game1.playerOffSet, 5);
                                    if (Game1.world[(int)Math.Round(trueposition.X / Game1.blocksize + b), (int)Math.Round(trueposition.Y / Game1.blocksize)] == 1)
                                    {
                                        Acceleration.X = thrusterStrength / (extrinzoidInstances[i].inventory.Items.Length + 1);
                                        
                                        foundblock = true;

                                        break;
                                    }

                                    else if (Game1.world[(int)Math.Round(trueposition.X / Game1.blocksize - b), (int)Math.Round(trueposition.Y / Game1.blocksize)] == 1)
                                    {
                                        Acceleration.X = -thrusterStrength / (extrinzoidInstances[i].inventory.Items.Length + 1);
                                        foundblock = true;
                                        Game1.Chat.NewLine("move", time);
                                        break;
                                    }

                                    else if (Game1.world[(int)Math.Round(trueposition.X / Game1.blocksize), (int)Math.Round(trueposition.Y / Game1.blocksize + b)] == 1)
                                    {
                                        Acceleration.Y = thrusterStrength / (extrinzoidInstances[i].inventory.Items.Length + 1);
                                        Game1.Chat.NewLine("move", time);
                                        foundblock = true;
                                        break;
                                    }

                                    else if (Game1.world[(int)Math.Round(trueposition.X / Game1.blocksize), (int)Math.Round(trueposition.Y / Game1.blocksize - b)] == 1)
                                    {
                                        Acceleration.Y = -thrusterStrength / (extrinzoidInstances[i].inventory.Items.Length + 1);
                                        Game1.Chat.NewLine("move", time);
                                        foundblock = true;
                                        break;
                                    }
                                }
                                if (!foundblock)
                                {
                                    extrinzoidInstances[i].mode = 0;
                                    if (lowerRadius.X / 2 - Math.Abs(distanceFromPlayer.X) < lowerRadius.Y / 2 - Math.Abs(distanceFromPlayer.Y))
                                        Acceleration.X = distanceFromPlayer.X < 0 ? thrusterStrength / (extrinzoidInstances[i].inventory.Items.Length + 1) : -thrusterStrength / (extrinzoidInstances[i].inventory.Items.Length + 1);
                                    else
                                        Acceleration.Y = distanceFromPlayer.Y < 0 ? thrusterStrength / (extrinzoidInstances[i].inventory.Items.Length + 1) : -thrusterStrength / (extrinzoidInstances[i].inventory.Items.Length + 1);
                                }
                            }


                        }
                        else if (extrinzoidInstances[i].position.X < 0 || extrinzoidInstances[i].position.X > Game1.width * Game1.blocksize)
                            Acceleration.X = extrinzoidInstances[i].position.X < Game1.width * Game1.blocksize / 2 ? thrusterStrength / (extrinzoidInstances[i].inventory.Items.Length + 1) : -thrusterStrength / (extrinzoidInstances[i].inventory.Items.Length + 1);
                        else if (extrinzoidInstances[i].position.Y < 0 || extrinzoidInstances[i].position.Y > Game1.height * Game1.blocksize)
                            Acceleration.Y = extrinzoidInstances[i].position.Y < Game1.height * Game1.blocksize / 2 ? thrusterStrength / (extrinzoidInstances[i].inventory.Items.Length + 1) : -thrusterStrength / (extrinzoidInstances[i].inventory.Items.Length + 1);


                        //if(Math.Sqrt(distanceFromPlayer.X * distanceFromPlayer.X + distanceFromPlayer.Y * distanceFromPlayer.Y) > 500 || Math.Sqrt(distanceFromPlayer.X * distanceFromPlayer.X + distanceFromPlayer.Y * distanceFromPlayer.Y) < 300)
                        //{
                        //    if (distanceFromPlayer.X > 100 && distanceFromPlayer.X < 500 || distanceFromPlayer.X < -100 && distanceFromPlayer.X > -500)
                        //        Acceleration.X = 0;
                        //    else
                        //        Acceleration.X += distanceFromPlayer.X > 300 || (distanceFromPlayer.X < 0 && distanceFromPlayer.X > -300) ? thrusterStrength / (extrinzoidInstances[i].inventory.Items.Length + 1) : -thrusterStrength / (extrinzoidInstances[i].inventory.Items.Length + 1);
                        //    if (distanceFromPlayer.Y > 100 && distanceFromPlayer.Y < 500 || distanceFromPlayer.Y < -100 && distanceFromPlayer.Y > -500)
                        //        Acceleration.Y = 0;
                        //    else
                        //        Acceleration.Y += distanceFromPlayer.Y > 300 || (distanceFromPlayer.Y < 0 && distanceFromPlayer.Y > -300) ? thrusterStrength / (extrinzoidInstances[i].inventory.Items.Length + 1) : -thrusterStrength / (extrinzoidInstances[i].inventory.Items.Length + 1);

                        //}



                        if (Acceleration != extrinzoidInstances[i].acceleration)
                            Time.newExtrinzoidAcceleration(Acceleration, i);

                    if (extrinzoidInstances[i].blast.image != null)
                    {
                        if (Collisions.lineBlock(extrinzoidInstances[i].blast.position, extrinzoidInstances[i].blast.velocity, Game1.blocksize) < 2 || checkLineHitPlayer(extrinzoidInstances[i].blast.position, extrinzoidInstances[i].blast.velocity) < 2)
                            newDestroyExtrinzoidProjectile(extrinzoidInstances[i].blast.position + extrinzoidInstances[i].blast.velocity, extrinzoidInstances[i].blast.velocity, i);
                        else
                            extrinzoidInstances[i].blast.position += extrinzoidInstances[i].blast.velocity;
                    }


                }
            }

        }
        public static void newExtrinzoidAcceleration(DoubleCoordinates acceleration, int instance)
        {
            ref EventData[] data = ref Time.extrinzoidInstances[instance].Events;
            ref int numberOfEvents = ref Time.extrinzoidInstances[instance].numberOfEvents;

            EventData newEvent = new EventData();
            newEvent.Time = time;
            newEvent.Reference = SpawnReference;
            newEvent.type = ACCELERATION;
            newEvent.auxDoubleCoords = extrinzoidInstances[instance].acceleration;
            newEvent.doubleCoords = acceleration;
            newDepandancie(ref instances[currantinstance].Events[0].dependancies, new Dependancies(instance, numberOfEvents, true));
            AddElementToArray(ref data, ref numberOfEvents, newEvent);
        }
        static void newCreateExtrinzoidProjectile(DoubleCoordinates position, DoubleCoordinates velocity, int instance)
        {
            ref EventData[] data = ref Time.extrinzoidInstances[instance].Events;
            ref int numberOfEvents = ref Time.extrinzoidInstances[instance].numberOfEvents;

            EventData newEvent = new EventData();
            newEvent.Time = time;
            newEvent.Reference = SpawnReference;
            newEvent.type = CREATEPROJECTILE;
            newEvent.auxDoubleCoords = velocity;
            newEvent.doubleCoords = position;
            newDepandancie(ref instances[currantinstance].Events[0].dependancies, new Dependancies(instance, numberOfEvents, true));
            AddElementToArray(ref data, ref numberOfEvents, newEvent);
        }
        static void newDestroyExtrinzoidProjectile(DoubleCoordinates position, DoubleCoordinates velocity, int instance)
        {
            ref EventData[] data = ref Time.extrinzoidInstances[instance].Events;
            ref int numberOfEvents = ref Time.extrinzoidInstances[instance].numberOfEvents;

            EventData newEvent = new EventData();
            newEvent.Time = time;
            newEvent.Reference = SpawnReference;
            newEvent.type = DESTROYPROJECTILE;
            newEvent.auxDoubleCoords = velocity;
            newEvent.doubleCoords = position;
            newDepandancie(ref instances[currantinstance].Events[0].dependancies, new Dependancies(instance, numberOfEvents, true));
            AddElementToArray(ref data, ref numberOfEvents, newEvent);
        }
        static void ExtrinzoidChangeAcceleration(int numberofthetime, int instance, bool iterateForward)
        {
            extrinzoidInstances[instance].acceleration = !iterateForward ? extrinzoidInstances[instance].Events[numberofthetime].auxDoubleCoords : extrinzoidInstances[instance].Events[numberofthetime].doubleCoords;
            //Game1.Chat.NewLine("Acceleration changed", time);
        }
        static void CreateExtrinzoidProjectile(int numberofthetime, int instance)
        {
            extrinzoidInstances[instance].blast.position = instances[currantinstance].timeReversed != timeUndo ? extrinzoidInstances[instance].Events[numberofthetime].doubleCoords : extrinzoidInstances[instance].Events[numberofthetime].doubleCoords - Game1.playerOffSet;
            extrinzoidInstances[instance].blast.velocity = extrinzoidInstances[instance].Events[numberofthetime].auxDoubleCoords;
            var image = CreateVertice.NewTexture(Game1._graphics.GraphicsDevice, new Point(0,0), DoubleCoordinates.convertToPoint(extrinzoidInstances[instance].blast.velocity));
            if (instances[currantinstance].timeReversed == timeUndo)
                extrinzoidInstances[instance].blast.position -= image.Item2;
            extrinzoidInstances[instance].blast.image = image.Item1;

        }
        static void DestroyExtrinzoidProjectile(int numberofthetime, int instance)
        {
            extrinzoidInstances[instance].blast.image = null;

        }



        static void AddElementToArray<T>(ref T[] array, ref int amount, T var)
        {
            //if (array == null)
            //{
            //    Array.Resize(ref array, amount +1);
            //    array[amount] = var;
            //    amount++;
            //}
            //else
            if (amount == 0)
            {
                Array.Resize(ref array, 1);
                array[amount] = var;
                amount++;
            }
            if (array.Length == amount)
            {
                Array.Resize(ref array, amount * 2);
                System.Diagnostics.Debug.WriteLine("Resizing " + (amount * 2).ToString() + " Time: " + time.ToString());
                array[amount] = var;
                amount++;
            }
            else
            {
                array[amount] = var;
                amount++;
            }

        }

        static void newDepandancie(ref Dependancies[] dependancies, Dependancies newDepandant)
        {
            if (dependancies == null)
                dependancies = new Dependancies[0];
            Array.Resize(ref dependancies, dependancies.Length + 1);
            dependancies[dependancies.Length -1] = newDepandant; 
        }
    }
    public struct InstanceData
    {
        public EventData[] Events;
        public int numberOfEvents;
        public Inventory inventory;
        public Inventory finalInventory;
        public DoubleCoordinates acceleration;
        public DoubleCoordinates velocity;
        public DoubleCoordinates position;
        public Projectile blast;
        public bool exsists;
        public bool timeReversed;
    }
    public struct EventData
    {
        public int Time;
        public Dependancies Reference;
        //public int Instance;
        public SByte type;
        public int BlockID;
        public short wholeNum;
        //public WholeCoords Origine;
        public WholeCoords Coords;
        public DoubleCoordinates doubleCoords;
        public DoubleCoordinates auxDoubleCoords;
        public Dependancies[] dependancies;
        public bool isDisabled;

    }
    public struct blockdatastrc
    {
        public int ID;
        
    }
    public struct blockID
    {
        public WholeCoords CurrentCoords;
        public WholeCoords Originalcoords;
        public Dependancies reference;
        public Dependancies[] dependancies;
    }
    public struct extrinzoid
    {
        public EventData[] Events;
        public int numberOfEvents;
        public DoubleCoordinates acceleration;
        public DoubleCoordinates velocity;
        public DoubleCoordinates position;
        public Inventory inventory;
        //public int ProjecileReference;
        public Projectile blast;
        public bool exsists;
        public byte mode;
        public Dependancies reference;
    }
    public struct Projectile
    {
        public DoubleCoordinates velocity;
        public DoubleCoordinates position;
        public Texture2D image;
        public int EventCreated;
        //public int Origine;
    }
    public struct Dependancies
    {
        public int instance;
        public int eventnum;
        public bool isExtrinzoid;
        public Dependancies(int instance, int eventnum, bool isExtrinzoid = false)
        {
            this.instance = instance;
            this.eventnum = eventnum;
            this.isExtrinzoid = isExtrinzoid;
        }
    }



}
