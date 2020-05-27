using System;

namespace MinecraftChunkBackup {
    public struct Position : IEquatable<Position> {
        public int X { get; }
        public int Z { get; }

        public Position(int x, int z) {
            X = x;
            Z = z;
        }

        public static Position operator +(Position lhs, int rhs) => new Position(lhs.X + rhs, lhs.Z + rhs);
        public static Position operator -(Position lhs, int rhs) => new Position(lhs.X - rhs, lhs.Z - rhs);
        public static Position operator <<(Position lhs, int rhs) => new Position(lhs.X << rhs, lhs.Z << rhs);
        public static Position operator >>(Position lhs, int rhs) => new Position(lhs.X >> rhs, lhs.Z >> rhs);

        public bool Equals(Position other) => X == other.X && Z == other.Z;
    }
}