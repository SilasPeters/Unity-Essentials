using UnityEngine;

namespace Unity_Essentials.Static
{
	public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		protected abstract void Awake();

		protected static T _instance;
		public static T Instance
		{
			get
			{
				if (_instance == null)
					_instance = FindObjectOfType<T>();
				return _instance;
			}
		}
	}
}