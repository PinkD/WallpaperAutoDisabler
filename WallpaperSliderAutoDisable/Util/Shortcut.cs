using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WallpaperSliderAutoDisable.util {
    public class Shortcut {
        public ModifierKeys MK;
        public Keys K;

        public Shortcut(ModifierKeys modifierKeys = ModifierKeys.None, Keys key = Keys.None) {
            MK = modifierKeys;
            K = key;
        }

        public bool Valid() {
            return MK != ModifierKeys.None && K != Keys.None;
        }

        public static Shortcut FromString(string str) {
            if (str == null) {
                return new Shortcut();
            }

            var keys = str.Split('+');
            if (keys.Length < 2) {
                return new Shortcut();
            }

            var keyStr = keys.Last();
            var key = keyFromString(keyStr);
            var modifierKeys = ModifierKeys.None;
            for (var i = 0; i < keys.Length - 1; i++) {
                modifierKeys |= modifierKeyFromString(keys[i]);
            }

            return new Shortcut(modifierKeys, key);
        }

        public override string ToString() {
            var key = K.ToString();
            var keys = new LinkedList<string>();
            var mks = new[] { ModifierKeys.Control, ModifierKeys.Alt, ModifierKeys.Win, ModifierKeys.Shift };
            foreach (var mk in mks) {
                if ((MK & mk) == mk) {
                    keys.AddLast(mk.ToString());
                }
            }

            keys.AddLast(key);
            return string.Join("+", keys.ToArray());
        }

        private static Keys keyFromString(string keyStr) {
            switch (keyStr) {
                case "a":
                case "A":
                    return Keys.A;
                case "b":
                case "B":
                    return Keys.B;
                case "c":
                case "C":
                    return Keys.C;
                case "d":
                case "D":
                    return Keys.D;
                case "e":
                case "E":
                    return Keys.E;
                case "f":
                case "F":
                    return Keys.F;
                case "g":
                case "G":
                    return Keys.G;
                case "h":
                case "H":
                    return Keys.H;
                case "i":
                case "I":
                    return Keys.I;
                case "j":
                case "J":
                    return Keys.J;
                case "k":
                case "K":
                    return Keys.K;
                case "l":
                case "L":
                    return Keys.L;
                case "m":
                case "M":
                    return Keys.M;
                case "n":
                case "N":
                    return Keys.N;
                case "o":
                case "O":
                    return Keys.O;
                case "p":
                case "P":
                    return Keys.P;
                case "q":
                case "Q":
                    return Keys.Q;
                case "r":
                case "R":
                    return Keys.R;
                case "s":
                case "S":
                    return Keys.S;
                case "t":
                case "T":
                    return Keys.T;
                case "u":
                case "U":
                    return Keys.U;
                case "v":
                case "V":
                    return Keys.V;
                case "w":
                case "W":
                    return Keys.W;
                case "x":
                case "X":
                    return Keys.X;
                case "y":
                case "Y":
                    return Keys.Y;
                case "z":
                case "Z":
                    return Keys.Z;
                default:
                    return Keys.None;
            }
        }

        private static ModifierKeys modifierKeyFromString(string keyStr) {
            switch (keyStr) {
                case "ctrl":
                case "control":
                case "CTRL":
                case "CONTROL":
                case "Ctrl":
                case "Control":
                    return ModifierKeys.Control;
                case "alt":
                case "Alt":
                case "ALT":
                    return ModifierKeys.Alt;
                case "shift":
                case "Shift":
                case "SHIFT":
                    return ModifierKeys.Shift;
                case "win":
                case "Win":
                case "WIN":
                    return ModifierKeys.Win;
                default:
                    return ModifierKeys.None;
            }
        }
    }
}