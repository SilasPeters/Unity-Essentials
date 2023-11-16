using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace Unity_Essentials.Static
{
	public static class HighLevelFunctions
	{
		public static IEnumerator RepeatWithInterval(float duration, Action action, [CanBeNull] Func<bool> loopCondition = null)
		{
			float nextTimestamp;
			setNextTimestamp();

			while (loopCondition?.Invoke() ?? true)
			{
				yield return new WaitUntil(() => nextTimestamp < Time.time);
				action.Invoke();
				setNextTimestamp();
			}

			void setNextTimestamp() => nextTimestamp = Time.time + duration;
		}

		public static IEnumerator Lerp(float duration, Action<float> action, bool onceAfter = false)
		{
			float timeStart = Time.time;

			while (Time.time - timeStart < duration)
			{
				action.Invoke((Time.time - timeStart) / duration);
				yield return null;
			}

			if (onceAfter)
			{
				action.Invoke(1f);
			}
		}

		public static IEnumerator Wait(float seconds)
		{
			yield return new WaitForSeconds(seconds);
		}
	}
}