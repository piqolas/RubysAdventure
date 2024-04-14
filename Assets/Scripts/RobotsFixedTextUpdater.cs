using UnityEngine;
using TextMeshProUGUI = TMPro.TextMeshProUGUI;
using piqey.Utilities.Editor;

namespace piqey
{
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class RobotsFixedTextUpdater : MonoBehaviour
	{
		[SerializeField, ReadOnly, Label("_textElement")]
		private TextMeshProUGUI _textElement;

		void Start() => _textElement = GetComponent<TextMeshProUGUI>();
		void Update() => _textElement.text = $"Robots Fixed: {RubyController.RobotsFixed}";
	}
}
