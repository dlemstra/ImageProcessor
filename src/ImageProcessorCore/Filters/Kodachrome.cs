﻿// <copyright file="Kodachrome.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>-------------------------------------------------------------------------------------------------------------------

namespace ImageProcessorCore
{
    using Processors;

    /// <summary>
    /// Extension methods for the <see cref="Image"/> type.
    /// </summary>
    public static partial class ImageExtensions
    {
        /// <summary>
        /// Alters the colors of the image recreating an old Kodachrome camera effect.
        /// </summary>
        /// <param name="source">The image this method extends.</param>
        /// <param name="progressHandler">A delegate which is called as progress is made processing the image.</param>
        /// <returns>The <see cref="Image"/>.</returns>
        public static Image Kodachrome(this Image source, ProgressEventHandler progressHandler = null)
        {
            return Kodachrome(source, source.Bounds, progressHandler);
        }

        /// <summary>
        /// Alters the colors of the image recreating an old Kodachrome camera effect.
        /// </summary>
        /// <param name="source">The image this method extends.</param>
        /// <param name="rectangle">
        /// The <see cref="Rectangle"/> structure that specifies the portion of the image object to alter.
        /// </param>
        /// <param name="progressHandler">A delegate which is called as progress is made processing the image.</param>
        /// <returns>The <see cref="Image"/>.</returns>
        public static Image Kodachrome(this Image source, Rectangle rectangle, ProgressEventHandler progressHandler = null)
        {
            KodachromeProcessor processor = new KodachromeProcessor();
            processor.OnProgress += progressHandler;

            try
            {
                return source.Process(rectangle, processor);
            }
            finally
            {
                processor.OnProgress -= progressHandler;
            }
        }
    }
}
