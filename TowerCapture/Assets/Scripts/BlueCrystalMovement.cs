using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueCrystalMovement : MonoBehaviour
{
	private void Start()
	{
		transform.DORotate(new Vector3(transform.rotation.x, 360, transform.rotation.z), 1.5f, RotateMode.WorldAxisAdd)
			.SetLoops(-1, LoopType.Restart)
			.SetEase(Ease.Linear);
		transform.DOLocalMoveY(0.55f, 1).SetLoops(-1, LoopType.Yoyo);
	}

	private void OnDestroy()
	{
		DOTween.Clear();
	}
}
