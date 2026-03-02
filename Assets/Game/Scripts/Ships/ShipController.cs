using System;
using DG.Tweening;
using Game.Combat;
using UnityEngine;

namespace Game.Ships
{
    // +
    public abstract class ShipController : MonoBehaviour, IDamageable
    {
        [SerializeField] private ShipControllerSO _config;

        [Header("Combat")]
        [SerializeField] private ShipAttack _shipAttack;

        [Header("Movement")]
        [SerializeField] private Motor _motor;

        [Header("Visual")]
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Transform _viewTransform;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private ShipControllerViewConfig _viewConfig;
        [SerializeField] private AudioClip _damageSFX;
        [SerializeField] private VfxSpawner _vfxSpawner;

        private Vector3 _moveDirection;

        private Material _material;
        private Tweener _damageAnimation;

        private int _currentHealthInternal;

        public event Action<int> OnHealthChanged;
        public event Action<ShipController> OnDead;

        public int MaxHealth => _config.Health;

        public int CurrentHealth
        {
            get => _currentHealthInternal;
            private set
            {
                bool isChanged = _currentHealthInternal != value;
                _currentHealthInternal = value;
                if (isChanged)
                {
                    OnHealthChanged?.Invoke(value);
                }
            }
        }

        public abstract TeamType Team { get; }

        protected virtual void LateUpdate()
        {
            AnimateMovement(Time.deltaTime);
        }

        public void InitStartValues()
        {
            CurrentHealth = _config.Health;
            _motor.SetSpeed(_config.MoveSpeed);

            _material = new Material(_viewConfig.MaterialPrefab);
            _renderer.material = _material;
        }

        public void Construct(VfxSpawner vfxSpawner)
        {
            _vfxSpawner = vfxSpawner;
        }

        public void ApplyDamage(int damage)
        {
            int oldHealth = CurrentHealth;
            CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, _config.Health);
            if (oldHealth == CurrentHealth)
            {
                return;
            }

            if (CurrentHealth > 0)
            {
                AnimateDamage();
            }
            else
            {
                NotifyAboutDead();
            }
        }

        public void SetPositionInstant(Vector3 position)
        {
            transform.position = position;
        }

        protected void MoveStep(Vector2 direction)
        {
            _motor.MoveStep(direction);
        }

        protected void Fire(Vector2 direction)
        {
            _shipAttack.Fire(direction, Team);
        }

        private void NotifyAboutDead()
        {
            _vfxSpawner.Spawn(_viewConfig.DestroyEffectPrefab, _viewTransform.position);

            OnDead?.Invoke(this);
        }

        private void AnimateMovement(float deltaTime)
        {
            Vector3 shipAngles = _viewTransform.localEulerAngles;
            shipAngles.x = _viewConfig.MoveRotationAngle * _moveDirection.y;
            shipAngles.y = _viewConfig.MoveRotationAngle / 2 * _moveDirection.x * -1f;

            Quaternion shipRotation = Quaternion.Euler(shipAngles);
            float t = _viewConfig.MoveSpeed * deltaTime;
            _viewTransform.localRotation = Quaternion.Lerp(_viewTransform.localRotation, shipRotation, t);
        }

        private void AnimateDamage()
        {
            if (_damageAnimation.IsActive())
            {
                _damageAnimation.Kill();
            }

            _damageAnimation = DOVirtual.Float(
                0f,
                1f,
                _viewConfig.HitDuration,
                progress => _material?.SetFloat(_viewConfig.HitPropertyName,
                    _viewConfig.HitAnimationCurve.Evaluate(progress))
            ).SetLink(_renderer.gameObject);

            if (_damageSFX)
            {
                _audioSource.PlayOneShot(_damageSFX);
            }
        }
    }
}