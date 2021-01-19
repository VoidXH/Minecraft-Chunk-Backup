using Controls;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Label = System.Windows.Controls.Label;

namespace MinecraftChunkBackup {
    public class BackupTask : IDisposable {
        const string regionFolder = "region";

        readonly Collection<RegionEntry> regions;
        readonly FolderPicker targetPath;
        readonly NumericTextBox hours, minutes, changes;
        readonly Task runner;
        readonly Label nextBackup;

        bool running = true;
        DateTime lastUpdate = DateTime.MinValue;

        void Run() {
            while (running) {
                TimeSpan nextIn = lastUpdate.AddHours(hours.Value).AddMinutes(minutes.Value) - DateTime.Now;
                if (nextIn <= TimeSpan.Zero)
                    nextBackup.Dispatcher.Invoke(() => nextBackup.Content = "Backing up...");
                else
                    nextBackup.Dispatcher.Invoke(() => nextBackup.Content = string.Format("Next backup in {0}:{1}:{2}",
                        nextIn.Hours.ToString("D2"), nextIn.Minutes.ToString("D2"), nextIn.Seconds.ToString("D2")));
                Thread.Sleep(1000);
                if (nextIn > TimeSpan.Zero)
                    continue;
                string path = targetPath.SelectedPath;
                if (string.IsNullOrEmpty(path))
                    continue;
                for (int region = 0, end = regions.Count; region < end; ++region) {
                    string source = Path.Combine(regions[region].World.Path, regionFolder, regions[region].Region.ToString());
                    if (File.Exists(source)) {
                        string target = regions[region].BackupPath(path, 0);
                        if (File.Exists(target)) {
                            if (File.GetLastWriteTime(source) == File.GetLastWriteTime(target))
                                continue;
                            string last = regions[region].BackupPath(path, changes.Value - 1);
                            if (File.Exists(last))
                                File.Delete(last);
                            for (int change = changes.Value - 1; change > 0;) {
                                string newTarget = regions[region].BackupPath(path, --change);
                                if (!File.Exists(newTarget))
                                    continue;
                                File.Move(newTarget, regions[region].BackupPath(path, change + 1));
                            }
                            File.Copy(source, target, true);
                        } else {
                            Directory.CreateDirectory(Path.Combine(targetPath.SelectedPath, regions[region].World.Name));
                            File.Copy(source, target, true);
                        }
                    }
                }
                lastUpdate = DateTime.Now;
            }
        }

        public BackupTask(Collection<RegionEntry> regions, FolderPicker targetPath, NumericTextBox hours, NumericTextBox minutes,
            NumericTextBox changes, Label nextBackup) {
            this.regions = regions;
            this.targetPath = targetPath;
            this.hours = hours;
            this.minutes = minutes;
            this.changes = changes;
            this.nextBackup = nextBackup;
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