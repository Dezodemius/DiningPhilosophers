using System;
using System.Collections.Generic;
using System.Threading;
using Timer = System.Timers.Timer;

namespace DiningPhilosophers.DiningStrategy
{
  public class QueryableDiningStrategy : IDiningStrategy
  {
    #region Константы

    /// <summary>
    /// Время, которое занимает обед.
    /// </summary>
    private const int DiningTimeout = 250;

    /// <summary>
    /// Время, необоходимое для смерти.
    /// </summary>
    private const int BeforeDeathTimeout = 3000;

    #endregion
    
    #region Поля
        
    /// <summary>
    /// Имя философа.
    /// </summary>
    private string _philosopherName;
        
    /// <summary>
    /// Номер философа.
    /// </summary>
    private int _id;
    
    /// <summary>
    /// Время, за которое философы проголодаются.
    /// </summary>
    private int _beforeStartStarvingTimeout;

    /// <summary>
    /// Признак того, что философ голодает.
    /// </summary>
    private bool _isStarving;

    /// <summary>
    /// Признак того, что философ мёртв.
    /// </summary>
    private bool _isDead;

    #endregion  
    
    #region Методы
    
    public void Run(string philosopherName, int id, IReadOnlyCollection<Fork> forks)
    { 
      _philosopherName = philosopherName;
      _id = id;
      _beforeStartStarvingTimeout = 0;
      
      while (true)
      {
        Thread.Sleep(_beforeStartStarvingTimeout);
        EvaluateIsStarving();
        if (_isStarving)
          StartEating((IReadOnlyList<Fork>)forks);
        if (_isDead) 
          return;
      }
    }
    
    /// <summary>
    /// Начать есть.
    /// </summary>
    /// <param name="forks">Вилки.</param>
    private void StartEating(IReadOnlyList<Fork> forks)
    {
        var timer = InitializeTimer();
        Console.WriteLine($"Философ {_philosopherName} ждёт вилку");
        
        timer.Start();
        Monitor.Enter(forks);
        try
        {
            if (_isDead)
                return;
            
            Dining(timer, forks);
        }
        finally
        {
            Monitor.Exit(forks);
        }
    }

    /// <summary>
    /// Взять вилки.
    /// </summary>
    /// <param name="forks">Набор вилок.</param>
    /// <param name="leftForkIndex">Номер левой вилки.</param>
    /// <param name="rightForkIndex">Номер правой вилки.</param>
    private void GetForks(IReadOnlyCollection<Fork> forks, out int leftForkIndex, out int rightForkIndex)
    {
        leftForkIndex = _id;
        rightForkIndex = leftForkIndex == forks.Count - 1 ? 0 : leftForkIndex + 1;
    }

    /// <summary>
    /// Начать есть.
    /// </summary>
    /// <param name="timer">Таймер.</param>
    /// <param name="forks">Вилки.</param>
    private void Dining(Timer timer, IReadOnlyList<Fork> forks)
    {
        GetForks(forks, out var leftForkIndex, out var rightForkIndex);
        
        if (forks[leftForkIndex].IsUsed || forks[rightForkIndex].IsUsed)
            return;
        
        Console.WriteLine($"Философ {_philosopherName} берёт вилку.");
        
        timer.Stop();
        timer.Dispose();
        
        forks[leftForkIndex].IsUsed = true;
        forks[rightForkIndex].IsUsed = true;

        Console.WriteLine($"Философ {_philosopherName} ест. Используются вилки под номером {leftForkIndex + 1} и {rightForkIndex + 1}.");
        Thread.Sleep(DiningTimeout);
        
        PutForksBack(forks, leftForkIndex, rightForkIndex);
    }
    
    /// <summary>
    /// Вернуть вилки назад в набор.
    /// </summary>
    /// <param name="forks">Набор вилок.</param>
    /// <param name="leftForkIndex">Номер левой вилки.</param>
    /// <param name="rightForkIndex">Номер правой вилки.</param>
    private void PutForksBack(IReadOnlyList<Fork> forks, int leftForkIndex, int rightForkIndex)
    {
        forks[leftForkIndex].IsUsed = false;
        forks[rightForkIndex].IsUsed = false;
        
        Console.WriteLine($"Философ {_philosopherName} поел. Положил вилки под номером {leftForkIndex + 1} и {rightForkIndex + 1}.");
    }

    /// <summary>
    /// Инициализировать таймер.
    /// </summary>
    /// <returns>Таймер.</returns>
    private Timer InitializeTimer()
    {
        var timer = new Timer();
        timer.Elapsed += OnTimer;
        timer.Interval = BeforeDeathTimeout;
        
        return timer;
    }

    /// <summary>
    /// Изменить статуc философа.
    /// </summary>
    private void EvaluateIsStarving()
    {
        _isStarving = !_isStarving;
        if (!_isStarving)
            Console.WriteLine($"Философ {_philosopherName} думает.");
    }
    
    #endregion
    
    #region Обработчики событий

    void OnTimer(object sender, System.Timers.ElapsedEventArgs e)
    {
      Console.BackgroundColor = ConsoleColor.DarkRed;
      var message = $"\nФИЛОСОФ {_philosopherName} УМЕР";
      Console.WriteLine(message);
      Console.ResetColor();
            
      _isDead = true;
      ((Timer)sender).Stop();
      
      throw new DiningPhilosophersStrategyFailedException(message);
    }

    #endregion
  }
}