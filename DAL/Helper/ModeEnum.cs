using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Helper
{
    public enum Mode
    {
        Insert,
        Edit,
        Readonly
    }

    public enum eSalesType
    {
        Satilik = 1,
        Kiralik = 2,
        Hepsi = 4
    }

    public enum eUseDurationunit
    {
        Gun = 1,
        Hafta = 7,
        Ay = 30,
        Yil = 365
    }

    public enum eResultType
    {
        Satis = 1,
        KayipSatis = 2,
        Bekliyor = 3
    }

    public enum eConditionType
    {
        Sifir = 1,
        IkinciEl = 2,
        Hepsi = 3
    }
    [Description("RequestOpenClose")]
    public enum RequestOpenCloseState
    {
        Open=1,
        Close = 2
    }
    public class DictonaryStaticList
    {

        public Dictionary<int, string> dicSalesType = new Dictionary<int, string>()
            {
                {1,"Satılık"},
                {2,"Kiralık"},
                {4, "Hepsi" },
            };
        public Dictionary<int, string> dicUseDurationUnitList = new Dictionary<int, string>()
            {
                {1,"Gün"},
                {7,"Hafta"},
                {30, "Ay" },
                {365, "Yıl" },
            };
        public Dictionary<int, string> dicResultType = new Dictionary<int, string>()
            {
                {1, "Satış"} ,
                { 2,"Kayıp Satış"},
                { 3,"Bekliyor"}
            };
        public Dictionary<int, string> dicConditionType = new Dictionary<int, string>()
            {
                {1, "Sıfır"} ,
                { 2,"2. El"},
                { 3,"Hepsi"}
            };
    }
}
