namespace Conduit.Engine.Mathematics
{
    public struct Color
    {
        public float R { get; set; }
        public float G { get; set; }
        public float B { get; set; }
        public float A { get; set; }

        public Color(float r, float g, float b, float a = 1f)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }

        public Color(byte r, byte g, byte b, byte a = 255) : this(r / 255f, g / 255f, b / 255f, a / 255f) { }

        public Color(Color color) : this(color.R, color.G, color.B, color.A) { }

        public Color(string hex) : this(FromHex(hex)) { }

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return this.R;
                    case 1: return this.G;
                    case 2: return this.B;
                    case 3: return this.A;
                    default: throw new IndexOutOfRangeException("Invalid color index");
                }
            }
            set
            {
                switch (index)
                {
                    case 0: this.R = value; break;
                    case 1: this.G = value; break;
                    case 2: this.B = value; break;
                    case 3: this.A = value; break;
                    default: throw new IndexOutOfRangeException("Invalid color index");
                }
            }
        }

        public static Color FromRGBA(float r, float g, float b, float a) => new Color(r, g, b, a);
        public static Color FromRGBA(byte r, byte g, byte b, byte a) => new Color(r, g, b, a);

        public static Color FromHex(string hex)
        {
            ArgumentNullException.ThrowIfNull(hex);

            if (hex.StartsWith('#'))
                hex = hex.Substring("#".Length);

            if (hex.Length != 6 && hex.Length != 8) throw new FormatException("Invalid hex format");

            float r = Convert.ToByte(hex.Substring(0, 2), 16) / 255f;
            float g = Convert.ToByte(hex.Substring(2, 2), 16) / 255f;
            float b = Convert.ToByte(hex.Substring(4, 2), 16) / 255f;
            float a = hex.Length == 8
                ? Convert.ToByte(hex.Substring(6, 2), 16) / 255f
                : 1f;

            return new Color(r, g, b, a);
        }
    }
}
