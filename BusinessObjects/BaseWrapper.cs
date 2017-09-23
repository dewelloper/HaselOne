using BusinessObjects.Base;
using DAL.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public interface IBaseWrapper
    {
    }

    public abstract class BaseWrapper
    {
        [JsonIgnore]
        public bool IsValid { get; set; }

        [JsonIgnore]
        public List<TextValue> ValidationResult { get; set; }

        public Mode FormMode { get; set; } = Mode.Edit;

        #region Metod

        protected void EntityValidate()
        {
            this.IsValid = true;
            List<ValidationResult> v;
            if (this.ValidationResult == null)
                this.ValidationResult = new List<TextValue>();
            var r = ValidationHelper.IsValid(this, out v);
            if (!r)
            {
                IsValid = false;
                foreach (var result in v)
                {
                    this.ValidationResult.Add(new TextValue
                    {
                        Text = result.ErrorMessage,
                        Value = 0
                    });
                }
            }
        }

        public virtual bool Validate()
        {
            if (ValidationResult.Count > 0) this.IsValid = false;
            return this.IsValid;
        }

        #endregion Metod
    }
}