// KomaBase 
// これを継承して駒を作る
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Windows.Forms;

namespace Brutus
{
    [Serializable]
    abstract class KomaBase
    {
        // protected int MAX_LEFT = 5;
        // protected int MAX_HEIGHT = 7;
        public Bitmap MyPicture { get; private set; }
        public abstract KomaKind Kind { get;}
        public PlayerNo Player { get; private set; }
        public int Height { get { return Location.Item2; } }
        public int Left { get { return Location.Item1; } }
        public int Index { get; private set; }
        public Tuple<int, int> Location { get; private set; }
        public bool IsDead { get; set; }

        public int JudgeReverse { get; set; }

        public KomaBase(PlayerNo playerNo, int index)
        {
            Player = playerNo;
            Index = index;

            CheckIndex();
            this.MyPicture = GetPicture();
        }

        public abstract void SetDefaultPoint();
        public abstract int GetMaxCount();

        public abstract List<Tuple<MoveType, int,int>> GetMovableLoacation();
        protected abstract Bitmap GetPicture();
        public void SetLocation(int left, int height)
        {
            this.Location = new Tuple<int, int>(left, height);
        }

        // 所有するユーザを変える
        public void ReverseKoma(PlayerNo playerNo)
        {
            if(playerNo == PlayerNo.One)
            {
                Player = PlayerNo.Two;
            }
            else
            {
                Player = PlayerNo.One;
            }
            this.MyPicture = GetPicture();
        }


        private void CheckIndex()
        {
            if (Index +1 > GetMaxCount())
            {
                throw new ArgumentException("Count is larger than max count!!");
            }

        }
        protected virtual int getHeight()
        {
            // 修正しました 一番下の高さ　
            return GetObjectByColor(6, 0);
        }

        protected T GetObjectByColor<T>(T whiteObject, T blackObject)
        {
            return Player == PlayerNo.Two ? whiteObject : blackObject;
        }

        protected T GetObjectReverseColor<T>(T whiteObject, T blackObject)
        {
            return Player != PlayerNo.Two ? whiteObject : blackObject;
        }

    }
}