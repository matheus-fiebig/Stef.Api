﻿using API.STEF.Data.Context;
using API.STEF.Domain.Shared.UnitOfWork;

namespace Stefanini.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StefaniniContext _context;

        public UnitOfWork(StefaniniContext context)
        {
            _context = context;
        }

        public async Task BeginTrasactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public async Task RollbackTrasactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }
    }
}