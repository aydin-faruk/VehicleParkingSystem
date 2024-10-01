namespace VehicleParkingSystem.Business.Core.Interfaces
{
    public interface IGenericService<T>
    {
        Task<T> Get(int id);
        Task<bool> Add(T data);
        Task<bool> Update(T data);
    }
}
