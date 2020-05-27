using System.Windows.Controls;
using System.Windows.Media;

using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;

namespace MinecraftChunkBackup {
    public class NumericTextBox : TextBox {
        public Brush InvalidBackground { get; set; } = new SolidColorBrush(Color.FromRgb(255, 0, 0));

        public Brush InvalidForeground { get; set; } = new SolidColorBrush(Color.FromRgb(255, 255, 255));

        public bool Valid { get; private set; } = true;

        public int Value {
            set => Text = value.ToString();
            get {
                if (Valid)
                    return int.Parse(Text);
                else
                    return 0;
            }
        }

        Brush baseBackground, baseForeground;

        protected override void OnTextChanged(TextChangedEventArgs e) {
            base.OnTextChanged(e);
            bool stillValid = Text.Length != 0;
            if (stillValid && (Text[0] < '0' || Text[0] > '9'))
                stillValid = Text[0] == '-' && Text.Length > 1;
            if (stillValid) {
                for (int i = 1; i < Text.Length; ++i) {
                    if (Text[i] < '0' || Text[i] > '9') {
                        stillValid = false;
                        break;
                    }
                }
            }
            if (Valid && !stillValid) {
                baseBackground = Background;
                baseForeground = Foreground;
                Background = InvalidBackground;
                Foreground = InvalidForeground;
            } else if (!Valid && stillValid) {
                Background = baseBackground;
                Foreground = baseForeground;
            }
            Valid = stillValid;
        }
    }
}