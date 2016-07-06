﻿namespace GenericImage.PackedVectors
{
    using System;
    using System.Numerics;

    /// <summary>
    /// Packed vector type containing four 8-bit unsigned normalized values ranging from 0 to 255.
    /// </summary>
    public struct Rgba32 : IPackedVector<uint>, IEquatable<Rgba32>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rgba32"/> struct. 
        /// </summary>
        /// <param name="r">The red component.</param>
        /// <param name="g">The green component.</param>
        /// <param name="b">The blue component.</param>
        /// <param name="a">The alpha component.</param>
        public Rgba32(float r, float g, float b, float a)
        {
            this.PackedValue = Pack(r, g, b, a);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rgba32"/> struct. 
        /// </summary>
        /// <param name="vector">
        /// Vector containing the components for the packed vector.
        /// </param>
        public Rgba32(Vector4 vector)
        {
            this.PackedValue = Pack(vector.X, vector.Y, vector.Z, vector.W);
        }

        /// <inheritdoc/>
        public uint PackedValue { get; set; }

        /// <summary>
        /// Compares two <see cref="Rgba32"/> objects for equality.
        /// </summary>
        /// <param name="left">
        /// The <see cref="Rgba32"/> on the left side of the operand.
        /// </param>
        /// <param name="right">
        /// The <see cref="Rgba32"/> on the right side of the operand.
        /// </param>
        /// <returns>
        /// True if the current left is equal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        public static bool operator ==(Rgba32 left, Rgba32 right)
        {
            return left.PackedValue == right.PackedValue;
        }

        /// <summary>
        /// Compares two <see cref="Rgba32"/> objects for equality.
        /// </summary>
        /// <param name="left">The <see cref="Rgba32"/> on the left side of the operand.</param>
        /// <param name="right">The <see cref="Rgba32"/> on the right side of the operand.</param>
        /// <returns>
        /// True if the current left is equal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        public static bool operator !=(Rgba32 left, Rgba32 right)
        {
            return left.PackedValue != right.PackedValue;
        }

        /// <inheritdoc/>
        public void PackVector(Vector4 vector)
        {
            Vector4 clamped = Vector4.Clamp(vector, Vector4.Zero, Vector4.One);
            this.PackedValue = Pack(clamped.X, clamped.Y, clamped.Z, clamped.W);
        }

        /// <inheritdoc/>
        public Vector4 ToVector4()
        {
            return new Vector4(
                ((this.PackedValue >> 16) & 0xFF) / 255f,
                ((this.PackedValue >> 8) & 0xFF) / 255f,
                (this.PackedValue & 0xFF) / 255f,
                ((this.PackedValue >> 24) & 0xFF) / 255f);
        }

        /// <inheritdoc/>
        public byte[] ToBytes()
        {
            return new[]
            {
                (byte)((this.PackedValue >> 16) & 0xFF),
                (byte)((this.PackedValue >> 8) & 0xFF),
                (byte)(this.PackedValue & 0xFF),
                (byte)((this.PackedValue >> 24) & 0xFF)
            };
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return (obj is Rgba32) && this.Equals((Rgba32)obj);
        }

        /// <inheritdoc/>
        public bool Equals(Rgba32 other)
        {
            return this.PackedValue == other.PackedValue;
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
        /// Sets the packed representation from the given component values.
        /// </summary>
        /// <param name="x">The x component.</param>
        /// <param name="y">The y component.</param>
        /// <param name="z">The z component.</param>
        /// <param name="w">The w component.</param>
        /// <returns>
        /// The <see cref="uint"/>.
        /// </returns>
        private static uint Pack(float x, float y, float z, float w)
        {
            return ((uint)Math.Round(x * 255f) << 16) |
                   ((uint)Math.Round(y * 255f) << 8) |
                   (uint)Math.Round(z * 255f) |
                   ((uint)Math.Round(w * 255f) << 24);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <param name="packed">
        /// The instance of <see cref="Rgba32"/> to return the hash code for.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        private int GetHashCode(Rgba32 packed)
        {
            return packed.PackedValue.GetHashCode();
        }
    }
}
