﻿/*
 * Openize.HEIC 
 * Copyright (c) 2024-2025 Openize Pty Ltd. 
 *
 * This file is part of Openize.HEIC.
 *
 * Openize.HEIC is available under Openize license, which is
 * available along with Openize.HEIC sources.
 */

namespace Openize.Heic.Tests
{
    using Openize.Heic.Decoder;
    using System.Threading.Tasks;

    internal class Tests : TestsCore
    {
        /// <summary>
        /// Test decoding of the images generated by iphone.
        /// </summary>
        [Test]
        [TestCase("iphone_photo.heic")]
        [TestCase("iphone_portrait_photo.heic")]
        public void TestIphoneImages(string filename)
        {
            using (var fs = new FileStream(Path.Combine(SamplesPath, filename), FileMode.Open))
            {
                var image = HeicImage.Load(fs);
                var pixels = image.GetByteArray(PixelFormat.Argb32);
                CompareWithEthalon(filename, pixels);
            }
        }

        /// <summary>
        /// Test decoding of the derived image.
        /// Image sourse: Nokia.
        /// </summary>
        [Test]
        [TestCase("nokia/grid_960x640.heic")]
        [TestCase("nokia/overlay_1000x680.heic")]
        public void TestDerivedImages(string filename)
        {
            using (var fs = new FileStream(Path.Combine(SamplesPath, filename), FileMode.Open))
            {
                var image = HeicImage.Load(fs);
                var pixels = image.GetByteArray(PixelFormat.Argb32);
                CompareWithEthalon(filename, pixels);
            }
        }

        /// <summary>
        /// Test decoding of image collection.
        /// Image sourse: Nokia.
        /// </summary>
        [Test]
        [TestCase("nokia/random_collection_1440x960.heic")]
        public void TestCollection(string filename)
        {
            using (var fs = new FileStream(Path.Combine(SamplesPath, filename), FileMode.Open))
            {
                var image = HeicImage.Load(fs);

                foreach (var frame in image.Frames)
                {
                    var pixels = frame.Value.GetByteArray(PixelFormat.Argb32);
                    CompareWithEthalon(filename + "_" + frame.Key, pixels);
                }
            }
        }

        /// <summary>
        /// Test decoding of image with alpha data.
        /// Image is created with Gimp.
        /// </summary>
        [Test]
        [TestCase("gimp_rgb_420_with_alpha.heic")]
        public void TestAlphaLayer(string filename)
        {
            using (var fs = new FileStream(Path.Combine(SamplesPath, filename), FileMode.Open))
            {
                var image = HeicImage.Load(fs);
                var pixels = image.GetByteArray(PixelFormat.Argb32);
                CompareWithEthalon(filename, pixels);
            }
        }

        /// <summary>
        /// Test decoding of image with bigger HandlerBox.
        /// Image is captured with Samsung A71.
        /// </summary>
        [Test]
        [TestCase("samsung_a71.heic")]
        public void TestBiggerHandlerBox(string filename)
        {
            using (var fs = new FileStream(Path.Combine(SamplesPath, filename), FileMode.Open))
            {
                var image = HeicImage.Load(fs);
                var pixels = image.GetByteArray(PixelFormat.Argb32);
                CompareWithEthalon(filename, pixels);
            }
        }

        /// <summary>
        /// Test decoding of image with scaling list.
        /// Image is captured with iPhone 12 Pro.
        /// </summary>
        [Test]
        [TestCase("iphone_telephoto_with_scaling_list.heic")]
        public void TestScalingList(string filename)
        {
            using (var fs = new FileStream(Path.Combine(SamplesPath, filename), FileMode.Open))
            {
                var image = HeicImage.Load(fs);
                var pixels = image.GetByteArray(PixelFormat.Argb32);
                CompareWithEthalon(filename, pixels);
            }
        }

        /// <summary>
        /// Test decoding of the rotated and mirrored image.
        /// Image is captured with iPhone 14 Pro.
        /// </summary>
        [Test]
        [TestCase("iphone_rotated_and_mirrored.heic")]
        public void TestRotatedAndMirrored(string filename)
        {
            using (var fs = new FileStream(Path.Combine(SamplesPath, filename), FileMode.Open))
            {
                var image = HeicImage.Load(fs);
                var pixels = image.GetByteArray(PixelFormat.Argb32);
                CompareWithEthalon(filename, pixels);
            }
        }

        /// <summary>
        /// Test decoding of the image with log2CbSize issue.
        /// Image is captured with iPhone 14 Pro.
        /// </summary>
        [Test]
        [TestCase("iphone_log2CbSize_issue.heic")]
        public void TestLog2CbSizeIssue(string filename)
        {
            using (var fs = new FileStream(Path.Combine(SamplesPath, filename), FileMode.Open))
            {
                var image = HeicImage.Load(fs);
                var pixels = image.GetByteArray(PixelFormat.Argb32);
                CompareWithEthalon(filename, pixels);
            }
        }

        /// <summary>
        /// Test decoding of the image with CleanAperture Box.
        /// Image is created with Gimp.
        /// </summary>
        [Test]
        [TestCase("gimp_with_clap.heic")]
        public void TestCleanApertureBox(string filename)
        {
            using (var fs = new FileStream(Path.Combine(SamplesPath, filename), FileMode.Open))
            {
                var image = HeicImage.Load(fs);
                var pixels = image.GetByteArray(PixelFormat.Argb32);
                CompareWithEthalon(filename, pixels);
            }
        }

        /// <summary>
        /// Test decoding of the parallel processing decoding.
        /// </summary>
        [Test]
        public void TestParallelProcessing()
        {
            Task t1 = Task.Run(() => Read(Path.Combine(SamplesPath, "nokia", "grid_960x640.HEIC")));
            Task t2 = Task.Run(() => Read(Path.Combine(SamplesPath, "nokia", "overlay_1000x680.HEIC")));
            Task t3 = Task.Run(() => Read(Path.Combine(SamplesPath, "nokia", "random_collection_1440x960.HEIC")));

            Task.WaitAll(t1, t2, t3);

            void Read(string filename)
            {
                using (var fs = new FileStream(filename, FileMode.Open))
                {
                    var image = HeicImage.Load(fs);
                    var pixels = image.GetInt32Array(PixelFormat.Argb32);
                }
            }
        }
    }
}
