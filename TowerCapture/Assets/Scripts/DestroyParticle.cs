using System.Collections;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    float waitTime = 0.5f;
    void Start()
    {
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}
