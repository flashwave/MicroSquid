using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MicroSquid {
    public readonly struct Colour {
        private const int INHERIT = 0x40000000;

        public static readonly Colour Black = new Colour(0x000000);
        public static readonly Colour Silver = new Colour(0xC0C0C0);
        public static readonly Colour Grey = new Colour(0x808080);
        public static readonly Colour White = new Colour(0xFFFFFF);
        public static readonly Colour Maroon = new Colour(0x800000);
        public static readonly Colour Red = new Colour(0xFF0000);
        public static readonly Colour Purple = new Colour(0x800080);
        public static readonly Colour Magenta = new Colour(0xFF00FF);
        public static readonly Colour Green = new Colour(0x008000);
        public static readonly Colour Lime = new Colour(0x00FF00);
        public static readonly Colour Olive = new Colour(0x808000);
        public static readonly Colour Yellow = new Colour(0xFFFF00);
        public static readonly Colour Navy = new Colour(0x000080);
        public static readonly Colour Blue = new Colour(0x0000FF);
        public static readonly Colour Teal = new Colour(0x008080);
        public static readonly Colour Cyan = new Colour(0x00FFFF);
        public static readonly Colour Orange = new Colour(0xFFA500);
        public static readonly Colour AliceBlue = new Colour(0xF0F8FF);
        public static readonly Colour AntiqueWhite = new Colour(0xFAEBD7);
        public static readonly Colour Aquamarine = new Colour(0x7FFFD4);
        public static readonly Colour Azure = new Colour(0xF0FFFF);
        public static readonly Colour Beige = new Colour(0xF5F5DC);
        public static readonly Colour Bisque = new Colour(0xFFE4C4);
        public static readonly Colour BlanchedAlmond = new Colour(0xFFEBCD);
        public static readonly Colour BlueViolet = new Colour(0x8A2BE2);
        public static readonly Colour Brown = new Colour(0xA52A2A);
        public static readonly Colour Burlywood = new Colour(0xDEB887);
        public static readonly Colour CadetBlue = new Colour(0x5F9EA0);
        public static readonly Colour Chartreuse = new Colour(0x7FFF00);
        public static readonly Colour Chocolate = new Colour(0xD2691E);
        public static readonly Colour Coral = new Colour(0xFF7F50);
        public static readonly Colour CornflowerBlue = new Colour(0x6495ED);
        public static readonly Colour CornSilk = new Colour(0xFFF8DC);
        public static readonly Colour Crimson = new Colour(0xDC143C);
        public static readonly Colour DarkBlue = new Colour(0x00008B);
        public static readonly Colour DarkCyan = new Colour(0x008B8B);
        public static readonly Colour DarkGoldenrod = new Colour(0xB8860B);
        public static readonly Colour DarkGrey = new Colour(0xA9A9A9);
        public static readonly Colour DarkGreen = new Colour(0x006400);
        public static readonly Colour DarkKhaki = new Colour(0xBDB76B);
        public static readonly Colour DarkMagenta = new Colour(0x8B008B);
        public static readonly Colour DarkOliveGreen = new Colour(0x556B2F);
        public static readonly Colour DarkOrange = new Colour(0xFF8C00);
        public static readonly Colour DarkOrchid = new Colour(0x9932CC);
        public static readonly Colour DarkRed = new Colour(0x8B0000);
        public static readonly Colour DarkSalmon = new Colour(0xE9967A);
        public static readonly Colour DarkSeaGreen = new Colour(0x8FBC8F);
        public static readonly Colour DarkSlateBlue = new Colour(0x483D8B);
        public static readonly Colour DarkSlateGrey = new Colour(0x2F4F4F);
        public static readonly Colour DarkTurquoise = new Colour(0x00CED1);
        public static readonly Colour DarkViolet = new Colour(0x9400D3);
        public static readonly Colour DeepPink = new Colour(0xFF1493);
        public static readonly Colour DeepSkyBlue = new Colour(0x00BFFF);
        public static readonly Colour DimGrey = new Colour(0x696969);
        public static readonly Colour DodgerBlue = new Colour(0x1E90FF);
        public static readonly Colour Firebrick = new Colour(0xB22222);
        public static readonly Colour FloralWhite = new Colour(0xFFFAF0);
        public static readonly Colour ForestGreen = new Colour(0x228B22);
        public static readonly Colour Gainsboro = new Colour(0xDCDCDC);
        public static readonly Colour GhostWhite = new Colour(0xF8F8FF);
        public static readonly Colour Gold = new Colour(0xFFD700);
        public static readonly Colour Goldenrod = new Colour(0xDAA520);
        public static readonly Colour GreenYellow = new Colour(0xADFF2F);
        public static readonly Colour Honeydew = new Colour(0xF0FFF0);
        public static readonly Colour HotPink = new Colour(0xFF69B4);
        public static readonly Colour IndianRed = new Colour(0xCD5C5C);
        public static readonly Colour Indigo = new Colour(0x4B0082);
        public static readonly Colour Ivory = new Colour(0xFFFFF0);
        public static readonly Colour Khaki = new Colour(0xF0E68C);
        public static readonly Colour Lavender = new Colour(0xE6E6FA);
        public static readonly Colour LavenderBlush = new Colour(0xFFF0F5);
        public static readonly Colour LawnGreen = new Colour(0x7CFC00);
        public static readonly Colour LemonChiffon = new Colour(0xFFFACD);
        public static readonly Colour LightBlue = new Colour(0xADD8E6);
        public static readonly Colour LightCoral = new Colour(0xF08080);
        public static readonly Colour LightCyan = new Colour(0xE0FFFF);
        public static readonly Colour LightGoldenrodYellow = new Colour(0xFAFAD2);
        public static readonly Colour LightGrey = new Colour(0xD3D3D3);
        public static readonly Colour LightGreen = new Colour(0x90EE90);
        public static readonly Colour LightPink = new Colour(0xFFB6C1);
        public static readonly Colour LightSalmon = new Colour(0xFFA07A);
        public static readonly Colour LightSeaGreen = new Colour(0x20B2AA);
        public static readonly Colour LightSkyBlue = new Colour(0x87CEFA);
        public static readonly Colour LightSlateGrey = new Colour(0x778899);
        public static readonly Colour LightSteelBlue = new Colour(0xB0C4DE);
        public static readonly Colour LightYellow = new Colour(0xFFFFE0);
        public static readonly Colour LimeGreen = new Colour(0x32CD32);
        public static readonly Colour Linen = new Colour(0xFAF0E6);
        public static readonly Colour MediumAquamarine = new Colour(0x66CDAA);
        public static readonly Colour MediumBlue = new Colour(0x0000CD);
        public static readonly Colour MediumOrchid = new Colour(0xBA55D3);
        public static readonly Colour MediumPurple = new Colour(0x9370DB);
        public static readonly Colour MediumSeaGreen = new Colour(0x3CB371);
        public static readonly Colour MediumSlateBlue = new Colour(0x7B68EE);
        public static readonly Colour MediumSpringGreen = new Colour(0x00FA9A);
        public static readonly Colour MediumTurquoise = new Colour(0x48D1CC);
        public static readonly Colour MediumVioletRed = new Colour(0xC71585);
        public static readonly Colour MidnightBlue = new Colour(0x191970);
        public static readonly Colour MintCream = new Colour(0xF5FFFA);
        public static readonly Colour MistyRose = new Colour(0xFFE4E1);
        public static readonly Colour Moccasin = new Colour(0xFFE4B5);
        public static readonly Colour NavajoWhite = new Colour(0xFFDEAD);
        public static readonly Colour OldLace = new Colour(0xFDF5E6);
        public static readonly Colour OliveDrab = new Colour(0x6B8E23);
        public static readonly Colour OrangeRed = new Colour(0xFF4500);
        public static readonly Colour Orchid = new Colour(0xDA70D6);
        public static readonly Colour PaleGoldenrod = new Colour(0xEEE8AA);
        public static readonly Colour PaleGreen = new Colour(0x98FB98);
        public static readonly Colour PaleTurquoise = new Colour(0xAFEEEE);
        public static readonly Colour PaleVioletRed = new Colour(0xDB7093);
        public static readonly Colour PapayaWhip = new Colour(0xFFEFD5);
        public static readonly Colour PeachPuff = new Colour(0xFFDAB9);
        public static readonly Colour Peru = new Colour(0xCD853F);
        public static readonly Colour Pink = new Colour(0xFFC0CD);
        public static readonly Colour Plum = new Colour(0xDDA0DD);
        public static readonly Colour PowderBlue = new Colour(0xB0E0E6);
        public static readonly Colour RosyBrown = new Colour(0xBC8F8F);
        public static readonly Colour RoyalBlue = new Colour(0x4169E1);
        public static readonly Colour SaddleBrown = new Colour(0x8B4513);
        public static readonly Colour Salmon = new Colour(0xFA8072);
        public static readonly Colour SandyBrown = new Colour(0xF4A460);
        public static readonly Colour SeaGreen = new Colour(0x2E8B57);
        public static readonly Colour Seashell = new Colour(0xFFF5EE);
        public static readonly Colour Sienna = new Colour(0xA0522D);
        public static readonly Colour SkyBlue = new Colour(0x87CEEB);
        public static readonly Colour SlateBlue = new Colour(0x6A5ACD);
        public static readonly Colour SlateGrey = new Colour(0x708090);
        public static readonly Colour Snow = new Colour(0xFFFAFA);
        public static readonly Colour SpringGreen = new Colour(0x00FF7F);
        public static readonly Colour SteelBlue = new Colour(0x4682B4);
        public static readonly Colour Tan = new Colour(0xD2B48C);
        public static readonly Colour Thistle = new Colour(0xD8BFD8);
        public static readonly Colour Tomato = new Colour(0xFF6347);
        public static readonly Colour Turquoise = new Colour(0x40E0D0);
        public static readonly Colour Violet = new Colour(0xEE82EE);
        public static readonly Colour Wheat = new Colour(0xF5DEB3);
        public static readonly Colour WhiteSmoke = new Colour(0xF5F5F5);
        public static readonly Colour YellowGreen = new Colour(0x9ACD32);
        public static readonly Colour RebeccaPurple = new Colour(0x663399);
        public static readonly Colour Inherit = new Colour(INHERIT);

        public int Raw { get; }

        public Colour(int argb) {
            Raw = argb;
        }

        public static implicit operator Colour(int argb) => new Colour(argb);
        public static implicit operator Colour(string str) => Parse(str);

        public bool Inherits => (Raw & INHERIT) > 0;
        public int R => (Raw >> 16) & 0xFF;
        public int G => (Raw >> 8) & 0xFF;
        public int B => Raw & 0xFF;

        public string CSS {
            get {
                if(Inherits)
                    return @"inherit";
                return string.Format(@"#{0:X6}", Raw);
            }
        }

        public Color ToDrawingColor()
            => Color.FromArgb(Raw & 0xFFFFFF);

        public override string ToString() {
            return CSS;
        }

        public static Colour Parse(string css) {
            // todo: move all creations to static readonly's
            switch(css.Trim().ToLowerInvariant()) {
                case @"inherit":
                    return Inherit;

                #region CSS Colours

                // CSS Level 1
                case @"black":
                    return Black;
                case @"silver":
                    return Silver;
                case @"gray":
                case @"grey": // CSS Level 3
                    return Grey;
                case @"white":
                    return White;
                case @"maroon":
                    return Maroon;
                case @"red":
                    return Red;
                case @"purple":
                    return Purple;
                case @"fuchsia":
                case @"magenta": // CSS Level 3
                    return Magenta;
                case @"green":
                    return Green;
                case @"lime":
                    return Lime;
                case @"olive":
                    return Olive;
                case @"yellow":
                    return Yellow;
                case @"navy":
                    return Navy;
                case @"blue":
                    return Blue;
                case @"teal":
                    return Teal;
                case @"aqua":
                case @"cyan": // CSS Level 3
                    return Cyan;

                // CSS Level 2
                case @"orange":
                    return Orange;

                // CSS Level 3
                case @"aliceblue":
                    return AliceBlue;
                case @"antiquewhite":
                    return AntiqueWhite;
                case @"aquamarine":
                    return Aquamarine;
                case @"azure":
                    return Azure;
                case @"beige":
                    return Beige;
                case @"bisque":
                    return Bisque;
                case @"blanchedalmond":
                    return BlanchedAlmond;
                case @"blueviolet":
                    return BlueViolet;
                case @"brown":
                    return Brown;
                case @"burlywood":
                    return Burlywood;
                case @"cadetblue":
                    return CadetBlue;
                case @"chartreuse":
                    return Chartreuse;
                case @"chocolate":
                    return Chocolate;
                case @"coral":
                    return Coral;
                case @"cornflowerblue":
                    return CornflowerBlue;
                case @"cornsilk":
                    return CornSilk;
                case @"crimson":
                    return Crimson;
                case @"darkblue":
                    return DarkBlue;
                case @"darkcyan":
                    return DarkCyan;
                case @"darkgoldenrod":
                    return DarkGoldenrod;
                case @"darkgrey":
                case @"darkgray":
                    return DarkGrey;
                case @"darkgreen":
                    return DarkGreen;
                case @"darkkhaki":
                    return DarkKhaki;
                case @"darkmagenta":
                    return DarkMagenta;
                case @"darkolivegreen":
                    return DarkOliveGreen;
                case @"darkorange":
                    return DarkOrange;
                case @"darkorchid":
                    return DarkOrchid;
                case @"darkred":
                    return DarkRed;
                case @"darksalmon":
                    return DarkSalmon;
                case @"darkseagreen":
                    return DarkSeaGreen;
                case @"darkslateblue":
                    return DarkSlateBlue;
                case @"darkslategrey":
                case @"darkslategray":
                    return DarkSlateGrey;
                case @"darkturquoise":
                    return DarkTurquoise;
                case @"darkviolet":
                    return DarkViolet;
                case @"deeppink":
                    return DeepPink;
                case @"deepskyblue":
                    return DeepSkyBlue;
                case @"dimgray":
                case @"dimgrey":
                    return DimGrey;
                case @"dodgerblue":
                    return DodgerBlue;
                case @"firebrick":
                    return Firebrick;
                case @"floralwhite":
                    return FloralWhite;
                case @"forestgreen":
                    return ForestGreen;
                case @"gainsboro":
                    return Gainsboro;
                case @"ghostwhite":
                    return GhostWhite;
                case @"gold":
                    return Gold;
                case @"goldenrod":
                    return Goldenrod;
                case @"greenyellow":
                    return GreenYellow;
                case @"honeydew":
                    return Honeydew;
                case @"hotpink":
                    return HotPink;
                case @"indianred":
                    return IndianRed;
                case @"indigo":
                    return Indigo;
                case @"ivory":
                    return Ivory;
                case @"khaki":
                    return Khaki;
                case @"lavender":
                    return Lavender;
                case @"lavenderblush":
                    return LavenderBlush;
                case @"lawngreen":
                    return LawnGreen;
                case @"lemonchiffon":
                    return LemonChiffon;
                case @"lightblue":
                    return LightBlue;
                case @"lightcoral":
                    return LightCoral;
                case @"lightcyan":
                    return LightCyan;
                case @"lightgoldenrodyellow":
                    return LightGoldenrodYellow;
                case @"lightgray":
                case @"lightgrey":
                    return LightGrey;
                case @"lightgreen":
                    return LightGreen;
                case @"lightpink":
                    return LightPink;
                case @"lightsalmon":
                    return LightSalmon;
                case @"lightseagreen":
                    return LightSeaGreen;
                case @"lightskyblue":
                    return LightSkyBlue;
                case @"lightslategray":
                case @"lightslategrey":
                    return LightSlateGrey;
                case @"lightsteelblue":
                    return LightSteelBlue;
                case @"lightyellow":
                    return LightYellow;
                case @"limegreen":
                    return LimeGreen;
                case @"linen":
                    return Linen;
                case @"mediumaquamarine":
                    return MediumAquamarine;
                case @"mediumblue":
                    return MediumBlue;
                case @"mediumorchid":
                    return MediumOrchid;
                case @"mediumpurple":
                    return MediumPurple;
                case @"mediumseagreen":
                    return MediumSeaGreen;
                case @"mediumslateblue":
                    return MediumSlateBlue;
                case @"mediumspringgreen":
                    return MediumSpringGreen;
                case @"mediumturquoise":
                    return MediumTurquoise;
                case @"mediumvioletred":
                    return MediumVioletRed;
                case @"midnightblue":
                    return MidnightBlue;
                case @"mintcream":
                    return MintCream;
                case @"mistyrose":
                    return MistyRose;
                case @"moccasin":
                    return Moccasin;
                case @"navajowhite":
                    return NavajoWhite;
                case @"oldlace":
                    return OldLace;
                case @"olivedrab":
                    return OliveDrab;
                case @"orangered":
                    return OrangeRed;
                case @"orchid":
                    return Orchid;
                case @"palegoldenrod":
                    return PaleGoldenrod;
                case @"palegreen":
                    return PaleGreen;
                case @"paleturquoise":
                    return PaleTurquoise;
                case @"palevioletred":
                    return PaleVioletRed;
                case @"papayawhip":
                    return PapayaWhip;
                case @"peachpuff":
                    return PeachPuff;
                case @"peru":
                    return Peru;
                case @"pink":
                    return Pink;
                case @"plum":
                    return Plum;
                case @"powderblue":
                    return PowderBlue;
                case @"rosybrown":
                    return RosyBrown;
                case @"royalblue":
                    return RoyalBlue;
                case @"saddlebrown":
                    return SaddleBrown;
                case @"salmon":
                    return Salmon;
                case @"sandybrown":
                    return SandyBrown;
                case @"seagreen":
                    return SeaGreen;
                case @"seashell":
                    return Seashell;
                case @"sienna":
                    return Sienna;
                case @"skyblue":
                    return SkyBlue;
                case @"slateblue":
                    return SlateBlue;
                case @"slategray":
                case @"slategrey":
                    return SlateGrey;
                case @"snow":
                    return Snow;
                case @"springgreen":
                    return SpringGreen;
                case @"steelblue":
                    return SteelBlue;
                case @"tan":
                    return Tan;
                case @"thistle":
                    return Thistle;
                case @"tomato":
                    return Tomato;
                case @"turquoise":
                    return Turquoise;
                case @"violet":
                    return Violet;
                case @"wheat":
                    return Wheat;
                case @"whitesmoke":
                    return WhiteSmoke;
                case @"yellowgreen":
                    return YellowGreen;

                // CSS Level 4
                case @"rebeccapurple":
                    return RebeccaPurple;
                    #endregion
            }

            int raw = 0;
            Match hex6Match = Regex.Match(css, @"#([0-9A-Fa-f]{2})([0-9A-Fa-f]{2})([0-9A-Fa-f]{2})");

            if(hex6Match.Success) {
                if(byte.TryParse(hex6Match.Groups[1].Value, NumberStyles.HexNumber, null, out byte red))
                    raw |= red << 16;
                if(byte.TryParse(hex6Match.Groups[2].Value, NumberStyles.HexNumber, null, out byte green))
                    raw |= green << 8;
                if(byte.TryParse(hex6Match.Groups[3].Value, NumberStyles.HexNumber, null, out byte blue))
                    raw |= blue;
                return new Colour(raw);
            }

            Match hex3Match = Regex.Match(css, @"#([0-9A-Fa-f])([0-9A-Fa-f])([0-9A-Fa-f])");

            if(hex3Match.Success) {
                if(byte.TryParse(hex3Match.Groups[1].Value + hex3Match.Groups[1].Value, NumberStyles.HexNumber, null, out byte red))
                    raw |= red << 16;
                if(byte.TryParse(hex3Match.Groups[2].Value + hex3Match.Groups[2].Value, NumberStyles.HexNumber, null, out byte green))
                    raw |= green << 8;
                if(byte.TryParse(hex3Match.Groups[3].Value + hex3Match.Groups[3].Value, NumberStyles.HexNumber, null, out byte blue))
                    raw |= blue;
                return new Colour(raw);
            }

            Match rgbMatch = Regex.Match(css, @"rgba?\((?:[\w\s]+)?([0-9]{1,3})(?:[\w\s]+)?,(?:[\w\s]+)?([0-9]{1,3})(?:[\w\s]+)?,(?:[\w\s]+)?([0-9]{1,3})(?:[\w\s]+)(?:,(?:[\w\s]+)([0-9])?\.?([0-9])?(?:[\w\s]+))?\);?");

            if(rgbMatch.Success) {
                if(byte.TryParse(rgbMatch.Groups[1].Value, out byte red))
                    raw |= red << 16;
                if(byte.TryParse(rgbMatch.Groups[2].Value, out byte green))
                    raw |= green << 8;
                if(byte.TryParse(rgbMatch.Groups[3].Value, out byte blue))
                    raw |= blue;
                return new Colour(raw);
            }

            return Inherit;
        }
    }
}
