using Unity_Essentials.Static;
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

		public KeyCode beamHorizontal;
		public KeyCode beamVertical;
		public KeyCode spot;

		// Get references to prefabs
		public GameObject beamHorizontalPrefab;
		public GameObject beamVerticalPrefab;
		public GameObject spotPrefab;

		public void Update()
		{
			if (Singleton<GameManager>.Instance.CurrentGameState != GameManager.GameState.Playing)
				return;

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

			// Get input of antagonist
			Vector3 mousePos = Camera.main!.ScreenToWorldPoint(Input.mousePosition);

			# nullable enable
			GameObject? placedPrefab = null;
			if (Input.GetKeyDown(beamHorizontal))
				placedPrefab = Instantiate(beamHorizontalPrefab);
			if (Input.GetKeyDown(beamVertical))
				placedPrefab = Instantiate(beamVerticalPrefab);
			if (Input.GetKeyDown(spot))
				placedPrefab = Instantiate(spotPrefab);

			if (placedPrefab != null)
				placedPrefab.transform.position = new Vector3(mousePos.x, mousePos.y, placedPrefab.transform.position.z);
			#nullable disable
		}
	}
}