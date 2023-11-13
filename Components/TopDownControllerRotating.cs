using UnityEngine;

namespace Unity_Essentials.Components
{
	public class TopDownControllerRotating : MonoBehaviour
	{
		public float movementSpeed;
		public float rotationSpeed;

		// Define keybindings
		public KeyCode rotateLeft;
		public KeyCode rotateRight;
		public KeyCode forward;
		public KeyCode backward;

		public void Update()
		{
			// Get input of player
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

			if (Mathf.Abs(transform.position.y) > 5)
				transform.position = new Vector3(transform.position.x, 5, transform.position.z);
			if (Mathf.Abs(transform.position.x) > 9)
				transform.position = new Vector3(9, transform.position.y, transform.position.z);
		}
	}
}