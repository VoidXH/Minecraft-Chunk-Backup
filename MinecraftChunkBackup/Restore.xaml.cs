using System.IO;
using System.Windows;

namespace MinecraftChunkBackup {
    public partial class Restore : Window {
        class RestoreEntry {
            public string Path { get; }

            readonly string name;

            public RestoreEntry(string path, string name) {
                Path = path;
                this.name = name;
            }

            public override string ToString() => name;
        }

        public Restore(RegionEntry entry, string backupPath, int backupCount) {
            InitializeComponent();
            for (int i = 0; i < backupCount; ++i) {
                string path = entry.BackupPath(backupPath, i);
                if (File.Exists(path))
                    version.Items.Add(new RestoreEntry(path, File.GetLastWriteTime(path).ToString()));
            }
            if (version.Items.Count != 0)
                version.SelectedIndex = 0;
        }

        void OK(object sender, RoutedEventArgs e) {
            // TODO: restore selected
            Close();
        }

        void Cancel(object sender, RoutedEventArgs e) => Close();
    }
}