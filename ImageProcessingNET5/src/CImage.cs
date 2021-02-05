using System;
using System.Collections.Generic;
using System.Text;

namespace ImageProcessingNET5.src
{
    public struct Pixel
    {
        public byte Blue, Green, Red, a;

        public Pixel(byte blue, byte green, byte red, byte a)
        {
            this.Blue = blue;
            this.Green = green;
            this.Red = red;
            this.a = a;
        }
    }

    public class CImage
    {
        public Pixel[] Grid;
        public Pixel[,] Matrix;

        public int width, height, nBits, stride;

        public CImage(int w, int h, int nbits, byte[] bytesPixels)
        {
            this.width = w;
            this.height = h;
            this.nBits = nbits;
            int max = this.width * this.height;

            this.stride = nbits * width / 8;

            Grid = new Pixel[bytesPixels.Length];
            Matrix = new Pixel[height, width];

            for (int i = 0; i < bytesPixels.Length - 3; i += 1)
            {
                Grid[i].a = bytesPixels[0 + i];
                Grid[i].Red = bytesPixels[1 + i];
                Grid[i].Green = bytesPixels[2 + i];
                Grid[i].Blue = bytesPixels[3 + i];
            }


            for (int y = 0; y < height; y += 1)
            {
                for (int x = 0; x < width; x += 1)
                {
                    Matrix[y, x].a = bytesPixels[y * stride + 4 * x + 0];
                    Matrix[y, x].Red = bytesPixels[y * stride + 4 * x + 1];
                    Matrix[y, x].Green = bytesPixels[y * stride + 4 * x + 2];
                    Matrix[y, x].Blue = bytesPixels[y * stride + 4 * x + 3];
                }
            }
        }


        public CImage(int nx, int ny, int nbits)
        {
            this.width = nx;
            this.height = ny;
            this.nBits = nbits;
            int max = this.width * this.height; // * (this.nBits / 8);

            Grid = new Pixel[max];
        }

        public byte[] GridAsBytes()
        {
            byte[] bytes = new byte[Grid.Length * 4];

            for (int i = 0; i < Grid.Length; i += 4)
            {
                bytes[0 + i] = Grid[i].a;
                bytes[1 + i] = Grid[i].Red;
                bytes[2 + i] = Grid[i].Green;
                bytes[3 + i] = Grid[i].Blue;
            }

            return bytes;
        }


        public byte[] MatrixAsBytes()
        {
            byte[] bytes = new byte[Matrix.Length * 4];

            for (int y = 0; y < height; y += 1)
            {
                for (int x = 0; x < width; x += 1)
                {
                    bytes[4 * x + 0 + y * stride] = Matrix[y, x].a;
                    bytes[4 * x + 1 + y * stride] = Matrix[y, x].Red;
                    bytes[4 * x + 2 + y * stride] = Matrix[y, x].Green;
                    bytes[4 * x + 3 + y * stride] = Matrix[y, x].Blue;
                }
            }

            return bytes;
        }




        public void Averaging(CImage input, int halfWidth)
        {
            int nR, nG, nB, nA, sumR, sumG, sumB, sumA;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    nR = nG = nB = nA = sumR = sumG = sumB = sumA = 0;

                    for (int yy = -halfWidth; yy <= halfWidth; yy++)
                    {
                        if (y + yy >= 0 && y + yy < height) //BOUNDS CHECK
                        {
                            for (int xx = -halfWidth; xx <= halfWidth; xx++)
                            {
                                if (x + xx >= 0 && x + xx < width) //BOUNDS CHECK
                                {
                                    //sumA += input.Grid[x + xx + width * (y + yy)].a;
                                    //sumR += input.Grid[x + xx + width * (y + yy)].Red;
                                    //sumG += input.Grid[x + xx + width * (y + yy)].Green;
                                    //sumB += input.Grid[x + xx + width * (y + yy)].Blue;

                                    sumA += input.Grid[x + xx + width * (y + yy)].a;
                                    sumR += input.Grid[x + xx + width * (y + yy)].Red;
                                    sumG += input.Grid[x + xx + width * (y + yy)].Green;
                                    sumB += input.Grid[x + xx + width * (y + yy)].Blue;

                                    nA++;
                                    nR++;
                                    nG++;
                                    nB++;
                                }
                            }
                        }
                    }

                    Grid[x + y % stride * y] = //    new Pixel(0, 0, 0, 0);

                      new Pixel(
                            (byte)((sumA + nA / 2) / nA),
                            (byte)((sumR + nR / 2) / nR),
                            (byte)((sumG + nG / 2) / nG),
                            (byte)((sumB + nB / 2) / nB)
                        );

                }
            }
        }

    }

    /**
    public class CImage
    {
        public Byte[] Grid;
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
            int max = width * height * (nBits / 8);
            Grid = new byte[max];
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
            int max = width * height * (nBits / 8);

            Grid = new byte[max];

            for (int i = 0; i < max; i++)
            {
                Grid[i] = img[i];
            }
        }



        public void Average(CImage input, int halfWidth)
        {
            int ns, sum;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    ns = sum = 0;
                    for (int yy = -halfWidth; yy <= halfWidth; yy++)
                    {
                        if (y + yy >= 0 && y + yy < height)
                        {
                            for (int xx = -halfWidth; xx <= halfWidth; xx++)
                            {
                                if (x + xx >= 0 && x + xx < width)
                                {
                                    sum += input.Grid[x + xx + width * (y + yy)];
                                    ns++;
                                }
                            }
                        }
                    }

                    Grid[x + width * y] = (byte)((sum + ns / 2) / ns);
                }
            }
        }


        public void Averaging(CImage input, int HalfWidth)
        {
            //int nS, sum;
            int nR, nG, nB, nX, sumR, sumG, sumB, sumX;

            for (int y = 0; y < height; y++) //================================
            {
                for (int x = 0; x < width ; x ++) //==============================
                {
                    //nS = sum = 0;
                    nR = nG = nB = nX = sumR = sumG = sumB = sumX = 0;

                    for (int yy = -HalfWidth; yy <= HalfWidth; yy++) //=======
                    {
                        if (y + yy >= 0 && y + yy < input.height)
                        {
                            for (int xx = -HalfWidth; xx <= HalfWidth; xx++) //===
                            {
                                if (x + xx >= 0 && x + xx < input.width)
                                {
                                    sumB += input.Grid[0 + x + xx + width * (y + yy)];
                                    sumG += input.Grid[1 + x + xx + width * (y + yy)];
                                    sumR += input.Grid[2 + x + xx + width * (y + yy)];
                                    sumX += input.Grid[3 + x + xx + width * (y + yy)];

                                    nB++;
                                    nG++;
                                    nR++;
                                    nX++;

                                    //sum += input.Grid[3 + x + xx + width * (y + yy)];
                                    //nS += 3;
                                }
                            } //====== end for (xx... ===================
                        }
                    } //======== end for (yy... =======================

                    //Grid[x + width * y] = (byte)((sum + nS / 2) / nS); //+nS/2 is for rounding
                    Grid[0 + x + width * y] = (byte)((sumB + nB / 2) / nB);
                    Grid[1 + x + width * y] = (byte)((sumG + nG / 2) / nG);
                    Grid[2 + x + width * y] = (byte)((sumR + nR / 2) / nR);
                    Grid[3 + x + width * y] = (byte)((sumX + nX / 2) / nX);

                } //============ end for (x... ========================
            } //============== end for (y... ========================
        }
    }
    */
}
