using UnityEngine;
using System.Collections;

public class FadeBehaviour : MonoBehaviour
{

    [SerializeField] Material fadeMat;
    [SerializeField] float speed;

    private void Start()
    {
        ResetParameters();
    }

    /// <summary>
    /// Resets see-through parameters (ONLY use when needed)
    /// </summary>
    public void ResetParameters()
    {
        fadeMat.SetFloat("_CanSeeThrough", 0);
        fadeMat.SetFloat("_SeeThroughDistance", 1);
    }

    /// <summary>
    /// Starts see-through shader effect
    /// </summary>
    public void SeeThrough()
    {
        StartCoroutine(SeeThroughTimer());
    }

    /// <summary>
    /// Stops see-through shader effect
    /// </summary>
    public void StopSeeThrough()
    {
        StartCoroutine(StopSeeThroughTimer());
    }

    IEnumerator SeeThroughTimer()
    {
        while (fadeMat.GetFloat("_SeeThroughDistance") > 0)
        {
            fadeMat.SetFloat("_SeeThroughDistance", fadeMat.GetFloat("_SeeThroughDistance") - Time.deltaTime * speed);
            yield return new WaitForSeconds(.1f / speed);
        }

        StopCoroutine(SeeThroughTimer());
    }
    IEnumerator StopSeeThroughTimer()
    {
        while (fadeMat.GetFloat("_SeeThroughDistance") < 1)
        {
            fadeMat.SetFloat("_SeeThroughDistance", fadeMat.GetFloat("_SeeThroughDistance") + Time.deltaTime * speed);
            yield return new WaitForSeconds(.1f / speed);
        }
        StopCoroutine(StopSeeThroughTimer());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) SeeThrough();
        if (Input.GetKeyDown(KeyCode.E)) StopSeeThrough();
    }
}
