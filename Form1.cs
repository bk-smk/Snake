using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//Согласно "Соглашениям о комментариях" - "Комментарий размещается на отдельной строке, а не в конце строки кода."
//В целях более удобной демонстрации проекта комментарии написаны с нарушением упомянутого Соглашения. 

namespace Snake
{
    public partial class Form1 : Form
    {
        private int rI, rJ; // Объявление переменных для получения рандомизированных координат.
        private PictureBox fruit; // Объявление поля fruit класса PictureBox.
        private PictureBox[] snake = new PictureBox[400]; // Объявление массив класса PictureBox для хранения структуры змейки на протяжении игры.
        private Label labelScore; // Объявление поля labelScore класса Label.
        private int dirX, dirY; // Объявление переменных, отвечающих за движение по оси X и Y.
        private int width = 700; // Объявление переменной, обозначающей ширину игрового поля (px).
        private int height = 600; // Объявление переменной, обозначающей высоту игрового поля (px).
        private int sizeOfSides = 40; // Объявление переменной, обозначающей размер квадрата (клетки, единицы объекта).
        private int score = 0; // Объявление переменной для счёта набранных очков.
        public Form1() // Конструктор формы.
        {
            InitializeComponent();
            this.Text = "Snake"; // Присвоение названия форме (название игры в левом верхнем углу).
            this.Width = width; // Присвоение значения ширины игровому окну (форме).
            this.Height = height; // Присвоение значение высоты игровому окну (форме).
            dirX = 1; // Изначально движение начинается только по оси X вправо.
            dirY = 0; // Изначально движение по оси Y не происходит (Y = 0).
            labelScore = new Label(); // Инициализация нового экземпляра класса Label.
            labelScore.Text = "Score: 0"; // Присвоение экземпляру класса Label текста "Score: 0".
            labelScore.Location = new Point(610, 10); // Установка позиции экземпляра на форме (px).
            this.Controls.Add(labelScore);// Добавление экземпляра класса Label на форму (карту).
            snake[0] = new PictureBox(); // Создание первого (головного) элемента змейки (в массиве) в начале игры.
            snake[0].Location = new Point(201, 201); // Присвоение стандартной (изначальной) позиции на карте элемента змейки.
            snake[0].Size = new Size(sizeOfSides - 1, sizeOfSides - 1); // Размеры элемента змейки (квадрата).
            snake[0].BackColor = Color.Red; // Установка цвета головного элемента змейки.
            this.Controls.Add(snake[0]); // Добавление элемента змейки на форму (карту).
            fruit = new PictureBox(); // Инициализация нового экземпляра класса PictureBox (фрукта).
            fruit.BackColor = Color.Green; // Установка цвета фрукта.
            fruit.Size = new Size(sizeOfSides, sizeOfSides); // Обозначение размера фрукта (равен размеру клетки).
            GenerateMap(); // Добавление функции генерации карты.
            GenerateFruit(); // Добавление функции рандомной генерации фрукта.
            timer.Tick += new EventHandler(Update); // Создание обработчика событий таймеру, передача функцию Update.
            timer.Interval = 300; // Установка интервала 0.5 сек.
            timer.Start(); // Активация таймера.
            this.KeyDown += new KeyEventHandler(OKP); // Делегат, обрабатывающий событие нажатия клавиш.
        }

        private void GenerateFruit() // Функция для рандомной генерации фрукта.
        {
            Random r = new Random(); // Объявление переменной, отвечающей за генерацию рандома.
            rI = r.Next(0, height - sizeOfSides); // Генерируем значение от нуля до ширины игрового окна.
            int tempI = rI % sizeOfSides; // Для получения размеров, кратных 40 (sizeOfSides), получаем остаток от деления -->.
            rI -= tempI; // Остаток от деления вычитаем из полученной переменной.
            rJ = r.Next(0, height - sizeOfSides); // Установка границы генерации фрукта в пределах карты (+строка с rI). 
            int tempJ = rJ % sizeOfSides;
            rJ -= tempJ;
            rI++; // Увеличение координаты в генерации фруктов.
            rJ++; // Увеличение координаты в генерации фруктов.
            fruit.Location = new Point(rI, rJ); // Задаём координаты, полученные с помощью рандомизации.
            this.Controls.Add(fruit); // Добавление экземпляра класса (фрукта) на форму (карту).
            // На выходе - переменная кратна 40, то есть "стороне квадрата".
        }

