﻿// <copyright file="SkewProcessor.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageProcessorCore.Processors
{
    using System.Numerics;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides methods that allow the skewing of images.
    /// </summary>
    public class SkewProcessor<T, TP> : Matrix3x2Processor<T, TP>
        where T : IPackedVector<TP>
        where TP : struct
    {
        /// <summary>
        /// The tranform matrix to apply.
        /// </summary>
        private Matrix3x2 processMatrix;

        /// <summary>
        /// Gets or sets the angle of rotation along the x-axis in degrees.
        /// </summary>
        public float AngleX { get; set; }

        /// <summary>
        /// Gets or sets the angle of rotation along the y-axis in degrees.
        /// </summary>
        public float AngleY { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to expand the canvas to fit the skewed image.
        /// </summary>
        public bool Expand { get; set; } = true;

        /// <inheritdoc/>
        protected override void OnApply(ImageBase<T, TP> target, ImageBase<T, TP> source, Rectangle targetRectangle, Rectangle sourceRectangle)
        {
            this.processMatrix = Point.CreateSkew(new Point(0, 0), -this.AngleX, -this.AngleY);
            if (this.Expand)
            {
                CreateNewTarget(target, sourceRectangle, this.processMatrix);
            }
        }

        /// <inheritdoc/>
        protected override void Apply(ImageBase<T, TP> target, ImageBase<T, TP> source, Rectangle targetRectangle, Rectangle sourceRectangle, int startY, int endY)
        {
            Matrix3x2 matrix = GetCenteredMatrix(target, source, this.processMatrix);

            using (IPixelAccessor<T, TP> sourcePixels = source.Lock())
            using (IPixelAccessor<T, TP> targetPixels = target.Lock())
            {
                Parallel.For(
                    0,
                    target.Height,
                    this.ParallelOptions,
                    y =>
                        {
                            for (int x = 0; x < target.Width; x++)
                            {
                                Point transformedPoint = Point.Skew(new Point(x, y), matrix);
                                if (source.Bounds.Contains(transformedPoint.X, transformedPoint.Y))
                                {
                                    targetPixels[x, y] = sourcePixels[transformedPoint.X, transformedPoint.Y];
                                }
                            }

                            OnRowProcessed();
                        });
            }
        }
    }
}