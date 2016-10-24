// <copyright file="IPixelWriter.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageProcessorCore
{
    using System;
    using System.IO;

    public unsafe interface IPixelWriter : IDisposable
    {
        void WriteRow(byte* input, Stream stream);
    }
}
