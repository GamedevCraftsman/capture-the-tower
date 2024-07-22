using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NeutralTower : MonoBehaviour
{
	[SerializeField] TextMeshPro hpTowerCountText;
	[HideInInspector] public int hpTowerCount;

	[SerializeField] public List<GameObject> attackTowers = new List<GameObject>();

	[HideInInspector] public bool canSound = true;
	AudioSource towerDestroySound;
	TowerValuesManager towerValuesManagerScript;
	private void Start()
	{
		towerValuesManagerScript = GameObject.FindObjectOfType<TowerValuesManager>();
		towerValuesManagerScript.neutralTowers.Add(this);
		towerDestroySound = GameObject.FindGameObjectWithTag("TowerDestroy").GetComponent<AudioSource>();
		hpTowerCount = towerValuesManagerScript.neutralHP;
	}

	private void Update()
	{
		hpTowerCountText.text = hpTowerCount.ToString();
	}

	public void OnDestroy()
	{
		if (canSound)
		{
			Debug.Log("Sound");
			towerDestroySound.Play();
		}
		else
		{
			Debug.Log("Not sound");
		}
		foreach (GameObject tower in attackTowers)
		{
			if (tower != null)
			{
				if (tower.GetComponent<EnemyTower>() != null && tower.GetComponent<EnemyTower>().lineRenderer != null)
				{
					Destroy(tower.GetComponent<EnemyTower>().lineRenderer.gameObject);
					tower.GetComponent<EnemyTower>().isFound = false;
					tower.GetComponent<EnemyTower>().canSpawn = false;
					tower.GetComponent<EnemyTower>().allTowers.Remove(transform);
				}
				else if (tower.GetComponent<PlayerTower>() != null && tower.GetComponent<PlayerTower>().lineRenderer != null)
				{
					Destroy(tower.GetComponent<PlayerTower>().lineRenderer.gameObject);
					tower.GetComponent<PlayerTower>().canSpawn = false;
				}
			}
		}
	}
}
