using UnityEngine;
using piqey.Utilities.Editor;

namespace piqey
{
	// I've worked in C# for years; please forgive me for writing my own code.
	// I didn't do everything the same way but I did it more consisely and more
	// optimized.
	[RequireComponent(typeof(Rigidbody2D))]
	public class EnemyController : MonoBehaviour
	{
		public enum Direction
		{ Left, Right, Up, Down }

		//
		// Damage
		//

		[Header("Damage")]

		[Tooltip("The amount of HP by which this zone should damage the player.")]
		public int DamageAmount = 10;

		//
		// MOVEMENT
		//

		[Header("Movement")]

		[Tooltip("The number of units per second this character can travel on each axis.")]
		public Vector2 Speed = Vector2.one;
		[Tooltip("The amount of time in seconds each movement direction in the array should last before changing.")]
		[Min(0.0f)]
		public float MovementDuration = 3.0f;
		[Tooltip("The array defining the sequence of directions this enemy should move in.")]
		public Direction[] MovementPattern;

		//
		// HIDDEN
		//

		[Header("Hidden")]

		[SerializeField, ReadOnly, Label("_body")]
		private Rigidbody2D _body;
		[SerializeField, ReadOnly, Label("_animator")]
		private Animator _animator;

		//
		// METHODS
		//

		void Start()
		{
			_body = GetComponent<Rigidbody2D>();
			_animator = GetComponent<Animator>();
		}

		void FixedUpdate()
		{
			Vector2 dir = MovementPattern[Mathf.FloorToInt(Time.fixedTime / MovementDuration % MovementPattern.Length)] switch
			{
				Direction.Left => Vector2.left,
				Direction.Right => Vector2.right,
				Direction.Up => Vector2.up,
				Direction.Down => Vector2.down,
				_ => throw new System.NotImplementedException()
			};

			_body.MovePosition(_body.position + dir * Speed * Time.fixedDeltaTime);

			_animator.SetFloat("Move X", dir.x);
			_animator.SetFloat("Move Y", dir.y);
		}

		void OnCollisionEnter2D(Collision2D other)
		{
			if (other.gameObject.TryGetComponent(out RubyController ruby))
				ruby.Health -= DamageAmount;
		}
	}
}
