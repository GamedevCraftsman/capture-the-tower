using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PlayerTower : MonoBehaviour
{
	WinManager winManagerScript;
	[SerializeField] TextMeshPro soldiersCountText;
	[HideInInspector] public int countOfSoldiers;
	[SerializeField] GameObject soldierPrefab;
	[HideInInspector] public float timeToSpawnSoldiers;
	[HideInInspector] public float timeToAddSoldiers;

	[SerializeField] LineRenderer linePrefab;
	public Transform saveEnemyTowerPos;
	public bool isLineAdded;
	public LineRenderer lineRenderer;

	[SerializeField] public List<NeutralTower> neutrallTowers = new List<NeutralTower>();
	[SerializeField] public List<GameObject> attackTowers = new List<GameObject>();
	[SerializeField] public List<EnemyTower> enemyTowers = new List<EnemyTower>();
	[SerializeField] public List<PlayerTower> playerTowers = new List<PlayerTower>();

	AudioSource towerDestroySound;
	TowerValuesManager towerValuesManagerScript;
	GameObject soldier;
	[HideInInspector] public bool canSpawn = false;
	bool isReadyToAdd = true;
	bool isReadyToSpawn = true;
	[HideInInspector] public bool canPlay = false;
	[HideInInspector] public bool canSound = true;
	[HideInInspector] public bool isDestroyed = false;
	private void Start()
	{
		winManagerScript = GameObject.FindObjectOfType<WinManager>();
		towerValuesManagerScript = GameObject.FindObjectOfType<TowerValuesManager>();
		towerValuesManagerScript.playerTowers.Add(this);

		towerDestroySound = GameObject.FindGameObjectWithTag("TowerDestroy").GetComponent<AudioSource>();

		countOfSoldiers = towerValuesManagerScript.playerHP;
		timeToSpawnSoldiers = towerValuesManagerScript.soldierSpawnSpeed;
		timeToAddSoldiers = towerValuesManagerScript.soldiersAddTime;

		soldiersCountText.text = countOfSoldiers.ToString();
	}

	private void Update()
	{
		if (canPlay)
		{
			soldiersCountText.text = countOfSoldiers.ToString();
			SpawnArmy();
			AddSoldiers();
		}
	}

	void SpawnArmy()
	{
		if (canSpawn && countOfSoldiers >= 0 && isReadyToSpawn)
		{
			StartCoroutine(SpawnSlodiers());
		}
	}

	IEnumerator SpawnSlodiers()
	{
		isReadyToSpawn = false;
		yield return new WaitForSeconds(timeToSpawnSoldiers);
		if (canSpawn && countOfSoldiers > 0)
		{
			soldier = Instantiate(soldierPrefab, transform.position, Quaternion.identity);
			winManagerScript.countOfSpawnedSoldiers++;
			soldier.GetComponent<PlayerSoldierMovement>().playerTowerScript = this;
			soldier.GetComponent<PlayerSoldierMovement>().enemyTowerPos = saveEnemyTowerPos;
			countOfSoldiers--;
		}
		isReadyToSpawn = true;
	}

	void AddSoldiers()
	{
		if (isReadyToAdd)
		{
			StartCoroutine(Add());
		}
	}

	IEnumerator Add()
	{
		isReadyToAdd = false;
		yield return new WaitForSeconds(timeToAddSoldiers);
		countOfSoldiers++;
		soldiersCountText.text = countOfSoldiers.ToString();
		isReadyToAdd = true;
	}

	public void SpawnLine(RaycastHit rayHit)
	{
		if (IsClickedGameobjectWithTad(rayHit, "Enemy", isLineAdded))
		{
			RemoveEnemyTowerSave();
			RemovePlayerTowerSave();
			enemyTowers.Add(rayHit.transform.gameObject.GetComponent<EnemyTower>());
			rayHit.transform.gameObject.GetComponent<EnemyTower>().attackTowers.Add(gameObject);
			RemoveNeutralTowerSave();
			Spawn();
		}
		else if (IsClickedGameobjectWithTad(rayHit, "Player", isLineAdded) && rayHit.transform.position != transform.position)
		{
			RemoveEnemyTowerSave();
			RemovePlayerTowerSave();
			playerTowers.Add(rayHit.transform.gameObject.GetComponent<PlayerTower>());
			rayHit.transform.gameObject.GetComponent<PlayerTower>().attackTowers.Add(gameObject);
			RemoveNeutralTowerSave();
			Spawn();
		}
		else if (IsClickedGameobjectWithTad(rayHit, "Neutral", isLineAdded))
		{
			RemoveEnemyTowerSave();
			RemovePlayerTowerSave();
			RemoveNeutralTowerSave();
			neutrallTowers.Add(rayHit.transform.gameObject.GetComponent<NeutralTower>());
			rayHit.transform.gameObject.GetComponent<NeutralTower>().attackTowers.Add(gameObject);
			Spawn();
		}
	}

	void Spawn()
	{
		if (lineRenderer != null)
		{
			Destroy(lineRenderer.gameObject);
		}
		lineRenderer = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
		lineRenderer.positionCount = 2;

		lineRenderer.SetPosition(0, transform.position);
		lineRenderer.SetPosition(1, saveEnemyTowerPos.position);
		canSpawn = true;
	}

	bool IsClickedGameobjectWithTad(RaycastHit rayHit, string gameObjectName, bool isLineAdded)
	{
		return rayHit.transform.tag == gameObjectName && !isLineAdded;
	}
	// Додай ремув для плеєра, бо коли буде помирати плейєр, то буде крашитись гра. Тобто додати плеєра до атакуючих веж.
	void RemoveNeutralTowerSave()
	{
		if (neutrallTowers.Count > 0)
		{
			foreach (NeutralTower tower in neutrallTowers)
			{
				tower.attackTowers.Distinct();
				tower.attackTowers.Remove(gameObject);
			}
			neutrallTowers.Clear();
		}
	}
	void RemoveEnemyTowerSave()
	{
		if (enemyTowers.Count > 0)
		{
			foreach (EnemyTower tower in enemyTowers)
			{
				tower.attackTowers.Distinct();
				tower.attackTowers.Remove(gameObject);
			}
			enemyTowers.Clear();
		}
	}

	void RemovePlayerTowerSave()
	{
		if (playerTowers.Count > 0)
		{
			foreach (PlayerTower tower in playerTowers)
			{
				tower.attackTowers.Distinct();
				tower.attackTowers.Remove(gameObject);
			}
			enemyTowers.Clear();
		}
	}

	//Add for enemy tower.
	public void OnDestroy()
	{
		if (canSound)
		{
			towerDestroySound.Play();
		}

		foreach (GameObject tower in attackTowers)
		{
			if (tower != null)
			{
				if (tower.GetComponent<PlayerTower>() != null && tower.GetComponent<PlayerTower>().lineRenderer != null)
				{
					Destroy(tower.GetComponent<PlayerTower>().lineRenderer.gameObject);
					tower.GetComponent<PlayerTower>().canSpawn = false;
				}
				if (tower.GetComponent<EnemyTower>() != null && tower.GetComponent<EnemyTower>().lineRenderer != null)
				{
					Destroy(tower.GetComponent<EnemyTower>().lineRenderer.gameObject);
					tower.GetComponent<EnemyTower>().canSpawn = false;
					tower.GetComponent<EnemyTower>().isFound = false;
					tower.GetComponent<EnemyTower>().allTowers.Remove(transform);
				}
			}
		}
		if (lineRenderer != null)
		{
			Destroy(lineRenderer.gameObject);
		}
	}
}
