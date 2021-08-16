using System;

namespace Snake
{
    interface IController 
    {
        public Snake Snake { get; set; }
        public void Read();
    }
}
