using Utility.Utils;
using NLog;

namespace Utility.Models;

public class Viewer
{
    private static readonly Logger LogHelper = LogManager.GetCurrentClassLogger();
    private readonly string? _processName;
    private readonly float _lifeTime;
    private readonly float _freq;
    private readonly ProcessModel _process;
    private const int MillisecondsPerMinute = 60000;
    private const string IncorrectDataValue = "-1";
    
    private ProcessModel NewProcess(string? name) => new(name);

    public Viewer(string? processName, string lifetime, string frequency)
    {
        _processName = processName ?? IncorrectDataValue;
        if (!float.TryParse(lifetime, out _lifeTime) || !float.TryParse(frequency, out _freq))
        {
            Console.WriteLine("Incorrect data!!! Try again pls");
            LogHelper.Warn("Closing viewer due to an error with data");
            Environment.Exit(-1);
        }
        _process = NewProcess(processName);
    }

    private void ViewerBody(object? state)
    {
        LogHelper.Info("Start perioudly checking");
        
        if (!WaitAndFindProcess(_process))
            return;
        LogHelper.Info($"{_processName} process was found");
        var processKillTime = _process.GetProcessStartTime()!.Value.AddMinutes(_lifeTime);
        
        if (DateTime.Now >= processKillTime)
        {
            _process.CloseWork();
            LogHelper.Debug($"{_processName} process was kill");
        }
    }
    
    private bool WaitAndFindProcess(ProcessModel process)
    {
        var timeForWriteMessage = DateTime.Now.AddSeconds(double.Parse(ConfigWork.GetFromConfig("delay_for_write_message_in_sec")));
        var endTime =  DateTime.Now.AddSeconds(double.Parse(ConfigWork.GetFromConfig("time_for_founding_process_in_sec")));
        while (!process.AreProcessWorking())
        {
            if (DateTime.Now >= timeForWriteMessage)
            {
                timeForWriteMessage = DateTime.Now.AddSeconds(double.Parse(ConfigWork.GetFromConfig("delay_for_write_message_in_sec")));
                LogHelper.Debug($"Founding {_processName} process in processes list");
            }

            if (DateTime.Now >= endTime)
            {
                LogHelper.Debug($"{_processName} process search time is up");
                return false;
            }
        }
        return true;
    }
    
    public void RunViewer()
    {
        LogHelper.Info("Logger started");
        Console.WriteLine("Press 'Q' to exit!");
        var freqInMilliseconds = _freq * MillisecondsPerMinute;
        var tm = new TimerCallback(ViewerBody);
        var timer = new Timer(tm, null, 0, (long) freqInMilliseconds);
        if (Console.ReadKey(true).Key == ConsoleKey.Q)
        {
            LogHelper.Info("Closing viewer...");
            Console.WriteLine("Closing util...");
            Environment.Exit(0);
        }
    }
}