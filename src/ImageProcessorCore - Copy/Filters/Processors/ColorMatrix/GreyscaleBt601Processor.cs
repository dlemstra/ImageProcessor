﻿// <copyright file="GreyscaleBt601Processor.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageProcessorCore.Processors
{
    using System.Numerics;

    /// <summary>
    /// Converts the colors of the image to greyscale applying the formula as specified by
    /// ITU-R Recommendation BT.601 <see href="https://en.wikipedia.org/wiki/Luma_%28video%29#Rec._601_luma_versus_Rec._709_luma_coefficients"/>.
    /// </summary>
    public class GreyscaleBt601Processor : ColorMatrixFilter
    {
        /// <inheritdoc/>
        public override Matrix4x4 Matrix => new Matrix4x4()
        {
            M11 = .299f,
            M12 = .299f,
            M13 = .299f,
            M21 = .587f,
            M22 = .587f,
            M23 = .587f,
            M31 = .114f,
            M32 = .114f,
            M33 = .114f
        };
    }
}
