using UnityEngine;

namespace Unity_Essentials.Static
{
	public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour
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