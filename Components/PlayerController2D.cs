using System.Collections;
using Unity_Essentials.Static;
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

		[Header("Keytar")]
		public Transform guitar;
		public GameObject soundWavePrefab;
		public GameObject soundWaveIconPrefab;
		public Vector3 soundwaveOffset;
		public Transform arm;
		public Transform otherArm;
		public Transform armAnimation;
		public float armMoveDuration;
		public float riffCooldown;

		// Required Components
		private Collisions2D _collisions2D;
		private Collider2D _collider;
		private Rigidbody2D _rigidbody;

		private bool Grounded => _collisions2D.OnGround;

		private bool _isWalking;
		// private bool _shouldJump;
		private Color _backgroundColor;
		private bool _canRiff = true;

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
		}

		private void FootStep(object o, RhythmEventArgs e)
		{
			if (!_isWalking) return;
			Instantiate(footStepPrefab, transform.position + footstepOffset + new Vector3(0, 0, 10), Quaternion.identity);
			
			// Play audio
			audioSource.PlayOneShot(footstepsSound[e.BeatIndex % footstepsSound.Length]);
		}

		// private void Jump(object o, RhythmEventArgs e)
		// {
		// 	if (!_shouldJump) return;
		// 	_rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
		// 	Instantiate(jumpPrefab, transform.position + jumpOffset + new Vector3(0, 0, 11), Quaternion.identity);
		// 	audioSource.PlayOneShot(jumpSound);
		// 	_shouldJump = false;
		// }

		private void Jump()
		{
			_rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
			Instantiate(jumpPrefab, transform.position + jumpOffset + new Vector3(0, 0, 11), Quaternion.identity);
			audioSource.PlayOneShot(jumpSound);
		}

		private void HitBackground(object o, RhythmEventArgs e)
		{
			if (!_isWalking) return;

			StartCoroutine(HighLevelFunctions.Lerp(e.PeriodLength * 0.9f, progression =>
			{
				Camera.main.backgroundColor = Color.Lerp(backgroundHitColor, _backgroundColor, progression);
			}, onceAfter: true));
		}

		private void Keytar()
		{
			Vector3 mousePos2D  = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			float directionSign = Mathf.Sign(mousePos2D.x - transform.position.x);

			Vector3 offset      = new Vector3(soundwaveOffset.x * directionSign, soundwaveOffset.y, soundwaveOffset.z);
			Vector3 sourcePos2D = transform.position + offset;
			mousePos2D.z  = 0;
			sourcePos2D.z = 0;

			Vector2 delta = mousePos2D - sourcePos2D;
			float   angle = Mathf.Atan2(delta.y, delta.x);

			Quaternion direction = Quaternion.AngleAxis(Mathf.Rad2Deg * angle, new Vector3(0, 0, 1));

			Instantiate(soundWavePrefab,     sourcePos2D, direction);
			Instantiate(soundWaveIconPrefab, transform.position, directionSign >= 0 ? Quaternion.identity : Quaternion.AngleAxis(180, Vector3.up), transform);

			StartCoroutine(MoveArm());
		}

		private IEnumerator MoveArm()
		{
			_canRiff = false;
			var t = audioSource.volume;
			audioSource.volume = t * 0.3f;
			yield return HighLevelFunctions.Lerp(armMoveDuration, progress =>
			{
				arm.localRotation = Quaternion.AngleAxis(Mathf.Sin(progress * 6 * 2 * Mathf.PI) * 10, Vector3.forward);
			}, onceAfter: true);
			audioSource.volume = t;

			yield return new WaitForSeconds(riffCooldown);
			_canRiff = true;
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
				Singleton<SmartCamera>.Instance.MoveCameraLeft();
			}
			if (walkRight.AnyPressed())
			{
				deltaWalkForce += Vector2.right;
				playerRotation += Grounded ? walkTilt : jumpTilt;
				Singleton<SmartCamera>.Instance.MoveCameraRight();
			}

			deltaWalkForce *= walkForce;

			// Jumping
			if (jump.AnyDown() && Grounded)
			{
				Jump();
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

			if (_canRiff && Input.GetKeyDown(KeyCode.Mouse0))
			{
				Keytar();
			}

			if (transform.position.y < -50)
			{
				transform.position = new Vector3(transform.position.x, 40, transform.position.z);
			}
		}

		private void OnDrawGizmos()
		{
			Gizmos.DrawSphere(transform.position + footstepOffset, 0.3f); // Footstep
			Gizmos.DrawSphere(transform.position + soundwaveOffset, 0.2f); // soundwave origin
		}
	}
}