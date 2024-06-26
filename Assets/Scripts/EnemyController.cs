using UnityEngine;
using UnityEngine.Events;
using piqey.Utilities.Editor;

namespace piqey
{
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

		public bool Broken
		{
			get => _broken;
			set {
				_broken = value;
				_body.simulated = value;

				if (value)
				{
					_animator.ResetTrigger("Fixed");
					_smoke.Play();
					AudioPlayer.Play();
				}
				else
				{
					_animator.SetTrigger("Fixed");
					_smoke.Stop(true, ParticleSystemStopBehavior.StopEmitting);
					AudioPlayer.Stop();
					AudioPlayer.PlayOneShot(FixSound);

					OnFixed?.Invoke();
					RubyController.RobotsFixed++;
				}
			}
		}

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
		// SOUND
		//

		[Header("Sound")]

		public AudioSource AudioPlayer;
		public AudioClip FixSound;

		//
		// HIDDEN
		//

		[Header("Hidden")]

		[SerializeField, ReadOnly, Label("_body")]
		private Rigidbody2D _body;
		[SerializeField, ReadOnly, Label("_animator")]
		private Animator _animator;
		[SerializeField, ReadOnly, Label("_smoke")]
		private ParticleSystem _smoke;
		[SerializeField, ReadOnly, Label("_broken")]
		private bool _broken = true;

		//
		// EVENTS
		//

		public event UnityAction OnFixed;

		//
		// METHODS
		//

		void Start()
		{
			if (AudioPlayer == null)
				AudioPlayer = GetComponent<AudioSource>();

			_body = GetComponent<Rigidbody2D>();
			_animator = GetComponent<Animator>();
			_smoke = GetComponentInChildren<ParticleSystem>();
		}

		void FixedUpdate()
		{
			if (!_broken)
				return;

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
