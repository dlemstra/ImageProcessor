// <copyright file="IPixelReader.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageProcessorCore
{
    using System;
    using System.IO;

    public unsafe interface IPixelReader : IDisposable
    {
        void ReadRow(Stream stream, byte* output);
    }
}
