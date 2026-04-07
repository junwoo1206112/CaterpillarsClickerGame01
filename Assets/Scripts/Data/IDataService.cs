using System;
using System.Threading.Tasks;

namespace ClickerGame.Data
{
    public interface IDataService<T>
    {
        Task<T> LoadAsync(string path);
        Task SaveAsync(string path, T data);
        bool Validate(T data);
    }
}
