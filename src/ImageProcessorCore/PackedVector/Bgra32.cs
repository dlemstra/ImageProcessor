﻿// <copyright file="Bgra32.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageProcessorCore
{
    using System;
    using System.Numerics;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Packed vector type containing four 8-bit unsigned normalized values ranging from 0 to 1.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct Bgra32 : IPackedVector<Bgra32, uint>, IEquatable<Bgra32>
    {
        /// <summary>
        /// Gets or sets the blue component.
        /// </summary>
        [FieldOffset(0)]
        public byte B;

        /// <summary>
        /// Gets or sets the green component.
        /// </summary>
        [FieldOffset(1)]
        public byte G;

        /// <summary>
        /// Gets or sets the red component.
        /// </summary>
        [FieldOffset(2)]
        public byte R;

        /// <summary>
        /// Gets or sets the alpha component.
        /// </summary>
        [FieldOffset(3)]
        public byte A;

        /// <summary>
        /// The packed value.
        /// </summary>
        [FieldOffset(0)]
        private readonly uint packedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bgra32"/> struct. 
        /// </summary>
        /// <param name="b">The blue component.</param>
        /// <param name="g">The green component.</param>
        /// <param name="r">The red component.</param>
        /// <param name="a">The alpha component.</param>
        public Bgra32(float b, float g, float r, float a)
            : this()
        {
            Vector4 clamped = Vector4.Clamp(new Vector4(b, g, r, a), Vector4.Zero, Vector4.One) * 255f;
            this.B = (byte)Math.Round(clamped.X);
            this.G = (byte)Math.Round(clamped.Y);
            this.R = (byte)Math.Round(clamped.Z);
            this.A = (byte)Math.Round(clamped.W);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bgra32"/> struct. 
        /// </summary>
        /// <param name="b">The blue component.</param>
        /// <param name="g">The green component.</param>
        /// <param name="r">The red component.</param>
        /// <param name="a">The alpha component.</param>
        public Bgra32(byte b, byte g, byte r, byte a)
            : this()
        {
            this.B = b;
            this.G = g;
            this.R = r;
            this.A = a;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bgra32"/> struct. 
        /// </summary>
        /// <param name="vector">
        /// The vector containing the components for the packed vector.
        /// </param>
        public Bgra32(Vector4 vector)
            : this()
        {
            Vector4 clamped = Vector4.Clamp(vector, Vector4.Zero, Vector4.One) * 255f;
            this.B = (byte)Math.Round(clamped.X);
            this.G = (byte)Math.Round(clamped.Y);
            this.R = (byte)Math.Round(clamped.Z);
            this.A = (byte)Math.Round(clamped.W);
        }

        /// <summary>
        /// Compares two <see cref="Bgra32"/> objects for equality.
        /// </summary>
        /// <param name="left">
        /// The <see cref="Bgra32"/> on the left side of the operand.
        /// </param>
        /// <param name="right">
        /// The <see cref="Bgra32"/> on the right side of the operand.
        /// </param>
        /// <returns>
        /// True if the current left is equal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        public static bool operator ==(Bgra32 left, Bgra32 right)
        {
            return left.packedValue == right.packedValue;
        }

        /// <summary>
        /// Compares two <see cref="Bgra32"/> objects for equality.
        /// </summary>
        /// <param name="left">The <see cref="Bgra32"/> on the left side of the operand.</param>
        /// <param name="right">The <see cref="Bgra32"/> on the right side of the operand.</param>
        /// <returns>
        /// True if the current left is equal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        public static bool operator !=(Bgra32 left, Bgra32 right)
        {
            return left.packedValue != right.packedValue;
        }

        /// <inheritdoc/>
        public void Add(Bgra32 value)
        {
            this.B = (byte)(this.B + value.B);
            this.G = (byte)(this.G + value.G);
            this.R = (byte)(this.R + value.R);
            this.A = (byte)(this.A + value.A);
        }

        /// <inheritdoc/>
        public void Subtract(Bgra32 value)
        {
            this.B = (byte)(this.B - value.B);
            this.G = (byte)(this.G - value.G);
            this.R = (byte)(this.R - value.R);
            this.A = (byte)(this.A - value.A);
        }

        /// <inheritdoc/>
        public void Multiply(Bgra32 value)
        {
            this.B = (byte)(this.B * value.B);
            this.G = (byte)(this.G * value.G);
            this.R = (byte)(this.R * value.R);
            this.A = (byte)(this.A * value.A);
        }

        /// <inheritdoc/>
        public void Multiply(float value)
        {
            this.B = (byte)(this.B * value);
            this.G = (byte)(this.G * value);
            this.R = (byte)(this.R * value);
            this.A = (byte)(this.A * value);
        }

        /// <inheritdoc/>
        public void Multiply(double value)
        {
            this.B = (byte)(this.B * value);
            this.G = (byte)(this.G * value);
            this.R = (byte)(this.R * value);
            this.A = (byte)(this.A * value);
        }

        /// <inheritdoc/>
        public void Divide(Bgra32 value)
        {
            this.B = (byte)(this.B / value.B);
            this.G = (byte)(this.G / value.G);
            this.R = (byte)(this.R / value.R);
            this.A = (byte)(this.A / value.A);
        }

        /// <inheritdoc/>
        public void Divide(float value)
        {
            this.B = (byte)(this.B / value);
            this.G = (byte)(this.G / value);
            this.R = (byte)(this.R / value);
            this.A = (byte)(this.A / value);
        }

        /// <inheritdoc/>
        public void Divide(double value)
        {
            this.B = (byte)(this.B / value);
            this.G = (byte)(this.G / value);
            this.R = (byte)(this.R / value);
            this.A = (byte)(this.A / value);
        }

        /// <inheritdoc/>
        public uint PackedValue()
        {
            return this.packedValue;
        }

        /// <inheritdoc/>
        public void PackVector(Vector4 vector)
        {
            Vector4 clamped = Vector4.Clamp(vector, Vector4.Zero, Vector4.One) * 255f;
            this.B = (byte)Math.Round(clamped.X);
            this.G = (byte)Math.Round(clamped.Y);
            this.R = (byte)Math.Round(clamped.Z);
            this.A = (byte)Math.Round(clamped.W);
        }

        /// <inheritdoc/>
        public void PackBytes(byte x, byte y, byte z, byte w)
        {
            this.B = x;
            this.G = y;
            this.R = z;
            this.A = w;
        }

        /// <inheritdoc/>
        public Vector4 ToVector4()
        {
            return new Vector4(this.B, this.G, this.R, this.A) / 255f;
        }

        /// <inheritdoc/>
        public byte[] ToBytes()
        {
            return new[]
            {
                this.B,
                this.G,
                this.R,
                this.A
            };
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return (obj is Bgra32) && this.Equals((Bgra32)obj);
        }

        /// <inheritdoc/>
        public bool Equals(Bgra32 other)
        {
            return this.packedValue == other.packedValue;
        }

        /// <summary>
        /// Gets a string representation of the packed vector.
        /// </summary>
        /// <returns>A string representation of the packed vector.</returns>
        public override string ToString()
        {
            return this.ToVector4().ToString();
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return this.GetHashCode(this);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <param name="packed">
        /// The instance of <see cref="Bgra32"/> to return the hash code for.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        private int GetHashCode(Bgra32 packed)
        {
            return packed.packedValue.GetHashCode();
        }
    }
}
