using UnityEngine;
using System.Collections;

public class FadeBehaviour : MonoBehaviour
{

    public static FadeBehaviour instance;

    [SerializeField] Material[] fadeMat;
    [SerializeField] float speed;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        ResetParameters();
    }

    /// <summary>
    /// Resets see-through parameters (ONLY use when needed)
    /// </summary>
    public void ResetParameters()
    {
        foreach (Material mat in fadeMat)
        {
            mat.SetFloat("_CanSeeThrough", 0);
            mat.SetFloat("_SeeThroughDistance", 1);
        }

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
        StopCoroutine(StopSeeThroughTimer());

        foreach (Material mat in fadeMat)
        {
            while (mat.GetFloat("_SeeThroughDistance") > 0)
            {
                mat.SetFloat("_SeeThroughDistance", mat.GetFloat("_SeeThroughDistance") - Time.deltaTime * speed);
                yield return new WaitForSeconds(.1f / speed);
            }

        }

        StopCoroutine(SeeThroughTimer());
    }
    IEnumerator StopSeeThroughTimer()
    {
        StopCoroutine(SeeThroughTimer());


        foreach (Material mat in fadeMat)
        {
            while (mat.GetFloat("_SeeThroughDistance") < 1)
            {
                mat.SetFloat("_SeeThroughDistance", mat.GetFloat("_SeeThroughDistance") + Time.deltaTime * speed);
                yield return new WaitForSeconds(.1f / speed);
            }
        }

        StopCoroutine(StopSeeThroughTimer());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) SeeThrough();
        if (Input.GetKeyDown(KeyCode.E)) StopSeeThrough();
    }
}
