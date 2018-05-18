namespace SmartValley.Domain.Core
{
    public interface IUpdateble<T>
    {
        void Update(T entity);
    }
}