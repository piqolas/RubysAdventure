using UnityEngine;
using piqey.Utilities.Editor;
using piqey.Utilities;

namespace piqey
{
	public class NonPlayerCharacter : MonoBehaviour
	{
		public GameObject DialogBox;
		public AudioSource DialogAudioSource;
		[Min(0.0f)] public float DialogBoxDisplayTime = 5.0f;
		[Min(0.0f)] public float DialogTalkTime = 2.0f;
		public AudioClip[] DialogSounds;

		[SerializeField, ReadOnly, Label("_lastDisplayed")]
		private float _lastDisplayed = 0.0f;
		private Bucket<AudioClip> _dialogBucket;

		void Start()
		{
			if (DialogAudioSource == null)
				DialogAudioSource = gameObject.AddComponent<AudioSource>();

			_dialogBucket = new(DialogSounds);
		}

		void Update()
		{
			if (DialogBox.activeSelf)
			{
				if (Time.time - _lastDisplayed >= DialogBoxDisplayTime)
					DialogBox.SetActive(false);
				else if (!DialogAudioSource.isPlaying && Time.time - _lastDisplayed <= DialogTalkTime)
					DialogAudioSource.PlayOneShot(_dialogBucket.Sample());
			}
		}

		public void DisplayDialog()
		{
			_lastDisplayed = Time.time;
			DialogBox.SetActive(true);
			DialogAudioSource.Stop();
		}
	}
}
