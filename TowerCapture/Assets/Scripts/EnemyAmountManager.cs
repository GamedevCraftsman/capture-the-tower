using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyAmountManager : MonoBehaviour
{
	[SerializeField] public List <EnemyTower> amountOfEnemyTowers = new List <EnemyTower>();

	[SerializeField] public List<Transform> allTowers = new List<Transform>();

	TouchInputManager touchInputManagerScript;
	WinManager winManagerScript;
	bool isWin = false;
	private void Start()
	{
		touchInputManagerScript = GameObject.FindAnyObjectByType<TouchInputManager>();
		winManagerScript = GameObject.FindObjectOfType<WinManager>();
		winManagerScript.enemyAmountManagerScript = this;
		foreach (EnemyTower tower in amountOfEnemyTowers)
		{
			tower.allTowers = allTowers;
		}
	}

	private void Update()
	{
		CheckEnemyWin();
		CheckPlayerWin();
	}

	void CheckEnemyWin()
	{
		if (allTowers != null && allTowers.Count(item => item.GetComponent<PlayerTower>() != null) > 0)
		{
			foreach (EnemyTower tower in amountOfEnemyTowers)
			{
				tower.allTowers = allTowers;
			}
		}
		else if (allTowers != null && allTowers.Count(item => item.GetComponent<PlayerTower>() != null) == 0 && !isWin)
		{
			Destroy(touchInputManagerScript.point);
			isWin = true;
			OffSound();
			winManagerScript.CountAPrize("Enemy");
			foreach (EnemyTower tower in amountOfEnemyTowers)
			{
				tower.isFound = true;
				tower.canSpawn = false;
			}
		}
	}

	void CheckPlayerWin()
	{
		if (amountOfEnemyTowers.Count == 0 && !isWin)
		{
			Destroy(touchInputManagerScript.point);
			isWin = true;
			OffSound();
			winManagerScript.CountAPrize("Player");
			foreach (Transform tower in allTowers)
			{
				if (tower != null && tower.GetComponent<PlayerTower>() != null)
				{
					tower.GetComponent<PlayerTower>().canSpawn = false;
				}
			}
		}
	}

	void OffSound()
	{
		foreach(Transform tower in allTowers)
		{
			if (tower.GetComponent<PlayerTower>() != null)
			{
				tower.GetComponent <PlayerTower>().canSound = false;
			}
			else if (tower.GetComponent<NeutralTower>() != null)
			{
				tower.GetComponent <NeutralTower>().canSound = false;
			}
		}
		
		foreach(EnemyTower tower in amountOfEnemyTowers)
		{
			if (tower.GetComponent<EnemyTower>() != null)
			{
				tower.GetComponent <EnemyTower>().canSound = false;
			}
		}
	}
}
