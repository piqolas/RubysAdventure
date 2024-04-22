using piqey.Utilities.Editor;
using UnityEngine;

namespace piqey
{
	public class HealthCollectible : MonoBehaviour
	{
		[Tooltip("The amount of HP by which this collectible should heal the player.")]
		public int HealingAmount = 15;
		public AudioClip HealingSound;

		[SerializeField, ReadOnly, Label("_renderer")]
		private SpriteRenderer _renderer;

		void Start()
		{
			_renderer = GetComponent<SpriteRenderer>();
		}

		void Update()
		{
			transform.eulerAngles = 7.5f * Mathf.Sin(5.0f * Time.time + GetInstanceID()) * Vector3.forward;
		}

		void OnTriggerEnter2D(Collider2D other)
		{
			// These Unity learning tutorials *provided officially by Unity* are blowing me away;
			// why aren't they using the functions they created specifically to shortcut and
			// optimize these situations? It's weird. Might be the fact that it's outdated.
			// Well, I'm going to use the new methods.
			if (other.TryGetComponent(out RubyController ruby) && ruby.Health < ruby.MaxHealth) // Also ensure health is not maxed
			{
				ruby.Health += HealingAmount;
				ruby.PlaySound(HealingSound);

				Destroy(gameObject);
			}
		}
	}
}
