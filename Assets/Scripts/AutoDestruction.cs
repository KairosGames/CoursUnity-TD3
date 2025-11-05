using System.Collections;
using UnityEngine;

public class AutoDestruction : MonoBehaviour
{
    public void AutoDestroy(float timer)
    {
        StartCoroutine(LauchAutoDestruction(timer));
    }

    IEnumerator LauchAutoDestruction(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
