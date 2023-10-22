﻿using DataStorageLayer.Contexts;
using DataStorageLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System.Dynamic;

namespace DataStorageLayer
{
    public class DbGateway : IDisposable
    {
        private EfContext _database;
        public DbGateway(bool doMigrate = false) => _database = new EfContext(doMigrate);

        #region Transaction
        private IDbContextTransaction _transaction;
        public IDbContextTransaction BeginTransaction() => _transaction = _database.Database.BeginTransaction();
        public async Task<IDbContextTransaction> BeginTransactionAsync() => _transaction = await _database.Database.BeginTransactionAsync();

        public void CommitTransaction() => _database.Database.CommitTransaction();
        public async Task CommitTransactionAsync() => await _database.Database.CommitTransactionAsync();
        public void RollBacktransaction() => _database.Database.RollbackTransaction();
        public async Task RollBacktransactionAsync() => await _database.Database.RollbackTransactionAsync();
        #endregion

        #region CRUD
        public EntityEntry<T> Add<T>(T model) where T : class => _database.Add(model);
        public EntityEntry<T> Update<T>(T model) where T : class => _database.Update(model);
        public EntityEntry<T> Remove<T>(T model) where T : class => _database.Remove(model);
        public int SaveChanges() => _database.SaveChanges();
        public Task<int> SaveChangesAsync() => _database.SaveChangesAsync();
        #endregion

        #region User
        public User? GetUser(Guid guid) => _database.Users.FirstOrDefault(x => x.Guid == guid);
        public IQueryable<User> GetAllUser() => _database.Users.AsNoTracking();
        public void UpdateUserRangeOnlineData(bool isOnline, DateTime timeStamp) 
            =>_database.Users
                .ExecuteUpdate(x => x
                    .SetProperty(u => u.IsOnline, isOnline)
                    .SetProperty(u => u.LastLogin, timeStamp));
        #endregion

        #region Message
        public async Task<List<MessageModel>> GetAllMessages_byUser(Guid guid)
        {
            //searching as sender and as receiver
            var query = from mr in _database.MessageReceivers.AsNoTracking()
                    join m in _database.Messages.AsNoTracking()
                        on mr.MessageModelId equals m.Id
                    where mr.UserReceiveGuid == guid || m.SenderGuid == guid
                    select m;
            return await query.ToListAsync();
        }
        #endregion

        #region UserReceivers_Message
        #endregion
        public void Dispose() => _database.Dispose(); 
    }
}
