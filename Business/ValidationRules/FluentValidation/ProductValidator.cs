using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
  public  class ProductValidator:AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(p => p.ProductName).NotEmpty().WithMessage("İsim boş geçilemez");
            RuleFor(p => p.ProductName).Length(2, 30);
            RuleFor(p => p.UnitPrice).GreaterThanOrEqualTo(0);
            RuleFor(p => p.UnitPrice).NotEmpty();
        }
    }
}
