using System;

namespace Menu.Api.Helpers
{
    public static class RandomHelper
    {
        private static readonly Random random = new Random();

        public static int Generate(int min, int max)
        {
            return random.Next(min, max);
        }
    }
}