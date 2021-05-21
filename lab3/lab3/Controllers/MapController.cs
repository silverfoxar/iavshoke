﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab3.Controllers
{
    public static class MapController
    {
        public const int mapSize = 8;
        public const int cellSize = 50;

        private static int currentPictureToSet = 0;

        public static int[,] map = new int[mapSize, mapSize];
        public static Button[,] Buttons = new Button[mapSize, mapSize];

        public static Image spriteSet;
        private static bool isFirstStep;

        private static Point firstCoord;

        public static Form form;

        private static void ConfigureMapSize(Form current)
        {
            current.Width = mapSize * cellSize + 20;
            current.Height = (mapSize + 1) * cellSize;
        }
        private static void InitMap()
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    map[i, j] = 0;
                }
            }
        }
        private static void InitButtons(Form current)
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    Button button = new Button();
                    button.Location = new Point(j * cellSize, i * cellSize);
                    button.Size = new Size(cellSize, cellSize);
                    button.Image = FindNeededImage(0, 0);
                    
                    button.MouseUp += new MouseEventHandler(OnButtonPressedMouse);
                    current.Controls.Add(button);
                    Buttons[i, j] = button;
                }
            }
        }

        private static void OnButtonPressedMouse(object sender, MouseEventArgs e)
        {
            Button pressedButton = sender as Button;
            switch (e.Button.ToString())
            {
                case "Right":
                    OnRightButtonPressed(pressedButton);
                    break;
                case "Left":
                    OnLeftButtonPressed(pressedButton);
                    break;
            }
        }

        private static void OnRightButtonPressed(Button pressedButton)
        {
            currentPictureToSet++;
            currentPictureToSet %= 3;
            int PosX = 0;
            int PosY = 0;
            switch (currentPictureToSet)
            {
                case 0:
                    PosX = 0;
                    PosY = 0;
                    break;
                case 1:
                    PosX = 0;
                    PosY = 2;
                    break;
                case 2:
                    PosX = 2;
                    PosY = 2;
                    break;
            }
            pressedButton.Image = FindNeededImage(PosX, PosY);
        }

        private static void OnLeftButtonPressed(Button pressedButton)
        {
            
            pressedButton.Enabled = false;
            int iButton = pressedButton.Location.Y / cellSize; 
            int jButton = pressedButton.Location.X / cellSize;
            if (isFirstStep)
            {
                firstCoord = new Point(jButton,iButton);
                SeedMap();
                CountCellBomb();
                isFirstStep = false;
            }
            OpenCells(iButton,jButton);

            if(map[iButton,jButton]==-1)
            {
                ShowAllBombs(iButton,jButton);
                MessageBox.Show("Game over :/", "The End");
                form.Controls.Clear();
                Init(form);
            }
        }

        public static void Init(Form current)
        {
            form = current;
            currentPictureToSet = 0;
            isFirstStep = true;
            spriteSet = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName.ToString(), "Sprites\\tiles.png"));
            ConfigureMapSize(current);
            InitMap();
            InitButtons(current);
        }
        public static Image FindNeededImage(int xPos, int yPos)
        {
            Image image = new Bitmap(cellSize, cellSize);
            Graphics g = Graphics.FromImage(image);
            g.DrawImage(spriteSet, new Rectangle(new Point(0, 0), new Size (cellSize,cellSize)),0+32*xPos,032*yPos,33,33,GraphicsUnit.Pixel);
            return image;
        }
        private static void SeedMap()
        {
            Random r = new Random();
            int number = r.Next(5, 10);
            for(int i = 0; i<number;i++)
            {
                int posI = r.Next(0, mapSize - 1);
                int posJ = r.Next(0, mapSize - 1);

                while (map[posI,posJ]==-1 || (Math.Abs(posI-firstCoord.Y)<=1 && Math.Abs(posJ-firstCoord.X)<=1))
                {
                    posI = r.Next(0, mapSize - 1);
                    posJ = r.Next(0, mapSize - 1);
                }
                map[posI, posJ] = -1;
            }
        }
        private static void CountCellBomb()
        {
            for(int i=0;i<mapSize;i++)
            {
                for(int j=0;j<mapSize;j++)
                {
                    if (map[i, j] == -1)
                    {
                        for (int k=i-1;k<i+2;k++)
                        {
                            for (int l=j-1;l<j+2;l++)
                            {
                                if (!IsInBorder(k, l) || map[k,l]==-1)                               
                                    continue;
                                map[k, l] = map[k, l] + 1;
                            }
                        }
                    }
                }
            }
        }
        private static bool IsInBorder (int i,int j)
        {
            if(i<0 || j<0|| j>mapSize-1|| i> mapSize-1)
            {
                return false;
            }
            return true;
        }
        private static void OpenCell(int i, int j)
        {
            Buttons[i, j].Enabled = false;
            switch (map[i, j])
            {
                case 1:
                    Buttons[i, j].Image = FindNeededImage(1, 0);
                    break;
                case 2:
                    Buttons[i, j].Image = FindNeededImage(2, 0);
                    break;
                case 3:
                    Buttons[i, j].Image = FindNeededImage(3, 0);
                    break;
                case 4:
                    Buttons[i, j].Image = FindNeededImage(4, 0);
                    break;
                case 5:
                    Buttons[i, j].Image = FindNeededImage(0, 1);
                    break;
                case 6:
                    Buttons[i, j].Image = FindNeededImage(1, 1);
                    break;
                case 7:
                    Buttons[i, j].Image = FindNeededImage(2, 1);
                    break;
                case 8:
                    Buttons[i, j].Image = FindNeededImage(3, 1);
                    break;
                case -1:
                    Buttons[i, j].Image = FindNeededImage(1, 2);
                    break;
                case 0:
                    Buttons[i, j].Image = FindNeededImage(0, 0);
                    break;
            }
        }
        private static void OpenCells(int i,int j)
        {
            OpenCell(i, j);
            if (map[i, j] > 0)
                return;
            for(int k = i+1; k<i+2; k++)
            {
                for (int l=j-1; l<j+2; l++)
                {
                    if (!IsInBorder(k, l))
                        continue;
                    if (!Buttons[k, l].Enabled)
                        continue;
                    if (map[k, l] == 0)
                        OpenCells(k, l);
                    else if (map[k, l] > 0)
                        OpenCell(k, l);
                }
            }
        }
        private static void ShowAllBombs (int iBomb, int jBomb)
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (i == iBomb && j == jBomb)
                        continue;
                    if(map[i, j] == -1)
                    {
                        Buttons[i, j].Image = FindNeededImage(3, 2);

                    }
                }
            }
        }
    }
}