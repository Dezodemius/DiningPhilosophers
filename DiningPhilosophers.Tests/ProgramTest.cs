using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DiningPhilosophers.Tests
{
  public class Tests
  {
    [Test]
    public void ProgramContinuousTest()
    {
      var programTask = Task.Factory.StartNew(Program.Main);
      var stopwatch = Task.Factory.StartNew(RunStopwatch);
      
      Task.WaitAll(programTask, stopwatch);
      
      Assert.Pass();
    }

    private static void RunStopwatch()
    {
      var finishDatetime = DateTime.Now.AddDays(1);
      while (finishDatetime > DateTime.Now) { }
    }
  }
}