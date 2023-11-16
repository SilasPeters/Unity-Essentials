using System;
using System.Collections;
using Unity_Essentials.Static;
using Unity_Essentials.Static.ExtensionMethods;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unity_Essentials.Components
{
	[RequireComponent(typeof(Collisions2D))]
	[RequireComponent(typeof(Collider2D))]
	[RequireComponent(typeof(Rigidbody2D))]
	public class PlayerController2D : MonoBehaviour
	{
		public float walkForce;
		public float walkTilt;
		public float jumpTilt;
		public float jumpForce;
		[Range(0, 1)] public float dropForceMultiplier;

		public KeyCode[] walkLeft;
		public KeyCode[] walkRight;
		public KeyCode[] jump;
		public KeyCode[] drop;

		public GameObject footStepPrefab;
		public Vector3 footstepOffset;
		public GameObject jumpPrefab;
		public Vector3 jumpOffset;

		[Header("SFX")]
		public AudioSource audioSource;
		public AudioClip[] footstepsSound;
		public AudioClip jumpSound;

		public Color backgroundHitColor;

		// Required Components
		private Collisions2D _collisions2D;
		private Collider2D _collider;
		private Rigidbody2D _rigidbody;

		private bool Grounded => _collisions2D.OnGround;

		private bool _isWalking;
		private bool _shouldJump;
		private Color _backgroundColor;

		private void Awake()
		{
			_collisions2D = GetComponent<Collisions2D>();
			_collider     = GetComponent<Collider2D>();
			_rigidbody    = GetComponent<Rigidbody2D>();
			_backgroundColor =  Camera.main.backgroundColor;
		}

		private void Start()
		{
			Singleton<Rhythm>.Instance.OnBeat += FootStep;
			Singleton<Rhythm>.Instance.OnBeat += HitBackground;
			Singleton<Rhythm>.Instance.OnBeat += Jump;
		}

		private void FootStep(object o, RhythmEventArgs e)
		{
			if (!_isWalking || _shouldJump) return;
			Instantiate(footStepPrefab, transform.position + footstepOffset + new Vector3(0, 0, 10), Quaternion.identity);
			
			// Play audio
			audioSource.PlayOneShot(footstepsSound[e.BeatIndex % footstepsSound.Length]);
		}

		private void Jump(object o, RhythmEventArgs e)
		{
			if (!_shouldJump) return;
			_rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
			Instantiate(jumpPrefab, transform.position + jumpOffset + new Vector3(0, 0, 11), Quaternion.identity);
			audioSource.PlayOneShot(jumpSound);
			_shouldJump = false;
		}

		private void HitBackground(object o, RhythmEventArgs e)
		{
			if (!(_isWalking || _shouldJump)) return;

			StartCoroutine(HighLevelFunctions.Lerp(e.PeriodLength * 0.8f, progression =>
			{
				Camera.main.backgroundColor = new Color(
					Mathf.Lerp(backgroundHitColor.r, _backgroundColor.r, progression),
					Mathf.Lerp(backgroundHitColor.g, _backgroundColor.g, progression),
					Mathf.Lerp(backgroundHitColor.b, _backgroundColor.b, progression),
					_backgroundColor.a);
			}, onceAfter: true));
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
				playerRotation -= Grounded ? walkTilt : jumpTilt;
			}
			if (walkRight.AnyPressed())
			{
				deltaWalkForce += Vector2.right;
				playerRotation += Grounded ? walkTilt : jumpTilt;
			}

			deltaWalkForce *= walkForce;

			// Jumping
			if (jump.AnyDown() && Grounded)
			{
				_shouldJump = true;
			}
			if (drop.AnyPressed() && !Grounded)
			{
				deltaJumpForce -= Vector2.up * (dropForceMultiplier * jumpForce);
			}

			// Apply movement
			_rigidbody.AddForce(deltaJumpForce, ForceMode2D.Impulse);
			transform.position      += (Vector3)deltaWalkForce * Time.deltaTime;
			transform.localRotation =  Quaternion.AngleAxis(playerRotation, Vector3.back);

			// Events
			_isWalking = deltaWalkForce != Vector2.zero && Grounded; // Player is walking
		}

		private void OnDrawGizmos()
		{
			Gizmos.DrawSphere(transform.position + footstepOffset, 0.3f); // Footstep
		}
	}
}