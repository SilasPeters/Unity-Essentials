using Unity_Essentials.Static;
using UnityEngine;

namespace Unity_Essentials.Components
{
	public class FirstPersonController2DTopdown : MonoBehaviour
	{
		public float movementSpeed;
		public float rotationSpeed;
		public int maxSpotCount;
		public int maxBeamCount;

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

			if (Mathf.Abs(transform.position.y) > 5)
				transform.position = new Vector3(transform.position.x, 5, transform.position.z);
			if (Mathf.Abs(transform.position.x) > 9)
				transform.position = new Vector3(9, transform.position.y, transform.position.z);

			// Get input of antagonist
			Vector3 mousePos = Camera.main!.ScreenToWorldPoint(Input.mousePosition);

			# nullable enable
			GameObject? placedPrefab = null;
			if (Input.GetKeyDown(beamHorizontal) && Singleton<GameManager>.Instance.BeamCount < maxBeamCount)
				placedPrefab = Instantiate(beamHorizontalPrefab);
			if (Input.GetKeyDown(beamVertical) && Singleton<GameManager>.Instance.BeamCount < maxBeamCount)
				placedPrefab = Instantiate(beamVerticalPrefab);
			if (Input.GetKeyDown(spot) && Singleton<GameManager>.Instance.SpotCount < maxSpotCount)
				placedPrefab = Instantiate(spotPrefab);

			if (placedPrefab != null)
				placedPrefab.transform.position = new Vector3(mousePos.x, mousePos.y, placedPrefab.transform.position.z);
			#nullable disable
		}
	}
}