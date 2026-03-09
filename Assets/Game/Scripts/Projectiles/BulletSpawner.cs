using System.Collections.Generic;
using UnityEngine;

namespace Game.Projectiles
{
    public class BulletSpawner : MonoBehaviour
    {
        [SerializeField] private Bullet _prefab;
        [SerializeField] private Transform _container;
        [SerializeField] private float _prewarmCount = 10;

        private readonly Stack<Bullet> _pool = new();

        private void Awake()
        {
            Prewarm();
        }

        public Bullet Create()
        {
            if (_pool.TryPop(out Bullet bullet))
            {
                bullet.gameObject.SetActive(true);
            }
            else
            {
                bullet = Instantiate(_prefab, _container);
            }

            return bullet;
        }

        public void DestroyBullet(Bullet bullet)
        {
            ReturnToPool(bullet);
        }

        private void Prewarm()
        {
            for (var i = 0; i < _prewarmCount; i++)
            {
                Bullet bullet = Instantiate(_prefab, _container);
                ReturnToPool(bullet);
            }
        }

        private void ReturnToPool(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
            _pool.Push(bullet);
        }
    }
}