using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CoinsManager : MonoBehaviour
{
    [SerializeField] public int amountOfCoins;
    [SerializeField] Text coinsText;

	private void Awake()
	{
		amountOfCoins = PlayerPrefs.GetInt("amountOfCoins");
		coinsText.text = amountOfCoins.ToString();
	}

	private void Start()
	{
		coinsText.text = amountOfCoins.ToString();
	}

	public void AddCoins(int coins)
    {
		amountOfCoins += coins;
		PlayerPrefs.SetInt("amountOfCoins", amountOfCoins);
		coinsText.DOText(amountOfCoins.ToString(), 1f, true, ScrambleMode.Numerals);
    }
}
