using System;
using System.Collections;
using UnityEngine;

namespace Submodules.Unity_Essentials.Static
{
	public static class General
	{
		public static IEnumerator RepeatWithInterval(float duration, Action action)
		{
			float lastTimestamp = Time.time;
			while (true)
			{
				if (lastTimestamp + duration > Time.time)
					yield return null;

				action.Invoke();
			}
		}

		public static IEnumerator RepeatWithInterval(float duration, Action action, Func<bool> loopCondition)
		{
			float lastTimestamp = Time.time;
			while (loopCondition.Invoke())
			{
				if (lastTimestamp + duration > Time.time)
					yield return null;

				action.Invoke();
			}
		}
	}
}