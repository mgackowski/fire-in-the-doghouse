using UnityEngine;

/**
 * By <author>electazxz</author>.
 * Destroy music object added by <author>mgackowski</author>.
 */
public class AnimationEndLoadScene : MonoBehaviour
{
	public string level;

	[Header("Stop music on scene end")]
	public bool destroyMusic = false;

	public void LoadNewScene()
	{
		if (destroyMusic)
		{
			Destroy(GameObject.FindGameObjectWithTag("Music"));
		}

		UnityEngine.SceneManagement.SceneManager.LoadScene(level);
	}
}