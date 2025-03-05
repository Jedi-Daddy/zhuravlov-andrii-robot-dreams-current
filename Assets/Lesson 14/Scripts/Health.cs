using System;
using UnityEngine;

namespace Dummies
{
    public class Health : MonoBehaviour
    {
        public event Action<int> OnHealthChanged;
        public event Action<float> OnHealthChanged01;
        public event Action OnDeath;

        [SerializeField] private CharacterController _characterController;
        [SerializeField] private int _maxHealth;

        private int _health;
        private bool _isAlive;

        
        public int HealthValue
        {
            get => _health;
            set
            {
                if (_health == value) return;

                _health = Mathf.Clamp(value, 0, _maxHealth);
                OnHealthChanged?.Invoke(_health);
                OnHealthChanged01?.Invoke(_health / (float)_maxHealth);

                
                if (_health <= 0 && _isAlive)
                {
                    IsAlive = false;
                }
            }
        }

        
        public bool IsAlive
        {
            get => _isAlive;
            private set
            {
                if (_isAlive == value) return;

                _isAlive = value;
                if (!_isAlive)
                {
                    OnDeath?.Invoke();
                }
            }
        }

        
        public float HealthValue01 => _health / (float)_maxHealth;

        
        public int MaxHealthValue => _maxHealth;

        
        public CharacterController CharacterController => _characterController;

        
        protected virtual void Awake()
        {
            SetHealth(MaxHealthValue);
        }

        
        public void TakeDamage(int damage)
        {
            if (!IsAlive) return;

            HealthValue -= damage;
        }

        
        public void Heal(int heal)
        {
            if (!IsAlive) return;

            HealthValue += heal;
        }

       
        public void SetHealth(int health)
        {
            HealthValue = Mathf.Clamp(health, 0, _maxHealth);
            IsAlive = _health > 0;
        }
    }
}
