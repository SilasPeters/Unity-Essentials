using UnityEngine;

namespace Unity_Essentials.Static
{
	// TODO remove internal modifier, and fix this class. Should it be a (abstract) component?
	// https://gamedevbeginner.com/singletons-in-unity-the-right-way/
	internal abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour
	{
		public new static T Instance
		{
			get
			{
				if (_instance != null) return _instance;

				_instance = FindObjectOfType<T>();
				DontDestroyOnLoad(_instance.gameObject);
				return _instance;
			}
		}
	}
}
