using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{

    NavMeshAgent agent;

    [SerializeField] GameObject GOAL;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (agent.remainingDistance < .5f)
        {
            agent.isStopped = true;

            gameObject.transform.LookAt(new Vector3(Camera.main.transform.position.x, gameObject.transform.position.y, Camera.main.transform.position.z));
            transform.GetChild(0).GetComponent<Animator>().SetBool("Walk", false);
            GetComponent<Collider>().enabled = true;
            gameObject.layer = LayerMask.NameToLayer("Grabbable");
            Destroy(GetComponent<NavMeshAgent>());
            Destroy(this);
        }

    }

    public void GoTo(GameObject place)
    {
        GOAL = place;
        transform.GetChild(0).GetComponent<Animator>().SetBool("Walk", true);
        agent.SetDestination(place.transform.position);
        transform.LookAt(place.transform.position);
    }

}
