using DG.Tweening;
using Game.Common;
using UnityEngine;

namespace Game.Ships
{
    public class ShipView : MonoBehaviour
    {
        [SerializeField] private Ship _ship;

        [Header("Visual")]
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Transform _viewTransform;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private ShipControllerViewConfig _viewConfig;
        [SerializeField] private AudioClip _damageSFX;
        [SerializeField] private VfxSpawner _vfxSpawner;

        private Material _material;
        private Tweener _damageAnimation;

        private void OnEnable()
        {
            _ship.OnDead += ShipDeadCallback;
            _ship.OnHit += ShipHitCallback;
            
            _material = new Material(_viewConfig.MaterialPrefab);
            _renderer.material = _material;
        }

        private void OnDisable()
        {
            _ship.OnDead -= ShipDeadCallback;
            _ship.OnHit -= ShipHitCallback;
        }

        private void ShipHitCallback()
        {
            AnimateDamage();
        }

        public void Construct(VfxSpawner vfxSpawner)
        {
            _vfxSpawner = vfxSpawner;
        }

        private void ShipDeadCallback(Ship ship)
        {
            _vfxSpawner.Spawn(_viewConfig.DestroyEffectPrefab, _viewTransform.position);
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
        
        protected virtual void LateUpdate()
        {
            AnimateMovement(Time.deltaTime);
        }
        
        private void AnimateMovement(float deltaTime)
        {
            Vector3 shipAngles = _viewTransform.localEulerAngles;
            shipAngles.x = _viewConfig.MoveRotationAngle * _ship.MoveDirection.y;
            shipAngles.y = _viewConfig.MoveRotationAngle / 2 * _ship.MoveDirection.x * -1f;

            Quaternion shipRotation = Quaternion.Euler(shipAngles);
            float t = _viewConfig.MoveSpeed * deltaTime;
            _viewTransform.localRotation = Quaternion.Lerp(_viewTransform.localRotation, shipRotation, t);
        }
    }
}