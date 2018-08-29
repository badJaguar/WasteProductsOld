﻿using System.Collections.Generic;

namespace WasteProducts.Logic.Common.Services
{
    /// <summary>
    /// Group administration service
    /// </summary>
    public interface IGroupService
    {
        /// <summary>
        /// Create - create new group
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="item">Object</param>
        void Create<T>(T item);
        /// <summary>
        /// Update - add or corect information in group
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="item">Object</param>
        void Update<T>(T item);
        /// <summary>
        /// Delete - delete group
        /// </summary>
        /// <param name="id">key</param>
        void Delete<T>(T id);
    }
}
