// ---------------------------------------------------------------------------------------------
//  Copyright (c) 2021-2023, Jiaqi Liu. All rights reserved.
//  See LICENSE file in the project root for license information.
// ---------------------------------------------------------------------------------------------

namespace Engine.DataLoader
{
    using System;
    using System.Buffers;
    using Core.Abstraction;

    /// <summary>
    /// .tga file loader and Texture2D converter.
    /// </summary>
    public sealed class TgaTextureLoader : ITextureLoader
    {
        private readonly ITextureFactory _textureFactory;

        private short _width;
        private short _height;
        private byte[] _rawRgbaData;

        public TgaTextureLoader(ITextureFactory textureFactory)
        {
            _textureFactory = textureFactory;
        }

        public unsafe void Load(byte[] data, out bool hasAlphaChannel)
        {
            if (_rawRgbaData != null) throw new Exception("TGA texture already loaded");

            byte bitDepth;

            fixed (byte* p = &data[12])
            {
                _width = *(short*)p;
                _height = *(short*)(p + 2);
                bitDepth = *(p + 4);
            }

            var dataStartIndex = 18;

            _rawRgbaData = ArrayPool<byte>.Shared.Rent(_width * _height * 4);

            switch (bitDepth)
            {
                case 24:
                    hasAlphaChannel = false;
                    Decode24BitDataToRgba32(data, dataStartIndex);
                    break;
                case 32:
                    Decode32BitDataToRgba32(data, dataStartIndex, out hasAlphaChannel);
                    break;
                default:
                    throw new Exception("TGA texture had non 32/24 bit depth");
            }
        }

        private unsafe void Decode24BitDataToRgba32(byte[] data, int startIndex)
        {
            fixed (byte* srcStart = &data[startIndex], dstStart = _rawRgbaData)
            {
                byte* src = srcStart, dst = dstStart;
                for (var i = 0; i < _width * _height; i++, src += 3, dst += 4)
                {
                    *dst = *(src + 2);
                    *(dst + 1) = *(src + 1);
                    *(dst + 2) = *src;
                    *(dst + 3) = 0; // 24-bit don't have alpha
                }
            }
        }

        private unsafe void Decode32BitDataToRgba32(byte[] data, int startIndex, out bool hasAlphaChannel)
        {
            hasAlphaChannel = false;
            fixed (byte* srcStart = &data[startIndex], dstStart = _rawRgbaData)
            {
                var firstAlpha = *(srcStart + 3);
                byte* src = srcStart, dst = dstStart;
                for (var i = 0; i < _width * _height; i++, src += 4, dst += 4)
                {
                    *dst = *(src + 2);
                    *(dst + 1) = *(src + 1);
                    *(dst + 2) = *src;

                    byte alpha = *(src + 3);
                    *(dst + 3) = alpha;

                    if (alpha != firstAlpha) hasAlphaChannel = true;
                }
            }
        }

        public ITexture2D ToTexture()
        {
            if (_rawRgbaData == null) return null;

            try
            {
                return _textureFactory.CreateTexture(_width, _height, _rawRgbaData);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(_rawRgbaData);
                _rawRgbaData = null;
            }
        }
    }
}