﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.DataAccess.Common.Repositories.Products;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Common.Services.Barcods;
using WasteProducts.Logic.Common.Services.Products;

namespace WasteProducts.Logic.Services.Products
{
    /// <summary>
    /// Implementation of IProductService.
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBarcodeService _barcodeService;
        private readonly IMapper _mapper;
        private bool _disposed;

        public ProductService(IProductRepository productRepository, 
            ICategoryRepository categoryRepository, 
            IBarcodeService barcodeService, 
            IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _barcodeService = barcodeService;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public Task<string> AddAsync(Stream imageStream)
        {
            if (imageStream == null) return null;

            var barcode = _barcodeService.GetBarcodeByStreamAsync(imageStream).Result;
            if (barcode == null) return null;

            if (IsProductsInDB(
                p => p.Barcode != null && string.Equals(p.Barcode.Code, barcode.Code, StringComparison.CurrentCultureIgnoreCase),
                out var products))
            {
                return new Task<string>(() => products.First().Id);
            }

            var newProduct = new Product
            {
                Barcode = barcode,
                Name = barcode.ProductName,
                Composition =  barcode.Composition,
                Brand = barcode.Brand,
                Country = barcode.Country,
                Weight = barcode.Weight,
                PicturePath = barcode.PicturePath,
            };

            return _productRepository.AddAsync(_mapper.Map<ProductDB>(newProduct))
                .ContinueWith(t => t.Result);
        }

        /// <inheritdoc/>
        public Task<string> AddAsync(string name)
        {
            if (IsProductsInDB(
                p => string.Equals(p.Name, name, StringComparison.CurrentCultureIgnoreCase),
                out var products))
            {
                return null;
            }

            var newProduct = new Product
            {
                Name = name,
                Barcode = new Barcode()
            };

            return _productRepository.AddAsync(_mapper.Map<ProductDB>(newProduct))
                .ContinueWith(t => t.Result);
        }

        /// <inheritdoc/>
        public Task<Product> GetByIdAsync(string id)
        {
            return _productRepository.GetByIdAsync(id)
                .ContinueWith(t => _mapper.Map<Product>(t.Result));
        }

        /// <inheritdoc/>
        public Task<Product> GetByBarcodeAsync(Barcode barcode)
        {
            return _productRepository.SelectWhereAsync(p =>
                    string.Equals(p.Barcode.Code, barcode.Code, StringComparison.OrdinalIgnoreCase))
                 .ContinueWith(t => _mapper.Map<Product>(t.Result.First()));
        }

        /// <inheritdoc/>
        public Task<IEnumerable<Product>> GetAllAsync()
        {
            return _productRepository.SelectAllAsync()
                .ContinueWith(t => _mapper.Map<IEnumerable<Product>>(t.Result));
        }

        /// <inheritdoc/>
        public Task<Product> GetByNameAsync(string name)
        {
            return _productRepository.SelectWhereAsync(p =>
                 string.Equals(p.Name, name, StringComparison.CurrentCultureIgnoreCase))
                 .ContinueWith(t => _mapper.Map<Product>(t.Result.First()));
        }

        /// <inheritdoc/>
        public Task<IEnumerable<Product>> GetByCategoryAsync(Category category)
        {
            return _productRepository.SelectByCategoryAsync(_mapper.Map<CategoryDB>(category))
                .ContinueWith(t => _mapper.Map<IEnumerable<Product>>(t.Result));
        }

        /// <inheritdoc/>
        public Task UpdateAsync(Product product)
        {
            if (!IsProductsInDB(p =>
                    string.Equals(p.Id, product.Id, StringComparison.CurrentCultureIgnoreCase),
                out IEnumerable<ProductDB> products)) return null;

            return _productRepository.UpdateAsync(_mapper.Map<ProductDB>(product));
        }

        /// <inheritdoc/>
        public Task DeleteAsync(string id)
        {
            if (!IsProductsInDB(p =>
                    string.Equals(p.Id, id, StringComparison.CurrentCultureIgnoreCase),
                out var products)) return null;

            return _productRepository.DeleteAsync(id);
        }

        /// <inheritdoc/>
        public Task AddToCategoryAsync(string productId, string categoryId)
        {
            return _productRepository.AddToCategoryAsync(productId, categoryId);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _productRepository?.Dispose();
                _disposed = true;
                GC.SuppressFinalize(this);
            }
        }

        private bool IsProductsInDB(Predicate<ProductDB> conditionPredicate, out IEnumerable<ProductDB> products)
        {
            products = _productRepository.SelectWhereAsync(conditionPredicate).Result;
            return products.Any();
        }

        ~ProductService()
        {
            Dispose();
        }
    }
}
