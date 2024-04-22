using UnityEngine;
using UnityEngine.Events;
using piqey.Utilities.Editor;

namespace piqey
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class Projectile : MonoBehaviour
	{
		[Tooltip("Maximum distance from the creator this projectile can travel before being culled.")]
		[Min(0.0f)]
		public float DeleteDistance = 100.0f;

		public float MagneticPull = 800.0f;
		public bool Magnetic = false;
		public float MagnetismColorChangeRate = 0.618f;

		public event UnityAction OnCollided;

		[SerializeField, ReadOnly, Label("_body")]
		private Rigidbody2D _body;
		[SerializeField, ReadOnly, Label("_renderer")]
		private Renderer _renderer;
		[SerializeField, ReadOnly, Label("_creator")]
		private GameObject _creator;

		void Start()
		{
			if (_body == null)
				_body = GetComponent<Rigidbody2D>();

			_renderer = GetComponent<Renderer>();
		}

		void OnCollisionEnter2D(Collision2D other)
		{
			// Debug.Log($"Collided with {other.gameObject}");

			if (other.gameObject.TryGetComponent(out EnemyController enemy))
				enemy.Broken = false;

			OnCollided?.Invoke();
			Destroy(gameObject);
		}

		void Update()
		{
			if (Magnetic)
			{
				if (_renderer.material.color == Color.white)
					_renderer.material.color = Color.red;

				Color.RGBToHSV(_renderer.material.color, out float h, out float s, out float v);
				_renderer.material.color = Color.HSVToRGB(h + Time.deltaTime * MagnetismColorChangeRate, s, v);
			}
		}

		void FixedUpdate()
		{
			if (_creator != null && Vector2.Distance(transform.position, _creator.transform.position) >= DeleteDistance)
				Destroy(gameObject);
			else if (Magnetic)
			{
				EnemyController[] enemies = FindObjectsOfType<EnemyController>();

				EnemyController closest = null;
				float minDist = Mathf.Infinity;

				foreach (EnemyController enemy in enemies)
				{
					float dist = Vector2.Distance(transform.position, enemy.transform.position);

					if (dist < minDist)
					{
						closest = enemy;
						minDist = dist;
					}
				}

				Vector2 dir = ((closest.TryGetComponent(out Renderer renderer) ? renderer.bounds.center : closest.transform.position) - transform.position).normalized;
				_body.AddForce(MagneticPull * Time.fixedDeltaTime * dir);
				Debug.Log($"Magnetism active! Added force {MagneticPull * Time.fixedDeltaTime * dir}");
			}
		}

		public void Launch(GameObject creator, Vector2 dir, float force)
		{
			// This method appears to be run before Start() in some situations
			if (_body == null)
				_body = GetComponent<Rigidbody2D>();

			_creator = creator;
			_body.AddForce(dir * force);
		}
	}
}
