using System;
using UnityEngine;

namespace Game.Ships
{
    public class ShipHealth : MonoBehaviour
    {
        private int _currentHealthInternal;

        public event Action<int> OnChanged;

        public int Max { get; private set; }

        public int Current
        {
            get => _currentHealthInternal;
            private set
            {
                bool isChanged = _currentHealthInternal != value;
                _currentHealthInternal = value;
                if (isChanged)
                {
                    OnChanged?.Invoke(value);
                }
            }
        }

        public void Init(int max)
        {
            Max = max;
            Current = max;
        }

        public bool ApplyDamage(int damage)
        {
            int oldHealth = Current;
            Current = Mathf.Clamp(Current - damage, 0, Max);
            return oldHealth != Current;
        }
    }
}