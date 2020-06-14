using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

using Button = System.Windows.Controls.Button;
using Label = System.Windows.Controls.Label;

namespace Controls {
    public class FolderPicker : Grid {
        readonly Button openFolder = new Button();
        readonly Label path = new Label();

        public string OpenFolderButtonText {
            get => (string)openFolder.Content;
            set => openFolder.Content = value;
        }

        public bool ShowFullPath {
            get => showFullPath;
            set {
                showFullPath = value;
                ResetPath();
            }
        }
        bool showFullPath = false;

        public string SelectedPath {
            get => selectedPath;
            set {
                selectedPath = value;
                ResetPath();
            }
        }
        string selectedPath;

        void ResetPath() {
            if (selectedPath == null)
                path.Content = string.Empty;
            else if (showFullPath)
                path.Content = selectedPath;
            else
                path.Content = Path.GetFileName(selectedPath);
        }

        public void OpenFolder() {
            FolderBrowserDialog browser = new FolderBrowserDialog();
            if (browser.ShowDialog() == DialogResult.OK)
                SelectedPath = browser.SelectedPath;
        }

        private void OnOpenFolderButton(object sender, RoutedEventArgs e) => OpenFolder();

        public FolderPicker() {
            ColumnDefinition button = new ColumnDefinition { Width = new GridLength(100) };
            ColumnDefinitions.Add(button);
            ColumnDefinitions.Add(new ColumnDefinition());
            openFolder.Content = "Open folder...";
            openFolder.Height = 20;
            openFolder.Click += OnOpenFolderButton;
            Children.Add(openFolder);
            path.VerticalContentAlignment = VerticalAlignment.Center;
            path.Padding = new Thickness(5, 0, 0, 0);
            Children.Add(path);
            SetColumn(path, 1);
        }
    }
}