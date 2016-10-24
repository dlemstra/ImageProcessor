// <copyright file="IPixelWriter.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageProcessorCore
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    internal unsafe abstract class PixelReaderWriterBase : IDisposable
    {
        protected delegate void DataCopier(byte* data);

        private readonly byte[] row;

        /// <summary>
        /// Provides a way to access the pixels from unmanaged memory.
        /// </summary>
        private readonly GCHandle pixelsHandle;

        protected int ComponentCount { get; private set; }

        protected DataCopier CopyMethod { get; set; }

        /// <summary>
        /// The position of the first pixel in the row.
        /// </summary>
        protected byte* PixelsBase;

        protected int Width { get; private set; }

        protected PixelReaderWriterBase(int width, int padding, ComponentOrder componentOrder)
        {
            this.Width = width;
            this.ComponentCount = GetComponentCount(componentOrder);
            this.row = new byte[(this.Width * this.ComponentCount) + padding];

            this.pixelsHandle = GCHandle.Alloc(this.row, GCHandleType.Pinned);
            this.PixelsBase = (byte*)this.pixelsHandle.AddrOfPinnedObject().ToPointer();
        }

        ~PixelReaderWriterBase()
        {
            this.Dispose();
        }

        protected void ReadRowCore(Stream stream, byte* output)
        {
            stream.Read(this.row, 0, this.row.Length);
            this.CopyMethod(output);
        }

        protected void WriteRowCore(byte* input, Stream stream)
        {
            this.CopyMethod(input);
            stream.Write(row, 0, row.Length);
        }

        private int GetComponentCount(ComponentOrder componentOrder)
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

        public void Dispose()
        {
            if (this.PixelsBase == null)
            {
                return;
            }

            if (this.pixelsHandle.IsAllocated)
            {
                this.pixelsHandle.Free();
            }

            this.PixelsBase = null;

            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SuppressFinalize to
            // take this object off the finalization queue 
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }
    }
}
