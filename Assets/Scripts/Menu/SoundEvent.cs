using UnityEngine;

/**
 * By <author>electazxz</author>.
 */
public class SoundEvent : MonoBehaviour
{
	public GameObject selectedCharacter;
	void OnMouseDown()
	{
		GetComponent<AudioSource>().Play();
		selectedCharacter.SetActive(true);
	}
}