namespace MinecraftChunkBackup {
    public class World {
        public string Name { get; }
        public string Path { get; }

        public World(string path) {
            Name = System.IO.Path.GetDirectoryName(path);
            Path = path;
        }

        public override string ToString() => Name;
    }
}