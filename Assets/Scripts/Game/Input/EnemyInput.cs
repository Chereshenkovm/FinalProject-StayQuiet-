using UnityEngine;

namespace Input
{
    public abstract class EnemyInput : MonoBehaviour
    {
        public abstract (Vector3 moveDirection, Quaternion viewDirection, bool something, bool noSoundSource, bool reachedPoint, bool playerFound) CurrentInput();
    }
}