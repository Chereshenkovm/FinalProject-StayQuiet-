using UnityEngine;

namespace Input
{
    public abstract class PlayerInput : MonoBehaviour
    {
        public abstract (Vector3 moveDirection, float horizAngleRotation, float vertAngleRotation, bool isFiring, bool tiptoe, bool canPickObject, bool pressedE, GameObject pickableObject) CurrentInput();
    }
}