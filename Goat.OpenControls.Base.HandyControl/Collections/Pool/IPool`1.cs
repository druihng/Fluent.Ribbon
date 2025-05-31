namespace Goat.OpenControls.Base.HandyControl.Collections
{
    public interface IPool<T>
    {
        T Acquire();

        bool Release(T instance);
    }
}
