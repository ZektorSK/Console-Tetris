using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    class MapNode
    {
        public int x;
        public int y;
        public char content;

        public MapNode(int _x, int _y, char _content)
        {
            x = _x;
            y = _y;
            content = _content;
        }
    }
}
