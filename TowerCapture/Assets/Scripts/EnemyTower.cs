using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class EnemyTower : MonoBehaviour
{
	[SerializeField] TextMeshPro soldierTowerCountText;
	[HideInInspector] public  int soldierTowerCount;
	[HideInInspector] float timeToSpawnSoldiers;//2
	[HideInInspector] float timeToAddSoldiers;//5
	[SerializeField] GameObject soldierPrefab;
	[SerializeField] LineRenderer linePrefab;

	public LineRenderer lineRenderer;

	[HideInInspector] public List<GameObject> attackTowers = new List<GameObject>();
	[HideInInspector] public List <Transform> allTowers = new List<Transform>();

	AudioSource towerDestroySound;
	TowerValuesManager towerValuesManagerScript;
	GameObject attackedTower;
	GameObject soldier;
	public bool canSpawn = false;
	bool isReadyToSpawn = true;
	bool isReadyToAdd = true;
	[HideInInspector] public bool isFound = false;
	[HideInInspector] public bool canPlay = false;
	[HideInInspector] public bool canSound = true;
	private void Start()
	{
		towerValuesManagerScript = GameObject.FindObjectOfType<TowerValuesManager>();
		towerValuesManagerScript.enemyTowers.Add(this);

		towerDestroySound = GameObject.FindGameObjectWithTag("TowerDestroy").GetComponent<AudioSource>();

		soldierTowerCount = towerValuesManagerScript.enemyHP;
		timeToSpawnSoldiers = towerValuesManagerScript.enemySpawnSpeed;
		timeToAddSoldiers = towerValuesManagerScript.enemiesAddTime;
	}

	private void Update()
	{
		if (canPlay)
		{
			soldierTowerCountText.text = soldierTowerCount.ToString();
			AddSoldiers();
			SearchTowerToAttack();
			SpawnArmy();
		}
	}

	void SpawnArmy()
	{
		if (canSpawn && soldierTowerCount > 0 && isReadyToSpawn)
		{
			StartCoroutine(SpawnSlodiers());
		}
	}

	IEnumerator SpawnSlodiers()
	{
		isReadyToSpawn = false;
		yield return new WaitForSeconds(timeToSpawnSoldiers);
		if (canSpawn && soldierTowerCount > 0)
		{
			soldier = Instantiate(soldierPrefab, transform.position, Quaternion.identity);
			soldier.GetComponent<EnemySoldierMovement>().enemyTowerScript = this;
			soldier.GetComponent<EnemySoldierMovement>().enemyTowerPos = attackedTower.transform;
			soldierTowerCount--;
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
		soldierTowerCount++;
		isReadyToAdd = true;
	}

	void SearchTowerToAttack()
	{
		if (!isFound && allTowers.Count(item => item.GetComponent<PlayerTower>() != null) > 0)
		{
			Vector3 firstObject = allTowers[0].position;
			attackedTower = allTowers[0].gameObject;
			foreach (Transform tower in allTowers)
			{
				if (Vector3.Distance(tower.transform.position, transform.position) < Vector3.Distance(firstObject, transform.position))
				{
					firstObject = tower.position;
					attackedTower = tower.gameObject;
				}
			}
			SpawnLine(firstObject);

			if (attackedTower.GetComponent<PlayerTower>() != null)
			{
				attackedTower.GetComponent<PlayerTower>().attackTowers.Add(gameObject);
			}
			else if(attackedTower.GetComponent<NeutralTower>() != null)
			{
				attackedTower.GetComponent<NeutralTower>().attackTowers.Add(gameObject);
			}
			isFound = true;
		}
	}

	void SpawnLine(Vector3 toObject)
	{
		if (lineRenderer != null)
		{
			Destroy(lineRenderer.gameObject);
		}

		lineRenderer = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
		lineRenderer.positionCount = 2;

		lineRenderer.SetPosition(0, transform.position);
		lineRenderer.SetPosition(1, toObject);
		canSpawn = true;
	}

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
					Destroy(tower.GetComponent<PlayerTower>().lineRenderer.gameObject);
				tower.GetComponent<PlayerTower>().canSpawn = false;
			}
		}
		if (attackedTower != null && attackedTower.GetComponent<NeutralTower>() != null) {
			attackedTower.GetComponent<NeutralTower>().attackTowers.Remove(gameObject);
		}
		if (lineRenderer != null)
		{
			Destroy(lineRenderer.gameObject);
		}
	}
}
