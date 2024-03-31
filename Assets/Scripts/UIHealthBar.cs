using UnityEngine;
using UnityEngine.UI;
using piqey.Utilities.Editor;

namespace piqey
{
	// I really prefer referencing each instance I make.
	// At least in this instance, where we're INSTANCING
	// a CLASS that's not a STATIC class.
	public class UIHealthBar : MonoBehaviour
	{
		public RubyController Ruby;
		public Image Mask;

		[SerializeField, ReadOnly, Label("_originalSize")]
		private float _originalSize;

		void Start()
		{
			Ruby.OnHealthChanged += () => SetValue((float)Ruby.Health / Ruby.MaxHealth);
			_originalSize = Mask.rectTransform.rect.width;
		}

		public void SetValue(float value)
		{
			// Debug.Log($"Setting size to ${value}");
			Mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _originalSize * value);
		}
	}
}
