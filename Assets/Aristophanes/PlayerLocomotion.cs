using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ATY
{
    public class PlayerLocomotion : MonoBehaviour
    {
        GameObject cameraObject;
        InputHandler inputHandler;
        Vector3 moveDirection;

        [HideInInspector]
        public Transform myTransform;

        public new Rigidbody rigidbody;
        public GameObject normalCamera;

        [Header("Stats")]
        [SerializeField]
        float movementSpeed = 5;
        [SerializeField]
        float rotationSpeed = 10;
        // Start is called before the first frame update
        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandler>();
            cameraObject = Camera.main.gameObject;
            myTransform = transform;
        }
        // Update is called once per frame
        void Update()
        {
            float delta = Time.deltaTime;
            inputHandler.TickInput(delta);
            moveDirection = cameraObject.transform.forward * inputHandler.vertical;
            moveDirection += cameraObject.transform.right * inputHandler.horizontal;
            moveDirection.Normalize();

            float speed = movementSpeed;
            moveDirection *= speed;

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rigidbody.velocity = projectedVelocity;
            HandleRotation(delta);
        }
        #region Movement
        Vector3 normalVector;
        Vector3 targetPosition;
        private void HandleRotation(float delta)
        {
            Vector3 targetDir = Vector3.zero;
            float mouseOverride = inputHandler.moveAmount;

            targetDir = cameraObject.transform.forward * inputHandler.vertical;
            targetDir += cameraObject.transform.right * inputHandler.horizontal;
            targetDir.Normalize();
            targetDir.y = 0;

            if (targetDir == Vector3.zero)
            {
                targetDir = myTransform.forward;
            }
            float rs = rotationSpeed;
            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);
            myTransform.rotation = targetRotation;
        }
        #endregion
    }
}
