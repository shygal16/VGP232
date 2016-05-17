using System;

namespace ConsoleSnake
{
    public class Food
    {
        public int X;
        public int Y;

        public Food()
        {
            Random random = new Random();

            int x, y;
            do
            {
                x = random.Next(2, Program.HORIZONTAL_LINE.Length - 1); 
                y = random.Next(2, Program.VERTICAL_LINES - 1);
            }
            while (!CheckSnake(x, y));

            X = x;
            Y = y;

        }

        private bool CheckSnake(int x, int y)
        {
            bool result = true;

            foreach (SnakePart snakePart in Program.Snake)
            {
                if (snakePart.X == x & snakePart.Y == y)
                    result = false;
            }

            return result;
        }
    }
}
