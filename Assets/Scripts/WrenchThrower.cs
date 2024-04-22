using UnityEngine;
using piqey.Utilities.Editor;
using piqey.Utilities;

namespace piqey
{
	[RequireComponent(typeof(EnemyController))]
	public class WrenchThrower : MonoBehaviour
	{
		public AudioSource AudioPlayer;

		[Min(0.0f)]
		public float ActivationRadius = 4.0f;
		[Min(0.0f)]
		public float ThrowForce = 1.0f;
		[Min(0.1f)]
		public float ThrowDelay = 0.5f;
		[Min(0.0f)]
		public float ThrowAngularSpeedRange = 180.0f;

		public AudioClip[] ThrowSounds;

		public GameObject WrenchPrefab;

		[SerializeField, ReadOnly, Label("_enemyController")]
		private EnemyController _enemyController;
		[SerializeField, ReadOnly, Label("_renderer")]
		private Renderer _renderer;
		[SerializeField, ReadOnly, Label("_lastThrown")]
		private float _lastThrown = 0.0f;
		private Bucket<AudioClip> _audioBucket;

		void Start()
		{
			if (AudioPlayer == null)
				AudioPlayer = GetComponent<AudioSource>();

			_enemyController = GetComponent<EnemyController>();
			_renderer = GetComponent<Renderer>();
			_audioBucket = new(ThrowSounds);
		}

		void Update()
		{
			if (_enemyController.Broken && Time.time - _lastThrown >= ThrowDelay && IsRubyInRange())
			{
				Throw();
				_lastThrown = Time.time;
			}
		}

		void Throw()
		{
			if (Instantiate(WrenchPrefab, _renderer.bounds.center, Quaternion.identity).TryGetComponent(out Wrench wrench))
			{
				RubyController ruby = RubyController.Get();
				Vector2 dir = ((ruby.TryGetComponent(out Renderer renderer) ? renderer.bounds.center : ruby.transform.position) - _renderer.bounds.center).normalized;
				wrench.Launch(gameObject, dir, ThrowForce, Random.Range(-ThrowAngularSpeedRange, ThrowAngularSpeedRange));
				AudioPlayer.PlayOneShot(_audioBucket.Sample());
			}
		}

		public bool IsRubyInRange() =>
			(transform.position - RubyController.Get().transform.position).magnitude <= ActivationRadius;
	}
}
