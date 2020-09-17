using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brutus
{
    class BoardManager
    {
        List<KomaBase> komaList = null;

        PlayerNo playerNo = PlayerNo.Two;
        KomaBase clickedKoma;


        List<Tuple<int, int>> movableLoacation = new List<Tuple<int,int>>();
        Context context;

        public BoardManager(Context context)
        {
            this.context = context;
        }

        public List<KomaBase> KomaCreate()
        {
            if (komaList != null)
            {
                return komaList;
            }
            komaList = new List<KomaBase>();
            foreach (KomaKind komaKind in Enum.GetValues(typeof(KomaKind)))
            {
                int index = 0;
                while(true)
                {
                    var komaKuro = createKoma(komaKind, PlayerNo.Two, index);
                    komaKuro.SetDefaultPoint();
                    var komaShiro = createKoma(komaKind, PlayerNo.One, index);
                    komaShiro.SetDefaultPoint();
                    komaList.AddRange(new[] { komaKuro, komaShiro });


                    if (komaShiro.GetMaxCount() == index + 1)
                        break;
                    ++index;
                    
                }
            }
            return komaList;
        }

        // 生きている駒のリストを返す
        public IEnumerable<KomaBase> GetAliveKoma()
        {
            return komaList.Where((koma) => !koma.IsDead);
        }

        // 動くことのできるリストを返す
        public List<Tuple<int,int>> GetMovableLocation()
        {
            return movableLoacation;
        }


        private List<Tuple< int, int>> GetCanMove(IEnumerable<Tuple<MoveType, int, int>> locateInfos,KomaBase koma)
        {
            var inBoardLocates = this.GetInBoardLocate(locateInfos);
            var canMoveLocates = new List<Tuple< int, int>>();

            canMoveLocates.AddRange(this.GetNormal(inBoardLocates));

            return canMoveLocates;
        }

        //
        private IEnumerable<Tuple<int, int>> GetNormal(IEnumerable<Tuple<MoveType, int, int>> locationInfos)
        {
            return locationInfos.Where((location) =>
                location.Item1 == MoveType.Normal
                && !existsKoma(location.Item2, location.Item3,playerNo))
                .Select(location => new Tuple<int, int>(location.Item2, location.Item3));
        }

        //　移動できる範囲の定義
        private bool existsKoma(int left, int heigth,PlayerNo player)
        {
            return this.GetAliveKoma().Where((koma) =>
            //koma.Player == player && // これを追加すると重なることができる
            //koma.Kind == KomaKind.King &&
            (koma.Kind != KomaKind.King ||
            koma.Player == player) &&
            koma.Left == left &&
            koma.Height == heigth).Count() > 0;
        }

        private IEnumerable<Tuple<MoveType, int, int>> GetInBoardLocate(IEnumerable<Tuple<MoveType, int, int>> locateInfos)
        {
            return locateInfos.Where((locateinfo) => isInBoard(locateinfo.Item2)
                && isInBoard(locateinfo.Item3));
        }

        // 高さの最大値　限界地設定
        private bool isInBoard(int point)
        {
            if (0 <= point && point <= 6)
            {
                return true;
            }
            return false;
        }

        private KomaBase GetKoma(int left, int height)
        {
            var komas = komaList.Where((koma)
                => !koma.IsDead
                && koma.Left == left && koma.Height == height);
            if (komas.Count() > 0)
            {
                return komas.First();
            }
            return null;
        }

        private KomaBase createKoma(KomaKind kind, PlayerNo playerNo, int index)
        {
            switch (kind)
            {
                case KomaKind.King:
                    return new King(playerNo, index);
                case KomaKind.Pawn:
                    return new Pawn(playerNo, index);
                case KomaKind.Pawn2:
                    return new Pawn2(playerNo, index);
                case KomaKind.Pawn3:
                    return new Pawn3(playerNo, index);
                default:
                    throw new ArgumentException("Unexpected Kind!!");
            }
        }

        // 同じ駒がクリックされたとき　TURE　
        internal bool IsSameKoma(int left, int height)
        {
            return (clickedKoma.Left == left && clickedKoma.Height == height);
        }

        internal void ClearClickedKoma()
        {
            clickedKoma = null;
            movableLoacation.Clear();
        }

        // 駒が選択状態であるとき　＝　Turu
        internal bool IsInsicateMovableLocation()
        {
            return movableLoacation.Count > 0;
        }

        // ハイライトされている位置、をクリックするとturu  
        internal bool CanMove(int left, int height)
        {
            return
                this.movableLoacation.Where((location) => 
                    location.Item1 == left &&
                    location.Item2 == height).Count() > 0;
        }

        internal void SetLocation(int left, int height)
        {
            foreach (var koma in this.GetAliveKoma().Where((koma) =>
                koma.Left == left &&
                koma.Height == height &&
                koma.Player != playerNo))
            {
                koma.IsDead = true;
            }
            clickedKoma.SetLocation(left, height);
        }

        // その時のターンの駒をクリックされたらtrue
        internal bool IsPlayerKoma(int left, int height)
        {
            return this.GetAliveKoma().Where((location) =>
                location.Player == playerNo
                && location.Left == left
                && location.Height == height).Count() > 0;
        }

        // 
        // 
        internal bool BWPlayerKoma(int left, int height)
        {
            // (座標の白でも黒でも)駒があるときにTrue　空の時にF
            return this.GetAliveKoma().Where((location) =>
                location.Left == left
                && location.Height == height).Count() > 0;
        }
        //
        //

        // 自分の色　0 相手の色 1 空 -1
        internal int handan(int left, int height)
        {
            
            if(BWPlayerKoma(left, height))
            {
                if(IsPlayerKoma(left, height))
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return -1;
            }
        }
        //
        //
        //
        //

        ///////////////////////aaaaaaaaaaaaa//////////////////////////
        internal void LocationKomaKomaa(int left)
        {
            var locationsa = this.GetAliveKoma();
            var C = locationsa.Count();
            

            foreach (var koma in this.GetAliveKoma())
            {
                if ( koma.Left == left)
                {
                   koma.IsDead  = true;
                   //System.Windows.Forms.MessageBox.Show("koma.Player" + koma.Player);
                   var komaKuro = createKoma(KomaKind.Pawn, PlayerNo.Two, 0);
                   //komaKuro.SetDefaultPoint();
                }
            }
        }


        internal void ReverseBW(int left, int height, int DirectionLeft, int DirectionHeight)
        {
            //clickedKoma2.SetLocation(clickedKoma.Left + i, height + j);
            foreach (var koma in this.GetAliveKoma())
            {
                if(koma.Left == (left + DirectionLeft) && koma.Height == (height + DirectionHeight))
                {
                    koma.JudgeReverse = 1;
                    koma.ReverseKoma(koma.Player);
                }
            }
        }

        internal void ReverseBW2(int left, int height, int number)
        {
            //clickedKoma2.SetLocation(clickedKoma.Left + i, height + j);
            foreach (var koma in this.GetAliveKoma())
            {
                if(koma.Left == left && koma.Height == height)
                {
                    koma.JudgeReverse = 1;
                    for(int i = 0; i < number; i++)
                        koma.ReverseKoma(koma.Player);
                }
            }
        }

        internal void ChangePlayer()
        {
            playerNo = playerNo == PlayerNo.Two ? PlayerNo.One : PlayerNo.Two;
            ClearClickedKoma();
        }
        
        //　movableLoacation　動かすことのできる地点の計算
        internal void setClicedKoma(int left, int height)
        {
            movableLoacation = new List<Tuple<int, int>>();

            clickedKoma = this.GetKoma(left, height);
            if (clickedKoma != null)
            {
                foreach (var locateInfo in this.GetCanMove(clickedKoma.GetMovableLoacation(), clickedKoma))
                {
                    // getcanmove から locateinfo を作り、movableLoacationに追加
                    movableLoacation.Add(locateInfo);
                }
            }
        }

        internal bool IsKingDead()
        {
            return this.komaList.Where((koma) =>
                koma.Kind == KomaKind.King
                && koma.IsDead
                && koma.Player != playerNo).Count() > 0;
        }

        internal string GetPlayerName()
        {
            return this.context.Players[playerNo].PlayerName;
        }
    }

}
