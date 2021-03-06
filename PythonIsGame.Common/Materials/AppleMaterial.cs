﻿using System;
using System.Drawing;

namespace PythonIsGame.Common.Materials
{
    [Serializable]
    public class AppleMaterial : IMaterial
    {
        public int NutritionalValue = 1;

        public void IntersectedWithSnake(Snake snake)
        {
            snake.Score += NutritionalValue;
            snake.AddTailSegment();
        }
    }
}
