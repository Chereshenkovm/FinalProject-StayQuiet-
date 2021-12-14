using UnityEngine;

namespace Input
{
    public abstract class PlayerMicInput: MonoBehaviour
    {
        public abstract (float micVolume, bool isTalking, bool isEnabled) MicInput();
    }
}