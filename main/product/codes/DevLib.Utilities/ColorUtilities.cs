﻿//-----------------------------------------------------------------------
// <copyright file="ColorUtilities.cs" company="YuGuan Corporation">
//     Copyright (c) YuGuan Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace DevLib.Utilities
{
    using System;
    using System.Drawing;

    /// <summary>
    /// Color Utilities.
    /// </summary>
    public static class ColorUtilities
    {
        /// <summary>
        /// Static Field _random.
        /// </summary>
        private static Random _random = new Random();

        /// <summary>
        /// Returns a random color.
        /// </summary>
        /// <returns>The result color.</returns>
        public static Color GetRandomColor()
        {
            return Color.FromKnownColor((KnownColor)_random.Next(1, 174));
        }
    }
}
