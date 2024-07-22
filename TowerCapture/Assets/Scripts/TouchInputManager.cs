using System.Collections.Generic;
using UnityEngine;

public class TouchInputManager : MonoBehaviour
{
	[SerializeField] GameObject isClickedPoint;
	[SerializeField] float addToYPosPoint; //0.5f
	[SerializeField] LineRenderer linePrefab;
	[SerializeField] GameObject soldierPrefab;

	[SerializeField] public List<PlayerTower> playerTowers = new List<PlayerTower>();

	[HideInInspector] public Transform enemyTowerPos;
	int xPointRotation = 90;
	[HideInInspector] public Transform clickedObject;
	[HideInInspector] public GameObject point;
	bool isClicked = false;
	[HideInInspector] public bool isGameStart = false;

	void Update()
	{
		if (isGameStart)
		{
			// check for touch input
			if (Input.touchCount > 0)
			{
				if (Input.GetTouch(0).phase == TouchPhase.Began)
				{
					handleClick(Input.GetTouch(0).position);
				}
			}
			// check for left mouse button clicked
#if UNITY_EDITOR
			if (Input.GetButtonDown("Fire1"))
			{
				handleClick(Input.mousePosition);
			}


			if (Input.GetButtonDown("Fire2"))
			{
				PlayerPrefs.DeleteAll();
				PlayerPrefs.DeleteKey("hasPlayed");
				Debug.Log("Delete");
			}
#endif
		}
	}

	void handleClick(Vector3 screenClickPosition)
	{
		Ray ray = Camera.main.ScreenPointToRay(screenClickPosition);
		RaycastHit rayHit;
		if (Physics.Raycast(ray.origin, ray.direction, out rayHit))
		{
			ManageClicks(rayHit);
		}
	}

	void ManageClicks(RaycastHit rayHit)
	{
		if (IsClicked(rayHit))
		{
			SpawnTag(rayHit);
		}
		else if (IsUnclicked(rayHit))
		{
			Unclicked();
		}
		else if (isClicked)
		{
			SpawnLine(rayHit);
		}
	}

	bool IsClicked(RaycastHit rayHit)
	{
		return rayHit.transform.tag == "Player" && !isClicked;
	}

	void SpawnTag(RaycastHit rayHit)
	{
		clickedObject = rayHit.transform;
		Quaternion offset = Quaternion.Euler(xPointRotation, 0f, 0f);
		point = Instantiate(isClickedPoint, new Vector3(rayHit.transform.position.x, rayHit.transform.position.y + addToYPosPoint,
			rayHit.transform.position.z), rayHit.transform.rotation * Quaternion.Inverse(offset));
		isClicked = true;
	}

	bool IsUnclicked(RaycastHit rayHit)
	{
		return clickedObject != null && rayHit.transform == clickedObject && isClicked;
	}

	void Unclicked()
	{
		clickedObject.GetComponent<PlayerTower>().canSpawn = false;
		Destroy(point);
		isClicked = false;

		foreach(PlayerTower tower in playerTowers)
		{
			if (tower.lineRenderer != null && clickedObject.position == tower.transform.position)
			{
				Destroy(tower.lineRenderer.gameObject);
				tower.isLineAdded = false;
			}
		}
	}

	void SpawnLine(RaycastHit rayHit)
	{
		enemyTowerPos = rayHit.transform;

		foreach (PlayerTower tower in playerTowers)
		{
			if (clickedObject != null && clickedObject.position == tower.transform.position)
			{
				tower.saveEnemyTowerPos = enemyTowerPos;
				tower.SpawnLine(rayHit);
			}
		}
		Destroy(point);
		SetValuesToDefault();
	}

	void SetValuesToDefault()
	{
		isClicked = false;
	}
}

