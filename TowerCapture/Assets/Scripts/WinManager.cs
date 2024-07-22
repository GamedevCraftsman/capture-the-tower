using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WinManager : MonoBehaviour
{
    [SerializeField] UnityEvent showUI;
    [SerializeField] UnityEvent clearLists;
    [SerializeField]
    GameObject[] mapPrefabs;
    [SerializeField] GameObject backPanel;
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject loseText;
    [SerializeField] GameObject winText;
    [SerializeField] Text prizeText;
    [SerializeField] Text lvlText;
    [SerializeField] float fadeTime;
    [SerializeField] public int countOfSpawnedSoldiers;
    [SerializeField] int multiplier;
    [SerializeField] CoinsManager coinsManagerScript;
    [SerializeField] Button collectButton;

    [SerializeField] TowerValuesManager towerValuesManagerScript;
    [HideInInspector] public EnemyAmountManager enemyAmountManagerScript;
    [SerializeField] GameObject spawnPoint;
    [SerializeField] GameObject currentMap;

    [Header("Audio Source")]
    [SerializeField] AudioSource buttonClick;
    [SerializeField] AudioSource winSound;
    [SerializeField] AudioSource loseSound;
    [SerializeField] AudioSource menuMusic;
    [SerializeField] AudioSource playMusic;

    int lvl = 1;
    bool isplayerWin = false;
    int numberOfCurrentPrefab = 0;
    int prize;
    int hasPlayed = 0;
	private void Awake()
	{
        hasPlayed = PlayerPrefs.GetInt("hasPlayedWinManager", hasPlayed);
        SetValues();
        GetValues();
    }

	private void Start()
	{
        currentMap = Instantiate(mapPrefabs[numberOfCurrentPrefab], spawnPoint.transform.position, Quaternion.identity);
	}

	void GetValues()
    {
		numberOfCurrentPrefab = PlayerPrefs.GetInt("numberOfCurrentPrefab");
		lvl = PlayerPrefs.GetInt("lvl");
	}

    void SetValues()
    {
        if (hasPlayed == 0)
        {
            PlayerPrefs.SetInt("lvl", lvl);
            hasPlayed++;
            PlayerPrefs.SetInt("hasPlayedWinManager", hasPlayed);
        } 
    }

    private void Update()
	{
		lvlText.text = lvl.ToString();
	}

	public void CountAPrize(string winSide)
    {
        playMusic.Stop();
        if (winSide == "Player")
        {
            winSound.Play();
            StartCoroutine(ShowWinPanel());         
        }
        else if (winSide == "Enemy")
        {
            loseSound.Play();
            StartCoroutine(ShowLosePanel());
        }
    }

    IEnumerator ShowWinPanel()
    {
        isplayerWin = true;
        prize = countOfSpawnedSoldiers * multiplier;
		winText.SetActive(true);
		backPanel.SetActive(true);
		winPanel.transform.localScale = Vector3.zero;
		winPanel.SetActive(true);
        winPanel.transform.DOScale(1f, fadeTime).SetEase(Ease.OutExpo);
		yield return new WaitForSeconds(fadeTime);
        prizeText.DOText("+" + prize, fadeTime, true, ScrambleMode.Numerals);
		yield return new WaitForSeconds(fadeTime);
		collectButton.interactable = true;
	}

    IEnumerator ShowLosePanel()
    {
		prize = countOfSpawnedSoldiers;
		loseText.SetActive(true);
		backPanel.SetActive(true);
		winPanel.transform.localScale = Vector3.zero;
		winPanel.SetActive(true);
		winPanel.transform.DOScale(1f, fadeTime).SetEase(Ease.OutExpo);
		yield return new WaitForSeconds(fadeTime);
		prizeText.DOText("+" + prize, fadeTime, true, ScrambleMode.Numerals);
		yield return new WaitForSeconds(fadeTime);
		collectButton.interactable = true;
	}

    public void CollectButton(Button button)
    {
        StartCoroutine(HidePanel(button));
    }

    IEnumerator HidePanel(Button button)
    {
        button.enabled = false;
        buttonClick.Play();
		winPanel.transform.DOScale(0, fadeTime).SetEase(Ease.InExpo);
		DestroyTowers();
		Destroy(currentMap);
		if (isplayerWin && numberOfCurrentPrefab < mapPrefabs.Length - 1)
        {    
			currentMap = Instantiate(mapPrefabs[numberOfCurrentPrefab + 1], mapPrefabs[numberOfCurrentPrefab].transform.position, Quaternion.identity);
            numberOfCurrentPrefab++;
            lvl++;
            PlayerPrefs.SetInt("numberOfCurrentPrefab", numberOfCurrentPrefab);
            PlayerPrefs.SetInt("lvl", lvl);
            IncreaseEnemyValues();
        }
        else if (numberOfCurrentPrefab >= mapPrefabs.Length - 1 || !isplayerWin)
        {
			currentMap = Instantiate(mapPrefabs[numberOfCurrentPrefab], mapPrefabs[numberOfCurrentPrefab].transform.position, Quaternion.identity);
		}
		clearLists?.Invoke();
		yield return new WaitForSeconds(fadeTime);
		prizeText.text = "0";
		menuMusic.Play();
        backPanel.SetActive(false);
        showUI?.Invoke();
		coinsManagerScript.AddCoins(prize);
        prize = 0;
        collectButton.interactable = false;
        isplayerWin = false;
		winText.SetActive(false);
		loseText.SetActive(false);
		button.enabled = true;
	}

	void IncreaseEnemyValues()
	{
		towerValuesManagerScript.enemyHP += 10;
		towerValuesManagerScript.enemiesAddTime -= 0.3f;
		towerValuesManagerScript.enemySpawnSpeed -= 0.1f;

        PlayerPrefs.SetInt("enemyHP", towerValuesManagerScript.enemyHP);
        PlayerPrefs.SetFloat("enemiesAddTime", towerValuesManagerScript.enemiesAddTime);
        PlayerPrefs.SetFloat("enemySpawnSpeed", towerValuesManagerScript.enemySpawnSpeed);
	}

	void DestroyTowers()
    {
		foreach (EnemyTower tower in enemyAmountManagerScript.amountOfEnemyTowers)
		{
			Destroy(tower.gameObject);
		}
        enemyAmountManagerScript.amountOfEnemyTowers.Clear();
		foreach (Transform tower in enemyAmountManagerScript.allTowers)
		{
			Destroy(tower.gameObject);
		}
        enemyAmountManagerScript.allTowers.Clear();
	}
}
