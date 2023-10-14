// ---------------------------------------------------------------------------------------------
//  Copyright (c) 2021-2023, Jiaqi Liu. All rights reserved.
//  See LICENSE file in the project root for license information.
// ---------------------------------------------------------------------------------------------

namespace Engine.Core.Abstraction
{
    /// <summary>
    /// Interface for a texture factory that creates textures.
    /// </summary>
    public interface ITextureFactory
    {
        /// <summary>
        /// Creates a new texture with the specified width, height, and RGBA data.
        /// </summary>
        /// <param name="width">The width of the texture.</param>
        /// <param name="height">The height of the texture.</param>
        /// <param name="rgbaData">The RGBA data of the texture.</param>
        /// <returns>The created texture.</returns>
        public ITexture2D CreateTexture(int width, int height, byte[] rgbaData);

        /// <summary>
        /// Creates a new white texture.
        /// </summary>
        /// <returns>The created texture.</returns>
        public ITexture2D CreateWhiteTexture();
    }
}