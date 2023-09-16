using System.Collections;
using UnityEngine;

namespace Submodules.Unity_Essentials.Static
{
	public static class Movement
	{
		public static IEnumerator MoveTo(Transform t, Vector3 targetPos, float duration)
		{
			Vector3 startPos    = t.position;
			float   timeStarted = Time.time;

			while (Time.time - timeStarted < duration)
			{
				t.position = Vector3.Lerp(startPos, targetPos, (Time.time - timeStarted) / duration);
				yield return null;
			}
		}
	}
}