using UnityEngine;

public class NPCMovement : MonoBehaviour
{

    [SerializeField] GameObject GOAL;


    bool moving = false;

    Vector3 movingVector;
    [SerializeField] float speed;

    private void FixedUpdate()
    {
        if (moving) Move();
        GoTo(GOAL);

        if (GOAL != null && Vector3.Distance(gameObject.transform.position, GOAL.transform.position)< .5f)
        {


            gameObject.transform.LookAt(new Vector3(Camera.main.transform.position.x, gameObject.transform.position.y, Camera.main.transform.position.z));
            transform.GetChild(0).GetComponent<Animator>().SetBool("Walk", false);
            gameObject.layer = LayerMask.NameToLayer("Grabbable");
            moving = false;
            Destroy(this);
        }
    }

    private void Move()
    {
        transform.position += movingVector * Time.deltaTime * speed;
    }

    public void GoTo(GameObject place)
    {
        GOAL = place;
        transform.GetChild(0).GetComponent<Animator>().SetBool("Walk", true);
        transform.LookAt(place.transform.position);
        movingVector = Vector3.Normalize(place.transform.position-gameObject.transform.position);
        moving = true;
    }

}
