using System.IO;
using System.Diagnostics;

namespace UnityProjectLauncher
{
    class Program
    {
        static void Main()
        {
            if (Directory.Exists(@".\Assets"))
            {
                string[] paths = Directory.GetFiles(@".\Assets", "*.unity", SearchOption.AllDirectories);
                if(paths.Length > 0)
                {
                    ProcessStartInfo processStartInfo = new ProcessStartInfo(paths[0]);
                    processStartInfo.UseShellExecute = true;
                    Process.Start(processStartInfo);
                }
                else
                {
                    File.AppendAllLines("LauncherLog.txt", new string[]{ "Error: Could not find a scene file. Make sure your project does have a scene file within the Assets or one of its subfolders!"});
                }
            }
            else
            {
                File.AppendAllLines("LauncherLog.txt", new string[] { "Error: Could not find a scene file because there was no Asset folder present. Make sure this Executable is placed in a Unity Project Folder (Not inside the Asset folder)!" });
            }
        }
    }
}
