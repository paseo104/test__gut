using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brutus
{
    [Serializable]
    class Pawn2 : KomaBase
    {
         public Pawn2(PlayerNo color, int index)
             : base(color, index)
         {

         }
        public override void SetDefaultPoint()
        {
            SetLocation(3 + 1 *Index, getHeight());
        }


        public override int GetMaxCount()
        {
            return 2;
            // 修正しました　最大個数
        }

        public override List<Tuple<MoveType, int, int>> GetMovableLoacation()
        {

            var list = new List<Tuple<MoveType, int, int>>();
            foreach (var l in new[] { -1, 0, 1 })
            {
                foreach (var h in new[] { -1, 0, 1 })
                {
                    list.Add(new Tuple<MoveType, int, int>(MoveType.Normal, Left + l, Height + h));
                }
            }

            return list;
        }

        public override KomaKind Kind
        {
            get { return KomaKind.Pawn; }
        }

        protected override System.Drawing.Bitmap GetPicture()
        {
            return GetObjectByColor(
                Properties.Resources._45px_Brutus_black_ken_svg
                ,Properties.Resources._45px_Brutus_white_ken_svg);
        }
        protected override int getHeight()
        {
            return GetObjectByColor(6, 0);
            //修正しました
        }
    }
}
