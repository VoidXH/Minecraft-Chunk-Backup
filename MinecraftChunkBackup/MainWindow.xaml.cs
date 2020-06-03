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

        /// <summary>Checks if a position is already in the list of regions.</summary>
        bool HasRegion(int x, int z) {
            for (int i = 0, end = regions.Count; i < end; ++i) {
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
        void AddSortedRegion(int x, int z) {
            int pos = 0, end = regions.Count;
            for (; pos < end; ++pos) {
                Position coords = regions[pos].Region.Pos;
                if (coords.X < x)
                    continue;
                if (coords.X > x || coords.Z > z)
                    break;
            }
            regions.Add(new RegionEntry(new Region(x, z)));
            if (pos != end)
                regions.Move(end, pos);
        }

        /// <summary>Show an <see cref="AddRegions"/> dialog and add the results that are not already on the <see cref="regions"/> list.</summary>
        void AddRegionsButton(object sender, RoutedEventArgs e) {
            AddRegions adder = new AddRegions();
            if (adder.ShowDialog().Value)
                for (int x = adder.regionStartX.Value, xEnd = adder.regionEndX.Value, xDir = Math.Sign(xEnd - x + .01);
                    x != xEnd + xDir; x += xDir)
                    for (int z = adder.regionStartZ.Value, zEnd = adder.regionEndZ.Value, zDir = Math.Sign(zEnd - z + .01);
                        z != zEnd + zDir; z += zDir)
                        if (!HasRegion(x, z))
                            AddSortedRegion(x, z);
        }
    }
}