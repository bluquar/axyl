using System;
using System.Collections.Generic;
using System.Text;

namespace EXYL
{
    class ListRemover
    {
        private int _timeLeft;
        private GameObject _piece;

        public int TimeLeft
        {
            get { return _timeLeft; }
            set { _timeLeft = value; }
        }
        public GameObject Piece
        {
            get { return _piece; }
            set { _piece = value; }
        }


        public ListRemover(GameObject piece, int timeLeft)
        {
            _piece = piece;
            _timeLeft = timeLeft;
        }
    }
}
