using UnityEngine;

namespace Unity_Essentials.Components
{
	public class Collisions2D : MonoBehaviour
	{
		public LayerMask groundLayer;
		public float groundCollisionRadius;
		public float wallCollisionRadius;
		public Vector2 bottomOffset, rightOffset, leftOffset;

		public bool OnGround    { get; private set; }
		public bool OnWall       => OnRightWall || OnLeftWall;
		public bool OnRightWall { get; private set; }
		public bool OnLeftWall  { get; private set; }

		void Update()
		{
			Vector2 position = transform.position;
			OnGround    = Physics2D.OverlapCircle(position + bottomOffset, groundCollisionRadius, groundLayer);
			OnRightWall = Physics2D.OverlapCircle(position + rightOffset,  groundCollisionRadius, groundLayer);
			OnLeftWall  = Physics2D.OverlapCircle(position + leftOffset,   groundCollisionRadius, groundLayer);
		}

		void OnDrawGizmos()
		{
			Vector2 position = transform.position;
			Gizmos.DrawWireSphere(position + bottomOffset, groundCollisionRadius);
			Gizmos.DrawWireSphere(position + rightOffset,  wallCollisionRadius);
			Gizmos.DrawWireSphere(position + leftOffset,   wallCollisionRadius);
		}
	}}