﻿// <copyright file="SobelProcessor.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageProcessorCore.Processors
{
    /// <summary>
    /// The Sobel operator filter.
    /// <see href="http://en.wikipedia.org/wiki/Sobel_operator"/>
    /// </summary>
    /// <typeparam name="TColor">The pixel format.</typeparam>
    /// <typeparam name="TPacked">The packed format. <example>uint, long, float.</example></typeparam>
    public class SobelProcessor<TColor, TPacked> : EdgeDetector2DFilter<TColor, TPacked>
        where TColor : IPackedVector<TPacked>
        where TPacked : struct
    {
        private static readonly float[][] kernelX = new float[3][]
        {
            new float[] { -1, 0, 1 },
            new float[] { -2, 0, 2 },
            new float[] { -1, 0, 1 }
        };

        private static readonly float[][] kernelY = new float[3][]
        {
            new float[] { -1, -2, -1 },
            new float[] { 0, 0, 0 },
            new float[] { 1, 2, 1 }
        };

        /// <inheritdoc/>
        public override float[][] KernelX => kernelX;

        /// <inheritdoc/>
        public override float[][] KernelY => kernelY;
    }
}
