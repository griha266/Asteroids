namespace Asteroids.Core.Models
{
    public readonly struct DestroyModelEvent<TModel>
    {
        public readonly TModel Model;

        public DestroyModelEvent(TModel model)
        {
            Model = model;
        }
    }
}