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
			set => _health = Mathf.Clamp(value, 0, MaxHealth);
		}

		[Tooltip("The number of units per second this character can travel on each axis.")]
		public Vector2 Speed = Vector2.one;

		private int _health;

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
			GUI.Label(new Rect(labelPos.x, Screen.height - labelPos.y, 100, 20), $"{Health:000}/{MaxHealth:000} HP");
		}
		#endif
	}
}
