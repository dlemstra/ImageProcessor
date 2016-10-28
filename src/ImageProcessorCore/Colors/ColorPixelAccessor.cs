// <copyright file="ColorPixelAccessor.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageProcessorCore
{
    using System;
    using System.Runtime.CompilerServices;

    public unsafe sealed class ColorPixelAccessor : PixelAccessor<Color, uint>
    {
        public ColorPixelAccessor(ImageBase<Color, uint> image)
          : base(image)
        {
        }

        protected override void CopyFromBGR(PixelRow<Color, uint> row, int targetY, int width)
        {
            byte* source = row.DataPointer;
            byte* destination = base.PixelsBase + (targetY * this.Width) * Unsafe.SizeOf<uint>();

            for (int x = 0; x < width; x++)
            {
                Unsafe.Write(destination, (uint)(*(source + 2) << 24 | *(source + 1) << 16 | *source << 8 | 255));

                source += 3;
                destination += 4;
            }
        }

        protected override void CopyFromBGRA(PixelRow<Color, uint> row, int targetY, int width)
        {
            byte* source = row.DataPointer;
            byte* destination = base.PixelsBase + (targetY * this.Width) * Unsafe.SizeOf<uint>();

            for (int x = 0; x < width; x++)
            {
                Unsafe.Write(destination, (uint)(*(source + 2) << 24 | *(source + 1) << 16 | *source << 8 | *(source + 3)));

                source += 4;
                destination += 4;
            }
        }

        protected override void CopyToBGR(PixelRow<Color, uint> row, int sourceY, int width)
        {
            byte* source = base.PixelsBase + (sourceY * this.Width) * Unsafe.SizeOf<uint>();
            byte* destination = row.DataPointer;

            for (int x = 0; x < width; x++)
            {
                if (BitConverter.IsLittleEndian)
                {
                    *(destination) = *(source + 1);
                    *(destination + 1) = *(source + 2);
                    *(destination + 2) = *(source + 3);
                }
                else
                {
                    *(destination) = *(source + 3);
                    *(destination + 1) = *(source + 2);
                    *(destination + 2) = *(source + 1);
                }

                source += 4;
                destination += 3;
            }
        }

        protected override void CopyToBGRA(PixelRow<Color, uint> row, int sourceY, int width)
        {
            byte* source = base.PixelsBase + (sourceY * this.Width) * Unsafe.SizeOf<uint>();
            byte* destination = row.DataPointer;

            for (int x = 0; x < width; x++)
            {
                if (BitConverter.IsLittleEndian)
                {
                    *(destination) = *(source + 1);
                    *(destination + 1) = *(source + 2);
                    *(destination + 2) = *(source + 3);
                    *(destination + 3) = *(source);
                }
                else
                {
                    *(destination) = *(source);
                    *(destination + 1) = *(source + 3);
                    *(destination + 2) = *(source + 2);
                    *(destination + 3) = *(source + 1);
                }

                source += 4;
                destination += 4;
            }
        }
    }
}
