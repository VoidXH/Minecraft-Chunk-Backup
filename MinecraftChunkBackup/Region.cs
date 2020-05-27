namespace MinecraftChunkBackup {
    public class Region {
        public Position Pos { get; }

        /// <summary>
        /// Lowest contained chunk's position.
        /// </summary>
        public Position ChunkStart => Pos << 5;
        /// <summary>
        /// Highest containd chunk's position.
        /// </summary>
        public Position ChunkEnd => (Pos << 5) + 31;
        /// <summary>
        /// Lowest contained world position.
        /// </summary>
        public Position WorldPosStart => Pos << 9;
        /// <summary>
        /// Highest containd world position.
        /// </summary>
        public Position WorldPosEnd => (Pos << 9) + 511;

        public Region(int x, int z) => Pos = new Position(x, z);

        public static Region FromChunk(int x, int z) => new Region(x >> 5, z >> 5);

        public static Region FromWorldPos(int x, int z) => new Region(x >> 9, z >> 9);

        public override string ToString() => string.Format("r.{0}.{1}.mca", Pos.X, Pos.Z);
    }
}