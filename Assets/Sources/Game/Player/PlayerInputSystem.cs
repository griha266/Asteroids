using Asteroids.Core.Systems;
using Asteroids.Utils;
using UnityEngine.InputSystem;

namespace Asteroids.Game.Player
{
    public class PlayerInputSystem : IUpdateable
    {
        private readonly InputAction _accelerateInputAction;
        private readonly InputAction _rotateInputAction;
        private readonly EventBus<AcceleratePlayerEvent> _accelerateEvents;
        private readonly EventBus<RotatePlayerEvent> _rotateEvents;

        public PlayerInputSystem(
            InputAction accelerateInputAction,
            InputAction rotateInputAction,
            EventBus<AcceleratePlayerEvent> accelerateEvents,
            EventBus<RotatePlayerEvent> rotateEvents
        )
        {
            _accelerateEvents = accelerateEvents;
            _rotateEvents = rotateEvents;
            _accelerateInputAction = accelerateInputAction;
            _rotateInputAction = rotateInputAction;
        }

        public void Update(float deltaTime)
        {
            if(_accelerateInputAction.enabled)
            {
                _accelerateEvents.Push(new AcceleratePlayerEvent(_accelerateInputAction.ReadValue<float>(), deltaTime));
            }

            if (_rotateInputAction.enabled)
            {
                _rotateEvents.Push(new RotatePlayerEvent(_rotateInputAction.ReadValue<float>(), deltaTime));
            }
        }
    }
}