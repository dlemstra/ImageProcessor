﻿// <copyright file="MitchellNetravaliResampler.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageProcessorCore
{
    /// <summary>
    /// The function implements the mitchell algorithm as described on
    /// <see href="https://de.wikipedia.org/wiki/Mitchell-Netravali-Filter">Wikipedia</see>
    /// </summary>
    public class MitchellNetravaliResampler : IResampler
    {
        /// <inheritdoc/>
        public float Radius => 2;

        /// <inheritdoc/>
        public float GetValue(float x)
        {
            const float B = 1 / 3f;
            const float C = 1 / 3f;

            return ImageMaths.GetBcValue(x, B, C);
        }
    }
}
