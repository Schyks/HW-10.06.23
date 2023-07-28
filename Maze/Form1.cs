using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Maze
{
    public partial class Form1 : Form
    {
        // размеры лабиринта в ячейках 16х16 пикселей
        int columns = 35;
        int rows = 30;
        int pictureSize = 16; // ширина и высота одной ячейки

        Labirint l; // ссылка на логику всего происходящего в лабиринте

        public Form1()
        {
            InitializeComponent();
            Options();
            StartGame();
        }

        public void Options()
        {
            Text = $"Maze!  Всього медалей: {Labirint.countMedalAll}   Зібрано: {Labirint.countMedal} Здоров'я: {Labirint.health}";
            BackColor = Color.FromArgb(255, 92, 118, 137);
            
            // размеры клиентской области формы (того участка, на котором размещаются ЭУ)
            ClientSize = new Size(columns * pictureSize, rows * pictureSize+25);

            StartPosition = FormStartPosition.CenterScreen;
        }

        public void StartGame()
        {
            l = new Labirint(this, columns, rows);
            Text = $"Maze!  Всього медалей: {Labirint.countMedalAll}   Зібрано: {Labirint.countMedal}    Здоров'я: {Labirint.health}";
            timer1.Start();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
               // проверка на то, свободна ли ячейка справа
                if (l.objects[l.CharacterPositionY, l.CharacterPositionX + 1].type ==
                    MazeObject.MazeObjectType.HALL) // проверяем ячейку правее на 1 позицию, является ли она коридором
                {
                    l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[0]; // hall
                    l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage = l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;

                    l.CharacterPositionX++;

                    l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[4]; // character
                    l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage = l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;
                    Labirint.step++;

                }
                else if (l.objects[l.CharacterPositionY, l.CharacterPositionX + 1].type ==
                    MazeObject.MazeObjectType.MEDAL) // проверяем ячейку правее на 1 позицию, является ли она медалью
                {
                    l.objects[l.CharacterPositionY, l.CharacterPositionX+1].type = MazeObject.MazeObjectType.HALL;
                    l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[0]; // hall
                    l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage = l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;
                    
                    l.CharacterPositionX++;
                    Labirint.countMedal++;
                    Text = $"Maze!  Всього медалей: {Labirint.countMedalAll}   Зібрано: {Labirint.countMedal}    Здоров'я: {Labirint.health}";

                    l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[4]; // character
                    l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage = l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;
                    Labirint.step++;

                    if (Labirint.countMedal == Labirint.countMedalAll)
                    {
                        timer1.Stop();
                        MessageBox.Show("Перемога.\n\nВсі медалі зібрано!!!");
                    }
                }
                else if (l.objects[l.CharacterPositionY, l.CharacterPositionX + 1].type ==
                    MazeObject.MazeObjectType.AID) // проверяем ячейку правее на 1 позицию, является ли она лекарством
                {
                    if (Labirint.health < 100)
                    {
                        l.objects[l.CharacterPositionY, l.CharacterPositionX + 1].type = MazeObject.MazeObjectType.HALL;
                        l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[0]; // hall
                        l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage = l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;
                        Labirint.health += 5;
                        Text = $"Maze!  Всього медалей: {Labirint.countMedalAll}   Зібрано: {Labirint.countMedal}    Здоров'я: {Labirint.health}";
                        l.CharacterPositionX++;
                        l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[4]; // character
                        l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage = l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;
                        Labirint.step++;

                    }
                }
                else if (l.objects[l.CharacterPositionY, l.CharacterPositionX + 1].type ==
                   MazeObject.MazeObjectType.ENEMY) // проверяем ячейку правее на 1 позицию, является ли она врагом
                {
                    
                    {
                        l.objects[l.CharacterPositionY, l.CharacterPositionX + 1].type = MazeObject.MazeObjectType.HALL;
                        l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[0]; // hall
                        l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage = l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;
                        Labirint.health -= 25;
                        Text = $"Maze!  Всього медалей: {Labirint.countMedalAll}   Зібрано: {Labirint.countMedal}    Здоров'я: {Labirint.health}";
                        l.CharacterPositionX++;
                        l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[4]; // character
                        l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage = l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;
                        if (Labirint.health <= 0) { MessageBox.Show("Поразка.\n\nЗдоров'я закінчилося!!!"); }
                        Labirint.step++;

                    }
                }

                if (l.objects[l.CharacterPositionY, l.CharacterPositionX] == l.objects[rows - 3, columns - 1])
                {
                    timer1.Stop();
                    MessageBox.Show("Перемога.\n\nВихід знайдено!!!");
                }
            }
            else if (e.KeyCode == Keys.Left)
            {
                if (l.objects[l.CharacterPositionY, l.CharacterPositionX - 1].type ==
                   MazeObject.MazeObjectType.HALL) // проверяем ячейку левее на 1 позицию, является ли она коридором
                {
                    l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[0]; // hall
                    l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage = l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;

                    l.CharacterPositionX--;

                    l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[4]; // character
                    l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage = l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;
                    Labirint.step++;
                }
                else if (l.objects[l.CharacterPositionY, l.CharacterPositionX - 1].type ==
                   MazeObject.MazeObjectType.MEDAL) // проверяем ячейку левее на 1 позицию, является ли она медалью
                {
                    l.objects[l.CharacterPositionY, l.CharacterPositionX - 1].type = MazeObject.MazeObjectType.HALL;
                    l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[0]; // hall
                    l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage = l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;

                    l.CharacterPositionX--;
                    Labirint.countMedal++;
                    Text = $"Maze!  Всього медалей: {Labirint.countMedalAll}   Зібрано: {Labirint.countMedal}    Здоров'я: {Labirint.health}";

                    l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[4]; // character
                    l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage = l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;
                    Labirint.step++;

                    if (Labirint.countMedal == Labirint.countMedalAll)
                    {
                        timer1.Stop();
                        MessageBox.Show("Перемога.\n\nВсі медалі зібрано!!!");
                    }
                }
                else if (l.objects[l.CharacterPositionY, l.CharacterPositionX - 1].type ==
                    MazeObject.MazeObjectType.AID) // проверяем ячейку правее на 1 позицию, является ли она лекарством
                {
                    if (Labirint.health < 100)
                    {
                        l.objects[l.CharacterPositionY, l.CharacterPositionX - 1].type = MazeObject.MazeObjectType.HALL;
                        l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[0]; // hall
                        l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage = l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;
                        Labirint.health += 5;
                        Text = $"Maze!  Всього медалей: {Labirint.countMedalAll}   Зібрано: {Labirint.countMedal}    Здоров'я: {Labirint.health}";
                        l.CharacterPositionX--;
                        l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[4]; // character
                        l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage = l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;
                        Labirint.step++;
                    }
                }
                else if (l.objects[l.CharacterPositionY, l.CharacterPositionX - 1].type ==
                   MazeObject.MazeObjectType.ENEMY) // проверяем ячейку правее на 1 позицию, является ли она врагом
                {
                    {
                        l.objects[l.CharacterPositionY, l.CharacterPositionX + 1].type = MazeObject.MazeObjectType.HALL;
                        l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[0]; // hall
                        l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage = l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;
                        Labirint.health -= 25;
                        Text = $"Maze!  Всього медалей: {Labirint.countMedalAll}   Зібрано: {Labirint.countMedal}    Здоров'я: {Labirint.health}";
                        l.CharacterPositionX--;
                        l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[4]; // character
                        l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage = l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;
                        Labirint.step++;
                        if (Labirint.health <= 0)
                        {
                            timer1.Stop();
                            MessageBox.Show("Поразка.\n\nЗдоров'я закінчилося!!!"); }
                        }
                }
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (l.objects[l.CharacterPositionY+1, l.CharacterPositionX].type ==
                   MazeObject.MazeObjectType.HALL) // проверяем ячейку ниже на 1 позицию, является ли она коридором
                {
                    l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[0]; // hall
                    l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage = l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;

                    l.CharacterPositionY++;
                    
                    l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[4]; // character
                    l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage = l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;
                    Labirint.step++;
                }
                else if (l.objects[l.CharacterPositionY + 1, l.CharacterPositionX].type ==
                   MazeObject.MazeObjectType.MEDAL) // проверяем ячейку ниже на 1 позицию, является ли она медалью
                {
                    l.objects[l.CharacterPositionY+1, l.CharacterPositionX].type = MazeObject.MazeObjectType.HALL;
                    l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[0]; // hall
                    l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage = l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;

                    l.CharacterPositionY++;
                    Labirint.countMedal++;
                    Text = $"Maze!  Всього медалей: {Labirint.countMedalAll}   Зібрано: {Labirint.countMedal}    Здоров'я: {Labirint.health}";

                    l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[4]; // character
                    l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage = l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;
                    Labirint.step++;
                    if (Labirint.countMedal == Labirint.countMedalAll)
                    {
                        timer1.Stop();
                        MessageBox.Show("Перемога.\n\nВсі медалі зібрано!!!");
                    }
                }
                else if (l.objects[l.CharacterPositionY+1, l.CharacterPositionX].type ==
                    MazeObject.MazeObjectType.AID) // проверяем ячейку правее на 1 позицию, является ли она лекарством
                {
                    if (Labirint.health < 100)
                    {
                        l.objects[l.CharacterPositionY+1, l.CharacterPositionX].type = MazeObject.MazeObjectType.HALL;
                        l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[0]; // hall
                        l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage = l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;
                        Labirint.health += 5;
                        Text = $"Maze!  Всього медалей: {Labirint.countMedalAll}   Зібрано: {Labirint.countMedal}    Здоров'я: {Labirint.health}";
                        l.CharacterPositionY++;
                        l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[4]; // character
                        l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage = l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;
                        Labirint.step++;
                    }
                }
                else if (l.objects[l.CharacterPositionY+1, l.CharacterPositionX].type ==
                   MazeObject.MazeObjectType.ENEMY) // проверяем ячейку правее на 1 позицию, является ли она врагом
                {

                    {
                        l.objects[l.CharacterPositionY, l.CharacterPositionX + 1].type = MazeObject.MazeObjectType.HALL;
                        l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[0]; // hall
                        l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage = l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;
                        Labirint.health -= 25;
                        Text = $"Maze!  Всього медалей: {Labirint.countMedalAll}   Зібрано: {Labirint.countMedal}    Здоров'я: {Labirint.health}";
                        l.CharacterPositionY++;
                        l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[4]; // character
                        l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage = l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;
                        Labirint.step++;
                        if (Labirint.health <= 0)
                        {
                            timer1.Stop();
                            MessageBox.Show("Поразка.\n\nЗдоров'я закінчилося!!!"); }
                    }
                }
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (l.objects[l.CharacterPositionY - 1, l.CharacterPositionX].type ==
                   MazeObject.MazeObjectType.HALL) // проверяем ячейку вверху на 1 позицию, является ли она коридором
                {
                    l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[0]; // hall
                    l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage = l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;

                    l.CharacterPositionY--;

                    l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[4]; // character
                    l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage = l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;
                    Labirint.step++;
                }
                else if (l.objects[l.CharacterPositionY - 1, l.CharacterPositionX].type ==
                   MazeObject.MazeObjectType.MEDAL) // проверяем ячейку вверху на 1 позицию, является ли она медалью
                {
                    l.objects[l.CharacterPositionY-1, l.CharacterPositionX].type = MazeObject.MazeObjectType.HALL;
                    l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[0]; // hall
                    l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage = l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;

                    l.CharacterPositionY--;
                    Labirint.countMedal++;

                    Text = $"Maze!  Всього медалей: {Labirint.countMedalAll}   Зібрано: {Labirint.countMedal}    Здоров'я: {Labirint.health}";

                    l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[4]; // character
                    l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage = l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;
                    Labirint.step++;
                    if (Labirint.countMedal == Labirint.countMedalAll)
                    {
                        timer1.Stop();
                        MessageBox.Show("Перемога.\n\nВсі медалі зібрано!!!");
                    }
                }
                else if (l.objects[l.CharacterPositionY - 1, l.CharacterPositionX].type ==
                    MazeObject.MazeObjectType.AID) // проверяем ячейку правее на 1 позицию, является ли она лекарством
                {
                    if (Labirint.health < 100)
                    {
                        l.objects[l.CharacterPositionY - 1, l.CharacterPositionX].type = MazeObject.MazeObjectType.HALL;
                        l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[0]; // hall
                        l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage = l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;
                        Labirint.health += 5;
                        Text = $"Maze!  Всього медалей: {Labirint.countMedalAll}   Зібрано: {Labirint.countMedal}    Здоров'я: {Labirint.health}";
                        l.CharacterPositionY--;
                        l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[4]; // character
                        l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage = l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;
                        Labirint.step++;
                    }
                }
                else if (l.objects[l.CharacterPositionY - 1, l.CharacterPositionX].type ==
                   MazeObject.MazeObjectType.ENEMY) // проверяем ячейку правее на 1 позицию, является ли она врагом
                {

                    {
                        l.objects[l.CharacterPositionY, l.CharacterPositionX + 1].type = MazeObject.MazeObjectType.HALL;
                        l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[0]; // hall
                        l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage = l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;
                        Labirint.health -= 25;
                        Text = $"Maze!  Всього медалей: {Labirint.countMedalAll}   Зібрано: {Labirint.countMedal}    Здоров'я: {Labirint.health}";
                        l.CharacterPositionY--;
                        l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[4]; // character
                        l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage = l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;
                        Labirint.step++;
                        if (Labirint.health <= 0)
                        {
                            timer1.Stop();
                            MessageBox.Show("Поразка.\n\nЗдоров'я закінчилося!!!"); }
                    }
                }
            }
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            string str;
            Labirint.dt = Labirint.dt.AddSeconds(1);
            str = Labirint.dt.ToLongTimeString();
            this.toolStripStatusLabel1.Text = "Ігровий час:   "+str+"       Здоров'я:";
            this.toolStripStatusLabel2.Text="      Кількість кроків - " +Labirint.step.ToString();
            this.toolStripProgressBar1.Value = Labirint.health;
        }
    }
}
