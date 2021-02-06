using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MicroSquid {
    public readonly struct Colour {
        private const int INHERIT = 0x40000000;

        public int Raw { get; }

        public static readonly Colour Black = new Colour(0x000000);
        public static readonly Colour Inherit = new Colour(INHERIT);

        public Colour(int argb) {
            Raw = argb;
        }

        public static implicit operator Colour(int argb) => new Colour(argb);
        public static implicit operator Colour(string str) => Parse(str);

        public bool Inherits => (Raw & INHERIT) > 0;
        public int Red => (Raw >> 16) & 0xFF;
        public int Green => (Raw >> 8) & 0xFF;
        public int Blue => Raw & 0xFF;

        public string CSS {
            get {
                if(Inherits)
                    return @"inherit";
                return string.Format(@"#{0:X6}", Raw);
            }
        }

        public Color ToDrawingColor()
            => Color.FromArgb(Raw & 0xFFFFFF);

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
                    return new Colour(0xC0C0C0);
                case @"gray":
                case @"grey": // CSS Level 3
                    return new Colour(0x808080);
                case @"white":
                    return new Colour(0xFFFFFF);
                case @"maroon":
                    return new Colour(0x800000);
                case @"red":
                    return new Colour(0xFF0000);
                case @"purple":
                    return new Colour(0x800080);
                case @"fuchsia":
                case @"magenta": // CSS Level 3
                    return new Colour(0xFF00FF);
                case @"green":
                    return new Colour(0x008000);
                case @"lime":
                    return new Colour(0x00FF00);
                case @"olive":
                    return new Colour(0x808000);
                case @"yellow":
                    return new Colour(0xFFFF00);
                case @"navy":
                    return new Colour(0x000080);
                case @"blue":
                    return new Colour(0x0000FF);
                case @"teal":
                    return new Colour(0x008080);
                case @"aqua":
                case @"cyan": // CSS Level 3
                    return new Colour(0x00FFFF);

                // CSS Level 2
                case @"orange":
                    return new Colour(0xFFA500);

                // CSS Level 3
                case @"aliceblue":
                    return new Colour(0xF0F8FF);
                case @"antiquewhite":
                    return new Colour(0xFAEBD7);
                case @"aquamarine":
                    return new Colour(0x7FFFD4);
                case @"azure":
                    return new Colour(0xF0FFFF);
                case @"beige":
                    return new Colour(0xF5F5DC);
                case @"bisque":
                    return new Colour(0xFFE4C4);
                case @"blanchedalmond":
                    return new Colour(0xFFEBCD);
                case @"blueviolet":
                    return new Colour(0x8A2BE2);
                case @"brown":
                    return new Colour(0xA52A2A);
                case @"burlywood":
                    return new Colour(0xDEB887);
                case @"cadetblue":
                    return new Colour(0x5F9EA0);
                case @"chartreuse":
                    return new Colour(0x7FFF00);
                case @"chocolate":
                    return new Colour(0xD2691E);
                case @"coral":
                    return new Colour(0xFF7F50);
                case @"cornflowerblue":
                    return new Colour(0x6495ED);
                case @"cornsilk":
                    return new Colour(0xFFF8DC);
                case @"crimson":
                    return new Colour(0xDC143C);
                case @"darkblue":
                    return new Colour(0x00008B);
                case @"darkcyan":
                    return new Colour(0x008B8B);
                case @"darkgoldenrod":
                    return new Colour(0xB8860B);
                case @"darkgrey":
                case @"darkgray":
                    return new Colour(0xA9A9A9);
                case @"darkgreen":
                    return new Colour(0x006400);
                case @"darkkhaki":
                    return new Colour(0xBDB76B);
                case @"darkmagenta":
                    return new Colour(0x8B008B);
                case @"darkolivegreen":
                    return new Colour(0x556B2F);
                case @"darkorange":
                    return new Colour(0xFF8C00);
                case @"darkorchid":
                    return new Colour(0x9932CC);
                case @"darkred":
                    return new Colour(0x8B0000);
                case @"darksalmon":
                    return new Colour(0xE9967A);
                case @"darkseagreen":
                    return new Colour(0x8FBC8F);
                case @"darkslateblue":
                    return new Colour(0x483D8B);
                case @"darkslategrey":
                case @"darkslategray":
                    return new Colour(0x2F4F4F);
                case @"darkturquoise":
                    return new Colour(0x00CED1);
                case @"darkviolet":
                    return new Colour(0x9400D3);
                case @"deeppink":
                    return new Colour(0xFF1493);
                case @"deepskyblue":
                    return new Colour(0x00BFFF);
                case @"dimgray":
                case @"dimgrey":
                    return new Colour(0x696969);
                case @"dodgerblue":
                    return new Colour(0x1E90FF);
                case @"firebrick":
                    return new Colour(0xB22222);
                case @"floralwhite":
                    return new Colour(0xFFFAF0);
                case @"forestgreen":
                    return new Colour(0x228B22);
                case @"gainsboro":
                    return new Colour(0xDCDCDC);
                case @"ghostwhite":
                    return new Colour(0xF8F8FF);
                case @"gold":
                    return new Colour(0xFFD700);
                case @"goldenrod":
                    return new Colour(0xDAA520);
                case @"greenyellow":
                    return new Colour(0xADFF2F);
                case @"honeydew":
                    return new Colour(0xF0FFF0);
                case @"hotpink":
                    return new Colour(0xFF69B4);
                case @"indianred":
                    return new Colour(0xCD5C5C);
                case @"indigo":
                    return new Colour(0x4B0082);
                case @"ivory":
                    return new Colour(0xFFFFF0);
                case @"khaki":
                    return new Colour(0xF0E68C);
                case @"lavender":
                    return new Colour(0xE6E6FA);
                case @"lavenderblush":
                    return new Colour(0xFFF0F5);
                case @"lawngreen":
                    return new Colour(0x7CFC00);
                case @"lemonchiffon":
                    return new Colour(0xFFFACD);
                case @"lightblue":
                    return new Colour(0xADD8E6);
                case @"lightcoral":
                    return new Colour(0xF08080);
                case @"lightcyan":
                    return new Colour(0xE0FFFF);
                case @"lightgoldenrodyellow":
                    return new Colour(0xFAFAD2);
                case @"lightgray":
                case @"lightgrey":
                    return new Colour(0xD3D3D3);
                case @"lightgreen":
                    return new Colour(0x90EE90);
                case @"lightpink":
                    return new Colour(0xFFB6C1);
                case @"lightsalmon":
                    return new Colour(0xFFA07A);
                case @"lightseagreen":
                    return new Colour(0x20B2AA);
                case @"lightskyblue":
                    return new Colour(0x87CEFA);
                case @"lightslategray":
                case @"lightslategrey":
                    return new Colour(0x778899);
                case @"lightsteelblue":
                    return new Colour(0xB0C4DE);
                case @"lightyellow":
                    return new Colour(0xFFFFE0);
                case @"limegreen":
                    return new Colour(0x32CD32);
                case @"linen":
                    return new Colour(0xFAF0E6);
                case @"mediumaquamarine":
                    return new Colour(0x66CDAA);
                case @"mediumblue":
                    return new Colour(0x0000CD);
                case @"mediumorchid":
                    return new Colour(0xBA55D3);
                case @"mediumpurple":
                    return new Colour(0x9370DB);
                case @"mediumseagreen":
                    return new Colour(0x3CB371);
                case @"mediumslateblue":
                    return new Colour(0x7B68EE);
                case @"mediumspringgreen":
                    return new Colour(0x00FA9A);
                case @"mediumturquoise":
                    return new Colour(0x48D1CC);
                case @"mediumvioletred":
                    return new Colour(0xC71585);
                case @"midnightblue":
                    return new Colour(0x191970);
                case @"mintcream":
                    return new Colour(0xF5FFFA);
                case @"mistyrose":
                    return new Colour(0xFFE4E1);
                case @"moccasin":
                    return new Colour(0xFFE4B5);
                case @"navajowhite":
                    return new Colour(0xFFDEAD);
                case @"oldlace":
                    return new Colour(0xFDF5E6);
                case @"olivedrab":
                    return new Colour(0x6B8E23);
                case @"orangered":
                    return new Colour(0xFF4500);
                case @"orchid":
                    return new Colour(0xDA70D6);
                case @"palegoldenrod":
                    return new Colour(0xEEE8AA);
                case @"palegreen":
                    return new Colour(0x98FB98);
                case @"paleturquoise":
                    return new Colour(0xAFEEEE);
                case @"palevioletred":
                    return new Colour(0xDB7093);
                case @"papayawhip":
                    return new Colour(0xFFEFD5);
                case @"peachpuff":
                    return new Colour(0xFFDAB9);
                case @"peru":
                    return new Colour(0xCD853F);
                case @"pink":
                    return new Colour(0xFFC0CD);
                case @"plum":
                    return new Colour(0xDDA0DD);
                case @"powderblue":
                    return new Colour(0xB0E0E6);
                case @"rosybrown":
                    return new Colour(0xBC8F8F);
                case @"royalblue":
                    return new Colour(0x4169E1);
                case @"saddlebrown":
                    return new Colour(0x8B4513);
                case @"salmon":
                    return new Colour(0xFA8072);
                case @"sandybrown":
                    return new Colour(0xF4A460);
                case @"seagreen":
                    return new Colour(0x2E8B57);
                case @"seashell":
                    return new Colour(0xFFF5EE);
                case @"sienna":
                    return new Colour(0xA0522D);
                case @"skyblue":
                    return new Colour(0x87CEEB);
                case @"slateblue":
                    return new Colour(0x6A5ACD);
                case @"slategray":
                case @"slategrey":
                    return new Colour(0x708090);
                case @"snow":
                    return new Colour(0xFFFAFA);
                case @"springgreen":
                    return new Colour(0x00FF7F);
                case @"steelblue":
                    return new Colour(0x4682B4);
                case @"tan":
                    return new Colour(0xD2B48C);
                case @"thistle":
                    return new Colour(0xD8BFD8);
                case @"tomato":
                    return new Colour(0xFF6347);
                case @"turquoise":
                    return new Colour(0x40E0D0);
                case @"violet":
                    return new Colour(0xEE82EE);
                case @"wheat":
                    return new Colour(0xF5DEB3);
                case @"whitesmoke":
                    return new Colour(0xF5F5F5);
                case @"yellowgreen":
                    return new Colour(0x9ACD32);

                // CSS Level 4
                case @"rebeccapurple":
                    return new Colour(0x663399);
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
