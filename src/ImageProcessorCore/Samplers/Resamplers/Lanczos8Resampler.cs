﻿// <copyright file="Lanczos8Resampler.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageProcessorCore
{
    /// <summary>
    /// The function implements the Lanczos kernel algorithm as described on
    /// <see href="https://en.wikipedia.org/wiki/Lanczos_resampling#Algorithm">Wikipedia</see>
    /// with a radius of 8 pixels.
    /// </summary>
    public class Lanczos8Resampler : IResampler
    {
        /// <inheritdoc/>
        public double Radius => 8;

        /// <inheritdoc/>
        public double GetValue(double x)
        {
            if (x < 0)
            {
                x = -x;
            }

            if (x < 8)
            {
                return ImageMaths.SinC(x) * ImageMaths.SinC(x / 8d);
            }

            return 0;
        }
    }
}
