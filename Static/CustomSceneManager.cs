using UnityEngine.SceneManagement;

namespace Unity_Essentials.Static
{
	public static class CustomSceneManager
	{
		public static void LoadNextScene()
		{
			int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
			SceneManager.LoadScene(currentSceneIndex + 1);
		}
	}
}