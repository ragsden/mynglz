using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Incyte.Entities;
using Incyte.Services;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Web;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using Newtonsoft.Json;

namespace CreateTestData
{
    class Program
    {
        static void Main(string[] args)
        {
            TestFBJSON();
            bool isn = bool.Parse("false");
            createUsersByBusiness();
            //NameValueCollection nvc = new NameValueCollection();
            //nvc.Add("id", "TTR");
            //nvc.Add("btn-submit-photo", "upload1");
            //HttpUploadFile("http://localhost:20676/user/upload1/1/jpeg",
            //     @"C:\inetpub\wwwroot\images\pic1.jpeg", "file", "image/jpeg", nvc);
        }

        public static void HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc)
        {
            
            //log.Debug(string.Format("Uploading {0} to {1}", file, url));
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            //wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            Stream rs = wr.GetRequestStream();

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in nvc.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, nvc[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, paramName, file, contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            try
            {
                wresp = wr.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                //log.Debug(string.Format("File uploaded, server response is: {0}", reader2.ReadToEnd()));
            }
            catch (Exception ex)
            {
                //log.Error("Error uploading file", ex);
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
            }
            finally
            {
                wr = null;
            }
        }

        static void createUsersByBusiness()
        {
            string baseurl = @"http://50.47.49.34/wyngr/images";
            //string picloc = @"C:\inetpub\wwwroot\images";
            string picloc = @"C:\inetpub\wwwroot\wyngr\images";
            int maxusercount = 10;
            List<Business> buss = GetBusiness();

            int j = 0;
            foreach (Business bus in buss)
            {
                bool male = true;
                for (int i = 0; i < 20; i++)
                {
                    CreateUser(male, baseurl, picloc, bus.BusinessId, j);
                    male = !male;
                    j++;
                }
            }

        }

        static void CreateUser(bool male, string url, string loc, int busid, int count)
        {
            int age = 30 + count;
            Incyte.GenderType gend = Incyte.GenderType.FEMALE;
            if(male)
                gend = Incyte.GenderType.MALE;

            UserService uss = new UserService();
            UserLogin ul = uss.LoginCreate(new UserLogin() { ExternalID = "EX" + count.ToString(), UserSourceType = Incyte.SourceType.Facebook });
            Console.WriteLine("USERID = " + ul.UserID + " ISNEW = " + ul.IsNew.ToString());
            UserInfo uf = uss.UserInfoCreate(new UserInfo() { UserID = ul.UserID,
                Age = short.Parse(age.ToString()),
                Gender = gend, OnlineStatusID = 1, 
                PictureLocation = url + "/pic" + ul.UserID.ToString() + ".Jpeg",
                Preference = (gend == Incyte.GenderType.MALE? Incyte.PreferenceType.FEMALE:Incyte.PreferenceType.MALE),
                 EmailAddress = "testemail" + ul.UserID.ToString() + "@mynglz.com",
                Handle = "Handle " + ul.UserID + " ISNEW : " + ul.IsNew.ToString(),
            InstantTitle = "Title " + ul.UserID.ToString()
           });

            if (System.IO.File.Exists(loc + @"\pic" + ul.UserID.ToString() + ".Jpeg"))
                System.IO.File.Delete(loc + @"\pic" + ul.UserID.ToString() + ".Jpeg");

            Bitmap bmp = CreateBitmapImage("USER ID " + ul.UserID.ToString() + " : " + gend.ToString());
            bmp.Save(loc + @"\pic" + ul.UserID.ToString() + ".Jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
            uss.CheckinCreate(uf.UserID, busid);
        }

        static List<Business> GetBusiness()
        {
            BusinessService bs = new BusinessService();
            return bs.BusinessGet(new Location() { Latitude = "1", Longitude = "2" });
            
        }

        static Bitmap CreateBitmapImage(string sImageText)
        {
            Bitmap objBmpImage = new Bitmap(1, 1);
            int intWidth = 0;
            int intHeight = 0;
            // Create the Font object for the image text drawing.

            Font objFont = new Font("Arial", 50, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);

            // Create a graphics object to measure the text's width and height.

            Graphics objGraphics = Graphics.FromImage(objBmpImage);

            // This is where the bitmap size is determined.

            intWidth = (int)objGraphics.MeasureString(sImageText, objFont).Width;

            intHeight = (int)objGraphics.MeasureString(sImageText, objFont).Height;

            // Create the bmpImage again with the correct size for the text and font.

            objBmpImage = new Bitmap(objBmpImage, new Size(intWidth, intHeight));
            // Add the colors to the new bitmap.

            objGraphics = Graphics.FromImage(objBmpImage);
            // Set Background color

            objGraphics.Clear(Color.White);

            objGraphics.SmoothingMode = SmoothingMode.AntiAlias;

            objGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            objGraphics.DrawString(sImageText, objFont, new SolidBrush(Color.FromArgb(102, 102, 102)), 0, 0);

            objGraphics.Flush();

            return (objBmpImage);

        }

        static void TestFBJSON()
        {
            string json = "{\"id\":\"759302593\",\"name\":\"Raghu Ram\",\"first_name\":\"Raghu\",\"last_name\":\"Ram\",\"link\":\"http://www.facebook.com/gvraghu\",\"username\":\"gvraghu\",\"gender\":\"male\",\"email\":\"gvraghu@hotmail.com\",\"timezone\":-7,\"locale\":\"en_US\",\"verified\":true,\"updated_time\":\"2012-03-27T21:32:46+0000\"}";
            FBUser root = JsonConvert.DeserializeObject<FBUser>(json);
        }
    }
}
