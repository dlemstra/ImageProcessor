﻿// <copyright file="GreyscaleBt709Processor.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageProcessorCore.Processors
{
    using System.Numerics;

    /// <summary>
    /// Converts the colors of the image to greyscale applying the formula as specified by
    /// ITU-R Recommendation BT.709 <see href="https://en.wikipedia.org/wiki/Rec._709#Luma_coefficients"/>.
    /// </summary>
    public class GreyscaleBt709Processor<T, TP> : ColorMatrixFilter<T, TP>
        where T : IPackedVector<TP>
        where TP : struct
    {
        /// <inheritdoc/>
        public override Matrix4x4 Matrix => new Matrix4x4()
        {
            M11 = .2126f,
            M12 = .2126f,
            M13 = .2126f,
            M21 = .7152f,
            M22 = .7152f,
            M23 = .7152f,
            M31 = .0722f,
            M32 = .0722f,
            M33 = .0722f
        };
    }
}
