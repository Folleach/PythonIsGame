using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PythonIsGame.Common.UI
{
    public class DrawLabel : Label
    {
        public DrawLabel(int fontSize, bool translucentBackground)
        {
            BackColor = Color.FromArgb(translucentBackground ? 50 : 0, 0, 0, 0);
            ForeColor = Color.White;
            Font = new Font(FontFamily.GenericMonospace, fontSize);
            DoubleBuffered = true;
        }
    }
}
