using Unity_Essentials.Static.ExtensionMethods;
using UnityEngine;
using UnityEngine.UI;
using static Unity_Essentials.Static.HighLevelFunctions;

namespace Unity_Essentials.Components
{
	[RequireComponent(typeof(Graphic))]
	public class GraphicFadeIn : MonoBehaviour
	{
		private Graphic _graphic;
		public float delay;
		public float duration;

		private void Awake()
		{
			_graphic = GetComponent<Graphic>();
			Color original = _graphic.color;
			_graphic.color = originalColorWithOpacity(0);

			StartCoroutine(Wait(delay).ThenLerp(duration, progress =>
				_graphic.color = originalColorWithOpacity(progress * original.a)));

			Color originalColorWithOpacity(float opacity) => new Color(original.r, original.g, original.b, opacity);
		}
	}
}