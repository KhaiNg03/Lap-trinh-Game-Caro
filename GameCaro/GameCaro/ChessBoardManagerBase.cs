using System;
using System.Drawing;
using System.Windows.Forms;

namespace GameCaro
{
    public class ChessBoardManagerBase
    {

        private void btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.BackgroundImage = Image.FromFile(Application.StartupPath + "\\Resources\\p1.jpg");

        }
    }
}