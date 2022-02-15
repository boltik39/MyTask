using Utility.Models;

namespace Utility;

internal static class Program
{

    private static void Main(string[] args)
    {
        var viewer = new Viewer(args[0], args[1], args[2]);
        viewer.RunViewer();
    }
}