using System.IO;
using System.Windows;

namespace MinecraftChunkBackup {
    public partial class Restore : Window {
        class RestoreEntry {
            public string Path { get; }

            public string TargetPath { get; }

            readonly string name;

            public RestoreEntry(string path, string target, string name) {
                Path = path;
                TargetPath = target;
                this.name = name;
            }

            public override string ToString() => name;
        }

        public Restore(RegionEntry entry, string backupPath, int backupCount) {
            InitializeComponent();
            string target = entry.OriginalPath;
            for (int i = 0; i < backupCount; ++i) {
                string path = entry.BackupPath(backupPath, i);
                if (File.Exists(path))
                    version.Items.Add(new RestoreEntry(path, target, File.GetLastWriteTime(path).ToString()));
            }
            if (version.Items.Count != 0)
                version.SelectedIndex = 0;
            else
                ok.IsEnabled = false;
        }

        void OK(object sender, RoutedEventArgs e) {
            RestoreEntry item = (RestoreEntry)version.SelectedItem;
            File.Delete(item.TargetPath);
            File.Copy(item.Path, item.TargetPath);
            Close();
        }

        void Cancel(object sender, RoutedEventArgs e) => Close();
    }
}