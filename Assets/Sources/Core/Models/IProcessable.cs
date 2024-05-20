using Asteroids.Utils;

namespace Asteroids.Core.Models
{
    public interface IProcessable
    {
        public IReactiveProperty<bool> IsEnabled { get; }
    }
}