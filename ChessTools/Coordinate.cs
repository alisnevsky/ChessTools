namespace Chess
{
    public struct Coordinate
    {
        private byte _index;

        public void SetFileRank(byte file, byte rank)
        {
#if DEBUG
            if (file < 1 || file > 8 || rank < 1 || rank > 8)
                throw new ChessException($"File/rank [{file},{rank}] out of range");
#endif
            _index = (byte)((file - 1) * 8 + rank - 1);
        }

        public byte File
        {
            get
            {
                return (byte)(_index / 8 + 1);
            }
            set
            {
                SetFileRank(value, Rank);
            }
        }

        public byte Rank
        {
            get
            {
                return (byte)(_index % 8 + 1);
            }
            set
            {
                SetFileRank(File, value);
            }
        }

        public void Parse(string s)
        {
            // Expected a2, h8, etc.

            if (string.IsNullOrEmpty(s) || s.Length != 2)
                throw new ChessException($"Invalid coordinate {s}");

            char f = s[0];
            if (f < 'a' || f > 'h')
                throw new ChessException($"Invalid file in coordinate {s}");

            char r = s[1];
            if (r < '1' || r > '8')
                throw new ChessException($"Invalid rank in coordinate {s}");

            SetFileRank((byte)(f - 'a' + 1), (byte)(r - '1' + 1));
        }

        public Coordinate(byte file, byte rank) : this()
        {
            SetFileRank(file, rank);
        }

        public Coordinate(string s) : this()
        {
            Parse(s);
        }

        public static implicit operator Coordinate(byte b)
        {
#if DEBUG
            if (b > 63)
                throw new ChessException($"Coordinate [{b}] out of range");
#endif
            Coordinate c = new Coordinate();
            c._index = b;

            return c;
        }

        public static implicit operator Coordinate(string s)
        {
            return new Coordinate(s);
        }

        public static implicit operator string(Coordinate c)
        {
            return c.ToString();
        }

        public static implicit operator byte(Coordinate c)
        {
            return c._index;
        }

        public override string ToString()
        {
            char f = (char)('a' + File - 1);
            char r = (char)('1' + Rank - 1);

            return f.ToString() + r;
        }
    }
}
