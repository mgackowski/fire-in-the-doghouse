using UnityEngine;

/**
 * By <author>electazxz</author>.
 */
public class AnimationEndLoadScene : MonoBehaviour
{
	public string level;
	public void LoadNewScene()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(level);
	}
}