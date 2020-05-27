namespace MinecraftChunkBackup {
    public class RegionToWorldPos {
        readonly string conversion;

        public RegionToWorldPos(Region region) {
            Position start = region.WorldPosStart, end = region.WorldPosEnd;
            conversion = string.Format("{0};{1} - {2};{3}", start.X, start.Z, end.X, end.Z);
        }

        public override string ToString() => conversion;
    }
}