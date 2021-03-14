using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DiningPhilosophers.Tests
{
  public class Tests
  {
    [TestCase(1)]
    [TestCase(5)]
    [TestCase(10)]
    public void ProgramContinuousTestForMinutes(int minute)
    {
      var programTask = Task.Factory.StartNew(Program.Main);
      var stopwatch = Task.Factory.StartNew(
        () => RunStopwatch(DateTime.Now.AddMinutes(minute)));
      Task.WaitAll(programTask, stopwatch);
      
      Assert.Pass();
    }

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(4)]
    [TestCase(8)]
    public void ProgramContinuousTestForHours(int hour)
    {
      var programTask = Task.Factory.StartNew(Program.Main);
      var stopwatch = Task.Factory.StartNew(
        () => RunStopwatch(DateTime.Now.AddMinutes(hour)));
      Task.WaitAll(programTask, stopwatch);
      
      Assert.Pass();
    }
    
    [TestCase(1)]
    [TestCase(2)]
    public void ProgramContinuousTestForDays(int days)
    {
      var programTask = Task.Factory.StartNew(Program.Main);
      var stopwatch = Task.Factory.StartNew(
        () => RunStopwatch(DateTime.Now.AddMinutes(days)));
      Task.WaitAll(programTask, stopwatch);
      
      Assert.Pass();
    }
    
    private static void RunStopwatch(DateTime finishDatetime)
    {
      while (finishDatetime > DateTime.Now) { }
    }
  }
}