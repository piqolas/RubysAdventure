using UnityEngine;
using UnityEngine.Events;
using piqey.Utilities.Editor;
using piqey.Utilities.Extensions;

namespace piqey
{
	// I've worked in C# for years; please forgive me for writing my own code.
	// I didn't do everything the same way but I did it more consisely and more
	// optimized.
	[RequireComponent(typeof(Rigidbody2D))]
	public class RubyController : MonoBehaviour
	{
		//
		// VITALS
		//

		[Header("Vitals")]

		[Tooltip("The maxium health value this character can have at any time (also spawns with this value).")]
		[Min(1)]
		public int MaxHealth = 100;
		public int Health
		{
			get => _health;
			set {
				int newHealth = Mathf.Clamp(value, 0, MaxHealth);
				bool isNetReduction = newHealth < _health;

				// Only update health if it's not a net reduction (within the cooldown period), else let it pass
				if (!isNetReduction || !_lastHurt.HasValue || Time.time - _lastHurt.Value >= HurtCooldown)
				{
					if (isNetReduction)
					{
						// Update the last hurt time only when health is actually reduced
						_lastHurt = Time.time;
						OnHurt?.Invoke();
					}

					_health = newHealth;
					OnHealthChanged?.Invoke();
				}
			}
		}

		[Tooltip("The amount of time in seconds after taking damage before this character can take damage again.")]
		[Min(0.0f)]
		public float HurtCooldown = 2.0f;

		//
		// MOVEMENT
		//

		[Header("Movement")]

		[Tooltip("The number of units per second this character can travel on each axis.")]
		public Vector2 Speed = Vector2.one;

		//
		// COMBAT
		//

		[Header("Combat")]

		public GameObject ProjectilePrefab;
		public float ProjectileForce = 300.0f;

		//
		// SOUND
		//

		[Header("Sound")]

		public AudioClip HurtSound;
		public AudioClip ThrowCogSound;

		//
		// HIDDEN
		//

		[Header("Hidden")]

		[SerializeField, ReadOnly, Label("_health")]
		private int _health;
		[SerializeField, ReadOnly, Label("_lastHurt")]
		private float? _lastHurt = null;

		[SerializeField, ReadOnly, Label("_body")]
		private Rigidbody2D _body;
		[SerializeField, ReadOnly, Label("_animator")]
		private Animator _animator;
		[SerializeField, ReadOnly, Label("_audioSource")]
		private AudioSource _audioSource;

		[SerializeField, ReadOnly, Label("_move")]
		private Vector2 _move;
		[SerializeField, ReadOnly, Label("_lookDir")]
		private Vector2 _lookDir;

		//
		// EVENTS
		//

		public event UnityAction OnHealthChanged;
		public event UnityAction OnHurt;

		//
		// METHODS
		//

		void Start()
		{
			Health = MaxHealth;

			_body = GetComponent<Rigidbody2D>();
			_animator = GetComponent<Animator>();
			_audioSource = GetComponent<AudioSource>();

			OnHurt += () =>
			{
				_animator.SetTrigger("Hit");
				_audioSource.PlayOneShot(HurtSound);
			};
		}

		void Update()
		{
			_move = new Vector2(
				Input.GetAxis("Horizontal"),
				Input.GetAxis("Vertical")
			) * Speed;

			if (_lookDir == Vector2.zero || _move.ApproximatelyOrNot(Vector2.zero))
				_lookDir = _move.normalized;

			_animator.SetFloat("Look X", _lookDir.x);
			_animator.SetFloat("Look Y", _lookDir.y);

			_animator.SetFloat("Speed", _move.magnitude);

			if (Input.GetKeyDown(KeyCode.C))
				Launch();
			
			if (Input.GetKeyDown(KeyCode.X))
			{
				RaycastHit2D hit2D = Physics2D.Raycast(_body.position + Vector2.up * 0.2f, _lookDir, 1.5f, LayerMask.GetMask("NPC"));

				if (hit2D.collider != null && hit2D.collider.TryGetComponent(out NonPlayerCharacter npc))
					npc.DisplayDialog();
			}
		}

		void FixedUpdate()
		{
			_body.MovePosition(_body.position + _move * Time.fixedDeltaTime /* The tutorial tells you to use the wrong one; you'd think Unity themselves would know this! */);
		}

		void Launch()
		{
			GameObject projectileObject = Instantiate(ProjectilePrefab, _body.position + Vector2.up * 0.5f, Quaternion.identity);

			if (projectileObject.TryGetComponent(out Projectile projectile))
			{
				projectile.Launch(gameObject, _lookDir, ProjectileForce);
				_audioSource.PlayOneShot(ThrowCogSound);
			}
			else
				throw new MissingComponentException($"Found no {typeof(Projectile)} component on {projectileObject}.");
		}

		#if UNITY_EDITOR
		void OnGUI()
		{
			Renderer renderer = GetComponentInParent<Renderer>();
			Vector3 labelPos = Camera.main.WorldToScreenPoint(renderer == null ? transform.position : renderer.bounds.center);
			GUI.Label(new Rect(labelPos.x + 20, Screen.height - labelPos.y, 100, 140), $"{Health:000}/{MaxHealth:000}HP\nHurt: {Time.time - _lastHurt:0.00}s ago");
		}
		#endif

		public void PlaySound(AudioClip clip) =>
			_audioSource.PlayOneShot(clip);
	}
}
