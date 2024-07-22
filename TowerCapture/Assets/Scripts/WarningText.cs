using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WarningText : MonoBehaviour
{
    [SerializeField] float waitTime;

    bool isShow = false;
    public void ShowWarningText(string text)
    {
		gameObject.SetActive(true);
		StartCoroutine(ShowHide(text));   
    }

    IEnumerator ShowHide(string text)
    {
        if (!isShow)
        {
            isShow = true;
            gameObject.GetComponentInChildren<Text>().text = text;
            yield return new WaitForSeconds(waitTime);
            gameObject.SetActive(false);
            isShow = false;
        }
    }
}
