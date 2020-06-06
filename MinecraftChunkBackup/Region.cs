using System;
using System.Collections.ObjectModel;
using System.IO;

namespace MinecraftChunkBackup {
    public class Region {
        public World World { get; }
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

        public Region(World world, int x, int z) {
            World = world;
            Pos = new Position(x, z);
        }

        static World GetWorld(string name, Collection<World> worlds) {
            for (int i = 0, end = worlds.Count; i < end; ++i) {
                if (worlds[i].Name.Equals(name)) {
                    return worlds[i];
                }
            }
            return null;
        }

        public Region(string serialization, Collection<World> worlds) {
            int split = serialization.IndexOf(':');
            World = GetWorld(serialization.Substring(0, split), worlds);
            Pos = new Position(serialization.Substring(split + 1));
        }

        public Region(BinaryReader reader, Collection<World> worlds) {
            World = GetWorld(reader.ReadString(), worlds);
            Pos = new Position(reader.ReadInt32(), reader.ReadInt32());
        }

        public static Region FromChunk(World world, int x, int z) => new Region(world, x >> 5, z >> 5);

        public static Region FromWorldPos(World world, int x, int z) => new Region(world, x >> 9, z >> 9);

        public void Serialize(BinaryWriter writer) {
            writer.Write(World.Name);
            writer.Write(Pos.X);
            writer.Write(Pos.Z);
        }

        public override string ToString() => string.Format("r.{0}.{1}.mca", Pos.X, Pos.Z);
    }
}