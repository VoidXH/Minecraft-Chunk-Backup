using System.Windows;
using System.Windows.Controls;

namespace MinecraftChunkBackup {
    public partial class AddRegions : Window {
        public World World { get; }
        public AddRegions(World world) {
            World = world;
            InitializeComponent();
        }

        bool Valid => regionStartX.Valid && regionStartZ.Valid && regionEndX.Valid && regionEndZ.Valid &&
            chunkStartX.Valid && chunkStartZ.Valid && chunkEndX.Valid && chunkEndZ.Valid &&
            worldStartX.Valid && worldStartZ.Valid && worldEndX.Valid && worldEndZ.Valid;

        void FocusGain(object sender, RoutedEventArgs e) => ((TextBox)sender).SelectAll();

        void ResetChunks(Position start, Position end) {
            chunkStartX.Value = start.X;
            chunkStartZ.Value = start.Z;
            chunkEndX.Value = end.X;
            chunkEndZ.Value = end.Z;
        }

        void ResetWorldPositions(Position start, Position end) {
            worldStartX.Value = start.X;
            worldStartZ.Value = start.Z;
            worldEndX.Value = end.X;
            worldEndZ.Value = end.Z;
        }

        void Reset(Region start, Region end) {
            Position startPos = start.Pos, endPos = end.Pos;
            regionStartX.Value = startPos.X;
            regionStartZ.Value = startPos.Z;
            regionEndX.Value = endPos.X;
            regionEndZ.Value = endPos.Z;
            if (startPos.X <= endPos.X)
                if (startPos.Z <= endPos.Z) {
                    ResetChunks(start.ChunkStart, end.ChunkEnd);
                    ResetWorldPositions(start.WorldPosStart, end.WorldPosEnd);
                } else {
                    ResetChunks(new Position(start.ChunkStart.X, start.ChunkEnd.Z), new Position(end.ChunkEnd.X, end.ChunkStart.Z));
                    ResetWorldPositions(new Position(start.WorldPosStart.X, start.WorldPosEnd.Z),
                        new Position(end.WorldPosEnd.X, end.WorldPosStart.Z));
                }
            else if (startPos.Z <= endPos.Z) {
                ResetChunks(new Position(start.ChunkEnd.X, start.ChunkStart.Z), new Position(end.ChunkStart.X, end.ChunkEnd.Z));
                ResetWorldPositions(new Position(start.WorldPosEnd.X, start.WorldPosStart.Z),
                       new Position(end.WorldPosStart.X, end.WorldPosEnd.Z));
            } else {
                ResetChunks(start.ChunkEnd, end.ChunkStart);
                ResetWorldPositions(start.WorldPosEnd, end.WorldPosStart);
            }
        }

        void SetRegion(object sender, RoutedEventArgs e) {
            if (!Valid)
                return;
            Reset(new Region(World, regionStartX.Value, regionStartZ.Value), new Region(World, regionEndX.Value, regionEndZ.Value));
        }

        void SetChunk(object sender, RoutedEventArgs e) {
            if (!Valid)
                return;
            Reset(Region.FromChunk(World, chunkStartX.Value, chunkStartZ.Value), Region.FromChunk(World, chunkEndX.Value, chunkEndZ.Value));
        }

        void SetWorldPos(object sender, RoutedEventArgs e) {
            if (!Valid)
                return;
            Reset(Region.FromWorldPos(World, worldStartX.Value, worldStartZ.Value), Region.FromWorldPos(World, worldEndX.Value, worldEndZ.Value));
        }

        void OKButton(object sender, RoutedEventArgs e) {
            if (!Valid) {
                MessageBox.Show("There are fields with invalid data. Please fill all fields with numbers.", "Invalid data",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DialogResult = true;
            Close();
        }

        void CancelButton(object sender, RoutedEventArgs e) {
            DialogResult = false;
            Close();
        }
    }
}