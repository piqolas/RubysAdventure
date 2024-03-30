using UnityEngine;

namespace piqey
{
	// I've worked in C# for years; please forgive me for writing my own code.
	// I didn't do everything the same way but I did it more consisely and more
	// optimized.
	[RequireComponent(typeof(Rigidbody2D))]
	public class RubyController : MonoBehaviour
	{
		[Tooltip("The maxium health value this character can have at any time (also spawns with this value).")]
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
						// Update the last hurt time only when health is actually reduced
						_lastHurt = Time.time;

					_health = newHealth;
				}
			}
		}

		public float HurtCooldown = 2.0f;

		[Tooltip("The number of units per second this character can travel on each axis.")]
		public Vector2 Speed = Vector2.one;

		private int _health;
		private float? _lastHurt = null;

		private Rigidbody2D _body;
		private Vector2 _input;

		void Start()
		{
			Health = MaxHealth;
			_body = GetComponent<Rigidbody2D>();
		}

		void Update()
		{
			_input = new Vector2(
					Input.GetAxis("Horizontal"),
					Input.GetAxis("Vertical")
			);
		}

		void FixedUpdate()
		{
			_body.MovePosition(_body.position + _input * Speed * Time.fixedDeltaTime /* The tutorial tells you to use the wrong one; you'd think Unity themselves would know this! */);
		}

		#if UNITY_EDITOR
		void OnGUI()
		{
			Renderer renderer = GetComponentInParent<Renderer>();
			Vector3 labelPos = Camera.main.WorldToScreenPoint(renderer == null ? transform.position : renderer.bounds.center);
			GUI.Label(new Rect(labelPos.x + 20, Screen.height - labelPos.y, 100, 140), $"{Health:000}/{MaxHealth:000}HP\nHurt: {Time.time - _lastHurt:0.00}s ago");
		}
		#endif
	}
}
