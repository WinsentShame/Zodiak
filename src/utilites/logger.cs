using System;
using System.IO;
using System.Text;

namespace Zodiak;

public static class logger
{
    private static readonly object lock_obj = new object();
    private static StreamWriter? file;
    private static string? log_path;

    public static void init()
    {
        lock (lock_obj)
        {
            if (file != null) return;
            string local_app_data = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string dir = Path.Combine(local_app_data, "Zodiak");
            Directory.CreateDirectory(dir);
            log_path = Path.Combine(dir, "latest.log");
            file = new StreamWriter(log_path, true, Encoding.UTF8) { AutoFlush = true };
            file.WriteLine("\n──── session start ────");
        }
    }

    public static void shutdown()
    {
        lock (lock_obj)
        {
            if (file != null)
            {
                file.Flush();
                file.Close();
                file = null;
            }
        }
    }

    public static void write(string level, string tag, string message)
    {
        lock (lock_obj)
        {
            if (file == null) init();
            string stamp = DateTime.Now.ToString("HH:mm:ss.fff");
            string line = $"[{stamp}] {level} [{tag}] {message}";
            file?.WriteLine(line);
            file?.Flush();
        }
    }

    public static void info(string tag, string msg) => write("INFO ", tag, msg);
    public static void warn(string tag, string msg) => write("WARN ", tag, msg);
    public static void error(string tag, string msg) => write("ERROR", tag, msg);
    public static void debug(string tag, string msg) => write("DEBUG", tag, msg);
}