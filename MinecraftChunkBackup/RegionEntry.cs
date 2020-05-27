namespace MinecraftChunkBackup {
    public class RegionEntry {
        public Region Region { get; }
        public RegionToChunk Chunk { get; }
        public RegionToWorldPos WorldPos { get; }

        public RegionEntry(Region region) {
            Region = region;
            Chunk = new RegionToChunk(region);
            WorldPos = new RegionToWorldPos(region);
        }
    }
}