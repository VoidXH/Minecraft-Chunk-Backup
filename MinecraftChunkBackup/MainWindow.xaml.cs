using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Forms;

using MessageBox = System.Windows.MessageBox;

namespace MinecraftChunkBackup {
    public partial class MainWindow : Window {
        const string regionsFile = "regions.bin";

        readonly BackupTask backup;
        readonly ObservableCollection<World> worlds = new ObservableCollection<World>();
        readonly ObservableCollection<RegionEntry> regions = new ObservableCollection<RegionEntry>();

        public MainWindow() {
            InitializeComponent();
            if (File.Exists(regionsFile)) {
                using (BinaryReader reader = new BinaryReader(new FileStream(regionsFile, FileMode.Open))) {
                    for (int i = 0, end = reader.ReadInt32(); i < end; ++i)
                        worlds.Add(new World(reader.ReadString()));
                    for (int i = 0, end = reader.ReadInt32(); i < end; ++i)
                        regions.Add(new RegionEntry(new Region(reader, worlds)));
                    hours.Value = reader.ReadInt32();
                    minutes.Value = reader.ReadInt32();
                    string path = reader.ReadString();
                    if (string.IsNullOrEmpty(path))
                        picker.SelectedPath = null;
                    else
                        picker.SelectedPath = path;
                }
            }
            worldList.ItemsSource = worlds;
            if (worlds.Count != 0)
                worldList.SelectedItem = worlds[worlds.Count - 1];
            regionList.ItemsSource = regions;
            backup = new BackupTask(regions, picker, hours, minutes);
        }

        /// <summary>Browse the PC for a Minecraft world.</summary>
        void AddWorldButton(object sender, RoutedEventArgs e) {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), mcPath = Path.Combine(path, ".minecraft", "saves");
            if (Directory.Exists(mcPath))
                path = mcPath;
            FolderBrowserDialog opener = new FolderBrowserDialog {
                SelectedPath = path
            };
            if (opener.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                string levelPath = Path.Combine(opener.SelectedPath, "level.dat"), regionPath = Path.Combine(opener.SelectedPath, "region");
                if (!File.Exists(levelPath) || !Directory.Exists(regionPath)) {
                    MessageBox.Show("The selected folder is not the root of a Minecraft world.", "Invalid folder",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                World world = new World(opener.SelectedPath);
                worlds.Add(world);
                worldList.SelectedItem = world;
            }
        }

        /// <summary>Removes the selected world and all its backed up regions.</summary>
        void RemoveWorldButton(object sender, RoutedEventArgs e) {
            while (worldList.SelectedItems.Count != 0) {
                World targetWorld = (World)worldList.SelectedItems[0];
                worlds.Remove(targetWorld);
                for (int i = 0, end = regions.Count; i < end; ++i) {
                    if (regions[i].World == targetWorld) {
                        regions.RemoveAt(i);
                        --i;
                        --end;
                    }
                }
            }
        }

        /// <summary>Checks if a position is already in the list of regions.</summary>
        bool HasRegion(World world, int x, int z) {
            for (int i = 0, end = regions.Count; i < end; ++i) {
                if (regions[i].World != world)
                    continue;
                Position pos = regions[i].Region.Pos;
                if (pos.X < x)
                    continue;
                if (pos.X > x)
                    return false;
                if (pos.Z == z)
                    return true;
            }
            return false;
        }

        /// <summary>Adds a region and keeps <see cref="regions"/> sorted.</summary>
        void AddSortedRegion(World world, int x, int z) {
            int pos = 0, end = regions.Count;
            for (; pos < end; ++pos) {
                Position coords = regions[pos].Region.Pos;
                if (coords.X < x)
                    continue;
                if (coords.X > x || coords.Z > z)
                    break;
            }
            regions.Add(new RegionEntry(new Region(world, x, z)));
            if (pos != end)
                regions.Move(end, pos);
        }

        /// <summary>Show an <see cref="AddRegions"/> dialog and add the results that are not already on the <see cref="regions"/> list.</summary>
        void AddRegionsButton(object sender, RoutedEventArgs e) {
            if (worldList.SelectedItems.Count != 1) {
                MessageBox.Show("Please select one world.", "World selection", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            World world = (World)worldList.SelectedItems[0];
            AddRegions adder = new AddRegions(world);
            if (adder.ShowDialog().Value)
                for (int x = adder.regionStartX.Value, xEnd = adder.regionEndX.Value, xDir = Math.Sign(xEnd - x + .01);
                    x != xEnd + xDir; x += xDir)
                    for (int z = adder.regionStartZ.Value, zEnd = adder.regionEndZ.Value, zDir = Math.Sign(zEnd - z + .01);
                        z != zEnd + zDir; z += zDir)
                        if (!HasRegion(world, x, z))
                            AddSortedRegion(world, x, z);
        }

        /// <summary>Removes the selected <see cref="regions"/>.</summary>
        void RemoveRegionsButton(object sender, RoutedEventArgs e) {
            while (regionList.SelectedItems.Count != 0)
                regions.Remove((RegionEntry)regionList.SelectedItems[0]);
        }

        void Window_Closed(object sender, EventArgs e) {
            backup.Dispose();
            using (BinaryWriter writer = new BinaryWriter(new FileStream(regionsFile, FileMode.Create))) {
                World.AssignIDs(worlds);
                writer.Write(worlds.Count);
                for (int i = 0, end = worlds.Count; i < end; ++i)
                    writer.Write(worlds[i].Path);
                writer.Write(regions.Count);
                for (int i = 0, end = regions.Count; i < end; ++i)
                    regions[i].Region.Serialize(writer);
                writer.Write(hours.Value);
                writer.Write(minutes.Value);
                if (picker.SelectedPath != null)
                    writer.Write(picker.SelectedPath);
                else
                    writer.Write(string.Empty);
            }
        }
    }
}