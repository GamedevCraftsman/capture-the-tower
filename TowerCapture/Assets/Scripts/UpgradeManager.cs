using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
	[Header("Scripts")]
	[SerializeField] UnityEvent warningText;
	[SerializeField] UnityEvent updateValues;
	[SerializeField] TowerValuesManager towerValuesManagerScript;
	[SerializeField] CoinsManager coinsManagerScript;
	[Header("Upgrade Start Soldiers Count")]
	[SerializeField] Button countButton;
	[SerializeField] Text soldiersCountLvlText;
	[SerializeField] int addToNextCountUpgrade;
	[SerializeField] int maxSoldiersCount;
	[SerializeField] int countUpgradePrice;
	[Header("Upgrade Speed Soldiers Spawn")]
	[SerializeField] Button speedSpawnButton;
	[SerializeField] Text speedSpawnSoldierText;
	[SerializeField] int addToNextSpeedSpawnUpgrade;
	[SerializeField] int maxSoldierSpeedSpawn;
	[SerializeField] int speedSpawnUpgradePrice;
	[SerializeField] float speedSpawnSubtrahend;
	[Header("Upgrade Speed Soldiers Add")]
	[SerializeField] Button speedAddButton;
	[SerializeField] Text speedAddSoldierText;
	[SerializeField] int addToNextAddSpeedUpgrade;
	[SerializeField] int maxSoldierAddSpeed;
	[SerializeField] int addSpeedUpgradePrice;
	[SerializeField] float addSpeedSubtrahend;
	[Header("Audio Sources")]
	[SerializeField] AudioSource buttonClick;

	//Upgrade soldiers count.
	int soldiersCountLvl = 1;
	//Upgrade spawn soldier speed.
	int spawnSpeedLvl = 1;
	//Upgrade add speed.
	int addSpeedLvl = 1;
	int hasPlayed = 0;
	private void Awake()
	{
		hasPlayed = PlayerPrefs.GetInt("hasPlayedUpgrade");
		SetValues();
		GetValues();
	}

	void GetValues()
	{
		countUpgradePrice = PlayerPrefs.GetInt("countUpgradePrice");
		soldiersCountLvl = PlayerPrefs.GetInt("soldiersCountLvl");

		speedSpawnUpgradePrice = PlayerPrefs.GetInt("speedSpawnUpgradePrice");
		spawnSpeedLvl = PlayerPrefs.GetInt("spawnSpeedLvl");

		addSpeedUpgradePrice = PlayerPrefs.GetInt("addSpeedUpgradePrice");
		addSpeedLvl = PlayerPrefs.GetInt("addSpeedLvl");
	}

	void SetValues()
	{
		if (hasPlayed == 0)
		{
			PlayerPrefs.SetInt("countUpgradePrice", countUpgradePrice);
			PlayerPrefs.SetInt("soldiersCountLvl", soldiersCountLvl);

			PlayerPrefs.SetInt("speedSpawnUpgradePrice", speedSpawnUpgradePrice);
			PlayerPrefs.SetInt("spawnSpeedLvl", spawnSpeedLvl);

			PlayerPrefs.SetInt("addSpeedUpgradePrice", addSpeedUpgradePrice);
			PlayerPrefs.SetInt("addSpeedLvl", addSpeedLvl);

			hasPlayed++;
			PlayerPrefs.SetInt("hasPlayedUpgrade", hasPlayed);
			Debug.Log("Set");
		}
	}

	//private void Start()
	//{
	//	soldiersCountLvlText.text = soldiersCountLvl + " lvl\nPrice: " + countUpgradePrice;
	//	speedSpawnSoldierText.text = spawnSpeedLvl + " lvl\nPrice: " + speedSpawnUpgradePrice;
	//	speedAddSoldierText.text = addSpeedLvl + " lvl\nPrice: " + addSpeedUpgradePrice;
	//}

	private void Update()
	{
		CountUpgradeLabel();
		SpawnUpgradeLabel();
		AddUpgradeLabel();
	}

	public void UpgradeStartSoldiersCount()
	{
		buttonClick.Play();
		if (coinsManagerScript.amountOfCoins - countUpgradePrice >= 0 && soldiersCountLvl < maxSoldiersCount)
		{
			coinsManagerScript.AddCoins(-countUpgradePrice);
			countUpgradePrice += addToNextCountUpgrade;
			towerValuesManagerScript.playerHP++;
			soldiersCountLvl++;

			SaveStartCount();
		}
		else if (coinsManagerScript.amountOfCoins - countUpgradePrice < 0)
		{
			warningText.Invoke();
		}
		
		updateValues.Invoke();
	}

	void CountUpgradeLabel()
	{
		soldiersCountLvlText.text = soldiersCountLvl + " lvl\nPrice: " + countUpgradePrice;
		if (soldiersCountLvl == maxSoldiersCount)
		{
			countButton.interactable = false;
			soldiersCountLvlText.text = "Max";
			countButton.GetComponentInChildren<Text>().text = "MAX";
		}
	}

	void SaveStartCount()
	{
		PlayerPrefs.SetInt("countUpgradePrice", countUpgradePrice);
		PlayerPrefs.SetInt("soldiersCountLvl", soldiersCountLvl);
		PlayerPrefs.SetInt("playerHP", towerValuesManagerScript.playerHP);
	}

	public void UpgradeSpeedSoldiersSpawn()
	{
		buttonClick.Play();
		if (coinsManagerScript.amountOfCoins - speedSpawnUpgradePrice >= 0 && spawnSpeedLvl < maxSoldierSpeedSpawn)
		{
			coinsManagerScript.AddCoins(-speedSpawnUpgradePrice);
			speedSpawnUpgradePrice += addToNextAddSpeedUpgrade;
			towerValuesManagerScript.soldierSpawnSpeed -= speedSpawnSubtrahend;
			spawnSpeedLvl++;

			SaveSpeedSpawn();
		}
		else if (coinsManagerScript.amountOfCoins - speedSpawnUpgradePrice < 0)
		{
			warningText.Invoke();
		}	

		updateValues.Invoke();
	}

	void SpawnUpgradeLabel()
	{
		speedSpawnSoldierText.text = spawnSpeedLvl + " lvl\nPrice: " + speedSpawnUpgradePrice;

		if (spawnSpeedLvl == maxSoldierSpeedSpawn)
		{
			speedSpawnButton.interactable = false;
			speedSpawnSoldierText.text = "Max";
			speedSpawnButton.GetComponentInChildren<Text>().text = "MAX";
		}
	}

	void SaveSpeedSpawn()
	{
		PlayerPrefs.SetInt("speedSpawnUpgradePrice", speedSpawnUpgradePrice);
		PlayerPrefs.SetInt("spawnSpeedLvl", spawnSpeedLvl);
		PlayerPrefs.SetFloat("soldierSpawnSpeed", towerValuesManagerScript.soldierSpawnSpeed);
	}

	public void UpgradeAddSoldierSpeed()
	{
		buttonClick.Play();
		if (coinsManagerScript.amountOfCoins - addSpeedUpgradePrice >= 0 && addSpeedLvl < maxSoldierAddSpeed)
		{
			coinsManagerScript.AddCoins(-addSpeedUpgradePrice);
			addSpeedUpgradePrice += addToNextSpeedSpawnUpgrade;
			towerValuesManagerScript.soldiersAddTime -= addSpeedSubtrahend;
			addSpeedLvl++;

			SaveAddSpeed();
		}
		else if (coinsManagerScript.amountOfCoins - addSpeedUpgradePrice < 0)
		{
			warningText.Invoke();
		}

		updateValues.Invoke();
	}

	void AddUpgradeLabel()
	{
		speedAddSoldierText.text = addSpeedLvl + " lvl\nPrice: " + addSpeedUpgradePrice;

		if (addSpeedLvl == maxSoldierAddSpeed)
		{
			speedAddButton.interactable = false;
			speedAddSoldierText.text = "Max";
			speedAddButton.GetComponentInChildren<Text>().text = "MAX";
		}
	}

	void SaveAddSpeed()
	{
		PlayerPrefs.SetInt("addSpeedUpgradePrice", addSpeedUpgradePrice);
		PlayerPrefs.SetInt("addSpeedLvl", addSpeedLvl);
		PlayerPrefs.SetFloat("soldiersAddTime", towerValuesManagerScript.soldiersAddTime);
	}
}
