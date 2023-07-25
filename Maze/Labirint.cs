using System;
using System.Windows.Forms;
using System.Drawing;

namespace Maze
{
    class Labirint
    {
        // позиция главного персонажа
        public int CharacterPositionX { get; set; }
        public int CharacterPositionY { get; set; }

        int height; // высота лабиринта (количество строк)
        int width; // ширина лабиринта (количество столбцов в каждой строке)

        public MazeObject[,] objects;

        public PictureBox[,] images;

        public static Random r = new Random();
        public static int countMedal = 0;
        public static int countMedalAll = 0;
        public static int health = 100;
        public Form parent;
       
        public Labirint(Form parent, int width, int height)
        {
            this.width = width;
            this.height = height;
            this.parent = parent;

            objects = new MazeObject[height, width];
            images = new PictureBox[height, width];
            CharacterPositionX = 0;
            CharacterPositionY = 2;
            Generate();
            
        }

        void Generate()
        {
           for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    MazeObject.MazeObjectType current = MazeObject.MazeObjectType.HALL;

                    // в 1 случае из 5 - ставим стену
                    if (r.Next(5) == 0)
                    {
                        current = MazeObject.MazeObjectType.WALL;
                    }

                    // в 1 случае из 100 - кладём аптечку
                    if (r.Next(50) == 0)
                    {
                        current = MazeObject.MazeObjectType.AID;
                    }

                    // в 1 случае из 250 - размещаем врага
                    if (r.Next(50) == 0)
                    {
                        current = MazeObject.MazeObjectType.ENEMY;
                    }
                    
                    // стены по периметру обязательны
                   if (y == 0 || x == 0 || y == height - 1 | x == width - 1)
                    {
                        current = MazeObject.MazeObjectType.WALL;
                    }
                    if (r.Next(75) == 0 && y != 0 && x != 0 && y != height - 1 && x != width - 1)
                    {
                        current = MazeObject.MazeObjectType.MEDAL;
                        countMedalAll++;
                    }
                    // наш персонажик
                    if (x == CharacterPositionX && y == CharacterPositionY)
                    {
                        current = MazeObject.MazeObjectType.CHAR;
                    }

                    // есть выход, и соседняя ячейка справа всегда свободна
                    if (x == CharacterPositionX + 1 && y == CharacterPositionY || x == width - 1 && y == height - 3 || x == width - 2 && y == height - 3)
                    {
                        current = MazeObject.MazeObjectType.HALL;
                    }

                    objects[y, x] = new MazeObject(current);
                    images[y, x] = new PictureBox();
                    images[y, x].Location = new Point(x * objects[y, x].width, y * objects[y, x].height);
                    images[y, x].Parent = parent;
                    images[y, x].Width = objects[y, x].width;
                    images[y, x].Height = objects[y, x].height;
                    images[y, x].BackgroundImage = objects[y, x].texture;
                }
            }
        }
    }
}
