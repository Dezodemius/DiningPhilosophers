using System.Collections.Generic;

namespace DiningPhilosophers.DiningStrategy
{
  /// <summary>
  /// Стратегия обеда.
  /// </summary>
  public interface IDiningStrategy
  {
    /// <summary>
    /// Начать есть вилкой.
    /// </summary>
    /// <param name="id">Номер философа.</param>
    /// <param name="philosopherName">Имя философа.</param>
    /// <param name="forks">Вилки.</param>
    void Run(string philosopherName, int id, IReadOnlyCollection<Fork> forks);
  }
}