using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Unity_Essentials.Static
{
	public static class InputExtensions
	{
		public static bool AnyDown(this IEnumerable<KeyCode> keyCodes)
			=> keyCodes.Any(Input.GetKeyDown);

		public static bool AnyPressed(this IEnumerable<KeyCode> keyCodes)
			=> keyCodes.Any(Input.GetKey);

		public static bool AnyUp(this IEnumerable<KeyCode> keyCodes)
			=> keyCodes.Any(Input.GetKeyUp);
	}
}