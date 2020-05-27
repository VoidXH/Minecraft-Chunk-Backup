namespace MinecraftChunkBackup {
    public class RegionToChunk {
        readonly string conversion;

        public RegionToChunk(Region region) {
            Position start = region.ChunkStart, end = region.ChunkEnd;
            conversion = string.Format("{0};{1} - {2};{3}", start.X, start.Z, end.X, end.Z);
        }

        public override string ToString() => conversion;
    }
}