using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace Охота
{
    enum positionMan
    {
        OfLeft,OfRight,manOfUp,OfDown,OfFar
    }

    public partial class Form1 : Form
    {

        PictureBox man = new PictureBox();
        int colichVragov = 5;
        int razmKletka = 25;
        Point pos;
        Point selectedCell;
        int radiusPorazeniy = 3;

        Random rand = new Random();

        public Form1(int widht, int height)
        { 
            Random rand = new Random();

            InitializeComponent();
            createFild(height, widht);
            LoadPicture(man, "Охотник.jpg");
            pos = new Point(rand.Next(widht), rand.Next(height));
            tableLayoutPanel1.Controls.Add(man,pos.X,pos.Y);
            addEnemy(colichVragov, rand);
        }

        private void LoadPicture(PictureBox picture, string p)
        {
            picture.Image = Bitmap.FromFile(p);
            picture.Margin = new Padding(0);
        }


        // Добавляем 
        private void addEnemy(int col, Random rand)
        {
            while (colichVragov > 0)
            {
                Point point = new Point(rand.Next(tableLayoutPanel1.ColumnCount), rand.Next(tableLayoutPanel1.RowCount)); // берём рандомную точку на панели
                do
                {
                    if (pointIsEmpty(point))
                    {
                        positionMan position = manIsClose(point);
                        if (position == positionMan.OfFar)
                        {
                            PictureBox saw = new PictureBox();
                            saw.Click += tableLayoutPanel1_Click;

                            LoadPicture(saw, "Медведь.jpg");
                            tableLayoutPanel1.Controls.Add(saw, point.X, point.Y);
                            colichVragov--;
                            break;
                        }
                        else
                        {
                            if (position == positionMan.OfLeft)
                            {
                                point.X += 1;
                            }
                            else if (position == positionMan.OfRight)
                            {
                                point.X -= 1;
                            }
                            else if (position == positionMan.manOfUp)
                            {
                                point.Y += 1;
                            }
                            else if (position == positionMan.OfDown)
                            {
                                point.Y -= 1;
                            }
                            point = normalization(point,true);
                        }
                    }

                    else
                    {
                        break;
                    }
                }
                while(true);
            }       
        }

        // нормализация точки
        private Point normalization(Point inPoint, bool around = false)
        {
            Point point = inPoint;
            if (point.X < 0)
                if (around)
                    point.X = tableLayoutPanel1.ColumnCount - 1;
                else
                    point.X = 0;
            if (point.X >= tableLayoutPanel1.ColumnCount)
                if (around)
                    point.X = 0;
                else
                    point.X = tableLayoutPanel1.ColumnCount - 1;
            if (point.Y < 0)
                if (around)
                    point.Y = tableLayoutPanel1.RowCount - 1;
                else
                    point.Y = 0;
            if (point.Y >= tableLayoutPanel1.RowCount)
                if (around)
                    point.Y = 0;
                else
                    point.Y = tableLayoutPanel1.RowCount - 1;
            return point;
        }

        // проверка близко ли игрок
        private positionMan manIsClose(Point point)
        {
            Point p1 = new Point(pos.X - 1, pos.Y - 1); // левый верхний угол
            Point p2 = new Point(pos.X + 1, pos.Y + 1); // правый нижний угол 
            p1.X = p1.X < 0 ? 0 : p1.X;
            if (p2.X < tableLayoutPanel1.RowCount)
                p2.X = p2.X;
            else
                p2.X = tableLayoutPanel1.RowCount - 1;


            int y = p1.Y;

            if (p1.Y>=0)
                for (int x = p1.X; x <= p2.X; x++)
                    if (x == point.X && y == point.Y)
                        return positionMan.OfDown;
    
            y++;
            if (p1.X == point.X && y == point.Y)
                return positionMan.OfRight;
            if (p2.X == point.X && y == point.Y)
                return positionMan.OfLeft;
            y++;
            if (p1.Y <= tableLayoutPanel1.RowCount)
                for (int x = p1.X; x <= p2.X; x++)
                    if (x == point.X && y == point.Y)
                        return positionMan.manOfUp;

            return positionMan.OfFar;
        }

        // пустая клетка
        private bool pointIsEmpty(Point point)
        {
            return tableLayoutPanel1.GetControlFromPosition(point.X,point.Y) == null;
        }

        // создаём поле
        private void createFild(int widht, int height)
        {
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;// границы у сетки
            tableLayoutPanel1.ColumnCount = widht;
            tableLayoutPanel1.RowCount = height;
            for (int i = 0; i <= height; i++)
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, razmKletka));
            for (int i = 0; i <= widht; i++)
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, razmKletka));
        }

        // переходим на следующий шаг
        private void GoToTheNext(object sender, EventArgs e)
        {
            Control dostPicture = selectedCell.X != -1 ? tableLayoutPanel1.GetControlFromPosition(selectedCell.X, selectedCell.Y) : null;

            if (dostPicture == null)
            {

                if (Math.Abs(pos.X - selectedCell.X) < 2 && Math.Abs(pos.Y - selectedCell.Y) < 2)
                {

                    tableLayoutPanel1.Controls.Add(man, selectedCell.X, selectedCell.Y);
                    pos.X = selectedCell.X;
                    pos.Y = selectedCell.Y;
                }
            }
            else
            {
                if (Math.Abs(pos.X - selectedCell.X) < radiusPorazeniy && Math.Abs(pos.Y - selectedCell.Y) < radiusPorazeniy)
                {
                    tableLayoutPanel1.Controls.Remove(dostPicture);
                }
            }

            selectedCell = new Point(-1, -1);// задали то, что ход совершён

            EnemyTurn();

            String message = null;
            // конец игры в случае поражения
            if (man == null)
            {
                message = "Вас убили...";
            }
            // конец игры в случае победы
            if (tableLayoutPanel1.Controls.Count == 1)
            {
                message = "Поздравляем,Вы убили всех!!!";
            }

            if (message != null)
            {
                MessageBox.Show(message);
                this.новаяИграToolStripMenuItem_Click(null, null);
            }
        } 

        // Шаг противника
        private void EnemyTurn()
        {
            
            Point newpoint = new Point();

            int poz = tableLayoutPanel1.Controls.Count;

            for (int i = 0; i < poz; i++)
            {
                Control enemy = tableLayoutPanel1.Controls[i];
                if (enemy as PictureBox == man)
                    continue; 
                TableLayoutPanelCellPosition enemyPos = tableLayoutPanel1.GetPositionFromControl(enemy);   
                positionMan positions = manIsClose(new Point(enemyPos.Column,enemyPos.Row));

                if (positions != positionMan.OfFar)
                {
                    man = null;
                    break;
                }

                int colPr = 50; 
                
                do
                {
                    newpoint.X = enemyPos.Column + rand.Next(2000) / 750 - 1;
                    newpoint.Y = enemyPos.Row + rand.Next(2000) / 750 - 1;
                    newpoint = normalization(newpoint);
                                     
                    if (pointIsEmpty(newpoint))
                    {
                        tableLayoutPanel1.Controls.Add(enemy, newpoint.X,newpoint.Y);
                        break;
                    }
                    colPr--;
                }
                
                while (colPr != 0);
            }
        }


        private void tableLayoutPanel1_Click(object sender, EventArgs e)
        {
            MouseEventArgs click = (MouseEventArgs)e;
                     
            if (sender is TableLayoutPanel)
                selectedCell = new Point(click.X/(razmKletka+2), click.Y/(razmKletka+2));// Запоминаем местоположения клика
            else
            {
                TableLayoutPanelCellPosition pos = tableLayoutPanel1.GetPositionFromControl(sender as Control);
                selectedCell = new Point(pos.Column, pos.Row);
            }

            GoToTheNext(sender, e);
        }

        //начать новую игру
        private void новаяИграToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Owner.Show();
            this.Dispose();
        }
        //выход из игры
        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}
