using System;
using System.Collections;
using UnityEngine;

namespace Unity_Essentials.Static.ExtensionMethods
{
	// ReSharper disable once InconsistentNaming
	public static class IEnumeratorExtensions
	{
		public static IEnumerator Then(this IEnumerator first, IEnumerator second)
		{
			yield return first;
			yield return second;
		}
		public static IEnumerator ThenWait(this IEnumerator first, float seconds)
		{
			yield return first;
			yield return new WaitForSeconds(seconds);
		}

		public static IEnumerator ThenLerp(this IEnumerator first, float duration, Action<float> action)
		{
			yield return first;
			yield return HighLevelFunctions.Lerp(duration, action);
		}
	}
}