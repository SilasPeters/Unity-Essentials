using UnityEngine;

namespace Unity_Essentials.Components
{
	public class FirstPersonController : MonoBehaviour
	{
        [Header("Movement")]
        public float movementSpeed;
        public float sprintMultiplier;
        public float g;
        public float jumpHeight;
        public float massPlayer;

        public float fovShiftTime;
        public float fovSprint;
        public float fovNormal;

        [Header("Look")]
        public float mouseSensX; //100
        public float mouseSensY; //60
        public Camera playerCam;

        private Transform _transform;
        private CharacterController _controller;

        void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            _transform       = transform;
            _controller      = GetComponent<CharacterController>();
        }

        private Vector3 _verticalMovement;
        private float _mouseY;

        public void Update()
        {
            #region Player Movement

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 horizontalMove = _transform.right * x + _transform.forward * z;

            // TODO isGrounded geeft vaak false negatives
            bool isGrounded = GetComponent<CharacterController>().SimpleMove(horizontalMove);
            if (isGrounded)
            {
                // Jump
                _verticalMovement.y = 0f; //prevents playerTransform from falling forever
                if (Input.GetButton("Jump"))
                {
                    _verticalMovement.y = jumpHeight;
                }

                // Sprint
                if (Input.GetKey("left shift"))
                {
                    horizontalMove = new Vector3(horizontalMove.x * sprintMultiplier, 0, horizontalMove.z * sprintMultiplier);
                    playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, fovSprint, fovShiftTime);
                }
                else
                {
                    playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, fovNormal, fovShiftTime);
                }

            }
            else
            {
                _verticalMovement.y -= g * massPlayer * Time.deltaTime;
            }

            _controller.Move(horizontalMove * movementSpeed * Time.deltaTime + _verticalMovement * Time.deltaTime);

            #endregion
            #region Player Look

            // Player Look
            float mouseX = Input.GetAxis("Mouse X") * mouseSensX * Time.deltaTime;
            _mouseY += Input.GetAxis("Mouse Y") * mouseSensY * Time.deltaTime;   //reads PER FRAME how your mouse moved (per frame, hence the +=)
            _mouseY = Mathf.Clamp(_mouseY, -85.5f, 85.5f); //prevents the playerTransform from breaking his virtual neck

            _transform.Rotate(0, mouseX, 0);
            playerCam.transform.localEulerAngles = new Vector3(
                -_mouseY,
                playerCam.transform.localEulerAngles.y,
                playerCam.transform.localEulerAngles.z); //rotates the cam and not the playerTransform, because, yeah, you don't move your whole body when looking up. Why *-1? I don't know man.

            #endregion
        }
	}
}