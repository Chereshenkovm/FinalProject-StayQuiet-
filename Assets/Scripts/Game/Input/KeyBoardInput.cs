using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Input
{
    public class KeyBoardInput : PlayerInput
    {
        [Header("Скорости камеры по X и Y")]
        public float cameraSpeedX = 0.001f;
        public float cameraSpeedY = 10f;

        [Header("Верхний и нижний угол поворота вертикальной камеры (верхний(0,90],нижний[-90,0))")]
        [SerializeField] private float topYAngle = 90f;
        [SerializeField] private float bottomYAngle = -90f;
        
        [Header("Transform главного персонажа")]
        [SerializeField]
        private Transform playerTransform;

        [Header("Главная камера персонажа")]
        [SerializeField] private Camera playerCamera;

        private RaycastHit hit;
        [SerializeField] private float raycastDistance = 10f;

        public override (Vector3 moveDirection, float horizAngleRotation, float vertAngleRotation, bool isFiring, bool tiptoe, bool canPickObject, bool pressedE, GameObject pickableObject) CurrentInput()
        {
            float horizAngleRotation = UnityEngine.Input.GetAxis("Mouse X") * cameraSpeedX;
            var vertMouseInput = -UnityEngine.Input.GetAxis("Mouse Y") * cameraSpeedY;
            var playerEA = playerCamera.transform.eulerAngles.x;
            float vertAngleRotation = ((playerEA+vertMouseInput<=(90-(90+bottomYAngle))) || (playerEA+vertMouseInput>=(270 + (90 - topYAngle))))? vertMouseInput * cameraSpeedY : 0;
            Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, raycastDistance);
            return (
                playerTransform.forward*UnityEngine.Input.GetAxis("Vertical") + playerTransform.right*UnityEngine.Input.GetAxis("Horizontal"),
                horizAngleRotation,
                vertAngleRotation,
                UnityEngine.Input.GetMouseButtonDown(0),
                UnityEngine.Input.GetKey(KeyCode.LeftShift),
                (hit.collider!=null)?hit.collider.GetComponent<Pickable>()!=null:false,
                UnityEngine.Input.GetKeyDown(KeyCode.E),
                (hit.collider!=null)?hit.collider.gameObject:null);
        }
    }
}