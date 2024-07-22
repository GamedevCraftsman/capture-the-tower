using UnityEngine;

public class PlayerSoldierMovement : MonoBehaviour
{
	[SerializeField] GameObject playerTowerPrefab;
	public PlayerTower playerTowerScript;
	[SerializeField] float speed;

	AudioSource soldierDestroy;
	WinManager winManagerScript;
	ParticleManager particleManagerScript;
	TouchInputManager touchInputManagerScript;
	EnemyAmountManager enemyAmountManagerScript;
	public GameObject nextTower;
	public Transform enemyTowerPos;
	private void Start()
	{
		touchInputManagerScript = GameObject.FindObjectOfType<TouchInputManager>();
		enemyAmountManagerScript = GameObject.FindObjectOfType<EnemyAmountManager>();
		particleManagerScript = GameObject.FindObjectOfType<ParticleManager>();
		winManagerScript = GameObject.FindObjectOfType<WinManager>();
		soldierDestroy = GameObject.FindGameObjectWithTag("SoldierDestroy").GetComponent<AudioSource>();
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
		if (other.CompareTag("Enemy") && enemyTowerPos != null && other.transform.position == enemyTowerPos.position)
		{
			if (other.GetComponent<EnemyTower>().soldierTowerCount <= 0)
			{
				SpawnPlayerTower(other);
			}
			else
			{
				other.GetComponent<EnemyTower>().soldierTowerCount--;
			}
			DestroySoldier();
		}
		else if (other.CompareTag("Neutral") && other.transform.position == enemyTowerPos.position)
		{
			if (other.GetComponent<NeutralTower>().hpTowerCount <= 0)
			{
				SpawnPlayerTower(other);
			}
			else
			{
				other.GetComponent<NeutralTower>().hpTowerCount--;
			}
			DestroySoldier();
		}
		else if (other.CompareTag("Player") && enemyTowerPos != null && other.transform.position == enemyTowerPos.position)
		{
			other.GetComponent<PlayerTower>().countOfSoldiers++;
			DestroySoldier();
		}
	}

	void SpawnPlayerTower(Collider other)
	{
		particleManagerScript.ParticlesManager(other.transform);
		nextTower = Instantiate(playerTowerPrefab, other.transform.position, Quaternion.identity);
		winManagerScript.countOfSpawnedSoldiers += 20;
		nextTower.GetComponent<PlayerTower>().canPlay = true;
		if (other.GetComponent<EnemyTower>() != null)
		{
			enemyAmountManagerScript.amountOfEnemyTowers.Remove(other.GetComponent<EnemyTower>());
			enemyAmountManagerScript.allTowers.Add(nextTower.transform);
		}
		else
		{
			enemyAmountManagerScript.allTowers.Remove(other.transform);
			enemyAmountManagerScript.allTowers.Add(nextTower.transform);
		}
		touchInputManagerScript.playerTowers.Add(nextTower.GetComponent<PlayerTower>());
		Destroy(other.gameObject);
	}

	void DestroySoldier()
	{
		soldierDestroy.Play();
		Destroy(gameObject);
	}
}
