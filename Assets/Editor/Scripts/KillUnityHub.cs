using System.Diagnostics;
using UnityEditor;

namespace piqey.Editor
{
	[InitializeOnLoad]
	static class KillUnityHub
	{
		private const string SessionFlag = "KillUnityHubExecuted";

		static KillUnityHub()
		{
			if (!SessionState.GetBool(SessionFlag, false))
			{
				// Check if this is the first time the editor is loaded (flag not set yet)
				SessionState.SetBool(SessionFlag, true);

				foreach (Process process in Process.GetProcessesByName("Unity Hub"))
				{
					UnityEngine.Debug.Log($"Killing Unity Hub (PID {process.Id})");
					process.Kill();
				}
			}
		}
	}
}
