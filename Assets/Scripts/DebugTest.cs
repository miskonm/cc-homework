using Modules;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Client
{
    public class DebugTest : MonoBehaviour
    {
        [Inject]
        [ShowInInspector]
        private IScore _score;
    }
}