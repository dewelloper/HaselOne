using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Base.StaticText
{
   public  static class Text
    {
        public static string Warning   ="Uyarı";
        public static string Error     ="Hata";
        public static string Info      ="Bilgi";
        public static string Success = "Başarılı";

         public static string Select { get; }=" Seçiniz";
        public  static string SelectChannel { get; } = "Kanal" + Select;

    }

}
