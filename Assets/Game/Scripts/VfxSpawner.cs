using UnityEngine;

namespace Game
{
    public class VfxSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _container;

        public void Spawn(ParticleSystem prefab, Vector3 positions)
        {
            Instantiate(prefab, positions, prefab.transform.rotation, _container);
        }
    }
}