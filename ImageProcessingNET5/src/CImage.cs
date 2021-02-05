using System;
using System.Collections.Generic;
using System.Text;

namespace ImageProcessingNET5.src
{
    public class CImage
    {
        public byte[] Grid;
        public int width, height, nBits;

        public CImage() { }

        /// <summary>
        /// Inicializa com array de bytes vazio, porém, de tamanho determinado.
        /// </summary>
        public CImage(int nx, int ny, int nbits) 
        {
            width = nx;
            height = ny;
            nBits = nbits;
            Grid = new byte[width * height * (nBits / 8)];
        }


        /// <summary>
        /// Inicializa fazendo a cópia byte a byte do array passado como parâmetro.
        /// </summary>
        /// <param name="nbits">Total de bits por pixel (canal * depth)</param>
        public CImage(int nx, int ny, int nbits, byte[] img) 
        {
            width = nx;
            height = ny;
            nBits = nbits;
            Grid = new byte[width * height * (nBits / 8)];
            
            int max = width * height * nBits / 8;

            for (int i = 0; i < max; i++)
            {
                Grid[i] = img[i];
            }
        }
    }
}
