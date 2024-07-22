using DG.Tweening;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	[SerializeField] GameObject[] uiObjects;
	[SerializeField] float fadeTime;

	public void HideUI()
	{
		foreach (GameObject go in uiObjects)
		{
			go.transform.DOScale(0f, fadeTime).SetEase(Ease.OutExpo);
		}
	}
	public void ShowUI()
	{
		foreach (GameObject go in uiObjects)
		{
			go.transform.DOScale(1f, fadeTime).SetEase(Ease.InExpo);
		}
	}
}
