
using Asteroids.Core.Models;

namespace Asteroids.Transform
{
    public interface IWithTransform : IProcessable
    {
        GameTransform Transform { get; }
    }
}