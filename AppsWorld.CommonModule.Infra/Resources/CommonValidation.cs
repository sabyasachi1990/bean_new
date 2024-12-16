using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace AppsWorld.CommonModule.Infra
{
    public class CommonValidation
    {
        public static string ValidateObject<T>(T entity)
        {
            //List<ValidationResult> _lstValidationResult = new List<ValidationResult>();
            //Validator.TryValidateObject(entity, new ValidationContext(entity, null, null), _lstValidationResult, true);

            //if (_lstValidationResult.Any())
            //{
            //    IEnumerable<Error> _lstErrors = _lstValidationResult.Select(c =>
            //            new Error()
            //            {
            //                Message = c.ErrorMessage
            //            }
            //        );

            //    return _lstErrors;
            //}

            //return new List<Error>().AsEnumerable();
            List<ValidationResult> _lstValidationResult = new List<ValidationResult>();
            Validator.TryValidateObject(entity, new ValidationContext(entity, null, null), _lstValidationResult, true);
            StringBuilder _validations = new StringBuilder();
            if (_lstValidationResult.Any())
            {
                foreach (var item in _lstValidationResult)
                {
                    _validations.Append(item.ErrorMessage).Append(Environment.NewLine);
                }
                return _validations.ToString().Trim();
            }
            return string.Empty;
        }
    }
}
