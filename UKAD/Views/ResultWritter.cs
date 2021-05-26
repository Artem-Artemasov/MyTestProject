﻿using System;
using UKAD.Interfaces.View;

namespace UKAD.Views
{
    public class ResultWritter : IResultWritter
    {
        public void WriteLine(string line)
        {
            Console.WriteLine(line);
        }

        public void Write(string line)
        {
            Console.Write(line);
        }

        public int GetOutputWidth()
        {
            return Console.BufferWidth;
        }

        public void ChangeCursorPositonX(int newPos)
        {
            Console.CursorLeft = newPos;
        }
    }
}