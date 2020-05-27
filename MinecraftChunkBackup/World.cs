namespace MinecraftChunkBackup {
    public class World {
        public string Name { get; }

        public World(string name) {
            Name = name;
        }

        public override string ToString() => Name;
    }
}