using System.Collections.Generic;
using DiningPhilosophers.DiningStrategy;


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
        private readonly int _id;

        /// <summary>
        /// Стратегия обедания.
        /// </summary>
        public IDiningStrategy DiningStrategy { get; init; }

        #endregion

        /// <summary>
        /// Начать есть вилкой.
        /// </summary>
        /// <param name="forks">Вилки.</param>
        public void Run(object forks)
        {
            DiningStrategy.Run(_philosopherName, _id, forks as IReadOnlyCollection<Fork>);
        }
 
        #region Конструкторы

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="name">Имя философа.</param>
        /// <param name="id">Номер философа.</param>
        public Philosopher(string name, int id)
        {
            _philosopherName = name;
            _id = id;
        }

        #endregion
    }
}