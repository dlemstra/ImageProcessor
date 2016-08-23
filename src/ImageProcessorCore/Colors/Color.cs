﻿// <copyright file="Color.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageProcessorCore
{
    using System;
    using System.Numerics;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Packed vector type containing four 8-bit unsigned normalized values ranging from 0 to 255.
    /// The color components are stored in red, green, blue, and alpha order.
    /// </summary>
    /// <remarks>
    /// This struct is fully mutable. This is done (against the guidelines) for the sake of performance,
    /// as it avoids the need to create new values for modification operations.
    /// </remarks>
    public partial struct Color : IPackedVector<uint>, IEquatable<Color>
    {
        private const float Max = 255F;
        private const float Min = 0F;
        private uint packedValue;

        /// <summary>
        /// Gets or sets the red component.
        /// </summary>
        public byte R
        {
            get
            {
                return (byte)this.packedValue;
            }
            set
            {
                this.packedValue = (uint)(this.packedValue & -0x100 | value);
            }
        }


        /// <summary>
        /// Gets or sets the green component.
        /// </summary>
        public byte G
        {
            get
            {
                return (byte)(this.packedValue >> 8);
            }
            set
            {
                this.packedValue = (uint)(this.packedValue & -0xff01 | (uint)value << 8);
            }
        }

        /// <summary>
        /// Gets or sets the blue component.
        /// </summary>
        public byte B
        {
            get
            {
                return (byte)(this.packedValue >> 16);
            }
            set
            {
                this.packedValue = (uint)(this.packedValue & -0xff0001 | (uint)(value << 16));
            }
        }

        /// <summary>
        /// Gets or sets the alpha component.
        /// </summary>
        public byte A
        {
            get
            {
                return (byte)(this.packedValue >> 24);
            }
            set
            {
                this.packedValue = this.packedValue & 0xffffff | (uint)value << 24;
            }
        }

        /// <summary>
        /// The packed value.
        /// </summary>
        public uint PackedValue { get { return this.packedValue; } set { this.packedValue = value; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> struct. 
        /// </summary>
        /// <param name="r">The red component.</param>
        /// <param name="g">The green component.</param>
        /// <param name="b">The blue component.</param>
        /// <param name="a">The alpha component.</param>
        public Color(byte r, byte g, byte b, byte a = 255)
            : this()
        {
            this.packedValue = (uint)(r | g << 8 | b << 16 | a << 24);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> struct.
        /// </summary>
        /// <param name="hex">
        /// The hexadecimal representation of the combined color components arranged
        /// in rgb, rrggbb, or aarrggbb format to match web syntax.
        /// </param>
        public Color(string hex)
            : this()
        {
            // Hexadecimal representations are layed out AARRGGBB to we need to do some reordering.
            hex = hex.StartsWith("#") ? hex.Substring(1) : hex;

            if (hex.Length != 8 && hex.Length != 6 && hex.Length != 3)
            {
                throw new ArgumentException("Hexadecimal string is not in the correct format.", nameof(hex));
            }

            if (hex.Length == 8)
            {
                this.packedValue =
                    (uint)(Convert.ToByte(hex.Substring(2, 2), 16)
                    | Convert.ToByte(hex.Substring(4, 2), 16) << 8
                    | Convert.ToByte(hex.Substring(6, 2), 16) << 16
                    | Convert.ToByte(hex.Substring(0, 2), 16) << 24);
            }
            else if (hex.Length == 6)
            {
                this.packedValue =
                    (uint)(Convert.ToByte(hex.Substring(0, 2), 16)
                    | Convert.ToByte(hex.Substring(2, 2), 16) << 8
                    | Convert.ToByte(hex.Substring(4, 2), 16) << 16
                    | 255 << 24);
            }
            else
            {
                string rh = char.ToString(hex[0]);
                string gh = char.ToString(hex[1]);
                string bh = char.ToString(hex[2]);

                this.packedValue =
                    (uint)(Convert.ToByte(rh + rh, 16)
                    | Convert.ToByte(gh + gh, 16) << 8
                    | Convert.ToByte(bh + bh, 16) << 16
                    | 255 << 24);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> struct. 
        /// </summary>
        /// <param name="r">The red component.</param>
        /// <param name="g">The green component.</param>
        /// <param name="b">The blue component.</param>
        /// <param name="a">The alpha component.</param>
        public Color(float r, float g, float b, float a = 1)
            : this()
        {
            Pack(ref r, ref g, ref b, ref a);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> struct. 
        /// </summary>
        /// <param name="vector">
        /// The vector containing the components for the packed vector.
        /// </param>
        public Color(Vector3 vector)
            : this()
        {
            float a = 1;
            Pack(ref vector.X, ref vector.Y, ref vector.Z, ref a);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> struct. 
        /// </summary>
        /// <param name="vector">
        /// The vector containing the components for the packed vector.
        /// </param>
        public Color(Vector4 vector)
            : this()
        {
            this.packedValue = Pack(ref vector);
        }

        /// <summary>
        /// Compares two <see cref="Color"/> objects for equality.
        /// </summary>
        /// <param name="left">
        /// The <see cref="Color"/> on the left side of the operand.
        /// </param>
        /// <param name="right">
        /// The <see cref="Color"/> on the right side of the operand.
        /// </param>
        /// <returns>
        /// True if the current left is equal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        public static bool operator ==(Color left, Color right)
        {
            return left.packedValue == right.packedValue;
        }

        /// <summary>
        /// Compares two <see cref="Color"/> objects for equality.
        /// </summary>
        /// <param name="left">The <see cref="Color"/> on the left side of the operand.</param>
        /// <param name="right">The <see cref="Color"/> on the right side of the operand.</param>
        /// <returns>
        /// True if the current left is equal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        public static bool operator !=(Color left, Color right)
        {
            return left.packedValue != right.packedValue;
        }

        /// <inheritdoc/>
        public uint GetPackedValue()
        {
            return this.packedValue;
        }

        /// <inheritdoc/>
        public void SetPackedValue(uint value)
        {
            this.packedValue = value;
        }

        /// <inheritdoc/>
        public void PackFromVector4(Vector4 vector)
        {
            this.packedValue = Pack(ref vector);
        }

        /// <inheritdoc/>
        public void PackFromBytes(byte x, byte y, byte z, byte w)
        {
            this.packedValue = (uint)(x | y << 8 | z << 16 | w << 24);
        }

        /// <inheritdoc/>
        public Vector4 ToVector4()
        {
            return new Vector4(this.R, this.G, this.B, this.A) / 255F;
        }

        /// <inheritdoc/>
        public byte[] ToBytes()
        {
            return new[] { this.R, this.G, this.B, this.A };
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return (obj is Color) && this.Equals((Color)obj);
        }

        /// <inheritdoc/>
        public bool Equals(Color other)
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
        /// Packs a vector into a uint.
        /// </summary>
        /// <param name="vector">The vector containing the values to pack.</param>
        /// <returns>The ulong containing the packed values.</returns>
        private static uint Pack(ref Vector4 vector)
        {
            // TODO: Maybe use Vector4.Clamp() instead.
            return (uint)((byte)Math.Round(vector.X * Max).Clamp(Min, Max)
                   | ((byte)Math.Round(vector.Y * Max).Clamp(Min, Max) << 8)
                   | (byte)Math.Round(vector.Z * Max).Clamp(Min, Max) << 16
                   | (byte)Math.Round(vector.W * Max).Clamp(Min, Max) << 24);
        }

        private static uint Pack(ref float x, ref float y, ref float z, ref float w)
        {
            return (uint)((byte)Math.Round(x * Max).Clamp(Min, Max)
                   | ((byte)Math.Round(y * Max).Clamp(Min, Max) << 8)
                   | (byte)Math.Round(z * Max).Clamp(Min, Max) << 16
                   | (byte)Math.Round(w * Max).Clamp(Min, Max) << 24);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <param name="packed">
        /// The instance of <see cref="Color"/> to return the hash code for.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetHashCode(Color packed)
        {
            return packed.packedValue.GetHashCode();
        }
    }
}