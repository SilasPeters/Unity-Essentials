using UnityEngine;

namespace Unity_Essentials.Static
{
	public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		protected abstract void Awake();

		public static void Forget() => _instance = null;

		private static T _instance;
		public static T Instance => _instance ??= FindObjectOfType<T>();
	}
}