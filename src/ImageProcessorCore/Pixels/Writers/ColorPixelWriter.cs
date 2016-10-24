// <copyright file="ColorPixelWriter.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageProcessorCore
{
    using System;
    using System.IO;

    internal unsafe sealed class ColorPixelWriter : PixelReaderWriterBase, IPixelWriter
    {
        public ColorPixelWriter(int width, int padding, ComponentOrder componentOrder)
          : base(width, padding, componentOrder)
        {
            this.CopyMethod = GetCopyMethod(componentOrder);
        }

        public void WriteRow(byte* input, Stream stream)
        {
            WriteRowCore(input, stream);
        }

        private DataCopier GetCopyMethod(ComponentOrder componentOrder)
        {
            switch (componentOrder)
            {
                case ComponentOrder.BGR:
                  return this.CopyBGR;
                case ComponentOrder.BGRA:
                  return this.CopyBGRA;
                case ComponentOrder.RGB:
                  return this.CopyRGB;
                case ComponentOrder.RGBA:
                  return this.CopyRGBA;
            }

            throw new NotSupportedException();
        }

        private void CopyBGR(byte* data)
        {
            byte* output = this.PixelsBase;

            for (int x = 0; x < this.Width; x++)
            {
                if (BitConverter.IsLittleEndian)
                {
                    *(output) = *(data + 1);
                    *(output + 1) = *(data + 2);
                    *(output + 2) = *(data + 3);
                }
                else
                {
                    *(output) = *(data + 3);
                    *(output + 1) = *(data + 2);
                    *(output + 2) = *(data + 1);
                }
                output += this.ComponentCount;
                data += 4;
            }
        }

        private void CopyBGRA(byte* data)
        {
            byte* output = this.PixelsBase;

            for (int x = 0; x < this.Width; x++)
            {
                if (BitConverter.IsLittleEndian)
                {
                    *(output) = *(data + 1);
                    *(output + 1) = *(data + 2);
                    *(output + 2) = *(data + 3);
                    *(output + 3) = *(data);
                }
                else
                {
                    *(output) = *(data);
                    *(output + 1) = *(data + 3);
                    *(output + 2) = *(data + 2);
                    *(output + 3) = *(data + 1);
                }
                output += this.ComponentCount;
                data += 4;
            }
        }

        private void CopyRGB(byte* data)
        {
            byte* output = this.PixelsBase;

            for (int x = 0; x < this.Width; x++)
            {
                if (BitConverter.IsLittleEndian)
                {
                    *(output) = *(data + 3);
                    *(output + 1) = *(data + 2);
                    *(output + 2) = *(data + 1);
                }
                else
                {
                    *(output) = *(data + 1);
                    *(output + 1) = *(data + 2);
                    *(output + 2) = *(data + 3);
                }
                output += this.ComponentCount;
                data += 4;
            }
        }

        private void CopyRGBA(byte* data)
        {
            byte* output = this.PixelsBase;

            for (int x = 0; x < this.Width; x++)
            {
                if (BitConverter.IsLittleEndian)
                {
                    *(output) = *(data + 3);
                    *(output + 1) = *(data + 2);
                    *(output + 2) = *(data + 1);
                    *(output + 3) = *(data);
                }
                else
                {
                    *(output) = *(data);
                    *(output + 1) = *(data + 1);
                    *(output + 2) = *(data + 2);
                    *(output + 3) = *(data + 3);
                }
                output += this.ComponentCount;
                data += 4;
            }
        }
    }
}
