using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using BusinessObjects.Base;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects
{
    public class MachineparkWrapper : BaseWrapper, IBaseWrapper
    {
        #region Entity

        [Required]
        public int Id { get; set; }

        [ZeroGreterReq(ErrorMessage = "Cari bilgisi zorunludur.")]
        public int CustomerId { get; set; }

        [SelectReqNull]
        public Nullable<int> LocationId { get; set; }

        public int OwnerId { get; set; }

        [ZeroGreterReq(ErrorMessage = "Kategori alanı zorunludur.")]
        public int CategoryId { get; set; }

        [SelectReqNull]
        public Nullable<int> MarkId { get; set; }

        [SelectReqNull]
        public Nullable<int> ModelId { get; set; }

        [SelectReqNull]
        public Nullable<int> RequestId { get; set; }

        public string SerialNo { get; set; }

        [SelectReqNull]
        public Nullable<int> ManufactureYear { get; set; }

        public Nullable<System.DateTime> SaleDate { get; set; }
        public Nullable<System.DateTime> ReleaseDate { get; set; }

        [ZeroGreterReq(ErrorMessage = "Adet bilgisi sıfırdan büyük olmak zorundadır.")]
        public int Quantity { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Nullable<int> RefRowId { get; set; }
        public int CreateUserId { get; set; }
        public System.DateTime CreateDate { get; set; }

        [SelectReqNull]
        public Nullable<int> UpdateUserId { get; set; }

        public Nullable<System.DateTime> UpdateDate { get; set; }

        [SelectReqNull]
        public Nullable<int> DeleteUserId { get; set; }

        public Nullable<System.DateTime> DeleteDate { get; set; }

        #endregion Entity

        #region Custom

        public bool HasRequest
        {
            get
            {
                return RequestId.HasValue;
            }
        }

        public string LocationName { get; set; }
        public string OwnerName { get; set; }
        public string CategoryName { get; set; }
        public string MarkName { get; set; }
        public string ModelName { get; set; }
        public string CreateUserName { get; set; }

        public MachineparkCategoryWrapper CmMachineparkCategory { get; set; }

        #endregion Custom

        #region Validate

        public override bool Validate()
        {
            base.EntityValidate();

            if (!string.IsNullOrEmpty(this.SerialNo) && this.Quantity > 1)
                ValidationResult.Add(new TextValue(0, "Adet birden fazla ise seri no girilemez."));

            //if (SaleDate.HasValue && ManufactureYear.HasValue && SaleDate.Value.Year < ManufactureYear.Value)
            //    ValidationResult.Add(new TextValue(0, "Satın alma yılı, üretim yılından küçük olamaz."));

            //if (ReleaseDate.HasValue && ManufactureYear.HasValue && ReleaseDate.Value.Year < ManufactureYear.Value)
            //    ValidationResult.Add(new TextValue(0, "Elden çıkarma alma yılı, üretim yılından küçük olamaz."));

            if (SaleDate.HasValue && ReleaseDate.HasValue && SaleDate.Value > ReleaseDate.Value)
                ValidationResult.Add(new TextValue(0, "Satın alma tarihi, elden çıkarma tarihinden büyük olamaz."));

            return base.Validate();
        }

        #endregion Validate
    }
}