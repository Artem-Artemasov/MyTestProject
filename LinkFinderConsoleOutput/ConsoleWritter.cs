﻿using System;

namespace LinkFinder.ConsoleOutput
{
    public class ConsoleWritter
    {
        public virtual void WriteLine(string line)
        {
            Console.WriteLine(line);
        }

        public virtual void Write(string line)
        {
            Console.Write(line);
        }

        public virtual void ChangeCursorPositonX(int newPos)
        {
            Console.CursorLeft = newPos;
        }

        public virtual int GetOutputWidth()
        {
            return Console.BufferWidth;
        }

        public virtual string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}
