// BrutusBoard 
// 表示のロジック部分
// 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brutus
{
    enum ImageSize
    {
        Thirty,
        FortyFive,
        Seventy
    }

    partial class BrutusBoard : UserControl
    {
         BoardManager boardManager;

         ImageSize komaImageSize;

         int PictureSize = 0;
         // [Browsable(true), Category("ImageSize"), DefaultValue(ImageSize.FortyFive)]
         public ImageSize KomaImageSize
         {
             get { return komaImageSize; }
             set { komaImageSize = value; }
         }
        
        public BrutusBoard(Context context)
        {
            InitializeComponent();

            boardManager = new BoardManager(context);
            this.komaImageSize = context.Size;  
            //seventy
            //this.PictureSize = 70;
            this.PictureSize = getImageSize();
            ClientSize = new Size(PictureSize * 5, PictureSize * 7);
            createKoma();
            //　修正しました　ボードサイズ
        }

        private void createKoma()
        {
            boardManager.KomaCreate();
        }

        // 駒の描画
        private void drawKoma(Graphics graphics)
        {
            //var locations = boardManager.GetAliveKoma();
            
            foreach (var kom in boardManager.GetAliveKoma())
            {
                //MessageBox.Show(kom.Height +"の勝ち");
                graphics.DrawImage(new Bitmap( kom.MyPicture,PictureSize,PictureSize), PictureSize * kom.Left, PictureSize * kom.Height);
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);  // クリックイベントの監視
            //　盤面の色の描画
            // 画像を再描画するためにオーバーライド
            for (int y = 0; y <= 6; y++)  //　縦
            {
                for (int x = 0; x <= 4; x++)  //　横
                {
                    if ((x + y) % 2 == 0)
                    {
//                        e.Graphics.FillRectangle(Brushes.LightGray, x * PictureSize, y * PictureSize, PictureSize, PictureSize);
                        e.Graphics.FillRectangle(Brushes.LightGray, x * PictureSize, y * PictureSize, PictureSize, PictureSize);                        
                    }
                    else
                    {
                        e.Graphics.FillRectangle(Brushes.Gray, x * PictureSize, y * PictureSize, PictureSize, PictureSize);
                    }
                }
            }
            this.drawKoma(e.Graphics);
        }


        // マウス押されたときの挙動
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            int left = e.X / PictureSize;       //押した座標
            int height = e.Y / PictureSize;
            int[] DirectionNumberL =   { -1, -1, -1,  0, 0,  1, 1, 1 };
            int[] DirectionNumberH = { -1,  0,  1, -1, 1, -1, 0, 1 };


            if (boardManager.IsInsicateMovableLocation())
            {
                if (boardManager.IsSameKoma(left, height)) // 同じ駒がクリックされたとき　クリック状態の駒を再度クリック
                {
                    boardManager.ClearClickedKoma(); // クリック状態の解除？
                    base.Refresh(); // ハイライトの削除
                    return;
                }
                else
                {
                    if (boardManager.CanMove(left, height))
                    {
                        boardManager.SetLocation(left, height); //新しい位置の読み込み

                        /// 反転判断   //////////
                        //var llocation = boardManager.GetMovableLocation(); // 動かすことのできる位置
                        //MessageBox.Show("その２　　0 = " + llocation[0] + "1 = " +  llocation[1] + "2 = " + llocation[2] +  "3 = " + llocation[3] + "4 = " + llocation[4]);
                        //MessageBox.Show("left = " + left + " height = " + height);
                        //boardManager.LocationKomaKomaa();

                        int PointL, PointH = 0;

                        for (int i = 0; i < 7; i++)
                        {
                            PointL = left + DirectionNumberL[i];
                            PointH = height + DirectionNumberH[i];

                            if (boardManager.handan(PointL, PointH) == 1) //動かした駒から増減分を見て、相手の駒だった時
                            {
                                PointL += DirectionNumberL[i];
                                PointH += DirectionNumberH[i];
                                if (boardManager.handan(PointL, PointH) == 1)
                                {
                                    PointL += DirectionNumberL[i];
                                    PointH += DirectionNumberH[i];
                                    if (boardManager.handan(PointL, PointH) == 1)
                                    {
                                        PointL += DirectionNumberL[i];
                                        PointH += DirectionNumberH[i];
                                        if (boardManager.handan(PointL, PointH) == 1)
                                        {
                                            PointL += DirectionNumberL[i];
                                            PointH += DirectionNumberH[i];
                                            if (boardManager.handan(PointL, PointH) == 1)
                                            {
                                                PointL += DirectionNumberL[i];
                                                PointH += DirectionNumberH[i];                                                
                                                if(boardManager.handan(PointL, PointH) == 0)
                                                {
                                                    boardManager.ReverseBW(left, height, DirectionNumberL[i], DirectionNumberH[i]); //4つ挟むことができた
                                                    boardManager.ReverseBW(left + DirectionNumberL[i], height + DirectionNumberH[i], DirectionNumberL[i], DirectionNumberH[i]);
                                                    boardManager.ReverseBW(left + DirectionNumberL[i] + DirectionNumberL[i], height + DirectionNumberH[i] + DirectionNumberH[i], DirectionNumberL[i], DirectionNumberH[i]);
                                                    boardManager.ReverseBW(left + DirectionNumberL[i] + DirectionNumberL[i] + DirectionNumberL[i], height + DirectionNumberH[i] + DirectionNumberH[i] + DirectionNumberH[i], DirectionNumberL[i], DirectionNumberH[i]);
                                                    boardManager.ReverseBW(left + DirectionNumberL[i] + DirectionNumberL[i] + DirectionNumberL[i] + DirectionNumberL[i], height + DirectionNumberH[i] + DirectionNumberH[i] + DirectionNumberH[i] + DirectionNumberH[i], DirectionNumberL[i], DirectionNumberH[i]);
                                                }
                                            }
                                            else if (boardManager.handan(PointL, PointH) == 0)
                                            {
                                                boardManager.ReverseBW(left, height, DirectionNumberL[i], DirectionNumberH[i]); //4つ挟むことができた
                                                boardManager.ReverseBW(left + DirectionNumberL[i], height + DirectionNumberH[i], DirectionNumberL[i], DirectionNumberH[i]);
                                                boardManager.ReverseBW(left + DirectionNumberL[i] + DirectionNumberL[i], height + DirectionNumberH[i] + DirectionNumberH[i], DirectionNumberL[i], DirectionNumberH[i]);
                                                boardManager.ReverseBW(left + DirectionNumberL[i] + DirectionNumberL[i] + DirectionNumberL[i], height + DirectionNumberH[i] + DirectionNumberH[i] + DirectionNumberH[i], DirectionNumberL[i], DirectionNumberH[i]);
                                            }
                                        }
                                        else if (boardManager.handan(PointL, PointH) == 0)
                                        {
                                            boardManager.ReverseBW(left, height, DirectionNumberL[i], DirectionNumberH[i]); //3つ挟むことができた
                                            boardManager.ReverseBW(left + DirectionNumberL[i], height + DirectionNumberH[i], DirectionNumberL[i], DirectionNumberH[i]);
                                            boardManager.ReverseBW(left + DirectionNumberL[i] + DirectionNumberL[i], height + DirectionNumberH[i] + DirectionNumberH[i], DirectionNumberL[i], DirectionNumberH[i]);
                                        }
                                    }
                                    else if (boardManager.handan(PointL, PointH) == 0)
                                    {
                                        boardManager.ReverseBW(left, height, DirectionNumberL[i], DirectionNumberH[i]); //二つ挟むことができた
                                        boardManager.ReverseBW(left + DirectionNumberL[i], height + DirectionNumberH[i], DirectionNumberL[i], DirectionNumberH[i]);
                                    }
                                }
                                else if (boardManager.handan(PointL, PointH) == 0)
                                {
                                    //MessageBox.Show("L =  " +PointL + "L =  " + PointH);
                                    //boardManager.ReverseBW2(PointL - DirectionNumberL[i], PointH - DirectionNumberH[i] , 1);
                                    boardManager.ReverseBW(left, height, DirectionNumberL[i], DirectionNumberH[i]); // 一つ挟むことができたので反転
                                }
                            }
                        }


                        if (boardManager.IsKingDead())
                        {
                            // キングが存在するかの判定　勝利　勝敗
                            MessageBox.Show(boardManager.GetPlayerName() + "の勝ち");
                        }
                        boardManager.ChangePlayer(); // ChangePlayer
                        this.Refresh();
                        return;
                    }
                }
            }

            //MessageBox.Show("IsPlayerKoma =  " + boardManager.IsPlayerKoma(left, height));
            //MessageBox.Show("left = " + left + " height = " + height + "handan =  " + boardManager.handan(left, height));
            //MessageBox.Show("left-1 = " + (left-1) + " height-1 = " + (height) + " (handan - 1 =  " + boardManager.handan(left - 1, height));
            if (boardManager.IsPlayerKoma(left, height))
            {
                // 自分の駒を押した時に　true
                // MessageBox.Show("の勝ち");

                this.Refresh(); // ハイライト消去
                boardManager.setClicedKoma(left, height);
                var locations = boardManager.GetMovableLocation(); // 動かすことのできる位置
                //MessageBox.Show("0 = " + locations[0] + "1 = " + locations[1] + "2 = " + locations[2] );
                //MessageBox.Show("counto = " + locations.Count);
                this.drawMovableLocation(locations); // ハイライト
            }
        }


        private void drawMovableLocation(IList<Tuple<int, int>> locations)
        {
            Graphics g = Graphics.FromHwnd(this.Handle);
            // 移動可能位置をハイライトする挙動
            
            foreach (var loaction in locations)
            {
                //MessageBox.Show("1 = " + loaction.Item1 + "2 = " + loaction.Item2);
                g.FillRectangle(Brushes.CornflowerBlue, loaction.Item1 * PictureSize, loaction.Item2 * PictureSize, PictureSize, PictureSize);
            }
            drawKoma(g);
        }

        private int getImageSize()
        {
            // imageサイズを変更できる
            switch (komaImageSize)
            {
                case ImageSize.Thirty:
                    return 100;
                case ImageSize.FortyFive:
                    return 45;
                case ImageSize.Seventy:
                    return 70;
                default:
                    throw new ArgumentException("unexpected ");
            }
        }
    }
}
