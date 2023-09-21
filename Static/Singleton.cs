using UnityEngine;

namespace Unity_Essentials.Static
{
	public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		protected virtual void Awake()
		{
			_instance = null;
			print("Forgot instance");
		}

		private static T _instance;
		public static T Instance
		{
			get {
				if (_instance != null) return _instance;
				var found = FindObjectOfType<T>();
				_instance = found;
				return _instance;
			}
		}
	}
}