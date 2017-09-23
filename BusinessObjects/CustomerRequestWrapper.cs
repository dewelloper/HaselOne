using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using BusinessObjects.Base;
using BusinessObjects.Base.StaticText;
using DAL;
using DAL.Helper;
using Newtonsoft.Json;

namespace BusinessObjects
{
    public class CustomerRequestWrapper : BaseWrapper, IBaseWrapper
    {
        //public CustomerRequestWrapper()
        //{
        //    InsertPre();
        //}

        #region AttributeFields

        [Required]
        public int Id { get; set; }

        [ZeroGreterReq(ErrorMessage = "Satış Tipi seçiniz")]
        public int SalesType { get; set; }

        //[ZeroGreterReq(ErrorMessage = "Sonuç seçiniz")]
        public int ResultType { get; set; }

        // [DisplayName("Kanal")] todo dısplayname den olusturcak hata mesajını

        [ZeroGreterReq(ErrorMessage = "Talep Kaynağı zorunludur")]
        public int ChannelId { get; set; }

        [ZeroGreterReq(ErrorMessage = "Talebi bulan zorunludur")]
        public Nullable<int> OwnerId { get; set; }

        [ZeroGreterReq(ErrorMessage = "Adet Zorunlu")]
        public Nullable<int> Quantity { get; set; }

        [ZeroGreterReq(ErrorMessage = "Kategori zorunludur")]
        public Nullable<int> CategoryId { get; set; }

        [RequiredDate(ErrorMessage = "Talep tarihi zorunludur")]
        public System.DateTime RequestDate { get; set; }

        [ZeroGreterReq(ErrorMessage = "Marka zorunludur")]
        public Nullable<int> MarkId { get; set; }

        [ZeroGreterReq(ErrorMessage = "Model zorunludur")]
        public Nullable<int> ModelId { get; set; }

        [ZeroGreterReq(ErrorMessage = "Satiş temsilcisi zorunludur")]
        public Nullable<int> SalesmanId { get; set; }

        [SelectReqNull]
        public Nullable<short> ConditionType { get; set; }

        [SelectReqNull]
        public Nullable<int> MonthlyWorkingHours { get; set; }

        [ZeroGreterReq(ErrorMessage = "cari zorunludur")]
        public int CustomerId { get; set; }

        [RequiredDate(ErrorMessage = "Satın alma tarihi zorunludur")]
        public Nullable<System.DateTime> EstimatedBuyDate { get; set; }

        #endregion AttributeFields

        #region ExtraField Entity de olmayan field lar icin

        public string ResultText
        {
            get
            {
                var item = new DictonaryStaticList().dicResultType;
                if (item.ContainsKey(this.ResultType))
                {
                    return item[this.ResultType];
                }
                return "-";
            }
        }

        public Mode FormMode { get; set; }

        #endregion ExtraField Entity de olmayan field lar icin

        #region NormalFields

        public Nullable<int> UseDuration { get; set; }
        public Nullable<int> UseDurationUnit { get; set; }

        public bool IsDeleted { get; set; }
        public int CreateUserId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<int> UpdateUserId { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> DeleteUserId { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }

        public string Note { get; set; }

        #endregion NormalFields

        #region Metod

        public override bool Validate()
        {
            //  var a =Text.SelectChanel;
            //if(ValidationResult==null) ValidationResult = new List<TextValue>();

            // this.ValidationResult.Clear();
            // this.IsValid = true;

            // var validationContext = new ValidationContext(this, null, null);
            // var validationResults = new List<ValidationResult>();
            // bool v =  Validator.TryValidateObject(this, validationContext, validationResults, true);
            //var val = new List<ValidationAttribute>();
            //var v = Validator.TryValidateProperty(this, validationContext, validationResults,);
            //var v = Validator.TryValidateProperty(this, validationContext , validationResults);
            var dicStatic = new DictonaryStaticList();
            base.EntityValidate();
            if (!this.IsValid)
            {
                return this.IsValid;
            }

            this.ValidationResult = new List<TextValue>();

            var estimatedBuyDate = this.EstimatedBuyDate;
            if (estimatedBuyDate != null)
            {
                TimeSpan tm = this.RequestDate - estimatedBuyDate.Value;
                if (tm.Days > 0)
                {
                    ValidationResult.Add(new TextValue(0, "Satın alma tarihi talep tarihinden eski olamaz"));
                }
            }


           

            //if (ResultType != (int)eResultType.Bekliyor)//satis veya kayip satis ise
            //{
            //    if (SalesType == (int)eSalesType.Kiralik)
            //    {
            //        ValidationResult.Add(new TextValue(0, "kiralik modulu hazır degildir. Satisi sonlandiramassiniz"));
            //    }
            //}

            if (this.Quantity < 1)
            {
                ValidationResult.Add(new TextValue(0, " Adet 0 dan büyük olmalı"));
            }
          

           

            if (ValidationResult.Count > 0) this.IsValid = false;

            return this.IsValid;
        }

        public bool DbValidate(Cm_CustomerRequest item, string CategoryName)
        {
            if (item == null)//yeni kayittir ve bu kontrole girmemesi gerekir
            {
                return true;
            }
            if (item.ResultType != (int)eResultType.Bekliyor)
            {
                if (item.CategoryId != this.CategoryId)
                {
                    this.IsValid = false;
                    this.ValidationResult.Add(new TextValue(1, "SONUCLANAN BIR TALEBIN KATEGORI DEGISTIRILEMEZ."));
                    return false;
                }
            }


            //kategoride diger key i varmi?
            {
                //string str = CategoryName;
                //if (string.IsNullOrEmpty(CategoryName)) return true;

                //if (str.ToLowerInvariant().IndexOf("diğer", StringComparison.Ordinal) > 0)
                //{
                //    this.IsValid = false;
                //    this.ValidationResult.Add(new TextValue(1, "Kategorilerde diğer seçilmez"));
                //    return false;
                //}

            }

            return true;
        }

       
        /// <summary>
        /// insert de resulttype i 3 cekiyor. ilk talep eklemede bekliyorda olmasi gerekiyor.
        /// </summary>
        public void InsertPre()
        {
            if (this.Id == 0 && this.ResultType == 0)
            {
                this.ResultType = (int)eResultType.Bekliyor;
            }


        }

        #endregion Metod
    }

    public struct RequestVm

    {
        public string Channel { get;    set; }
        public int CustomerId { get;    set; }
        public DateTime? EstimatedBuyDate { get;    set; }
        public int Id { get; set; }
        public int IdForCommand { get; set; }
        public string ModelName { get;    set; }
        public int? Quantity { get;    set; }
        public string SalesType { get; set; }
        public string ConditionType { get; set; }
        public string CategoryName { get;    set; }
        public string MarkName { get;    set; }
        public string Owner { get;    set; }
        public DateTime RequestDate { get;    set; }
        public string Salesman { get;    set; }
        public int? MonthlyWorkingHours { get; set; }
        public int ResultType { get; set; }
        public string UseDurationFull { get; set; }
        public int? UseDuration { get; set; }
        public string ResultText { get; set; }
        public int SerialNoHasntMacCount { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime CreateDate { get; set; }
        public int? CategoryId { get; set; }
        public int? MarkId { get; set; }
    }

    public class CustomerRequestFilter
    {
        public int CustomerId { get; set; }
        public bool IsDeleted { get; set; } = false;

        public int Id { get; set; }

        public RequestOpenCloseState OpenClose { get; set; }
    }


}