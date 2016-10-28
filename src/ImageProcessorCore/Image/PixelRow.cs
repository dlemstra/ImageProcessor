// <copyright file="PixelAccessor.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>


namespace ImageProcessorCore
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    public unsafe sealed class PixelRow<TColor, TPacked> : IDisposable
        where TColor : struct, IPackedPixel<TPacked>
        where TPacked : struct
    {
        private readonly GCHandle handle;

        public PixelRow(int width, ComponentOrder componentOrder)
          : this(width, componentOrder, 0)
        {
        }

        public PixelRow(int width, ComponentOrder componentOrder, int padding)
        {
            this.Width = width;
            this.ComponentOrder = componentOrder;
            this.Data = new byte[width * GetComponentCount(componentOrder) + padding];
            this.handle = GCHandle.Alloc(this.Data, GCHandleType.Pinned);
            this.DataPointer = (byte*)this.handle.AddrOfPinnedObject().ToPointer();
        }

        ~PixelRow()
        {
            this.Dispose();
        }

        public byte[] Data { get; private set; }

        public byte* DataPointer;

        public ComponentOrder ComponentOrder { get; private set; }

        public int Width { get; private set; }

        public void Read(Stream stream)
        {
            stream.Read(this.Data, 0, this.Data.Length);
        }

        public void Write(Stream stream)
        {
            stream.Write(this.Data, 0, this.Data.Length);
        }

        public void Dispose()
        {
          if (this.DataPointer == null)
          {
              return;
          }

          if (this.handle.IsAllocated)
          {
              this.handle.Free();
          }

          this.DataPointer = null;
        }

        private static int GetComponentCount(ComponentOrder componentOrder)
        {
          switch (componentOrder)
          {
            case ComponentOrder.BGR:
            case ComponentOrder.RGB:
              return 3;
            case ComponentOrder.BGRA:
            case ComponentOrder.RGBA:
              return 4;
          }

          throw new NotSupportedException();
        }
    }
}
