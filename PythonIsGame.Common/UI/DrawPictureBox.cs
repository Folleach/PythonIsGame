using System.Drawing;
using System.Windows.Forms;

namespace PythonIsGame.Common.UI
{
    public class DrawPictureBox : PictureBox
    {
        public DrawPictureBox()
        {
            DoubleBuffered = true;
            BackColor = Color.Transparent;
        }
    }
}
