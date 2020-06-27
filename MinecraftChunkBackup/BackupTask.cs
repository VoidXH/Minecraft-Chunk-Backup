using Controls;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MinecraftChunkBackup {
    public class BackupTask : IDisposable {
        const string regionFolder = "region";

        readonly Collection<RegionEntry> regions;
        readonly FolderPicker targetPath;
        readonly NumericTextBox hours, minutes;
        readonly Task runner;

        bool running = true;
        DateTime lastUpdate = DateTime.MinValue;

        void Run() {
            while (running) {
                Thread.Sleep(1000);
                if (lastUpdate.AddHours(hours.Value).AddMinutes(minutes.Value) > DateTime.Now)
                    continue;
                string path = targetPath.SelectedPath;
                if (string.IsNullOrEmpty(path))
                    continue;
                for (int i = 0, end = regions.Count; i < end; ++i) {
                    string regionFile = regions[i].Region.ToString(),
                        source = Path.Combine(regions[i].World.Path, regionFolder, regionFile);
                    if (File.Exists(source)) {
                        string outputFolder = Path.Combine(targetPath.SelectedPath, regions[i].World.Name);
                        string target = Path.Combine(outputFolder, regionFile);
                        if (File.Exists(target) && File.GetLastWriteTime(source) == File.GetLastWriteTime(target))
                            continue;
                        Directory.CreateDirectory(outputFolder);
                        File.Copy(source, target, true);
                    }
                }
                lastUpdate = DateTime.Now;
            }
        }

        public BackupTask(Collection<RegionEntry> regions, FolderPicker targetPath, NumericTextBox hours, NumericTextBox minutes) {
            this.regions = regions;
            this.targetPath = targetPath;
            this.hours = hours;
            this.minutes = minutes;
            runner = new Task(Run);
            runner.Start();
        }

        public void Dispose() {
            running = false;
            runner.Wait();
            runner.Dispose();
        }
    }
}