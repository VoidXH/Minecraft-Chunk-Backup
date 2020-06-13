using System.Collections.ObjectModel;

namespace MinecraftChunkBackup {
    public class World {
        public string Name { get; }
        public string Path { get; }
        /// <summary>Autoincrement world ID assigned on saving.</summary>
        internal int ID { get; private set; }

        public World(string path) {
            Name = System.IO.Path.GetFileName(path);
            Path = path;
        }

        public override string ToString() => Name;

        public static void AssignIDs(Collection<World> worlds) {
            for (int i = 0, end = worlds.Count; i < end; ++i)
                worlds[i].ID = i;
        }
    }
}