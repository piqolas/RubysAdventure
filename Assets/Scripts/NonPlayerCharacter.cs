using UnityEngine;
using piqey.Utilities.Editor;

namespace piqey
{
	public class NonPlayerCharacter : MonoBehaviour
	{
		public GameObject DialogBox;
		[Min(0.0f)]
		public float DisplayTime = 5.0f;

		[SerializeField, ReadOnly, Label("_lastDisplayed")]
		private float _lastDisplayed = 0.0f;

		void Update()
		{
			if (DialogBox.activeSelf && Time.time - _lastDisplayed >= DisplayTime)
				DialogBox.SetActive(false);
		}

		public void DisplayDialog()
		{
			_lastDisplayed = Time.time;
			DialogBox.SetActive(true);
		}
	}
}
