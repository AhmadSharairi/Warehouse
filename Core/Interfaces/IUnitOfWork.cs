namespace Core.Interfaces
{
    public interface IUnitOfWork
    {
        IWarehouseRepository WarehouseRepository { get; }
        IUserRepository  UserRepository{ get; }

        Task<int> CompleteAsync(); 
        void Dispose();
    }
}
