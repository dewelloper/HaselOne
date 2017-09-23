using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using BusinessObjects.Base;

namespace BusinessObjects
{
   
    public class CustomerInterviewsWrapper:BaseWrapper
    {
        #region AttributeFields
        [ZeroGreterReq(ErrorMessage = "cari zorunludur")]
        public int CustomerId { get; set; }

        [ZeroGreterReq(ErrorMessage = "Yetkili seçiniz")]
        public Nullable<int> AuthenticatorId { get; set; }

        [ZeroGreterReq(ErrorMessage = "Görüşen seçiniz")]
        public int UserId { get; set; }

        [ZeroGreterReq(ErrorMessage = "Görüşme tipi seçiniz")]
        public int InterviewTypeId { get; set; }
        #endregion AttributeFields

        #region NormalFields
        public int Id { get; set; }
        public Nullable<System.DateTime> InterviewDate { get; set; }
        public string Note { get; set; }
        public bool Interviewed { get; set; }
        public Nullable<int> ImportantId { get; set; }
        public bool IsDeleted { get; set; }
        public int CreateUserId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<int> UpdateUserId { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> DeleteUserId { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }


        #endregion NormalFields
        #region ExtraField Entity de olmayan field lar icin





        #endregion ExtraField Entity de olmayan field lar icin
        #region Metod

        public override bool Validate()
        {
           
            base.EntityValidate();
            if (!this.IsValid)
            {
                return this.IsValid;
            }

            this.ValidationResult = new List<TextValue>();
            return this.IsValid;
        }
        #endregion Metod
    }

    public class CustomerInterviewsFilter:Filter
    {
        public int AuthenticatorId { get; set; }
        public int CustomerId { get; set; }
        public bool IsDelete { get; set; } = false;
        public bool Desc_Id { get; set; }


    }

    public class InterviewViewModel
    {

    }

    public class NotificationsFilter
    {
        public  bool Desc_Id { get; set; }
        public int SenderUserId { get; set; }
    }

    public class ServiceResponse<T>
    {
        public bool HasError { get; set; }
        public string ErrorDetail { get; set; }
        public List<T> List { get; set; }
        public T Entity { get; set; }
    }
}
