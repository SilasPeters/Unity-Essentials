using System;
using TMPro;
using Unity_Essentials.Static;
using UnityEngine;

namespace Unity_Essentials.Components
{
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class Timer : MonoBehaviour
	{
		private TextMeshProUGUI _text;

		private void Awake()
		{
			_text = GetComponent<TextMeshProUGUI>();
		}

		private void Update()
		{
			if (Singleton<GameManager>.Instance.CurrentGameState == GameManager.GameState.Playing)
				_text.text = TimeSpan.FromSeconds(Time.timeSinceLevelLoad).ToString(@"mm\:ss");
		}
	}
}