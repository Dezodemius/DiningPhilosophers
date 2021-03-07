using System;
using System.Collections.Generic;
using System.Threading;

namespace DiningPhilosophers
{
    /// <summary>
    /// Философ.
    /// </summary>
    internal class Philosopher
    {
        #region Поля и свойства
        
        /// <summary>
        /// Имя философа.
        /// </summary>
        private readonly string _philosopherName;
        
        /// <summary>
        /// Номер философа.
        /// </summary>
        private readonly int _number;
        
        /// <summary>
        /// Признак того, что философ голодает.
        /// </summary>
        private bool _isHunger;
        
        /// <summary>
        /// Признак того, что философ мёртв.
        /// </summary>
        private bool _isDead;

        #endregion

        #region Обработчики событий

        void OnTimer(object sender, System.Timers.ElapsedEventArgs e)
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\nФИЛОСОФ {0} УМЕР", _philosopherName);
            Console.ResetColor();
            _isDead = true;
            ((System.Timers.Timer) sender).Stop();
        }

        #endregion
        
        #region Методы

        /// <summary>
        /// Взять вилку.
        /// </summary>
        /// <param name="forks">Вилки.</param>
        void GetFork(IReadOnlyList<Fork> forks)
        {
            var timer = new System.Timers.Timer();
            timer.Elapsed += OnTimer;
            timer.Interval = 3000;
            Console.WriteLine("Философ {0} ждёт вилку", _philosopherName);
            timer.Start();
            Monitor.Enter(forks);
            try
            {
                if (_isDead)
                    return;
                
                var first = _number;
                var second = _number == forks.Count - 1 ? 0 : _number + 1;
                if (forks[first].IsUsing || forks[second].IsUsing) 
                    return;
                
                Console.WriteLine("Философ {0} берёт вилку ({1})", _philosopherName, timer.Enabled);
                timer.Stop();
                timer.Dispose();
                forks[first].IsUsing = true;
                forks[second].IsUsing = true;
                Console.WriteLine("Философ {0} ест.", _philosopherName);
                Console.WriteLine("Используются вилки под номером {0} и {1}.", first + 1, second + 1);
                Thread.Sleep(250);
                forks[first].IsUsing = false;
                forks[second].IsUsing = false;
            }
            finally
            {
                Monitor.Exit(forks);
            }
        }
        
        /// <summary>
        /// Начать есть вилкой.
        /// </summary>
        /// <param name="fork">Вилка.</param>
        public void Run(object fork)
        {
            while (true)
            {
                Thread.Sleep(2000 + _number * 1000);
                ChangeStatus();
                if (_isHunger)
                    GetFork((List<Fork>) fork);
                if (_isDead) 
                    return;
            }
        }

        /// <summary>
        /// Изменить статуc философа.
        /// </summary>
        void ChangeStatus()
        {
            _isHunger = !_isHunger;
            if (!_isHunger)
                Console.WriteLine("Философ {0} думает.", _philosopherName);
        }
        
        #endregion

        #region Конструкторы

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="name">Имя философа.</param>
        /// <param name="number">Номер философа.</param>
        public Philosopher(string name, int number)
        {
            _philosopherName = name;
            _number = number;
        }

        #endregion
    }
    
    /// <summary>
    /// Вилка.
    /// </summary>
    internal class Fork
    {
        /// <summary>
        /// Признак того, что вилка используется.
        /// </summary>
        public bool IsUsing { get; set; }
    }
}