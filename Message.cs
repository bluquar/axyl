using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXYL
{
    class Message
    {
        private string _message;
        private int _totalTime;
        private int _currentTime;
        private int _x;
        private int _y;

        public string MessageDisplayed
        {
            get { return _message; }
            set { _message = value; }
        }
        public int TotalTime
        {
            get { return _totalTime; }
            set { _totalTime = value; }
        }
        public int CurrentTime
        {
            get { return _currentTime; }
            set { _currentTime = value; }
        }
        public int X
        {
            get { return _x; }
            set { _x = value; }
        }
        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public static Message NewMessage(string message, int time, int x)
        {
            Message m = new Message();
            m._message = message;
            m._totalTime = time;
            m._currentTime = time;
            m._x = x;
            m._y = 20;
            return m;
        }
    }
}
