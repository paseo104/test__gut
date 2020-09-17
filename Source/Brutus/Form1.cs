using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brutus
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            var context =new Context();
            var board = new BrutusBoard(context);

            board.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            Text = "brutus";
            // this.Controls.Add(board);
            ClientSize = new Size(board.ClientSize.Width + 200, board.ClientSize.Height) ;
            var panel = new FlowLayoutPanel();
            panel.Size = ClientSize;
            this.Controls.Add(panel);

            panel.Controls.Add(board);
            var pnl2 = createPanel(board.ClientSize.Height,string.Empty);
            panel.Controls.Add(pnl2);
            pnl2.Controls.Add(createPanel(board.ClientSize.Height / 2, 
                context.Players[PlayerNo.One].PlayerName ));
            pnl2.Controls.Add(createPanel(board.ClientSize.Height / 2, 
                context.Players[PlayerNo.Two].PlayerName));

            TextBox tb = new TextBox();
            pnl2.Controls.Add(tb);
            tb.BringToFront();
            tb.AppendText("テキストボックスです");

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;


        }
        private Control createPanel(int height,string playerName)
        {
            var panel2 = new FlowLayoutPanel();
            panel2.Size = new Size(200, height);
            panel2.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            panel2.BackColor = System.Drawing.Color.Beige;
            // panel2.BackgroundImage = System.Drawing.Image.FromFile(@"C:\Users\EA020393\Pictures\b_ken.png");
            // panel2 画像の挿入

            //TextBox TextBox1 = new TextBox();
            //// パネルへテキストボックスを追加
            //panel2.Controls.Add(TextBox1);

            //// テキストボックスを最前面へ
            //TextBox1.BringToFront();
            //TextBox1.AppendText("テキストボックスです");



            if (!string.IsNullOrEmpty(playerName))
            {
                var lablel1 = new Label();
                lablel1.Text = playerName;
                panel2.Controls.Add(lablel1);
            }
            return panel2;

        }


    }
}
