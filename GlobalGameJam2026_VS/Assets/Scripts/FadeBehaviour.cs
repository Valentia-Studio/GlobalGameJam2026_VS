using UnityEngine;

public class FadeBehaviour : MonoBehaviour
{

    [SerializeField] Material fadeMat;
    [SerializeField] float speed;

    private void Start()
    {
        fadeMat.SetFloat("_CanSeeThrough", 0);
        fadeMat.SetFloat("_SeeThroughDistance", 0);
    }

    void Update()
    {
        if (fadeMat.GetFloat("_CanSeeThrough") == 1 && fadeMat.GetFloat("_SeeThroughDistance") < 3) 
            fadeMat.SetFloat("_SeeThroughDistance", fadeMat.GetFloat("_SeeThroughDistance") +Time.deltaTime * speed);

        if (fadeMat.GetFloat("_CanSeeThrough") == 0 && fadeMat.GetFloat("_SeeThroughDistance") > 0)
            fadeMat.SetFloat("_SeeThroughDistance", fadeMat.GetFloat("_SeeThroughDistance") - Time.deltaTime * speed);
    }

    public void InstaStopFade()
    {
        fadeMat.SetFloat("_SeeThroughDistance", 0);
    }

}
