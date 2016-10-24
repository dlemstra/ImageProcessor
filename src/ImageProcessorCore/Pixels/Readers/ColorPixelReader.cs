// <copyright file="ColorPixelReader.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageProcessorCore
{
    using System;
    using System.IO;

    internal unsafe sealed class ColorPixelReader : PixelReaderWriterBase, IPixelReader
    {
        public ColorPixelReader(int width, int padding, ComponentOrder componentOrder)
          : base(width, padding, componentOrder)
        {
            this.CopyMethod = GetCopyMethod(componentOrder);
        }

        public void ReadRow(Stream stream, byte* output)
        {
            ReadRowCore(stream, output);
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
            byte* input = this.PixelsBase;

            for (int x = 0; x < this.Width; x++)
            {
                if (BitConverter.IsLittleEndian)
                {
                    *(data) = 255;
                    *(data + 1) = *(input);
                    *(data + 2) = *(input + 1);
                    *(data + 3) = *(input + 2);
                }
                else
                {
                    *(data) = *(input + 2);
                    *(data + 1) = *(input + 1);
                    *(data + 2) = *(input);
                    *(data + 3) = 255;
                }
                input += this.ComponentCount;
                data += 4;
            }
        }

        private void CopyBGRA(byte* data)
        {
            byte* input = this.PixelsBase;

            for (int x = 0; x < this.Width; x++)
            {
                if (BitConverter.IsLittleEndian)
                {
                    *(data) = *(input + 3);
                    *(data + 1) = *(input);
                    *(data + 2) = *(input + 1);
                    *(data + 3) = *(input + 2);
                }
                else
                {
                    *(data) = *(input + 2);
                    *(data + 1) = *(input + 1);
                    *(data + 2) = *(input);
                    *(data + 3) = *(input + 3);
                }
                input += this.ComponentCount;
                data += 4;
            }
        }

        private void CopyRGB(byte* data)
        {
            byte* input = this.PixelsBase;

            for (int x = 0; x < this.Width; x++)
            {
                if (BitConverter.IsLittleEndian)
                {
                    *(data) = 255;
                    *(data + 1) = *(input + 2);
                    *(data + 2) = *(input + 1);
                    *(data + 3) = *(input);
                }
                else
                {
                    *(data) = *(input);
                    *(data + 1) = *(input + 1);
                    *(data + 2) = *(input + 2);
                    *(data + 3) = 255;
                }
                input += this.ComponentCount;
                data += 4;
            }
        }

        private void CopyRGBA(byte* data)
        {
            byte* input = this.PixelsBase;

            for (int x = 0; x < this.Width; x++)
            {
                if (BitConverter.IsLittleEndian)
                {
                    *(data) = *(input + 3);
                    *(data + 1) = *(input + 2);
                    *(data + 2) = *(input + 1);
                    *(data + 3) = *(input);
                }
                else
                {
                    *(data) = *(input);
                    *(data + 1) = *(input + 1);
                    *(data + 2) = *(input + 2);
                    *(data + 3) = *(input + 3);
                }
                input += this.ComponentCount;
                data += 4;
            }
        }
    }
}
