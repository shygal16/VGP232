using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;

namespace ConsoleSnake
{
    public class Program
    {
        const string TITLE = "Snake | Score: ";
        const string GAME_OVER = "Game Over!";

        public const string HORIZONTAL_LINE = " ###################################";
        const string VERTICAL_LINE =   " #                                 #";
        public const int VERTICAL_LINES = 20;

        const double FRAME_RATE = 200;

        private static int _Score;
        public static int Score { get { return _Score; }  private set { _Score = value; } }

        static System.Timers.Timer _timer;

        public static List<SnakePart> Snake;
        public static Direction SnakeDirection;

        static Food food;

        static int _xToClear;
        static int _yToClear;

        [DllImport("kernel32")]
        static extern bool AllocConsole();

        static void Main(string[] args)
        {
            /*while (!System.Diagnostics.Debugger.IsAttached)
            {
                Thread.Sleep(100);
            }*/
            try
            {
                Console.Clear();
            }
            catch(System.IO.IOException e)
            {
                bool success = AllocConsole();
                if (!success)
                {
                    Console.WriteLine("Running ConsoleSnake from with an output redirect?");
                    return;
                }
            }
            Console.WindowWidth = HORIZONTAL_LINE.Length + 1;
            Console.BufferWidth = HORIZONTAL_LINE.Length + 1;
            Console.WindowHeight = VERTICAL_LINES + 3;
            Console.BufferHeight = VERTICAL_LINES + 3;

            Initialization();

            SetTimer();
            while(_timer.Enabled)
            {

            }
        }

        private static void SetTimer()
        {
            _timer = new System.Timers.Timer(FRAME_RATE);
            _timer.Elapsed += Update;
            _timer.Start();
        }

        private static void Initialization()
        {
            Console.CursorVisible = false;
            Score = 0;

            Snake = new List<SnakePart>();
            SnakePart head = new SnakePart(10, 10);
            Snake.Add(head);

            SnakeDirection = Direction.Right;

            food = new Food();

            // Draw Chrome
            Console.WriteLine(HORIZONTAL_LINE);
            for (int i = 0; i < VERTICAL_LINES; i++)
            {
                Console.WriteLine(VERTICAL_LINE);
            }
            Console.WriteLine(HORIZONTAL_LINE);

            Thread inputThread = new Thread(Input);
            inputThread.Start();
        }

        static void Input()
        {
            ConsoleKeyInfo keyPressed = new ConsoleKeyInfo();
            while(true)
            {
                keyPressed = Console.ReadKey(true);

                if (keyPressed.Key == ConsoleKey.UpArrow & SnakeDirection != Direction.Down)
                    SnakeDirection = Direction.Up;

                if (keyPressed.Key == ConsoleKey.DownArrow & SnakeDirection != Direction.Up)
                    SnakeDirection = Direction.Down;

                if (keyPressed.Key == ConsoleKey.LeftArrow & SnakeDirection != Direction.Right)
                    SnakeDirection = Direction.Left;

                if (keyPressed.Key == ConsoleKey.RightArrow & SnakeDirection != Direction.Left)
                    SnakeDirection = Direction.Right;
            }
        }

        static void Update(object sender, ElapsedEventArgs e)
        {
            _xToClear = Snake[Snake.Count - 1].X;
            _yToClear = Snake[Snake.Count - 1].Y;

            for (int i = Snake.Count - 1; i >= 1; i--)
            {
                Snake[i].X = Snake[i - 1].X;
                Snake[i].Y = Snake[i - 1].Y;
            }

            switch (SnakeDirection)
            {
                case Direction.Up:
                    Snake[0].Y--;
                    break;
                case Direction.Down:
                    Snake[0].Y++;
                    break;
                case Direction.Left:
                    Snake[0].X--;
                    break;
                case Direction.Right:
                    Snake[0].X++;
                    break;
            }

            if(Snake[0].X == food.X && Snake[0].Y == food.Y)
            {
                Score++;
                food = new Food();
                SnakePart snakePart = new SnakePart(Snake[Snake.Count - 1].X, Snake[Snake.Count - 1].Y);
                Snake.Add(snakePart);
            }

            if(Snake[0].X == 0 || 
                Snake[0].X == HORIZONTAL_LINE.Length || 
                Snake[0].Y == 0 || 
                Snake[0].Y == VERTICAL_LINES)
            {
                GameOver();
            }

            for (int i = 1; i < Snake.Count-1; i++)
            {
                if (Snake[0].X == Snake[i].X & Snake[0].Y == Snake[i].Y)
                    GameOver();
            }
           
            Draw();
        }

        private static void GameOver()
        {
            _timer.Stop();
            Console.CursorTop = VERTICAL_LINES + 2;
            Console.CursorLeft = HORIZONTAL_LINE.Length / 2 - GAME_OVER.Length / 2;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(GAME_OVER);
        }

        static void Draw()
        {
            ConsoleColor color = Console.ForegroundColor;

            Console.Title = TITLE + Score;

            // Clearing
            Console.CursorTop = _yToClear;
            Console.CursorLeft = _xToClear;
            Console.Write(" ");

            // Draw food
            Console.CursorTop = food.Y;
            Console.CursorLeft = food.X;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("*");

            // Draw snake
            for (int i = 0; i < Snake.Count; i++)
            {
                Console.CursorTop = Snake[i].Y;
                Console.CursorLeft = Snake[i].X;

                if (i == 0)
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                else
                    Console.ForegroundColor = ConsoleColor.Green;

                Console.Write("O");
            }

            Console.ForegroundColor = color;
        }
    }
}
