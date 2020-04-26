using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace TelegramFontPatcher
{
    internal class Program
    {
        private const string ProcName = "Telegram";
        private const string ExeName = ProcName + ".exe";

        private static void Error(string message)
        {
            Console.WriteLine($"ERROR: {message}");
            Console.ReadLine();
            Environment.Exit(1);
        }

        private static void Main(string[] args)
        {
            Console.Title = $"Telegram Font Patcher {Assembly.GetExecutingAssembly().GetName().Version}";
            Console.WriteLine(Console.Title + Environment.NewLine);

            var processes = Process.GetProcessesByName("Telegram");
            if (processes.Length == 0)
                Error(ExeName + " not found");

            // Backup original Telegram.exe
            try
            {
                var proc = processes[0];
                var path = proc.MainModule.FileName;

                proc.Kill();
                proc.WaitForExit();

                File.Copy(path, path + ".bak", true);
                Console.WriteLine("INFO: Backup saved");

                Console.WriteLine($"NOTICE: Trying to patch {ExeName}...");
                
                var patternsRegular = new PatternList("Arial", new[] {
                    "Segoe UI", "Segoe UI Semibold",
                    "Open Sans", "Open Sans Semibold",
                    "DAOpenSansRegular",
                    "DAOpenSansRegularItalic",
                    "DAOpenSansBold",
                    "DAOpenSansBoldItalic",
                    "DAOpenSansSemibold",
                    "DAOpenSansSemiboldItalic",
                    "Microsoft YaHei",
                    "Microsoft JhengHei UI",
                    "Yu Gothic UI"                    
                });

                string patchedName = path + ".patched";
                if (File.Exists(patchedName))
                    File.Delete(patchedName);

                Console.WriteLine("Patching...");
                
                using (var reader = new BinaryReader(new FileStream(ExeName, FileMode.Open)))
                using (var writer = new BinaryWriter(new FileStream(patchedName, FileMode.Create)))
                {
                    BinaryUtility.Replace(reader, writer, patternsRegular);
                }

                File.Delete(path);
                File.Move(patchedName, path);

                Process.Start(path);
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }
    }
}

