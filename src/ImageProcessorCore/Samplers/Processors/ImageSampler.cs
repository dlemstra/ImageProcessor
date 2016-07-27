﻿// <copyright file="ImageSampler.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageProcessorCore.Processors
{
    /// <summary>
    /// Applies sampling methods to an image. 
    /// All processors requiring resampling or resizing should inherit from this.
    /// </summary>
    public abstract class ImageSampler<T, TP> : ImageProcessor<T, TP>, IImageSampler<T, TP>
        where T : IPackedVector<TP>
        where TP : struct
    {
        /// <inheritdoc/>
        public virtual bool Compand { get; set; } = false;
    }
}
