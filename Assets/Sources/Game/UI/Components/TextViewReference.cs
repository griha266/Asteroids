using System;
using UnityEngine.UI;

namespace Asteroids.Game.UI
{
    [Serializable]
    public struct TextViewReference
    {
        public string TextTemplate;
        public Text TextComponent;
    }
}