using UnityEngine;

namespace Unity_Essentials.Components
{
	public class FirstPersonController2DTopdown : MonoBehaviour
	{
		public float movementSpeed;
		public float rotationSpeed;

		// Define keybindings
		public KeyCode rotateLeft;
		public KeyCode rotateRight;
		public KeyCode forward;
		public KeyCode backward;
		
		private void Start()
		{
			Cursor.visible  = false;
		}

		public void Update()
		{
			if (GameManager.currentGameState != GameManager.GameState.Playing)
				return;

			// Get input to determine movement direction
			float forwardMovement = 0;
			float rotationDegrees = 0;

			if (Input.GetKey(rotateLeft))
				rotationDegrees += rotationSpeed;
			if (Input.GetKey(rotateRight))
				rotationDegrees -= rotationSpeed;
			if (Input.GetKey(forward))
				forwardMovement += movementSpeed;
			if (Input.GetKey(backward))
				forwardMovement += -movementSpeed;

			// Apply movement and rotation
			transform.Rotate(0, 0, rotationDegrees * Time.deltaTime);
			transform.localPosition += transform.up * (forwardMovement * Time.deltaTime);
		}
	}
}