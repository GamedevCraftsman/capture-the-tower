using DG.Tweening;
using UnityEngine;

public class EnemySoldierMovement : MonoBehaviour
{
	EnemyAmountManager enemyAmountManagerScript;
	TouchInputManager touchInputManagerScript;
	ParticleManager particleManagerScript;
	[SerializeField] GameObject enemyTowerPrefab;
	public EnemyTower enemyTowerScript;
	[SerializeField] float speed;

	public GameObject nextTower;
	public Transform enemyTowerPos;
	private void Start()
	{
		enemyAmountManagerScript = GameObject.FindObjectOfType < EnemyAmountManager>();
		touchInputManagerScript = GameObject.FindObjectOfType<TouchInputManager>();
		particleManagerScript = GameObject.FindObjectOfType<ParticleManager>();
	}

	private void Update()
	{
		if (enemyTowerPos == null)
		{
			Destroy(gameObject);
		}
		else
		{
			Vector3 relativePos = enemyTowerPos.position - transform.position;
			Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
			transform.rotation = rotation;
			transform.position = Vector3.MoveTowards(transform.position, enemyTowerPos.position, speed * Time.deltaTime);
		}
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Neutral") && enemyTowerPos != null && other.transform.position == enemyTowerPos.position)
		{
			if (other.GetComponent<NeutralTower>().hpTowerCount <= 0)
			{
				SpawnEnemyTower(other);
			}
			else
			{
				other.GetComponent<NeutralTower>().hpTowerCount--;
			}
			Destroy(gameObject);
		}
		else if (other.CompareTag("Player") && enemyTowerPos != null && other.transform.position == enemyTowerPos.position)
		{
			if (other.GetComponent<PlayerTower>().countOfSoldiers <= 0)
			{
				if (!other.GetComponent<PlayerTower>().isDestroyed)
				{
					SpawnEnemyTower(other);
				}
				other.GetComponent<PlayerTower>().isDestroyed = true;
			}
			else
			{
				other.GetComponent<PlayerTower>().countOfSoldiers--;
			}
			Destroy(gameObject);
		}
	}

	void SpawnEnemyTower(Collider other)
	{
		particleManagerScript.ParticlesManager(other.transform);
		enemyTowerScript.isFound = false;
		if(other.gameObject.GetComponent<PlayerTower>() != null)
		{
			touchInputManagerScript.playerTowers.Remove(other.gameObject.GetComponent<PlayerTower>());
			if (touchInputManagerScript.point != null)
			{
				GameObject savePoint = touchInputManagerScript.point;
				if (new Vector3(other.transform.position.x, 0, other.transform.position.z) == new Vector3(savePoint.transform.position.x, 0, savePoint.transform.position.z))
				{
					Destroy(touchInputManagerScript.point);
				}
			}
		}
		nextTower = Instantiate(enemyTowerPrefab, other.transform.position, Quaternion.Euler(-90, 0, 0));
		nextTower.GetComponent<EnemyTower>().canPlay = true;
		enemyAmountManagerScript.allTowers.Remove(other.transform);
		enemyAmountManagerScript.amountOfEnemyTowers.Add(nextTower.GetComponent<EnemyTower>());
		Destroy(other.gameObject);
	}
}
