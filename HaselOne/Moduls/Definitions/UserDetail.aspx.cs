using DAL;
using DAL_Dochuman;
using HaselOne.Services.Interfaces;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HaselOne
{
    public partial class UserDetail : System.Web.UI.Page
    {

        [Dependency]
        public IUserService _us { get; set; }

        [Dependency]
        public ICustomerService _cs { get; set; }

        DCHEntities _dContext = new DCHEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Util.Utility.LoadCombo<Gn_Category>(ddUserAuthenticationGroup, _us.GetUserGroups().OrderBy(k => k.Title).ToList(), "Title", "Id");
                ddUserAuthenticationGroup.Items.Insert(0, new ListItem() { Value = "0", Text = "Seçiniz..." });

                Util.Utility.LoadCombo<Ns_BranchCode>(ddBranchCode, _us.GetBranchs().OrderBy(k => k.BranchName).ToList(), "BranchName", "NetsisBranchCode");
                ddBranchCode.Items.Insert(0, new ListItem() { Value = "0", Text = "Seçiniz..." });
                Util.Utility.LoadCombo<Gn_Area>(ddMainArea, _us.GetMainAreas().OrderBy(k => k.AreaName).ToList(), "AreaName", "Id");
                ddMainArea.Items.Insert(0, new ListItem() { Value = "0", Text = "Seçiniz..." });
                Util.Utility.LoadCombo<Gn_Department>(ddDepartments, _us.GetDepartments().OrderBy(k => k.DepartmentName).ToList(), "DepartmentName", "Id");
                ddDepartments.Items.Insert(0, new ListItem() { Value = "0", Text = "Seçiniz..." });
                
                LoadIfExistUser();
            }
        }

        private void LoadIfExistUser()
        {
            if (Request.QueryString["UId"] != null)
            {
                int uid = Convert.ToInt32(Request.QueryString["UId"]);
                hdnUserId.Value = uid.ToString();
                Gn_User user = _us.GetUserById(uid);
                txtUserName.Value = user.Name;
                txtUserSurname.Value = user.Surname;
                txtUserUserName.Value = user.UserName;
                txtUserEmail.Value = user.Email;
                txtUserPhone.Value = user.Phone;
                txtUserPosition.Value = user.Title;
                txtUserGsm.Value = user.Gsm;
                txtFaxNumber.Value = user.Fax;
                txtUserPassword.Value = user.Password;
                btnUserInsert.Visible = false;

                rbIsAdmin.SelectedIndex = user.IsAdmin == true ? 0 : 1;

                if (user.AreaId != null)
                {
                    var ubranch = ddMainArea.Items.FindByValue(user.AreaId.Value.ToString());
                    if (ubranch != null)
                    {
                        ddMainArea.SelectedItem.Text = ddMainArea.Items.FindByValue(user.AreaId.Value.ToString()).Text;
                        ddMainArea.SelectedItem.Value = ddMainArea.Items.FindByValue(user.AreaId.Value.ToString()).Value;
                    }
                }

                if (user.BranchCode != null)
                {
                    var ubranch = ddBranchCode.Items.FindByValue(user.BranchCode.Value.ToString());
                    if (ubranch != null)
                    {
                        ddBranchCode.SelectedItem.Text = ddBranchCode.Items.FindByValue(user.BranchCode.Value.ToString()).Text;
                        ddBranchCode.SelectedItem.Value = ddBranchCode.Items.FindByValue(user.BranchCode.Value.ToString()).Value;
                    }
                }

                DFSUserSet duser = _dContext.DFSUserSet.Where(k => k.OneId == user.Id).FirstOrDefault();
                DCH_KULLANICIBOLGE dkb = _dContext.DCH_KULLANICIBOLGE.Where(k => k.OneId == user.Id).FirstOrDefault();
                if(duser != null && dkb != null)
                {
                    if (dkb.GRUP != null)
                    {
                        var ubranch = ddUserAuthenticationGroup.Items.FindByText(dkb.GRUP.ToString());
                        if (ubranch != null)
                        {
                            ddUserAuthenticationGroup.SelectedItem.Text = ddUserAuthenticationGroup.Items.FindByText(dkb.GRUP.ToString()).Text;
                            ddUserAuthenticationGroup.SelectedItem.Value = ddUserAuthenticationGroup.Items.FindByText(dkb.GRUP.ToString()).Value;
                        }
                    }
                    if (dkb.DEPARTMAN != null)
                    {
                        var ubranch = ddDepartments.Items.FindByText(dkb.DEPARTMAN.ToString());
                        if (ubranch != null)
                        {
                            ddDepartments.SelectedItem.Text = ddDepartments.Items.FindByText(dkb.DEPARTMAN.ToString()).Text;
                            ddDepartments.SelectedItem.Value = ddDepartments.Items.FindByText(dkb.DEPARTMAN.ToString()).Value;
                        }
                    }
                }
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
                            int userid = Convert.ToInt32(Request.QueryString["UId"]);
                            string routedLocation = "~/ProfileImages" +"\\" + uid + "_45_" + fn;
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
            //if (FileUpload1.HasFile)
            //{
            //    string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
            //    FileUpload1.PostedFile.SaveAs(Server.MapPath("~/ProfileImages/") + fileName);
            //    Response.Redirect(Request.Url.AbsoluteUri);
            //}
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
    }
}