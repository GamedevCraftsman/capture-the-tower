using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerValuesManager : MonoBehaviour
{
	[Header("Enemy Values")]
	[SerializeField] public int enemyHP;
	[SerializeField] public float enemySpawnSpeed;
	[SerializeField] public float enemiesAddTime;
	public List<EnemyTower> enemyTowers = new List<EnemyTower>();
	[Header("Player Values")]
	[SerializeField] public int playerHP;
	[SerializeField] public float soldierSpawnSpeed;
	[SerializeField] public float soldiersAddTime;
	public List<PlayerTower> playerTowers = new List<PlayerTower>();
	[Header("Neutral Valued")]
	[SerializeField] public int neutralHP;
	public List<NeutralTower> neutralTowers = new List<NeutralTower>();
	[Header("Touch Input manager")]
	[SerializeField] TouchInputManager touchInputManagerScript;

	int hasPlayed = 0;
	private void Awake()
	{
		hasPlayed = PlayerPrefs.GetInt("hasPlayedValues");
		SetValues();
		GetValues();
	}

	void GetValues()
	{
		enemyHP = PlayerPrefs.GetInt("enemyHP");
		enemySpawnSpeed = PlayerPrefs.GetFloat("enemySpawnSpeed");
		enemiesAddTime = PlayerPrefs.GetFloat("enemiesAddTime");

		playerHP = PlayerPrefs.GetInt("playerHP");
		soldiersAddTime = PlayerPrefs.GetFloat("soldiersAddTime");
		soldierSpawnSpeed = PlayerPrefs.GetFloat("soldierSpawnSpeed");
	}

	void SetValues()
	{
		if (hasPlayed == 0)
		{
			PlayerPrefs.SetInt("enemyHP", enemyHP);
			PlayerPrefs.SetFloat("enemySpawnSpeed", enemySpawnSpeed);
			PlayerPrefs.SetFloat("enemiesAddTime", enemiesAddTime);

			PlayerPrefs.SetInt("playerHP", playerHP);
			PlayerPrefs.SetFloat("soldiersAddTime", soldiersAddTime);
			PlayerPrefs.SetFloat("soldierSpawnSpeed", soldierSpawnSpeed);

			hasPlayed++;
			PlayerPrefs.SetInt("hasPlayedValues", hasPlayed);
		}
	}

	public void StartPlay()
	{
		foreach (EnemyTower tower in enemyTowers)
		{
			tower.canPlay = true;
			tower.GetComponentInChildren<TextMeshPro>().gameObject.GetComponent<MeshRenderer>().enabled = true;
		}

		foreach (PlayerTower tower in playerTowers)
		{
			tower.canPlay = true;
			tower.GetComponentInChildren<TextMeshPro>().gameObject.GetComponent<MeshRenderer>().enabled = true;
		}

		foreach (NeutralTower tower in neutralTowers)
		{
			tower.GetComponentInChildren<TextMeshPro>().gameObject.GetComponent<MeshRenderer>().enabled = true;
			Debug.Log(tower.GetComponentInChildren<TextMeshPro>().gameObject.name);
		}

		touchInputManagerScript = GameObject.FindAnyObjectByType<TouchInputManager>();
		touchInputManagerScript.isGameStart = true;
	}

	public void UpdateValues()
	{
		foreach (PlayerTower tower in playerTowers)
		{
			tower.countOfSoldiers = playerHP;
			tower.timeToSpawnSoldiers = soldierSpawnSpeed;
			tower.timeToAddSoldiers = soldiersAddTime;
		}
	}


	public void ClearLists()
	{
		touchInputManagerScript.isGameStart = false;
		enemyTowers.Clear();
		playerTowers.Clear();
		neutralTowers.Clear();
	}

}
