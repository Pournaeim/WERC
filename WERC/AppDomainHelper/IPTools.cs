using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace WERC.AppDomainHelper
{
    public static class IPTools
    {
        public static Image ResizeImage(Image imgToResize, Size size)
        {

            int destWidth = size.Width;
            int destHeight = size.Height;

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }

        public static void Resize_Picture(ref Bitmap bmp, string Des, int FinalWidth, int FinalHeight, int ImageQuality)
        {
            System.Drawing.Bitmap NewBMP;
            System.Drawing.Graphics graphicTemp;

            int iWidth;
            int iHeight;
            if ((FinalHeight == 0) && (FinalWidth != 0))
            {
                iWidth = FinalWidth;
                iHeight = (bmp.Size.Height * iWidth / bmp.Size.Width);
            }
            else if ((FinalHeight != 0) && (FinalWidth == 0))
            {
                iHeight = FinalHeight;
                iWidth = (bmp.Size.Width * iHeight / bmp.Size.Height);
            }
            else
            {
                iWidth = FinalWidth;
                iHeight = FinalHeight;
            }

            NewBMP = new System.Drawing.Bitmap(iWidth, iHeight);
            graphicTemp = System.Drawing.Graphics.FromImage(NewBMP);
            graphicTemp.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
            graphicTemp.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            graphicTemp.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            graphicTemp.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphicTemp.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            graphicTemp.DrawImage(bmp, 0, 0, iWidth, iHeight);
            graphicTemp.Dispose();
            System.Drawing.Imaging.EncoderParameters encoderParams = new System.Drawing.Imaging.EncoderParameters();
            System.Drawing.Imaging.EncoderParameter encoderParam = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, ImageQuality);
            encoderParams.Param[0] = encoderParam;
            System.Drawing.Imaging.ImageCodecInfo[] arrayICI = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();

            //for (int fwd = 0; fwd <= arrayICI.Length - 1; fwd++)
            //{
            //    if (arrayICI[fwd].FormatDescription.Equals("JPEG"))
            //    {
            //        NewBMP.Save(, arrayICI[fwd], encoderParams);
            //    }
            //}

            bmp = NewBMP;

            //NewBMP.Dispose();
            //bmp.Dispose();
        }
        public static void ResizeImageWidthBySaveScale(ref Bitmap imgToResize, float width)
        {
            if (width == imgToResize.Width) return;

            float AcpectRatio = (float)imgToResize.Width / (float)imgToResize.Height;

            float NewHeight = 0;

            NewHeight = Math.Abs(width - imgToResize.Width) / AcpectRatio;

            if (width > imgToResize.Width)
            {
                NewHeight += imgToResize.Height;
            }
            else if (width < imgToResize.Width)
            {
                NewHeight = imgToResize.Height - NewHeight;
            }
            else
            {
                NewHeight = width;
            }
            Bitmap b = new Bitmap((int)width, (int)NewHeight);

            Graphics g = Graphics.FromImage((Image)b);

            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, width, NewHeight);
            g.Dispose();
            imgToResize = b;
        }

        public static void ResizeImageHeightBySaveScale(ref Bitmap imgToResize, float height)
        {

            float AcpectRatio = (float)imgToResize.Width / (float)imgToResize.Height;

            float NewWidth = 0;
            if (height > imgToResize.Width)
            {
                NewWidth = (imgToResize.Width >= imgToResize.Height) ? (height - imgToResize.Height) / AcpectRatio : (height - imgToResize.Height) * AcpectRatio;
                NewWidth += imgToResize.Height;
            }
            else
                if (height < imgToResize.Width)
            {
                NewWidth = (imgToResize.Width >= imgToResize.Height) ? (imgToResize.Height - height) / AcpectRatio : (imgToResize.Height - height) * AcpectRatio;
                NewWidth = imgToResize.Height - NewWidth;
            }
            else
            {
                NewWidth = height;
            }

            Bitmap b = new Bitmap((int)NewWidth, (int)height);

            Graphics g = Graphics.FromImage((Image)b);

            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, NewWidth, height);
            g.Dispose();
            imgToResize = b;
        }

        public static Image AddSizeFromBottomImage(Image imgToResize, Size size)
        {

            int destWidth = size.Width;
            int destHeight = size.Height;

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, imgToResize.Width, imgToResize.Height);
            g.Dispose();

            return (Image)b;
        }

        public static void MergeImagesFilesVerticallyApplyDrawing(
            string[] FileList,
            int ThumbnailWidth,
            int FullSizeWidth,
            int FileNo,
            string DestinationPath,
            string WatermarkFile)
        {
            Bitmap TempBitmap = null;

            foreach (string file in FileList)
            {
                string guid = Guid.NewGuid().ToString();
                TempBitmap = new Bitmap(file);

                ResizeImageWidthBySaveScale(ref TempBitmap, ThumbnailWidth);

                WriteWaterMark(WatermarkFile, "YOUR TEXT", ref TempBitmap);
                //DrawWaterMark(WatermarkFile, "YOUR TEXT", ref TempBitmap);

                TempBitmap.Save(DestinationPath + FileNo + "_" + guid + ".png", ImageFormat.Png);
                TempBitmap = null;

                ForceGCCollect();

                TempBitmap = new Bitmap(file);

                ResizeImageWidthBySaveScale(ref TempBitmap, FullSizeWidth);

                WriteWaterMark(WatermarkFile, "YOUR TEXT", ref TempBitmap);
                //DrawWaterMark(WatermarkFile, "YOUR TEXT", ref TempBitmap);
                TempBitmap.Save(DestinationPath + FileNo + "_" + guid + ".jpg", ImageFormat.Jpeg);

                TempBitmap = null;

                ForceGCCollect();
                FileNo++;
            }

            ForceGCCollect();
        }

        public static void WriteWaterMark(
           string WatermarkFile, string waterMarkText, ref Bitmap bitmap)
        {
            Graphics g = Graphics.FromImage(bitmap);

            // Trigonometry: Tangent = Opposite / Adjacent
            double tangent = (double)bitmap.Height /
                             (double)bitmap.Width;

            // convert arctangent to degrees
            double angle = Math.Atan(tangent) * (180 / Math.PI);

            // a^2 = b^2 + c^2 ; a = sqrt(b^2 + c^2)
            double halfHypotenuse = (Math.Sqrt((bitmap.Height
                                   * bitmap.Height) +
                                   (bitmap.Width *
                                   bitmap.Width))) / 2;

            // Horizontally and vertically aligned the string
            // This makes the placement Point the physical 
            // center of the string instead of top-left.
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            g.RotateTransform((float)angle);

            //g.DrawString(waterMarkText, new Font(new FontFamily("Times New Roman"), 26), new SolidBrush(Color.Violet),
            //             new Point((int)halfHypotenuse, -(bitmap.Height / 3)),
            //             stringFormat);

            Bitmap WatermarkPhoto = new Bitmap(WatermarkFile);
            int phWidth = WatermarkPhoto.Width;
            int phHeight = WatermarkPhoto.Height;
            ResizeImageWidthBySaveScale(ref WatermarkPhoto, bitmap.Width / 2);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            g.DrawImage(
                WatermarkPhoto,                               // Photo Image object
                (int)halfHypotenuse,
                0);
        }

        public static void DrawWaterMark(
               string WatermarkFile, string waterMarkText, ref Bitmap imgWatermark)
        {

            //create a image object containing the photograph to watermark
            Image imgPhoto = Image.FromFile(WatermarkFile);
            int phWidth = imgPhoto.Width;
            int phHeight = imgPhoto.Height;

            //load the Bitmap into a Graphics object 
            Graphics grPhoto = Graphics.FromImage(imgWatermark);

            //create a image object containing the watermark
            int wmWidth = imgWatermark.Width;
            int wmHeight = imgWatermark.Height;

            //------------------------------------------------------------
            //Step #1 - Insert Copyright message
            //------------------------------------------------------------

            //Set the rendering quality for this Graphics object
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;

            //Draws the photo Image object at original size to the graphics object.
            grPhoto.DrawImage(
                imgPhoto,                               // Photo Image object
                new Rectangle(0, 0, phWidth, phHeight), // Rectangle structure
                0,                                      // x-coordinate of the portion of the source image to draw. 
                0,                                      // y-coordinate of the portion of the source image to draw. 
                phWidth,                                // Width of the portion of the source image to draw. 
                phHeight,                               // Height of the portion of the source image to draw. 
                GraphicsUnit.Pixel);                    // Units of measure 

            //-------------------------------------------------------
            //to maximize the size of the Copyright message we will 
            //test multiple Font sizes to determine the largest posible 
            //font we can use for the width of the Photograph
            //define an array of point sizes you would like to consider as possiblities
            //-------------------------------------------------------
            int[] sizes = new int[] { 16, 14, 12, 10, 8, 6, 4 };

            //Since all photographs will have varying heights, determine a 
            //position 5% from the bottom of the image
            int yPixlesFromBottom = (int)(phHeight * .05);

            //Now that we have a point size use the Copyrights string height 
            //to determine a y-coordinate to draw the string of the photograph
            float xCenterOfImg = (phWidth / 2);

            //Define the text layout by setting the text alignment to centered
            StringFormat StrFormat = new StringFormat();
            StrFormat.Alignment = StringAlignment.Center;

            //define a Brush which is semi trasparent black (Alpha set to 153)
            SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(153, 0, 0, 0));

            //define a Brush which is semi trasparent white (Alpha set to 153)
            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(153, 255, 255, 255));

            //Draw the Copyright string a second time to create a shadow effect
            //Make sure to move this text 1 pixel to the right and down 1 pixel

            //Create a Bitmap based on the previously modified photograph Bitmap
            Bitmap bmWatermark = new Bitmap(imgWatermark);
            bmWatermark.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);
            //Load this Bitmap into a new Graphic Object
            Graphics grWatermark = Graphics.FromImage(bmWatermark);

            //To achieve a transulcent watermark we will apply (2) color 
            //manipulations by defineing a ImageAttributes object and 
            //seting (2) of its properties.
            ImageAttributes imageAttributes = new ImageAttributes();

            //The first step in manipulating the watermark image is to replace 
            //the background color with one that is trasparent (Alpha=0, R=0, G=0, B=0)
            //to do this we will use a Colormap and use this to define a RemapTable
            ColorMap colorMap = new ColorMap();

            //My watermark was defined with a background of 100% Green this will
            //be the color we search for and replace with transparency
            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);

            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            //The second color manipulation is used to change the opacity of the 
            //watermark.  This is done by applying a 5x5 matrix that contains the 
            //coordinates for the RGBA space.  By setting the 3rd row and 3rd column 
            //to 0.3f we achive a level of opacity
            float[][] colorMatrixElements = {
                                                new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  0.0f,  0.0f,  0.3f, 0.0f},
                                                new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}};
            ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default,
                ColorAdjustType.Bitmap);

            //For this example we will place the watermark in the upper right
            //hand corner of the photograph. offset down 10 pixels and to the 
            //left 10 pixles

            int xPosOfWm = ((phWidth - wmWidth) - 10);
            int yPosOfWm = 10;

            grWatermark.DrawImage(imgWatermark,
                new Rectangle(xPosOfWm, yPosOfWm, wmWidth, wmHeight),  //Set the detination Position
                0,                  // x-coordinate of the portion of the source image to draw. 
                0,                  // y-coordinate of the portion of the source image to draw. 
                wmWidth,            // Watermark Width
                wmHeight,		    // Watermark Height
                GraphicsUnit.Pixel, // Unit of measurment
                imageAttributes);   //ImageAttributes Object

            //Replace the original photgraphs bitmap with the new Bitmap
            bmWatermark = (Bitmap)imgWatermark;
        }
        public static Image ByteArrayToImage(byte[] byteArray)
        {
            MemoryStream ms = new MemoryStream(byteArray);
            Image image = Image.FromStream(ms);
            return image;
        }

        public static void ForceGCCollect()
        {
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
        }

        private static Image DrawText(String text, Font font, Color textColor, Color backColor)
        {
            //first, create a dummy bitmap just to get a graphics object
            Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);

            //measure the string to see how big the image needs to be
            SizeF textSize = drawing.MeasureString(text, font);

            //free up the dummy image and old graphics object
            img.Dispose();
            drawing.Dispose();

            //create a new image of the right size
            img = new Bitmap((int)textSize.Width, (int)textSize.Height);

            drawing = Graphics.FromImage(img);

            //paint the background
            drawing.Clear(backColor);

            //create a brush for the text
            Brush textBrush = new SolidBrush(textColor);

            drawing.DrawString(text, font, textBrush, 0, 0);

            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose();

            return img;

        }

    }
}
