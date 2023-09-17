using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace Submodules.Unity_Essentials.Static
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
	}
}