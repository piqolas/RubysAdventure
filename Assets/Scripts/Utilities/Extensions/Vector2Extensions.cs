using UnityEngine;

namespace piqey.Utilities.Extensions
{
	public static class Vector2Extensions
	{
		public static bool Approximately(this ref Vector2 vec_A, Vector2 vec_B) =>
			Mathf.Approximately(vec_A.x, vec_B.x) && Mathf.Approximately(vec_A.y, vec_B.y);
		
		public static bool ApproximatelyOr(this ref Vector2 vec_A, Vector2 vec_B) =>
			Mathf.Approximately(vec_A.x, vec_B.x) || Mathf.Approximately(vec_A.y, vec_B.y);

		public static bool ApproximatelyOrNot(this ref Vector2 vec_A, Vector2 vec_B) =>
			!Mathf.Approximately(vec_A.x, vec_B.x) || !Mathf.Approximately(vec_A.y, vec_B.y);
	}
}
