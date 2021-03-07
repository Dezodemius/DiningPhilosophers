using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DiningPhilosophers
{
    /// <summary>
    /// Стандартный класс приложения.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Точка входа в приложение.
        /// </summary>
        public static void Main()
        {
            const int numberOfPhilosophers = 5;
            var forks = Enumerable.Repeat(new Fork(), numberOfPhilosophers).ToList();
            var philosophers = new List<Philosopher>();
            
            for (var i = 0; i < numberOfPhilosophers; i++)
            {
                philosophers.Add(new Philosopher((i + 1).ToString(), i));
                var thread = new Thread(philosophers[i].Run);
                thread.Start(forks);
            }
        }
    }
}