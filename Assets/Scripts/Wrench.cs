using UnityEngine;
using piqey.Utilities.Editor;

namespace piqey
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class Wrench : MonoBehaviour
	{
		[Min(0.0f)]
		public int Damage = 10;
		[Tooltip("Maximum distance from the creator this projectile can travel before being culled.")]
		[Min(0.0f)]
		public float DeleteDistance = 100.0f;

		[SerializeField, ReadOnly, Label("_body")]
		private Rigidbody2D _body;
		[SerializeField, ReadOnly, Label("_creator")]
		private GameObject _creator;

		void Start()
		{
			if (_body == null)
				_body = GetComponent<Rigidbody2D>();
		}

		void OnCollisionEnter2D(Collision2D other)
		{
			// Debug.Log($"Collided with {other.gameObject}");

			if (other.gameObject.TryGetComponent(out RubyController ruby))
				ruby.Health -= Damage;

			Destroy(gameObject);
		}

		void FixedUpdate()
		{
			if (_creator == null || (transform.position - _creator.transform.position).magnitude >= DeleteDistance)
				Destroy(gameObject);
		}

		public void Launch(GameObject creator, Vector2 dir, float force, float angularVelocity)
		{
			// This method appears to be run before Start() in some situations
			if (_body == null)
				_body = GetComponent<Rigidbody2D>();

			_creator = creator;
			_body.AddForce(dir * force);
			_body.angularVelocity = angularVelocity;
		}
	}
}
