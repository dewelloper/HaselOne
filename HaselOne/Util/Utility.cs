using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft;
using static System.String;

namespace HaselOne.Util
{
    public static class Utility
    {
        static HASELONEEntities _context = new HASELONEEntities();

        public static List<Cm_MachineparkCategory> GetCategoryDropdownSource()
        {
            List<Cm_MachineparkCategory> mps = _context.Cm_MachineparkCategory.Where(k => k.ParentId == 0).ToList();
            List<int> subChildItemIds = new List<int>();
            List<Cm_MachineparkCategory> targetList = new List<Cm_MachineparkCategory>();
            Cm_MachineparkCategory cmpc = new Cm_MachineparkCategory();
            cmpc.CategoryName = "_Bütün Kategoriler";
            cmpc.Id = 0;
            targetList.Add(cmpc);
            foreach (Cm_MachineparkCategory cat in mps)
            {
                targetList.Add(cat);

                List<Cm_MachineparkCategory> mps2 = _context.Cm_MachineparkCategory.Where(k => k.ParentId == cat.Id).ToList();
                foreach (Cm_MachineparkCategory cat2 in mps2)
                {
                    if (subChildItemIds.Contains(cat2.Id))
                        continue;

                    int thisChildIsAParent = cat2.Id;
                    List<Cm_MachineparkCategory> subChilds = _context.Cm_MachineparkCategory.Where(k => k.ParentId == thisChildIsAParent).ToList();
                    subChildItemIds.AddRange(subChilds.Select(k => k.Id).ToList());
                    if (subChilds.Count > 0)
                    {
                        cat2.CategoryName = "&nbsp;&nbsp;" + cat2.CategoryName.Replace(".", "");
                        targetList.Add(cat2);
                        foreach (Cm_MachineparkCategory cat3 in subChilds)
                        {
                            cat3.CategoryName = "&nbsp;&nbsp;" + cat3.CategoryName.Replace(".", "");
                            targetList.Add(cat3);
                        }
                    }
                    else
                    {
                        cat2.CategoryName = "&nbsp;" + cat2.CategoryName.Replace("&nbsp;", "");
                        targetList.Add(cat2);
                    }
                }
            }

            return targetList;
        }

        public static void ClearDropDownDataSource(DropDownList ddlist)
        {
            for (int i = ddlist.Items.Count - 1; i >= 0; i--)
            {
                ddlist.Items.RemoveAt(i);
            }
        }

