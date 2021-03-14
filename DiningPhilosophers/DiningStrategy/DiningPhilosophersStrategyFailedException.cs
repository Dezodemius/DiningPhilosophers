using System;

namespace DiningPhilosophers.DiningStrategy
{
  public class DiningPhilosophersStrategyFailedException : Exception
  {
    public DiningPhilosophersStrategyFailedException(string message) : base(message)
    {
      
    }
  }
}