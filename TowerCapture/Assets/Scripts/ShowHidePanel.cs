using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShowHidePanel : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject backPanel;
    [SerializeField] float fadeTime;
	[SerializeField] AudioSource buttonClick;
    public void ShowPanel()
    {
		buttonClick.Play();
		backPanel.SetActive(true);
		panel.transform.localScale = Vector3.zero;
		panel.SetActive(true);
		panel.transform.DOScale(1f, fadeTime).SetEase(Ease.OutExpo);
	}

	public void HidePanel(Button button)
    {
		StartCoroutine(Hide(button));
	}

	IEnumerator Hide(Button button)
	{
		buttonClick.Play();
		button.enabled = false;
		panel.transform.DOScale(0, fadeTime).SetEase(Ease.InExpo);
		yield return new WaitForSeconds(fadeTime);
		backPanel.SetActive(false);
		button.enabled = true;
	}

}
