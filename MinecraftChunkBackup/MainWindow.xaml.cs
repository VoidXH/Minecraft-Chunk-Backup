using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace MinecraftChunkBackup {
    public partial class MainWindow : Window {
        readonly ObservableCollection<RegionEntry> regions = new ObservableCollection<RegionEntry>();

        public MainWindow() {
            InitializeComponent();
            regionList.ItemsSource = regions;
        }

        void AddWorldButton(object sender, RoutedEventArgs e) {
        }

        void AddRegionsButton(object sender, RoutedEventArgs e) {
            // TODO: skip duplicates
            AddRegions adder = new AddRegions();
            if (adder.ShowDialog().Value)
                for (int x = adder.regionStartX.Value, xEnd = adder.regionEndX.Value, xDir = Math.Sign(xEnd - x + .01);
                    x != xEnd + xDir; x += xDir)
                    for (int z = adder.regionStartZ.Value, zEnd = adder.regionEndZ.Value, zDir = Math.Sign(zEnd - z + .01);
                        z != zEnd + zDir; z += zDir)
                        regions.Add(new RegionEntry(new Region(x, z)));
        }
    }
}