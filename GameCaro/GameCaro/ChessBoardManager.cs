using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameCaro
{
    public class ChessBoardManager
    {
        #region Properties
        private Panel chessBoard;
        public Panel ChessBoard { get => chessBoard; set => chessBoard = value; }
        private List<Player> player;
        public List<Player> Player
        {
            get => player;
            set => player = value;
        }

        private int currentPlayer;

        private System.Windows.Forms.TextBox playerName;
        public System.Windows.Forms.TextBox PlayerName
        {
            get => playerName;
            set => playerName = value;
        }
        private PictureBox playerMark;
        public PictureBox PlayerMark
        {
            get => playerMark;
            set => playerMark = value;
        }
        public int CurrentPlayer { get => currentPlayer; set => currentPlayer = value; }
        #endregion

        #region Initialize
        public ChessBoardManager(Panel chessBoard, System.Windows.Forms.TextBox playerName, PictureBox mark)
        {
            this.ChessBoard = chessBoard;  // Assign the provided chessboard panel to the field
            this.PlayerName = playerName;
            this.playerMark = mark;

            this.Player = new List<Player>() // Day co viet them duoc thay doi ten nguoi choi
            {
                new Player("LamLam", Image.FromFile(Application.StartupPath + "\\Resources\\p1.jpg")),
                new Player("QuanKhai", Image.FromFile(Application.StartupPath + "\\Resources\\p2.jpg"))
            };

            CurrentPlayer = 0;
            ChangePlayer();

        }
        #endregion

        #region Methods
        public void DrawChessBoard()
        {
            Button oldButton = new Button() { Width = 0, Location = new Point(0, 0) };

            for (int i = 0; i < Cons.CHESS_BOARD_HEIGHT; i++)
            {
                for (int j = 0; j < Cons.CHESS_BOARD_WIDTH; j++)
                {
                    Button btn = new Button()
                    {
                        Width = Cons.CHESS_WIDTH,
                        Height = Cons.CHESS_HEIGHT,
                        Location = new Point(oldButton.Location.X + oldButton.Width, oldButton.Location.Y),
                        BackgroundImageLayout = ImageLayout.Stretch
                    };
                    btn.Click += btn_Click;
                    ChessBoard.Controls.Add(btn);  // Add button to panel
                    oldButton = btn;
                }
                oldButton.Location = new Point(0, oldButton.Location.Y + Cons.CHESS_HEIGHT);
                oldButton.Width = 0;
                oldButton.Height = 0;
            }
        }

        private void btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.BackgroundImage != null)
                return;
            Mark(btn);
            ChangePlayer();

        }

        private void ChangePlayer()
        {
            PlayerName.Text = Player[CurrentPlayer].Name;

            PlayerMark.Image = Player[CurrentPlayer].Mark;
        }

        private void Mark(Button btn)
        {
            btn.BackgroundImage = Player[CurrentPlayer].Mark;
            CurrentPlayer = CurrentPlayer == 1 ? 0 : 1;
        }

        #endregion
    }

}
