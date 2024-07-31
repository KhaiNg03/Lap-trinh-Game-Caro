using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameCaro
{
    public partial class Form1 : Form
    {
        #region Properties
        ChessBoardManager ChessBoard;
        SocketManager socket;
        #endregion
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            ChessBoard = new ChessBoardManager(pnlChessBoard, txbPlayerName, pctbMark);
            ChessBoard.EndedGame += ChessBoard_EndedGame;
            ChessBoard.PlayerMarked += ChessBoard_PlayerMarked;

            prcbCoolDown.Step = Cons.COOL_DOWN_STEP;
            prcbCoolDown.Maximum = Cons.COOL_DOWN_TIME;
            prcbCoolDown.Value = Cons.COOL_DOWN_INTERVAL;
            timerCoolDown.Interval = Cons.COOL_DOWN_INTERVAL;
            socket = new SocketManager();
            timerCoolDown.Start();

            NewGame();
        }
        #region Methods
        void EndGame()
        {
            timerCoolDown.Stop();
            pnlChessBoard.Enabled = false;
            undoToolStripMenuItem.Enabled = false;
            MessageBox.Show("Kết thúc Game!");
        }
        void NewGame()
        {
            prcbCoolDown.Value = 0;
            timerCoolDown.Stop();
            undoToolStripMenuItem.Enabled = true;

            ChessBoard.DrawChessBoard();
        }
        void Quit()
        {
            Application.Exit();
        }
        void Undo()
        {
            ChessBoard.Undo();
        }
        private void ChessBoard_PlayerMarked(object sender, ButtonClickEvent e)
        {
            timerCoolDown.Start();
            pnlChessBoard.Enabled = false;
            prcbCoolDown.Value = 0;

            socket.Send(new SocketData((int)SocketCommand.SEND_POINT, "", e.ClickedPoint));
            
            Listen();
        }

        private void ChessBoard_EndedGame(object sender, EventArgs e)
        {
            EndGame();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pctbAvatar_Click(object sender, EventArgs e)
        {

        }

        private void txbPlayerName_TextChanged(object sender, EventArgs e)
        {

        }

        private void pnlChessBoard_Paint(object sender, PaintEventArgs e)
        {

        }

        private void timerCoolDown_Tick(object sender, EventArgs e)
        {
            prcbCoolDown.PerformStep();

            if (prcbCoolDown.Value >= prcbCoolDown.Maximum)
            {
                EndGame() ;
                
            }
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void quiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Quit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn thoát", "Thông báo", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
                e.Cancel = true;
        }

        private void bntLAN_Click(object sender, EventArgs e)
        {
            socket.IP = txbIP.Text;

            if (!socket.ConnectSever())
            {
                socket.isSever = true;
                pnlChessBoard.Enabled = true;
                socket.CreateServer();
            }
            else
            {
                socket.isSever = false;
                pnlChessBoard.Enabled = false;
                Listen();

            }

        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            txbIP.Text = socket.GetLocalIPv4(NetworkInterfaceType.Wireless80211);

            if (string.IsNullOrEmpty(txbIP.Text))
            {
                txbIP.Text = socket.GetLocalIPv4(NetworkInterfaceType.Ethernet);
            }
        }
        void Listen()
        {
            Thread listenThread = new Thread(() =>
            {
                try
                {
                    SocketData data = (SocketData)socket.Receive();
                    ProcessData(data);
                }
                catch( Exception e)
                {
                }
            });

            listenThread.IsBackground = true;
            listenThread.Start();
        }

        private void ProcessData(SocketData data)
        {
            switch (data.Command)
            {
                case (int)SocketCommand.NOTIFY:
                    MessageBox.Show(data.Message);
                    break;

                case (int)SocketCommand.NEW_GAME:
                    break;

                case (int)SocketCommand.SEND_POINT:
                    this.Invoke((MethodInvoker)(() =>
                    {
                        prcbCoolDown.Value = 0;
                        pnlChessBoard.Enabled = true;
                        timerCoolDown.Start();
                        ChessBoard.OtherPlayerMark(data.Point);
                    }));



                    break;

                case (int)SocketCommand.UNDO:
                    break;
                case (int)SocketCommand.END_GAME:
                    break;

                case (int)SocketCommand.QUIT:
                    break;

                default: 
                    break;
            }
            Listen();
        }

        #endregion


    }
}
