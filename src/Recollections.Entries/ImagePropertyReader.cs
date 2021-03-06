﻿using ExifLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Recollections.Entries
{
    public partial class ImagePropertyReader : DisposableBase
    {
        private readonly ExifReader reader;

        public ImagePropertyReader(string imagePath)
        {
            try
            {
                reader = new ExifReader(imagePath);
            }
            catch (ExifLibException)
            {
                reader = null;
            }
        }

        public ImagePropertyReader(Stream imageContent)
        {
            try
            {
                reader = new ExifReader(imageContent);
            }
            catch (ExifLibException)
            {
                reader = null;
            }
        }

        public Orientation? FindOrientation()
        {
            ushort? value = Find<ushort>(ExifTags.Orientation);
            if (value == null)
                return null;

            return (Orientation)value.Value;
        }

        public DateTime? FindTakenWhen()
            => Find<DateTime>(ExifTags.DateTimeDigitized)
            ?? Find<DateTime>(ExifTags.DateTimeOriginal)
            ?? Find<DateTime>(ExifTags.DateTime);

        public double? FindLatitude()
            => FindCoordinate(ExifTags.GPSLatitude);

        public double? FindLongitude()
            => FindCoordinate(ExifTags.GPSLongitude);

        public double? FindAltitude()
        {
            if (reader == null)
                return null;

            if (reader.GetTagValue(ExifTags.GPSAltitude, out uint[] value))
            {
                if (value != null)
                    return value.FirstOrDefault();
            }

            return null;
        }

        private double? FindCoordinate(ExifTags type)
        {
            if (reader == null)
                return null;

            if (reader.GetTagValue(type, out double[] coordinates))
                return ToDoubleCoordinates(coordinates);

            return null;
        }

        private double ToDoubleCoordinates(double[] coordinates)
            => Math.Round(coordinates[0] + (coordinates[1] / 60f) + coordinates[2] / 3600f, 13);

        private T? Find<T>(ExifTags type)
            where T : struct
        {
            if (reader == null)
                return null;

            try
            {
                if (reader.GetTagValue(type, out T value))
                    return value;
            }
            catch (FormatException)
            { }

            return default;
        }

        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();

            if (reader != null)
                reader.Dispose();
        }
    }
}
