namespace PCSX2GridView.Backend
{
    using System.Diagnostics;

    public class ProcessManager : IProcessManager
    {
        public void Start(string command)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"" + command + "\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                },
            };

            proc.Start();
        }
    }
}