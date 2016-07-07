﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IImageEncoder.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South and contributors.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Encapsulates properties and methods required for decoding an image to a stream.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ImageProcessorCore.Formats
{
    using System.IO;

    /// <summary>
    /// Encapsulates properties and methods required for encoding an image to a stream.
    /// </summary>
    public interface IImageEncoder
    {
        /// <summary>
        /// Gets or sets the quality of output for images.
        /// </summary>
        int Quality { get; set; }

        /// <summary>
        /// Gets the standard identifier used on the Internet to indicate the type of data that a file contains.
        /// </summary>
        string MimeType { get; }

        /// <summary>
        /// Gets the default file extension for this encoder.
        /// </summary>
        string Extension { get; }

        /// <summary>
        /// Returns a value indicating whether the <see cref="IImageEncoder"/> supports the specified
        /// file header.
        /// </summary>
        /// <param name="extension">The <see cref="string"/> containing the file extension.</param>
        /// <returns>
        /// <c>True</c> if the decoder supports the file extension; otherwise, <c>false</c>.
        /// </returns>
        bool IsSupportedFileExtension(string extension);

        /// <summary>
        /// Encodes the image to the specified stream from the <see cref="ImageBase{TPackedVector}"/>.
        /// </summary>
        /// <typeparam name="TPackedVector">The type of pixels contained within the image.</typeparam>
        /// <param name="image">The <see cref="ImageBase{TPackedVector}"/> to encode from.</param>
        /// <param name="stream">The <see cref="Stream"/> to encode the image data to.</param>
        void Encode<TPackedVector>(ImageBase<TPackedVector> image, Stream stream)
            where TPackedVector : IPackedVector, new();
    }
}
