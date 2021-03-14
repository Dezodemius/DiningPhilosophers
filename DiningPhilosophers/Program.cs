using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DiningPhilosophers.DiningStrategy;

namespace DiningPhilosophers
{
    /// <summary>
    /// Стандартный класс приложения.
    /// </summary>
    public static class Program
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
                var newPhilosopher = new Philosopher((i + 1).ToString(), i)
                {
                    DiningStrategy = new QueryableDiningStrategy()
                };
                philosophers.Add(newPhilosopher);
                
                var thread = new Thread(philosophers[i].Run);
                thread.Start(forks);
            }
        }
    }
}