using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CellOrganism
{
    
    public struct Inventory
    {
        public int NumberOfInvItems;
        public Itemstrc[] Items;
        public static ItemProperties[] ItemsProperties;
        public static int weight;


    }
    public struct Slot
    {
        public Rectangle Rectangle;
        public int InvReference;
        
    }
    public struct StorageSlot
    {
        public Rectangle Rectangle;
        public int InvReference;
        public bool IsReferenced;

    }
    public struct Itemstrc
    {
        public int number;
        public acquireryData acquireryData;
    }
    public struct acquireryData
    {
        //public int Instance;
        public Dependancies reference;
        public int ID;
    }
    public struct ItemProperties
    {
        public Texture2D image;
        public bool autoswing;
    }
    public struct SettingSlot//so we can enable fast placing and stuffl
    {
        public Rectangle Rectangle;
        public Texture2D Icon;
        public Texture2D IconSelected;
        public string Text;
        public bool Selected;
    }
    public class Hotbar//just the items they have
    {

        
        public Slot[] Slots;
        public bool InventoryOpen = false, Godmode = false;
        public int mouseOnSlot = 0;
        public int HOLDINGSLOT;
        public int Selected;
        public int NumberOfInvSlots = 10;
        public const int ITEMNOTHING = 0;
        public const int ITEMBREAK = 1;
        public const int ITEMRED = 2;
        public const int ITEMGREY = 3;
        public const int ITEMBLACK = 4;
        public const int ITEMORE = 5;
        public const int ITEMCORRUPTION = 6;
        public const int TELEPORTER = 7;
        public const int ITEMPLANETRED = 8;
        public const int ITEMPLANETGREY = 9;
        public const int ITEMPLANETBLACK = 10;
        public const int ITEMPLANETORE = 11;
        public const int ITEMPLANETCORRUPTION = 12;
        public int SelectedUseSlot = 0;
        public Label SelectedSlotText = new Label();
        public bool ClickCoolDown = true;
        public StorageSlot[] StorageSlots;
        public int mouseOnStorageSlot = -1;
        public Rectangle StorageRectangle;
        public int NumberOfStorageSlots = 20;
        public SettingSlot[] SettingSlots;
        public int mouseOnSettingSlot = -1;
        public Rectangle SettingRectangle;
        public const int BLOCKCHAINING = 0;






        public void InitializeInv(GraphicsDevice _GraphicsDevice)
        {
            ref Inventory Inv = ref Time.instances[Time.currantinstance].inventory;
            Inventory.ItemsProperties = new ItemProperties[ITEMPLANETCORRUPTION + 1];
            for (int i = 1; i < Game1.Blocks.Length; i++)
            {
                Inventory.ItemsProperties[i + 1].image = Game1.Blocks[i];
                Inventory.ItemsProperties[i + 1].autoswing = true;
            }
            Inventory.ItemsProperties[1].image = Game1.Letters[Convert.ToInt32(char.Parse("0"))];
            Inventory.ItemsProperties[1].autoswing = true;
            Inventory.ItemsProperties[Game1.Blocks.Length + 1].image = Game1.Letters[Convert.ToInt32(char.Parse("T"))];
            for (int i = 1; i < Game1.Blocks.Length; i++)
            {
                Inventory.ItemsProperties[i + 1 + Game1.Blocks.Length].image = SpriteBatchExtensions.Resize(_GraphicsDevice, Game1.Blocks[i], 2);
                Inventory.ItemsProperties[i + 1 + Game1.Blocks.Length].autoswing = false;
            }


            HOLDINGSLOT = NumberOfInvSlots;
            Slots = new Slot[NumberOfInvSlots + 1]; // +1 for holding item
            Slots[HOLDINGSLOT].InvReference = -1;
            int Row = 0;
            for (int i = 0; i < NumberOfInvSlots; i++)
            {
                if (i - Row * 10 == 10)
                    Row += 1;
                Slots[i].Rectangle = new Rectangle((i - Row *10)* 50 + 5, 5 + 50 * Row, 50, 50);
                Slots[i].InvReference = -1;


            }

            Selected = 0;
            if (Godmode)
            {

                Inv.Items = new Itemstrc[Inventory.ItemsProperties.Length];
                for (int i = 0; i < Inventory.ItemsProperties.Length - 1; i++)
                {
                    Inv.Items[i].number = i + 1;
                    Slots[i].InvReference = i;
                    Inv.NumberOfInvItems += 1;
                }
            }
            else
            {
                Inv.Items = new Itemstrc[2];
                Inv.Items[0].number = ITEMBREAK;
                Inv.Items[0].acquireryData.ID = -1;
                Slots[0].InvReference = 0;
                Inv.Items[1].number = TELEPORTER;
                Inv.Items[1].acquireryData.ID = -1;
                Slots[1].InvReference = 1;
                Inv.NumberOfInvItems = 2;
                Time.starterInventory = Inv;
            }
            StorageSlots = new StorageSlot[NumberOfStorageSlots];
            int Row2 = 0;
            for (int i = 0; i < NumberOfStorageSlots; i++)
            {
                if (i - Row2 * 10 == 10)
                    Row2 += 1;
                StorageSlots[i].Rectangle = new Rectangle((i - Row2 * 10) * 50 + 5, 5 + 50 * (Row2) + 50 * (Row+1), 50, 50);
                StorageSlots[i].InvReference = (i < Inv.NumberOfInvItems && Inv.Items[i].number !=0) ? i : -1;


            }
            StorageRectangle = new Rectangle(5, 5 + 50 *(Row+1), 500, 50*(Row2 + 1));


            SettingSlots = new SettingSlot[1];
            int Row3 = 0;
            for (int i = 0; i < SettingSlots.Length; i++)
            {
                if (i - Row3 * 10 == 10)
                    Row3 += 1;
                SettingSlots[i].Rectangle = new Rectangle((i - Row3 * 10) * 50 + 5, 5 + 50 * Row3 + 50*(Row + Row2 + 1 + 1), 50, 50);


            }
            SettingRectangle = new Rectangle(5, 5 + 50 * (Row + Row2 + 1 + 1), 500, 50 * (Row3 + 1));
            SettingSlots[BLOCKCHAINING].Icon = Game1.Blocks[1];
            SettingSlots[BLOCKCHAINING].IconSelected = Game1.Getborders(Game1.Blocks[1], 2, Color.White, 1);
            SettingSlots[BLOCKCHAINING].Text = "Enable Block Chaining";

        }
        public void Open()
        {

            InventoryOpen = !InventoryOpen;
        }
        public void CheckSelected(Point MousePosition)
        {
            mouseOnSlot = -1;
            mouseOnStorageSlot = -1;
            mouseOnSettingSlot = -1;
                if (InventoryOpen && StorageRectangle.Contains(MousePosition))
                {

                for (int s = 0; s < NumberOfStorageSlots; s++)
                {

                    if (StorageSlots[s].Rectangle.Contains(MousePosition))
                    {
                        mouseOnStorageSlot = s;
                    }
                }
            }
            else if (InventoryOpen && SettingRectangle.Contains(MousePosition))
            {
                for (int s = 0; s < SettingSlots.Length; s++)
                {

                    if (SettingSlots[s].Rectangle.Contains(MousePosition))
                    {
                        mouseOnSettingSlot = s;

                    }
                }

            }

            else
                for (int s = 0; s < NumberOfInvSlots; s++)//not Slots.Length cuz holding slot
                {
                    if (s < 10 || InventoryOpen)
                    {
                        if (Slots[s].Rectangle.Contains(MousePosition))
                        {
                            mouseOnSlot = s;
                        }

                    }
                }


        }

        public void DoStorageSlots()//only need to call when it changes but I call every cycle
        {
            ref Inventory Inv = ref Time.instances[Time.currantinstance].inventory;
            for (int s = 0; s < NumberOfStorageSlots; s++)
            {
                if (s < Inv.NumberOfInvItems)
                    StorageSlots[s].InvReference = s;
                else
                    StorageSlots[s].InvReference = -1;
            }
                
        }

        public void Click(Point MousePosition, bool MouseLeftPressed, WholeCoords SelectedBlock)
        {
            ref Inventory Inv = ref Time.instances[Time.currantinstance].inventory;

            if (mouseOnSlot != -1 && !MouseLeftPressed)
            {

                if (Slots[HOLDINGSLOT].InvReference != -1 && InventoryOpen)
                {
                    int temporary = Slots[HOLDINGSLOT].InvReference;
                    Slots[HOLDINGSLOT].InvReference = Slots[mouseOnSlot].InvReference;
                    Slots[mouseOnSlot].InvReference = temporary;
                    Selected = mouseOnSlot;
                }
                else if (mouseOnSlot == Selected && InventoryOpen)
                {
                    Slots[HOLDINGSLOT].InvReference = Slots[mouseOnSlot].InvReference;
                    Slots[mouseOnSlot].InvReference = -1;
                }
                else
                {
                    Selected = mouseOnSlot;
                }

            }
            else if (mouseOnSettingSlot != -1)
            {
                if (!MouseLeftPressed)
                    SettingSlots[mouseOnSettingSlot].Selected = !SettingSlots[mouseOnSettingSlot].Selected;





            }


            else if (mouseOnStorageSlot != -1)
            {
                if (Slots[HOLDINGSLOT].InvReference != -1)
                    Slots[HOLDINGSLOT].InvReference = -1;
                else if (StorageSlots[mouseOnStorageSlot].InvReference != -1)
                    Slots[HOLDINGSLOT].InvReference = StorageSlots[mouseOnStorageSlot].InvReference;
                ClickCoolDown = true;
            }
            else if ((!MouseLeftPressed || (Slots[Selected].InvReference != -1 && Inventory.ItemsProperties[Inv.Items[Slots[Selected].InvReference].number].autoswing == true)) && mouseOnSlot == -1)
                if (Slots[Selected].InvReference != -1 || Slots[HOLDINGSLOT].InvReference != -1)
                {

                    SelectedUseSlot = Slots[HOLDINGSLOT].InvReference != -1 ? HOLDINGSLOT : Selected;
                    ref int SelectedNum = ref Inv.Items[Slots[SelectedUseSlot].InvReference].number;
                    if (SelectedNum == TELEPORTER)
                    {
                        Time.instances[Time.currantinstance].position.X = MousePosition.X + Time.instances[Time.currantinstance].position.X - Game1.GraphicsRectangle.Width / 2;
                        Time.instances[Time.currantinstance].position.Y = MousePosition.Y + Time.instances[Time.currantinstance].position.Y - Game1.GraphicsRectangle.Height / 2;
                    }
                    if (SelectedNum == 1)//check break
                    {
                        if (SelectedBlock.X < Game1.width && SelectedBlock.Y < Game1.height && SelectedBlock.X > -1 && SelectedBlock.Y > -1)
                        {
                            if (Game1.world[SelectedBlock.X, SelectedBlock.Y] != 0)
                            {
                                if(!Godmode && !Game1.Paused)
                                    Time.newBlockBreak(SelectedBlock);
                                else
                                    Game1.world[SelectedBlock.X, SelectedBlock.Y] = (short)(SelectedNum - 1);
                            }
                        }
                    }
                    else if (SelectedNum > 1 && SelectedNum < TELEPORTER)//check if place block
                    {
                        if (SelectedBlock.X < Game1.width && SelectedBlock.Y < Game1.height && SelectedBlock.X > -1 && SelectedBlock.Y > -1)
                        {
                            if (Game1.world[SelectedBlock.X, SelectedBlock.Y] == 0)
                            {
                                Time.newBlockPlace(SelectedBlock, Slots[SelectedUseSlot].InvReference);
                                if (!Godmode)
                                {
                                    ClickCoolDown = !SettingSlots[BLOCKCHAINING].Selected;
                                }

                            }
                        }
                    }
                    else if (SelectedNum > TELEPORTER && SelectedNum < 9)
                    {

                    }
                }
        }

        
    }
}
