using System.Text;

public static class logger
{
    private static readonly object lock_obj = new object();
    private static StreamWriter log_file;
    private static string log_path;

    public static void init()
    {
        lock (lock_obj)
        {
            if (log_file != null) return;
            string LocalAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string Dir = Path.Combine(LocalAppData, "AerialClient");
            Directory.CreateDirectory(Dir);
            log_path = Path.Combine(Dir, "latest.log");
            log_file = new StreamWriter(log_path, true, Encoding.UTF8) { AutoFlush = true };
            log_file.WriteLine("\n---- session start ----");
        }
    }

    public static void shutdown()
    {
        lock (lock_obj)
        {
            if (log_file != null)
            {
                log_file.Flush();
                log_file.Close();
                log_file = null;
            }
        }
    }

    public static void write(string level, string tag, string msg)
    {
        lock (lock_obj)
        {
            if (log_file == null) init();
            string Stamp = DateTime.Now.ToString("HH:mm:ss.fff");
            string Line = $"[{Stamp}] {level} [{tag}] {msg}";
            log_file?.WriteLine(Line);
            log_file?.Flush();
        }
    }

    public static void info(string Tag, string Msg) => write("INFO", Tag, Msg);
    public static void warn(string Tag, string Msg) => write("WARN", Tag, Msg);
    public static void error(string Tag, string Msg) => write("ERROR", Tag, Msg);
    public static void debug(string Tag, string Msg) => write("DEBUG", Tag, Msg);
}