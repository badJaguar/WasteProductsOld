﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.DataAccess.Common.Repositories.Products;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Common.Services;
using WasteProducts.Logic.Common.Services.Products;

namespace WasteProducts.Logic.Services.Products
{
    /// <summary>
    /// Implementation of ICategoryService.
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Tries to add a new category by name and returns whether the addition is successful or not.
        /// </summary>
        /// <param name="name">The name of the category to be added.</param>
        /// <returns>Boolean represents whether the addition is successful or not.</returns>
        public Task<string> Add(string name)
        {
            if (IsCategoryInDB(p =>
                string.Equals(p.Name, name, StringComparison.CurrentCultureIgnoreCase),
                out var categories)) return null;

            var newCategory = new Category { Name = name };
            return _categoryRepository.AddAsync(_mapper.Map<CategoryDB>(newCategory)).ContinueWith(c => c.Result);
        }

        /// <summary>
        /// Returns a spicific category by its name.
        /// </summary>
        /// <param name="name">The name of the category to be gotten.</param>
        /// <returns>The specific category to be returned.</returns>
        public Task<Category> Get(string name)
        {
            return _categoryRepository.GetByNameAsync(name).ContinueWith(t => _mapper.Map<Category>(t.Result));
        }

        /// <summary>
        /// Adds the description for specific category.
        /// </summary>
        /// <param name="category">The specific category for which a description is added.</param>
        /// <param name="description">The specific description for the specfic category.</param>
        public void SetDescription(Category category, string description)
        {
            if (!IsCategoryInDB(p =>
                    string.Equals(p.Name, category.Name, StringComparison.CurrentCultureIgnoreCase),
                out var categories)) return;

            var categoryFromDB = categories.ToList().First();
            categoryFromDB.Description = description;
            _categoryRepository.UpdateAsync(categoryFromDB);
        }

        /// <summary>
        /// Tries to delete the specific category.
        /// </summary>
        /// <param name="name">The name of the category to be deleted.</param>
        /// <returns>Boolean represents whether the deletion is successful or not.</returns>
        public bool Delete(string name)
        {
            if (!IsCategoryInDB(p =>
                    string.Equals(p.Name, name, StringComparison.CurrentCultureIgnoreCase),
                out var categories)) return false;

            var categoryFromDB = categories.ToList().First();
            _categoryRepository.DeleteAsync(categoryFromDB);

            return true;
        }

        /// <summary>
        /// Tries to delete the list of specific categories.
        /// </summary>
        /// <param name="names">The list of names of the categories to be deleted.</param>
        /// <returns>Boolean represents whether the deletion is successful or not.</returns>
        public bool DeleteRange(IEnumerable<string> names)
        {
            var result = false;

            foreach (var name in names)
            {
                if (Delete(name) && !result) result = true;
            }

            return result;
        }

        private bool IsCategoryInDB(Predicate<CategoryDB> conditionPredicate, out IEnumerable<CategoryDB> categories)
        {
            categories = _categoryRepository.SelectWhereAsync(conditionPredicate).Result;
            return categories.Any();
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _categoryRepository.Dispose();
                }

                _disposed = true;
            }
        }

        

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~CategoryService()
        {
            Dispose(false);
        }
    }
}
