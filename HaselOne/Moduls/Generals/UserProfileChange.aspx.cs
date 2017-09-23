using DAL;
using HaselOne.Services.Interfaces;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HaselOne
{
    public partial class UserProfileChange : System.Web.UI.Page
    {

        [Dependency]
        public IUserService _us { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                LoadIfExistUser();
            }
        }

        private void LoadIfExistUser()
        {
            if (Session["UserId"] != null)
            {
                int uid = Convert.ToInt32(Session["UserId"]);
                hdnUserId.Value = uid.ToString();
                Gn_User user = _us.GetUserById(uid);
                txtUserName.Value = user.Name;
                txtUserSurname.Value = user.Surname;
                txtUserUserName.Value = user.UserName;
                int lvl = 0;
                //if (user.UserLevel != null)
                //    lvl = Convert.ToInt32(user.UserLevel);
                txtUserEmail.Value = user.Email;
                txtUserGsm.Value = user.Gsm;
                txtUserPassword.Value = user.Password;
            }
            else btnUserUpdate.Visible = false;
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            lblmsg.InnerText = "";
            if ((FileUpload1.PostedFile != null) && (FileUpload1.PostedFile.ContentLength > 0))
            {
                Guid uid = Guid.NewGuid();
                string fn = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName);
                string SaveLocation = Server.MapPath("~/ProfileImages") + "\\" + uid + "_45_" + fn;
                string SaveLocationBig = Server.MapPath("~/ProfileImages") + "\\" + uid + "_750_" + fn;
                try
                {
                    string fileExtention = FileUpload1.PostedFile.ContentType;
                    int fileLenght = FileUpload1.PostedFile.ContentLength;
                    if (fileExtention == "image/png" || fileExtention == "image/jpeg" || fileExtention == "image/x-png")
                    {
                        if (fileLenght <= 1048576)
                        {
                            System.Drawing.Bitmap bmpPostedImage = new System.Drawing.Bitmap(FileUpload1.PostedFile.InputStream);
                            System.Drawing.Image objImage = ScaleImage(bmpPostedImage, 45);
                            objImage.Save(SaveLocation, ImageFormat.Png);
                            int userid = Convert.ToInt32(Session["UserId"]);
                            string routedLocation = "~/ProfileImages" + "\\" + uid + "_45_" + fn;
                            _us.UpdateUserImage(userid, routedLocation);
                            System.Drawing.Image objImage2 = ScaleImage(bmpPostedImage, 750);
                            objImage2.Save(SaveLocationBig, ImageFormat.Png);

                            lblmsg.InnerText = "Imaj yüklendi.";
                            lblmsg.Style.Add("Color", "Green");
                        }
                        else
                        {
                            lblmsg.InnerText = "Imaj boyutu 1 MB'tan fazla olmamalı";
                            lblmsg.Style.Add("Color", "Red");
                        }
                    }
                    else
                    {
                        lblmsg.InnerText = "Kabul edilmeyen Format!";
                        lblmsg.Style.Add("Color", "Red");
                    }
                }
                catch (Exception ex)
                {
                    lblmsg.InnerText = "Hata: " + ex.Message;
                    lblmsg.Style.Add("Color", "Red");
                }
            }

        }

        public static System.Drawing.Image ScaleImage(System.Drawing.Image image, int maxHeight)
        {
            var ratio = (double)maxHeight / image.Height;

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            using (var g = Graphics.FromImage(newImage))
            {
                g.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }

        protected void btnUserUpdate_ServerClick(object sender, EventArgs e)
        {
            try
            {
                int uid = Convert.ToInt32(Session["UserId"]);
                Gn_User user = _us.GetUserById(uid);
                user.Name = txtUserName.Value;
                user.Surname = txtUserSurname.Value;
                user.UserName = txtUserUserName.Value;
                user.Email = txtUserEmail.Value;
                user.Password = txtUserPassword.Value;
                user.Gsm = txtUserGsm.Value;
                if (FileUpload1.FileName.ToString().TrimStart().TrimEnd() != "")
                    user.ImagePath = FileUpload1.FileName;
                _us.UpdateUser(user);

                knlow.InnerText = "Bilgiler başarı ile güncellenmiştir";
            }
            catch(Exception ex)
            {
                knlow.InnerText = "Mesaj: " + ex.Message;
            }
        }
    }
}