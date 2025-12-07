using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Common.Application
{

    public static class ImageResize
    {
        /// <summary>
        /// برای کوچک کردن عکس از این متد استفاده کنید
        /// </summary>
        /// <param name="inputImagePath">آدرس عکس را وارد کنید</param>
        /// <param name="outputPath">مسیری که قراره فایل بیت مپ ذخیره شود </param>
        /// <param name="newWidth">عرض عکس</param>
        /// <param name="new_height">ارتفاع عکس</param>
        public static void CreateBitMap(string inputImagePath, string outputPath, int newWidth, int new_height)
        {

            var inputDirectory = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{inputImagePath.Replace("/", "\\")}");
            #region OutPut
            var pathSplit = inputImagePath.Split('/');
            var imageName = pathSplit[^1];

            var folderName = Path.Combine(Directory.GetCurrentDirectory(), outputPath.Replace("/", "\\"));
            if (!Directory.Exists(folderName))
            {
                //Create Folder
                Directory.CreateDirectory(folderName);
            }
            var outputDirectory = Path.Combine(folderName, imageName);

            #endregion
            Image_resize(inputDirectory, outputDirectory, newWidth, new_height);
        }
        private static void Image_resize(string input_Image_Path, string output_Image_Path, int new_Width, int new_height)
        {

            const long quality = 55L;

            Bitmap source_Bitmap = new Bitmap(input_Image_Path);



            double dblWidth_origial = source_Bitmap.Width;

            double dblHeigth_origial = source_Bitmap.Height;

            double relation_heigth_width = dblHeigth_origial / dblWidth_origial;

            int new_Height = (int)(new_Width * relation_heigth_width);



            //< create Empty Drawarea >

            var new_DrawArea = new Bitmap(new_Width, new_Height);

            //</ create Empty Drawarea >



            using (var graphic_of_DrawArea = Graphics.FromImage(new_DrawArea))

            {

                //< setup >

                graphic_of_DrawArea.CompositingQuality = CompositingQuality.HighSpeed;

                graphic_of_DrawArea.InterpolationMode = InterpolationMode.HighQualityBicubic;

                graphic_of_DrawArea.CompositingMode = CompositingMode.SourceCopy;

                //</ setup >



                //< draw into placeholder >

                //*imports the image into the drawarea

                graphic_of_DrawArea.DrawImage(source_Bitmap, 0, 0, new_Width, new_Height);

                //</ draw into placeholder >



                //--< Output as .Jpg >--

                using (var output = System.IO.File.Open(output_Image_Path, FileMode.Create))

                {

                    //< setup jpg >

                    var qualityParamId = System.Drawing.Imaging.Encoder.Quality;

                    var encoderParameters = new EncoderParameters(1);

                    encoderParameters.Param[0] = new EncoderParameter(qualityParamId, quality);

                    //</ setup jpg >



                    //< save Bitmap as Jpg >

                    var codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(c => c.FormatID == ImageFormat.Jpeg.Guid);

                    new_DrawArea.Save(output, codec, encoderParameters);

                    //resized_Bitmap.Dispose ();

                    output.Close();

                    //</ save Bitmap as Jpg >

                }
                //--</ Output as .Jpg >--
                graphic_of_DrawArea.Dispose();
            }

            source_Bitmap.Dispose();

            //---------------</ Image_resize() >---------------

        }
    }
}
