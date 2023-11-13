using Unity_Essentials.Static;
using UnityEngine;

namespace Unity_Essentials.Components
{
	[RequireComponent(typeof(Collider2D))]
	[RequireComponent(typeof(Rigidbody2D))]
	public class PlayerController2D : MonoBehaviour
	{
		public float walkForce;
		public float jumpForce;
		[Range(0, 1)] public float dropForceMultiplier;

		public KeyCode[] walkLeft;
		public KeyCode[] walkRight;
		public KeyCode[] jump;
		public KeyCode[] drop;

		public GroundedBox groundedBox;

		// Required Components
		private Collider2D _collider;
		private Rigidbody2D _rigidbody;

		private void Start()
		{
			_collider  = GetComponent<Collider2D>();
			_rigidbody = GetComponent<Rigidbody2D>();
		}

		protected void FixedUpdate()
		{
			Vector2 deltaWalkForce = Vector2.zero;
			Vector2 deltaJumpForce = Vector2.zero;

			// Walking
			if (walkLeft.AnyPressed())
			{
				deltaWalkForce -= Vector2.right;
			}
			if (walkRight.AnyPressed())
			{
				deltaWalkForce += Vector2.right;
			}

			deltaWalkForce *= walkForce;

			// Jumping
			if (jump.AnyDown() && IsGrounded())
			{
				deltaJumpForce += Vector2.up;
			}
			if (drop.AnyPressed() && !IsGrounded())
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
			transform.position += (Vector3)deltaWalkForce * Time.deltaTime;
		}

		private bool IsGrounded() => groundedBox.IsGrounded;
	}
}