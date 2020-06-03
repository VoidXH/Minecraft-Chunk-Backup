namespace MinecraftChunkBackup {
    public class RegionToChunk {
        readonly string conversion;

        public RegionToChunk(Region region) => conversion = string.Format("{0} - {1}", region.ChunkStart, region.ChunkEnd);

        public override string ToString() => conversion;
    }
}