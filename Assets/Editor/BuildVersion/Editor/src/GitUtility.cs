using System;
using System.Diagnostics;
using UnityEngine.PlayerLoop;

public static class GitUtility
{
    public static string RunCommand(string command)
    {
        ProcessStartInfo info = new ProcessStartInfo
        {
            FileName = "git",
            Arguments = command,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = false,
            UseShellExecute = false
        };
        using (Process process = Process.Start(info))
        {
            if (process == null)
            {
                throw new InvalidOperationException("Failed to start the Git process.");
            }

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new Exception($"Git command failed with exit code {process.ExitCode}. Error: {error}");
            }

            return output.Trim();
        }
    }
    public static string GetBranch()
    {
        return RunCommand("branch --show-current");
    }

    public static string GetStatus()
    {
        string output = RunCommand("status --porcelain");
        string status = output == "" ? "Up to Date" : "Waiting commit";
        return status;

    }
    public static string GetTag()
    {
        return RunCommand("describe --tags");
    }
    public static void SetTag(string version)
    {
        RunCommand($"tag {version}");
    }
    public static string GetCommit()
    {
        return RunCommand("rev-parse HEAD");
    }
}
