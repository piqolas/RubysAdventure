using UnityEngine;

namespace piqey
{
	public class HealthCollectible : MonoBehaviour
	{
		[Tooltip("The amount of HP by which this collectible should heal the player.")]
		public int HealingAmount = 15;

		void OnTriggerEnter2D(Collider2D other)
		{
			// These Unity learning tutorials *provided officially by Unity* are blowing me away;
			// why aren't they using the functions they created specifically to shortcut and
			// optimize these situations? It's weird. Might be the fact that it's outdated.
			// Well, I'm going to use the new methods.
			if (other.TryGetComponent(out RubyController ruby) && ruby.Health < ruby.MaxHealth) // Also ensure health is not maxed
			{
				ruby.Health += HealingAmount;
				Destroy(gameObject);
			}
		}
	}
}
