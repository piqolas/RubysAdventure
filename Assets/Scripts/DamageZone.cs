using UnityEngine;

namespace piqey
{
	public class DamageZone : MonoBehaviour
	{
		[Tooltip("The amount of HP by which this zone should damage the player.")]
		public int DamageAmount = 10;

		void OnTriggerStay2D(Collider2D other)
		{
			if (other.TryGetComponent(out RubyController ruby))
				ruby.Health -= DamageAmount;
		}
	}
}
