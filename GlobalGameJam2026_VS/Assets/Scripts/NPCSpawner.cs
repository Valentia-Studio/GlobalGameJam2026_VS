using UnityEngine;
using System.Collections.Generic;

public class NPCSpawner : MonoBehaviour
{

    public static NPCSpawner instance;

    public List<GameObject> NPCs = new List<GameObject>();
    public List<GameObject> spawnPoints = new List<GameObject>();
    public List<GameObject> queueGoals = new List<GameObject>();

    public int currentQueueGoal = 0;  //IMPORTANT TO RESET EACH ROUND

    public enum Mask {Chameleon, Elephant, Mouse, Owl, Tiger, Zebra }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        Spawn(Mask.Mouse);
        Spawn(Mask.Zebra);
        Spawn(Mask.Elephant);
        Spawn(Mask.Chameleon);
        Spawn(Mask.Chameleon);
    }

    /// <summary>
    /// Use for spawn NPCs and choose what mask it has. It has a limit dependant on available spaces in queue.
    /// </summary>
    /// <param name="mask"></param>
    public void Spawn(Mask mask)
    {

        if (currentQueueGoal >= queueGoals.Count) return;

        foreach (GameObject NPC in NPCs)
        {

            if (NPC.name == mask.ToString())
            {
                int chosen = Random.Range(0, spawnPoints.Count);

                GameObject spawnedNPC = Instantiate(NPC, spawnPoints[chosen].transform.position, spawnPoints[chosen].transform.rotation);

                if (mask != Mask.Chameleon) spawnedNPC.transform.GetChild(0).GetComponent<Animator>().SetBool(mask.ToString(), true);
                
                spawnedNPC.GetComponent<NPCMovement>().GoTo(queueGoals[currentQueueGoal]);
                currentQueueGoal++;

                break;
            }
        }
    }

}
