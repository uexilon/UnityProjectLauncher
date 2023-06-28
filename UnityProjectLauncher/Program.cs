using System.IO;
using System.Diagnostics;
using System;

namespace UnityProjectLauncher
{
    class Program
    {
        static void Main()
        {
            if (File.Exists(@".\ProjectSettings\ProjectVersion.txt"))
            {
                string[] lines = File.ReadAllLines(@".\ProjectSettings\ProjectVersion.txt");
                string version = lines[0].Substring(lines[0].IndexOf(":") + 2);
                string changeset = lines[1].Substring(lines[1].IndexOf("(") + 1).Replace(")","");

                string path = @$"C:\Program Files\Unity\Hub\Editor\{version}\Editor\";
                Console.WriteLine(path);
                string argument = $"-projectPath \"{Directory.GetCurrentDirectory()}\"\\";
                Console.WriteLine(argument);
                

                ProcessStartInfo processStartInfo = new ProcessStartInfo("Unity.exe", argument);
                processStartInfo.WorkingDirectory = path;
                processStartInfo.UseShellExecute = true;
                try
                {
                    Process process = Process.Start(processStartInfo);
                }
                catch (Exception exception)
                {
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Logs");
                    File.AppendAllLines(Directory.GetCurrentDirectory() + "\\Logs\\LauncherLog.txt", new string[] { exception.Message, "If it can't find the file, it's most likely a missing unity version. Install the correct Unity Version and try again." });

                    ProcessStartInfo hubProcess = new ProcessStartInfo("Unity Hub.exe", $"-- --headless install --version {version} --changeset {changeset}");
                    hubProcess.WorkingDirectory = @$"C:\Program Files\Unity Hub\";
                    hubProcess.UseShellExecute = true;
                    try
                    {
                        Process hub = Process.Start(hubProcess);
                        hub.Exited += Hub_Exited;
                    }
                    catch (Exception)
                    {
                        Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Logs");
                        File.AppendAllLines(Directory.GetCurrentDirectory() + "\\Logs\\LauncherLog.txt", new string[] { exception.Message, "If it can't find the file, it's most likely a missing unity version. Install the correct Unity Version and try again." });

                        throw;
                    }
                }
            }
            else
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Logs");
                File.AppendAllLines(Directory.GetCurrentDirectory() + "\\Logs\\LauncherLog.txt", new string[] { "Error: Could not find a scene file. Make sure your project does have a scene file within the Assets or one of its subfolders!" });
            }
        }

        private static void Hub_Exited(object sender, EventArgs e)
        {
            Main();
        }

        private static void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Logs");
            File.AppendAllText(Directory.GetCurrentDirectory()+"\\Logs\\LauncherLog.txt", "\n"+e.Data);
        }
    }
}
