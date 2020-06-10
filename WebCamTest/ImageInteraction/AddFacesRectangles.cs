using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Json;
using System.Text;

namespace WebCamTest
{
    static class AddFacesRectangles
    {

        public static void WorkWithImage(string jsonData)
        {

            if (!File.Exists("image.jpg")) return;

            try
            {

                Bitmap image = new Bitmap("image.jpg");
                JsonValue json = JsonValue.Parse(jsonData);

                using (Graphics gr = Graphics.FromImage(image))
                {
                    using (Pen thick_pen = new Pen(Color.LimeGreen, 5))
                    {
                        foreach (JsonValue item in json)
                        {

                            string gender = item["faceAttributes"]["gender"];
                            int age = item["faceAttributes"]["age"];

                            int top = item["faceRectangle"]["top"];
                            int left = item["faceRectangle"]["left"];
                            int width = item["faceRectangle"]["width"];
                            int height = item["faceRectangle"]["top"];

                            Rectangle rect = new Rectangle(left, top, width, height);
                            string textForDraw = $"Age: {age}";

                            int textGeight = height / 10;

                            gr.DrawRectangle(thick_pen, rect);
                            gr.DrawString(textForDraw, new System.Drawing.Font("Tahoma", textGeight, FontStyle.Regular), Brushes.White, new PointF(left, top + 3));
                        }
                    }
                }

                if (File.Exists("image_temp.jpg")) File.Delete("image_temp.jpg");
                image.Save("image_temp.jpg", ImageFormat.Jpeg);
            }
            catch (Exception)
            {
                /// Azure request limit reached
            }
        }
    }
}
