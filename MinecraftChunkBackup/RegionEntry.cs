namespace MinecraftChunkBackup {
    public class RegionEntry {
        public World World { get; }
        public Region Region { get; }
        public RegionToChunk Chunk { get; }
        public RegionToWorldPos WorldPos { get; }

        public RegionEntry(Region region) {
            Region = region;
            World = region.World;
            Chunk = new RegionToChunk(region);
            WorldPos = new RegionToWorldPos(region);
        }
    }
}