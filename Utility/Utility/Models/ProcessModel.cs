using System.Diagnostics;

namespace Utility.Models;

public class ProcessModel
{
    private readonly string? _processName;
    private Process[] LstWithProcesses => Process.GetProcesses();

    public ProcessModel(string? processName)
    {
        _processName = processName;
    }

    public bool AreProcessWorking()
    {
        var lst = LstWithProcesses;
        return lst.Any(process => process.ProcessName == _processName);
    }

   public DateTime? GetProcessStartTime()
   {
       var lst = LstWithProcesses;
       foreach (var proc in lst)
       {
           if (proc.ProcessName == _processName)
           {
               return proc.StartTime;
           }
       }
       return null;
   }

   public void CloseWork()
   {
       var lst = LstWithProcesses;
       foreach (var proc in lst)
       {
           if (proc.ProcessName == _processName)
           {
               proc.Kill();
               return;
           }
       }
   }
}