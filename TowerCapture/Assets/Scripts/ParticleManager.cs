using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] GameObject destroyParticles;

	public void ParticlesManager(Transform particlePosition)
	{
		GameObject particles = Instantiate(destroyParticles, particlePosition.transform.position, Quaternion.identity);
	}
}
