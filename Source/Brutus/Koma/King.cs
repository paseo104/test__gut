using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brutus
{
    [Serializable]
    class King : KomaBase
    {
        public King(PlayerNo color,int index)
             : base(color, index)
        {

        }
        public override void SetDefaultPoint()
        {
            //修正しました
            SetLocation(2, getHeight());
        }


        public override int GetMaxCount()
        {
            return 1;
        }

        public override List<Tuple<MoveType, int, int>> GetMovableLoacation()
        {

            var list = new List<Tuple<MoveType, int, int>>();

            return list;
        }

        public override KomaKind Kind
        {
            get
            {
                return KomaKind.King;
            }
        }

        protected override System.Drawing.Bitmap GetPicture()
        {
            return GetObjectByColor(
                Properties.Resources._45px_Brutus_black_oo_svg
                , Properties.Resources._45px_Brutus_white_oo_svg);

        }

    }
}
