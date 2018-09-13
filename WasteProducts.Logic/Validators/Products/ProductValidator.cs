﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentAssertions;
using WasteProducts.Logic.Common.Models.Products;

namespace WasteProducts.Logic.Validators.Products
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Category.Name).NotEmpty().When(x => x.Category != null);
            RuleFor(x => x.Barcode.Code).NotEmpty().Matches(@"\d{13}").When(x => x.Barcode != null);
        }
    }
}
