using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace MinecraftChunkBackup {
    public class BackupTask : IDisposable {
        readonly Collection<RegionEntry> regions;
        readonly Task runner;

        bool running = true;

        void Run() {
            while (running) {
                for (int i = 0, end = regions.Count; i < end; ++i) {

                }
                Thread.Sleep(1000);
            }
        }

        public BackupTask(Collection<RegionEntry> regions) {
            this.regions = regions;
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