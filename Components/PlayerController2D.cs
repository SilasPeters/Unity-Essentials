using Unity_Essentials.Static;
using UnityEngine;

namespace Unity_Essentials.Components
{
	[RequireComponent(typeof(Collisions2D))]
	[RequireComponent(typeof(Collider2D))]
	[RequireComponent(typeof(Rigidbody2D))]
	public class PlayerController2D : MonoBehaviour
	{
		public float walkForce;
		public float walkTilt;
		public float jumpForce;
		[Range(0, 1)] public float dropForceMultiplier;

		public KeyCode[] walkLeft;
		public KeyCode[] walkRight;
		public KeyCode[] jump;
		public KeyCode[] drop;

		// Required Components
		private Collisions2D _collisions2D;
		private Collider2D _collider;
		private Rigidbody2D _rigidbody;

		private void Start()
		{
			_collisions2D = GetComponent<Collisions2D>();
			_collider     = GetComponent<Collider2D>();
			_rigidbody    = GetComponent<Rigidbody2D>();
		}

		protected void Update()
		{
			Vector2 deltaWalkForce = Vector2.zero;
			Vector2 deltaJumpForce = Vector2.zero;

			float playerRotation = 0;

			// Walking
			if (walkLeft.AnyPressed())
			{
				deltaWalkForce -= Vector2.right;
				playerRotation -= walkTilt;
			}
			if (walkRight.AnyPressed())
			{
				deltaWalkForce += Vector2.right;
				playerRotation += walkTilt;
			}

			deltaWalkForce *= walkForce;

			// Jumping
			if (jump.AnyDown() && _collisions2D.OnGround)
			{
				deltaJumpForce += Vector2.up;
			}
			if (drop.AnyPressed() && _collisions2D.OnGround)
			{
				deltaJumpForce -= Vector2.up * dropForceMultiplier;
			}

			deltaJumpForce *= jumpForce;

			// Apply movement
			// if (IsGrounded() && deltaWalkForce == Vector2.zero)
			// {
			// 	walkForce *= 0.7f;
			// }

			_rigidbody.AddForce(deltaJumpForce, ForceMode2D.Impulse);
			transform.position      += (Vector3)deltaWalkForce * Time.deltaTime;
			transform.localRotation =  Quaternion.AngleAxis(playerRotation, Vector3.back);
		}
	}
}