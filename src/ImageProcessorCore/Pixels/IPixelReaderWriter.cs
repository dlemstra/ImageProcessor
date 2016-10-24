// <copyright file="IPixelWriter.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageProcessorCore
{
    public interface IPixelReaderWriter
    {
        IPixelReader CreateReader(int width, int padding, ComponentOrder componentOrder);

        IPixelWriter CreateWriter(int width, int padding, ComponentOrder componentOrder);
    }
}