        public static List<int?> GetSubChilds(int catId)
        {
            List<int> childIds = _context.Cm_MachineparkCategory.Where(k => k.ParentId == catId).Select(m => m.Id).ToList();
            List<int?> allChildIds = new List<int?>();
            allChildIds.Add(catId);
            foreach (int pid in childIds)
            {
                allChildIds.Add(pid);
                List<int> childIds2 = _context.Cm_MachineparkCategory.Where(k => k.ParentId == pid).Select(m => m.Id).ToList();
                if (childIds2.Count > 0)
                {
                    foreach (int i in childIds2)
                    {
                        allChildIds.Add(i);
                        List<int> childIds3 = _context.Cm_MachineparkCategory.Where(k => k.ParentId == i).Select(m => m.Id).ToList();
                        foreach (int j in childIds3)
                            allChildIds.Add(j);
                    }
                }
            }
            return allChildIds;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
             (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            _context.Database.CommandTimeout = 300;
            HashSet<TKey> knownKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (knownKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static void LoadCategories(DropDownList ddCategories, List<Cm_MachineparkCategory> cats)
        {
            List<Cm_MachineparkCategory> mps = cats.Where(k => k.ParentId == 0).ToList();
            ddCategories.DataTextField = "CategoryName";
            ddCategories.DataValueField = "Id";


            List<int> subChildItemIds = new List<int>();
            List<Cm_MachineparkCategory> targetList = new List<Cm_MachineparkCategory>();
            //Cm_MachineparkCategory cmpc = new Cm_MachineparkCategory();
            //cmpc.CategoryName = "_Bütün Kategoriler";
            //cmpc.Id = 0;
            //targetList.Add(cmpc);
            foreach (Cm_MachineparkCategory cat in mps)
            {
                targetList.Add(cat);

                List<Cm_MachineparkCategory> mps2 = cats.Where(k => k.ParentId == cat.Id).ToList();
                foreach (Cm_MachineparkCategory cat2 in mps2)
                {
                    if (subChildItemIds.Contains(cat2.Id))
                        continue;

                    int thisChildIsAParent = cat2.Id;
                    List<Cm_MachineparkCategory> subChilds = cats.Where(k => k.ParentId == thisChildIsAParent).ToList();
                    subChildItemIds.AddRange(subChilds.Select(k => k.Id).ToList());
                    if (subChilds.Count > 0)
                    {
                        cat2.CategoryName = "." + cat2.CategoryName;
                        targetList.Add(cat2);
                        foreach (Cm_MachineparkCategory cat3 in subChilds)
                        {
                            cat3.CategoryName = ".." + cat3.CategoryName;
                            targetList.Add(cat3);
                        }
                    }
                    else
                    {
                        cat2.CategoryName = "." + cat2.CategoryName;
                        targetList.Add(cat2);
                    }
                }
            }
            ddCategories.DataSource = targetList;
            ddCategories.DataBind();
        }

        public static void LoadCombo<T>(DropDownList ddown, List<T> list, string dtTextField, string dtValField)
        {
            ddown.DataTextField = dtTextField;
            ddown.DataValueField = dtValField;
            ddown.DataSource = list;
            ddown.DataBind();
        }

        public static bool HasProperty(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName) != null;
        }

        public static DateTime? StringToDatetimeForJson(string strDate)
        {
            if (IsNullOrEmpty(strDate)) return null;
            if (strDate.Trim().Length > 0)
            {
                DateTime dateValue;
                if (DateTime.TryParseExact(strDate, "yyyy-MM-dd", new CultureInfo("tr-TR"), DateTimeStyles.None, out dateValue))
                {
                    return dateValue;
                }

            }
            return null;
        }

        public static bool IsNullOrZero(this int? a)
        {
            if (a == null) return true;
            if (a == 0) return true;

            return false;
        }
        public static bool IsNullOrZero(this int a)
        {
            if (a == null) return true;
            if (a == 0) return true;

            return false;
        }
        public static bool IsNullOrZero(this short? a)
        {
            if (a == null) return true;
            if (a == 0) return true;

            return false;
        }

    }

    public static class Logger
    {
        
        public static void Log(object o, bool isJs = false)
        {
            try
            {
                MailMessage mail = new MailMessage("mutabakat@hasel.com", "mehmet.temel@hasel.com");

                SmtpClient client = new SmtpClient();
                client.Port = 25;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new NetworkCredential("mutabakat@hasel.com", "muthasel13");
                client.Host = "mail.hasel.com";
                mail.Subject = "Haselone Hata" + (isJs ? "Javascript" : "Server");
                if (o is System.Web.Mvc.ExceptionContext)
                {
                    //jsonda timeout a dusuyordu
                    mail.Body = JsonConvert.SerializeObject((o as System.Web.Mvc.ExceptionContext).Exception, Formatting.Indented, new JsonSerializerSettings()
                    {
                        PreserveReferencesHandling = PreserveReferencesHandling.All,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,

                    });
                }
                else
                {
                    mail.Body = JsonConvert.SerializeObject(o, Formatting.Indented, new JsonSerializerSettings()
                    {
                        PreserveReferencesHandling = PreserveReferencesHandling.All,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,

                    });
                }

                client.Send(mail);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


    }

    public  class ExceptionCustom
    {
        public Exception Ex { get; set; }
        public string UserInfo { get; set; }

        public object Request { get; set; }

        public object UserId { get; set; }


    }
    public enum ResultType
    {
        [Description("w")]
        Warning = 1,
        [Description("e")]
        Error = 2,
        [Description("i")]
        Info,
        [Description("h")]
        Hide,
        Success

    }
   



}