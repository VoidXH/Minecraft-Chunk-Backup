namespace MinecraftChunkBackup {
    public class RegionToWorldPos {
        readonly string conversion;

        public RegionToWorldPos(Region region) => conversion = string.Format("{0} to {1}", region.WorldPosStart, region.WorldPosEnd);

        public override string ToString() => conversion;
    }
}