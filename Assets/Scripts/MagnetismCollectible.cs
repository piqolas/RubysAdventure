using piqey.Utilities.Editor;
using UnityEngine;

namespace piqey
{
	public class MagnetismCollectible : MonoBehaviour
	{
		public float MagnetismColorChangeRate = 0.35f;
		public AudioClip CollectedSound;

		[SerializeField, ReadOnly, Label("_renderer")]
		private Renderer _renderer;

		void Start()
		{
			_renderer = GetComponent<Renderer>();
			_renderer.material.color = Color.red;
		}

		void Update()
		{
			Color.RGBToHSV(_renderer.material.color, out float h, out float s, out float v);
			_renderer.material.color = Color.HSVToRGB(h + Time.deltaTime * MagnetismColorChangeRate, s, v);
		}

		void OnTriggerEnter2D(Collider2D other)
		{
			if (other.TryGetComponent(out RubyController ruby))
			{
				ruby.MagnetismActive = true;
				ruby.PlaySound(CollectedSound);

				Destroy(gameObject);
			}
		}
	}
}
