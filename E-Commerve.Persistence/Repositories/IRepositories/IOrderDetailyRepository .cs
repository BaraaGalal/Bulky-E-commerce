﻿using E_Commerce.Domain.Models;

namespace E_Commerve.Persistence.Repositories.IRepositories
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        void Update(OrderDetail orderDetail);

    }
}
