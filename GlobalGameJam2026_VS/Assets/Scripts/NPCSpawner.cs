using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class NPCSpawner : MonoBehaviour
{
    public static NPCSpawner instance;

    public List<GameObject> NPCs = new List<GameObject>();
    public List<GameObject> spawnPoints = new List<GameObject>();
    public List<GameObject> queueGoals = new List<GameObject>();

    public int currentQueueGoal = 0;
    private bool hasStartedSpawning = false;

    public enum Mask { Chameleon, Elephant, Mouse, Owl, Tiger, Zebra }
    Mask mask1 = 0;

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
        // Eliminar el StartCoroutine(SpawnAll()) de aquí
        // Ahora se llamará manualmente cuando el tren llegue
    }

    public void StartSpawning()
    {
        if (!hasStartedSpawning)
        {
            hasStartedSpawning = true;
            StartCoroutine(SpawnAll());
        }
    }

    IEnumerator SpawnAll()
    {
        Spawn(Mask.Elephant);
        yield return new WaitForSeconds(3);
        Spawn(Mask.Mouse);
        yield return new WaitForSeconds(3);
        Spawn(Mask.Owl);
        yield return new WaitForSeconds(3);
        Spawn(Mask.Tiger);
        yield return new WaitForSeconds(3);
        Spawn(Mask.Zebra);
        yield return new WaitForSeconds(3);
        Spawn(Mask.Chameleon);
        yield return new WaitForSeconds(3);
    }

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
                else spawnedNPC.transform.GetChild(0).GetComponent<Animator>().SetBool("Zebra", true);

                spawnedNPC.GetComponent<NPCMovement>().GoTo(queueGoals[currentQueueGoal]);
                currentQueueGoal++;

                break;
            }
        }
    }

    public void ResetSpawner()
    {
        StopAllCoroutines();
        hasStartedSpawning = false;
        currentQueueGoal = 0;

        // Destruir todos los NPCs existentes
        NPCMovement[] allNPCs = FindObjectsByType<NPCMovement>(FindObjectsSortMode.None);
        foreach (NPCMovement npc in allNPCs)
        {
            Destroy(npc.gameObject);
        }
    }
}