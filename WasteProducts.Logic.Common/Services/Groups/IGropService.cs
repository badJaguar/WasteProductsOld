﻿using System.Collections.Generic;

namespace WasteProducts.Logic.Common.Services
{
    public interface IGropService
    {
        void Create<T>(T item);
        void Update<T>(T item);
        void Delete<T>(T item);
        void Dispose();
    }
}