        private void CheckBorders() // Функция для ограничения выхода змейки за пределы карты.
        {
            if (snake[0].Location.X < 1) // Условие, при котором змейка не заходит дальше левого борта карты.
            {
                for (int i = 1; i <= score; i++) // Цикл определения диапазона набранных очков (длины змейки).
                {
                    this.Controls.Remove(snake[i]); // Удаление определённой длины змейки (кроме головного элемента).
                }
                score = 0; // Обнуление очков.
                labelScore.Text = "Score: " + score; // Вывод очков на лэйбл.
                dirX = 1; // Разворот змейки в противоположную сторону (вправо).
                dirY = 0; // Фиксация разворота только по оси X.
            }
            if (snake[0].Location.X > height - 30) // Условие, при котором змейка не заходит дальше правого борта карты.
            {
                for (int i = 1; i <= score; i++) // Цикл определения диапазона набранных очков (длины змейки).
                {
                    this.Controls.Remove(snake[i]); // Удаление определённой длины змейки(кроме головного элемента).
                }
                score = 0; // Обнуление очков.
                labelScore.Text = "Score: " + score; // Вывод очков на лэйбл.
                dirX = -1; // Разворот змейки в противоположную сторону (влево).
                dirY = 0; // Фиксация разворота только по оси X.
            }
            if (snake[0].Location.Y < 1) // Условие, при котором змейка не заходит дальше верхнего борта карты.
            {
                for (int i = 1; i <= score; i++) // Цикл определения диапазона набранных очков (длины змейки).
                {
                    this.Controls.Remove(snake[i]); // Удаление определённой длины змейки(кроме головного элемента).
                }
                score = 0; // Обнуление очков.
                labelScore.Text = "Score: " + score; // Вывод очков на лэйбл.
                dirY = 1; // Разворот змейки в противоположную сторону (вниз).
                dirX = 0; // Фиксация разворота только по оси Y.
            }
            if (snake[0].Location.Y > height - 50) // Условие, при котором змейка не заходит дальше нижнего борта карты.
            {
                for (int i = 1; i <= score; i++) // Цикл определения диапазона набранных очков (длины змейки).
                {
                    this.Controls.Remove(snake[i]); // Удаление определённой длины змейки(кроме головного элемента).
                }
                score = 0; // Обнуление очков.
                labelScore.Text = "Score: " + score; // Вывод очков на лэйбл.
                dirY = -1; // Разворот змейки в противоположную сторону (вниз).
                dirX = 0; // Фиксация разворота только по оси Y.
            }
        }

        private void EatItself() // Функция при поедании змейки самой себя.
        {
            for (int i = 1; i < score; i++) // Цикл определения позиции элемента в змейке исходя из набранных очков.
            {
                if (snake[0].Location == snake[i].Location) // Условие одинакового расположения первого (головного) элемента змейки и одного из других элементов змейки.
                {
                    for (int j = i; j <= score; j++) // Цикл определения диапазона набранных очков (длины змейки).
                        this.Controls.Remove(snake[j]); // Удаление определённой длины змейки (кроме головного элемента).
                    score = score - (score - i); // Сброс набранных очков.
                    labelScore.Text = "Score: " + score; // Вывод очков на лэйбл.
                }
            }
        }

        private void EatFruit() // Функция для поедания фрукта, набора очков.
        {
            if (snake[0].Location.X == rI && snake[0].Location.Y == rJ) // Условие одинакового расположения первого (головного) элемента змейки и фрукта.
            {
                labelScore.Text = "Score: " + ++score; // Вывод очков на лэйбл, инкрементирование очков.
                snake[score] = new PictureBox(); // Добавление нового элемента змейки (в массиве).
                snake[score].Location = new Point(snake[score - 1].Location.X + 40 * dirX, snake[score - 1].Location.Y - 40 * dirY); // Установка позиции нового элемента змейки.
                snake[score].Size = new Size(sizeOfSides - 1, sizeOfSides - 1); // Обозначение размера нового элемента змейки.
                snake[score].BackColor = Color.Red; // Установка цвета нового элемента змейки.
                this.Controls.Add(snake[score]); // Добавление нового элемента змейки на карту (форму).
                GenerateFruit(); // Вызов функции генерации фрукта для появления следующего фрукта.
            }
        }

        private void GenerateMap() // Функция для генерации карты (линии по горизонтали и по вертикали).
        {
            for (int i = 0; i < height / sizeOfSides; i++)
            {
                PictureBox pic = new PictureBox();
                pic.BackColor = Color.Black;
                pic.Location = new Point(0, sizeOfSides * i);
                pic.Size = new Size(width - 100, 1);
                this.Controls.Add(pic);
            }
            for (int i = 0; i <= height / sizeOfSides; i++)
            {
                PictureBox pic = new PictureBox();
                pic.BackColor = Color.Black;
                pic.Location = new Point(sizeOfSides * i, 0);
                pic.Size = new Size(1, width -140);
                this.Controls.Add(pic);
            }
        }

        private void MoveSnake() // Функция для корректного движения змейки при её увеличении.
        {
            for (int i = score; i >= 1; i--) // Цикл определения позиции элемента в змейке исходя из набранных очков.
            {
                snake[i].Location = snake[i - 1].Location; // Присвоение новому элементу змейки координат предыдущего элемента змейки.
            }
            snake[0].Location = new Point(snake[0].Location.X + dirX * (sizeOfSides), snake[0].Location.Y + dirY * (sizeOfSides)); // Присвоение координат головному элементу змейки.
            EatItself();
        }

        private void Update(Object myObject, EventArgs eventArgs)
        {
            CheckBorders();
            EatFruit();
            MoveSnake();
            //cube.Location = new Point(cube.Location.X + dirX * _sizeOfSides, cube.Location.Y + dirY * _sizeOfSides);
        }

        private void OKP(object sender, KeyEventArgs e) // Обработчик нажатия с клавиатуры.
        {
            switch (e.KeyCode.ToString())
            {
                case "Right": // Вправо.
                    dirX = 1; 
                    dirY = 0;
                    break;
                case "Left": // Влево.
                    dirX = -1;
                    dirY = 0;
                    break;
                case "Up": // Вверх.
                    dirY = -1;
                    dirX = 0;
                    break;
                case "Down": // Вниз.
                    dirY = 1;
                    dirX = 0;
                    break;
            }
        }
    }
}
