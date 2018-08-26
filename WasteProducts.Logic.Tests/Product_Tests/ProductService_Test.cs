﻿using System;
using System.Text;
using System.Collections.Generic;
using AutoMapper;
using Moq;
using NUnit.Framework;
using WasteProducts.DataAccess.Common.Models.Barcods;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.DataAccess.Common.Repositories;
using WasteProducts.DataAccess.Repositories;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Mappings;
using WasteProducts.Logic.Services;

namespace WasteProducts.Logic.Tests.Product_Tests
{
    /// <summary>
    /// Summary description for ProductService_Test
    /// </summary>
    [TestFixture]
    class ProductService_Test
    {
        private Barcode barcode;
        private BarcodeDB barcodeDB;
        private Product product;
        private ProductDB productDB;
        private List<ProductDB> selectedList;
        private MapperConfiguration mapConfig;
        private Mapper mapper;
        private Mock<IProductRepository> mockProductRepository;
        private Category category;

        [SetUp]
        public void Init()
        {
            barcode = new Barcode
            {
                Id = (new Guid()).ToString(),
                Code = "456731556",
                ProductName = "Some product"
            };
            barcodeDB = new BarcodeDB
            {
                Id = (new Guid()).ToString(),
                Code = "456731556",
                ProductName = "Some product"
            };

            selectedList = new List<ProductDB>();

            mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDB>()
                    .ForMember(m => m.Created,
                        opt => opt.MapFrom(p => p.Name != null ? DateTime.UtcNow : default(DateTime)))
                    .ForMember(m => m.Modified, opt => opt.UseValue((DateTime?) null))
                    .ForMember(m => m.Barcode, opt => opt.Ignore())
                    .ReverseMap();
                cfg.AddProfile<CategoryProfile>();
            });

             mapper = new Mapper(mapConfig);

             mockProductRepository = new Mock<IProductRepository>();

            category = new Category
            {
                Name = "Vegetables",
                Description = "Some description",
                Products = new List<Product>()
            };

            productDB = new ProductDB {Id = (new Guid()).ToString(), Name = "Some name"};
        }

        [Test]
        public void AddByBarcode_InsertNewProduct_returns_true()
        {
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            var result = productService.AddByBarcode(barcode);

            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void AddByBarcode_DoNotInsertNewProduct_returns_false()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            var result = productService.AddByBarcode(barcode);

            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void AddByBarcode_InsertNewProduct_callsMethod_AddOfRepository()
        {
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            productService.AddByBarcode(barcode);

            mockProductRepository.Verify(m => m.Add(It.IsAny<ProductDB>()), Times.Once);
        }

        [Test]
        public void AddByBarcode_DoNotInsertNewProduct_DoesNotcallMethod_AddOfRepository()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            productService.AddByBarcode(barcode);

            mockProductRepository.Verify(m => m.Add(It.IsAny<ProductDB>()), Times.Never);
        }

        [Test]
        public void AddByName_InsertNewProduct_returns_true()
        {
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            var result = productService.AddByName(It.IsAny<string>());

            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void AddByName_DoNotInsertNewProduct_returns_false()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            var result = productService.AddByName(It.IsAny<string>());

            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void AddByName_InsertNewProduct_callsMethod_AddOfRepository()
        {
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            productService.AddByName(It.IsAny<string>());

            mockProductRepository.Verify(m => m.Add(It.IsAny<ProductDB>()), Times.Once);
        }

        [Test]
        public void AddByName_DoNotInsertNewProduct_DoesNotcallMethod_AddOfRepository()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            productService.AddByName(It.IsAny<string>());

            mockProductRepository.Verify(m => m.Add(It.IsAny<ProductDB>()), Times.Never);
        }

        [Test]
        public void AddCategoty_AddsCategoryInProductWithoutCategory_returns_true()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            var result = productService.AddCategory(product, category);

            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void AddCategoty_DoesNotAddCategoryInProductWithCategory_returns_false()
        {
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            var result = productService.AddCategory(product, category);

            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void AddCategoty_AddsCategoryInProductWithoutCategory_callsMethod_UpdateOfRepository()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            productService.AddCategory(product, category);

            mockProductRepository.Verify(m => m.Update(It.IsAny<ProductDB>()), Times.Once);
        }

        [Test]
        public void AddCategoty_DoesNotAddCategoryInProductWithCategory_DoesNotcallMethod_UpdateOfRepository()
        {
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            productService.AddCategory(product, category);

            mockProductRepository.Verify(m => m.Update(It.IsAny<ProductDB>()), Times.Never);
        }
    }
}
