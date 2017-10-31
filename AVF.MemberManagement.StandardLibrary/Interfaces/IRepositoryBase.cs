﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace AVF.MemberManagement.StandardLibrary.Interfaces
{
    public interface IRepositoryBase<T, in TId>
    {
        Task<List<T>> GetAsync();

        Task<T> GetAsync(TId id);
    }
}
