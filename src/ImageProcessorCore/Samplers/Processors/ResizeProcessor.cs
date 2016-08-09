﻿// <copyright file="ResizeProcessor.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageProcessorCore.Processors
{
    using System;
    using System.Numerics;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides methods that allow the resizing of images using various algorithms.
    /// </summary>
    /// <remarks>
    /// This version and the <see cref="CompandingResizeProcessor{T,TP}"/> have been separated out to improve performance.
    /// </remarks>
    /// <typeparam name="T">The pixel format.</typeparam>
    /// <typeparam name="TP">The packed format. <example>long, float.</example></typeparam>
    public class ResizeProcessor<T, TP> : ResamplingWeightedProcessor<T, TP>
        where T : IPackedVector<TP>
        where TP : struct
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResizeProcessor{T,TP}"/> class.
        /// </summary>
        /// <param name="sampler">
        /// The sampler to perform the resize operation.
        /// </param>
        public ResizeProcessor(IResampler sampler)
            : base(sampler)
        {
        }

        /// <inheritdoc/>
        protected override void Apply(ImageBase<T, TP> target, ImageBase<T, TP> source, Rectangle targetRectangle, Rectangle sourceRectangle, int startY, int endY)
        {
            // Jump out, we'll deal with that later.
            if (source.Bounds == target.Bounds && sourceRectangle == targetRectangle)
            {
                return;
            }

            int width = target.Width;
            int height = target.Height;
            int sourceHeight = sourceRectangle.Height;
            int targetX = target.Bounds.X;
            int targetY = target.Bounds.Y;
            int targetRight = target.Bounds.Right;
            int targetBottom = target.Bounds.Bottom;
            int startX = targetRectangle.X;
            int endX = targetRectangle.Right;

            int minX = Math.Max(targetX, startX);
            int maxX = Math.Min(targetRight, endX);
            int minY = Math.Max(targetY, startY);
            int maxY = Math.Min(targetBottom, endY);

            if (this.Sampler is NearestNeighborResampler)
            {
                // Scaling factors
                float widthFactor = sourceRectangle.Width / (float)targetRectangle.Width;
                float heightFactor = sourceRectangle.Height / (float)targetRectangle.Height;

                using (IPixelAccessor<T, TP> sourcePixels = source.Lock())
                using (IPixelAccessor<T, TP> targetPixels = target.Lock())
                {
                    Parallel.For(
                        minY,
                        maxY,
                        this.ParallelOptions,
                        y =>
                        {
                            // Y coordinates of source points
                            int originY = (int)((y - startY) * heightFactor);

                            for (int x = minX; x < maxX; x++)
                            {
                                // X coordinates of source points
                                targetPixels[x, y] = sourcePixels[(int)((x - startX) * widthFactor), originY];
                            }

                            this.OnRowProcessed();
                        });
                }

                // Break out now.
                return;
            }

            // Interpolate the image using the calculated weights.
            // A 2-pass 1D algorithm appears to be faster than splitting a 1-pass 2D algorithm 
            // First process the columns. Since we are not using multiple threads startY and endY
            // are the upper and lower bounds of the source rectangle.
            Image<T, TP> firstPass = new Image<T, TP>(target.Width, source.Height);
            using (IPixelAccessor<T, TP> sourcePixels = source.Lock())
            using (IPixelAccessor<T, TP> firstPassPixels = firstPass.Lock())
            using (IPixelAccessor<T, TP> targetPixels = target.Lock())
            {
                minX = Math.Max(0, startX);
                maxX = Math.Min(width, endX);
                minY = Math.Max(0, startY);
                maxY = Math.Min(height, endY);

                Parallel.For(
                    0,
                    sourceHeight,
                    this.ParallelOptions,
                    y =>
                    {
                        for (int x = minX; x < maxX; x++)
                        {
                            // Ensure offsets are normalised for cropping and padding.
                            Weight[] horizontalValues = this.HorizontalWeights[x - startX].Values;

                            // Destination color components
                            Vector4 destination = Vector4.Zero;

                            for (int i = 0; i < horizontalValues.Length; i++)
                            {
                                Weight xw = horizontalValues[i];
                                destination += sourcePixels[xw.Index, y].ToVector4() * xw.Value;
                            }

                            T d = default(T);
                            d.PackVector(destination);
                            firstPassPixels[x, y] = d;
                        }
                    });

                // Now process the rows.
                Parallel.For(
                    minY,
                    maxY,
                    this.ParallelOptions,
                    y =>
                    {
                        // Ensure offsets are normalised for cropping and padding.
                        Weight[] verticalValues = this.VerticalWeights[y - startY].Values;

                        for (int x = 0; x < width; x++)
                        {
                            // Destination color components
                            Vector4 destination = Vector4.Zero;

                            for (int i = 0; i < verticalValues.Length; i++)
                            {
                                Weight yw = verticalValues[i];
                                destination += firstPassPixels[x, yw.Index].ToVector4() * yw.Value;
                            }

                            T d = default(T);
                            d.PackVector(destination);
                            targetPixels[x, y] = d;
                        }

                        this.OnRowProcessed();
                    });
            }
        }
    }
}