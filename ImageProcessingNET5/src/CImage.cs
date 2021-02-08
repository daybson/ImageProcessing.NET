using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;

namespace ImageProcessingNET5.src
{
    public struct Pixel
    {
        public byte B, G, R, a;

        public Pixel(byte a, byte red, byte green, byte blue)
        {
            this.a = a;
            this.R = red;
            this.G = green;
            this.B = blue;
        }
    }

    //----------------------------------------------------------------------------------------

    public class CImage
    {
        public Pixel[] Grid;
        public Pixel[,] Matrix;
        public byte[] Bytes;
        public int width, height, nBits, stride;

        #region Constructors

        public CImage(int w, int h, int nbits, byte[] bytesPixels)
        {
            this.width = w;
            this.height = h;
            this.nBits = nbits;

            this.stride = nbits * width / 8;

            this.Bytes = new byte[this.width * this.height * (this.nBits / 8)];
            this.Grid = new Pixel[height * width];
            this.Matrix = new Pixel[height, width];


            for (int i = 0; i < this.width * this.height * (this.nBits / 8); i++)
            {
                Bytes[i] = bytesPixels[i];
            }

            for (int i = 0; i < Grid.Length; i++)
            {
                Grid[i].a = bytesPixels[0 + i];
                Grid[i].R = bytesPixels[1 + i];
                Grid[i].G = bytesPixels[2 + i];
                Grid[i].B = bytesPixels[3 + i];
            }

            for (int y = 0; y < this.height; y++)
            {
                for (int x = 0; x < this.width; x++)
                {
                    this.Matrix[y, x].a = bytesPixels[y * stride + 4 * x + 0];
                    this.Matrix[y, x].R = bytesPixels[y * stride + 4 * x + 1];
                    this.Matrix[y, x].G = bytesPixels[y * stride + 4 * x + 2];
                    this.Matrix[y, x].B = bytesPixels[y * stride + 4 * x + 3];
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

        #endregion


        #region Overload Operators

        public static bool operator ==(CImage self, CImage other)
        {
            if (self.width != other.width || self.height != other.height)
                return false;

            for (int i = 0; i < self.Bytes.Length; i++)
            {
                if (self.Bytes[i] != other.Bytes[i])
                    return false;
            }

            return true;
        }

        public static bool operator !=(CImage self, CImage other)
        {
            if (self.width != other.width || self.height != other.height)
                return true;

            for (int i = 0; i < self.Bytes.Length; i++)
            {
                if (self.Bytes[i] != other.Bytes[i])
                    return true;
            }

            return false;
        }
        
        #endregion


        public byte[] GridAsBytes()
        {
            byte[] bytes = new byte[this.width * this.height * (this.nBits / 8)];

            for (int i = 0; i < this.Grid.Length; i++)
            {
                bytes[4 * i + 0] = Grid[i].a;
                bytes[4 * i + 1] = Grid[i].R;
                bytes[4 * i + 2] = Grid[i].G;
                bytes[4 * i + 3] = Grid[i].B;
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
                    bytes[4 * x + 1 + y * stride] = Matrix[y, x].R;
                    bytes[4 * x + 2 + y * stride] = Matrix[y, x].G;
                    bytes[4 * x + 3 + y * stride] = Matrix[y, x].B;
                }
            }

            return bytes;
        }


        #region Filters

        public void AveragingBytes(CImage input, int halfWidth)
        {
            for (int y = 0; y < this.height; y++)
            {
                for (int x = 0; x < this.width; x++)
                {
                    int sumA, sumR, sumG, sumB, ns;
                    ns = sumA = sumR = sumG = sumB = 0;

                    for (int yy = -halfWidth; yy <= halfWidth; yy++)
                    {
                        //BOUNDS CHECK
                        if (y + yy >= 0 && y + yy < input.height)
                        {
                            for (int xx = -halfWidth; xx <= halfWidth; xx++)
                            {
                                //BOUNDS CHECK
                                if (x + xx >= 0 && x + xx < input.width)
                                {
                                    var index = (y + yy) * stride + 4 * (xx + x);
                                    sumA += input.Bytes[index + 0];
                                    sumR += input.Bytes[index + 1];
                                    sumG += input.Bytes[index + 2];
                                    sumB += input.Bytes[index + 3];
                                    ns++;
                                }
                            }
                        }
                    }

                    this.Bytes[4 * x + 0 + y * this.stride] = (byte)((sumA + ns / 2) / ns);
                    this.Bytes[4 * x + 1 + y * this.stride] = (byte)((sumR + ns / 2) / ns);
                    this.Bytes[4 * x + 2 + y * this.stride] = (byte)((sumG + ns / 2) / ns);
                    this.Bytes[4 * x + 3 + y * this.stride] = (byte)((sumB + ns / 2) / ns);
                }
            }
        }

        public void AveragingGrid(CImage input, int halfWidth)
        {
            int sumR, sumG, sumB, sumA, ns;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    ns = sumR = sumG = sumB = sumA = 0;

                    for (int yy = -halfWidth; yy <= halfWidth; yy++)
                    {
                        // BOUNDS CHECK
                        if (y + yy >= 0 && y + yy < input.height)
                        {
                            for (int xx = -halfWidth; xx <= halfWidth; xx++)
                            {
                                // BOUNDS CHECK
                                if (x + xx >= 0 && x + xx < input.width)
                                {
                                    var index = (y + yy) * stride + 4 * (xx + x);
                                    sumA += input.Bytes[index + 0];
                                    sumR += input.Bytes[index + 1];
                                    sumG += input.Bytes[index + 2];
                                    sumB += input.Bytes[index + 3];
                                    ns++;
                                }
                            }
                        }
                    }

                    int xyw = x + y * this.width;
                    int ns2 = ns / 2;

                    Grid[xyw] =
                      new Pixel(
                            (byte)((sumA + ns2) / ns),
                            (byte)((sumR + ns2) / ns),
                            (byte)((sumG + ns2) / ns),
                            (byte)((sumB + ns2) / ns));
                }
            }
        }

        public void AveragingMatrix(CImage input, int halfWidth)
        {
            int sumR, sumG, sumB, sumA, ns;

            for (int y = 0; y < input.height; y++)
            {
                for (int x = 0; x < input.width; x++)
                {
                    ns = sumR = sumG = sumB = sumA = 0;

                    // LOOP FILTRO
                    for (int yy = -halfWidth; yy <= halfWidth; yy++)
                    {
                        // BOUNDS CHECK
                        if (y + yy >= 0 && y + yy < input.height)
                        {
                            for (int xx = -halfWidth; xx <= halfWidth; xx++)
                            {
                                // BOUNDS CHECK
                                if (x + xx >= 0 && x + xx < input.width)
                                {
                                    var index = (y + yy) * stride + 4 * (xx + x);
                                    sumA += input.Bytes[index + 0];
                                    sumR += input.Bytes[index + 1];
                                    sumG += input.Bytes[index + 2];
                                    sumB += input.Bytes[index + 3];
                                    ns++;
                                }
                            }
                        }
                    }
                    // END LOOP FILTRO
                    
                    int ns2 = ns / 2;
                    Matrix[y, x] =
                      new Pixel(
                            (byte)((sumA + ns2) / ns),
                            (byte)((sumR + ns2) / ns),
                            (byte)((sumG + ns2) / ns),
                            (byte)((sumB + ns2) / ns));
                }
            }
        }

        #endregion
    }
}
