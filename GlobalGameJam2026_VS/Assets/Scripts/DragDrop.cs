using UnityEngine;

public class DragDrop : MonoBehaviour
{
    [SerializeField] LayerMask seatLayer, interactableLayer, allLayers;
    [SerializeField] GameObject shadow;

    bool holding = false;
    GameObject holdingGameObject;
    Vector3 draggedPosition;
    Quaternion draggedRotation;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, interactableLayer)&& Input.GetMouseButtonDown(0))
        {
            StartDrag(hit.collider.gameObject);
        }

        if (holding)
        {
            Drag();

            if (Input.GetMouseButtonUp(0))
            {
                Drop();
            }
        }
    }

    private void StartDrag(GameObject gameObject)
    {

        holdingGameObject = gameObject;
        holdingGameObject.GetComponent<Rigidbody>().isKinematic = true;

        SoundManager.instance.PlayRepetitive(SoundList.instance.grabPeople);

        holdingGameObject.transform.GetChild(0).GetComponent<Animator>().SetBool("Grab", true);
        holdingGameObject.transform.GetChild(0).GetComponent<Animator>().SetBool("Sit", false);
        
        holdingGameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        Cursor.visible = false;
        shadow.SetActive(true);

        draggedPosition = holdingGameObject.transform.position;
        draggedRotation = holdingGameObject.transform.rotation;

        holding = true;
    }

    private void Drag()
    {
        Debug.DrawRay(holdingGameObject.transform.position, -Vector3.up, Color.yellow);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);      
        RaycastHit hit;

        holdingGameObject.transform.Rotate(new Vector3(0, 1, 0));


        if (Physics.Raycast(ray, out hit, 100, allLayers))
        {
            holdingGameObject.transform.position = hit.point + Vector3.up * 5;
            shadow.transform.position = hit.point + Vector3.up * .1f;
        }
    }

    void Drop()
    {

        SoundManager.instance.StopRepetitive();
        Cursor.visible = true;
        shadow.SetActive(false);

        Ray ray = new(holdingGameObject.transform.position, -Vector3.up);
        RaycastHit hit;
        NPC npc = holdingGameObject != null ? holdingGameObject.GetComponent<NPC>() : null;

        if (Physics.Raycast(ray, out hit, 100, seatLayer))
        {

            Debug.Log(hit.transform.gameObject.name);

            var seat = hit.transform.GetComponent<Seat>();
            if (seat != null && npc != null)
            {
                if (!seat.IsSeatFreeFor(npc))
                {
                    holdingGameObject.transform.position = draggedPosition;
                    holdingGameObject.transform.rotation = draggedRotation;
                }
                else
                {
                    holdingGameObject.transform.position = hit.transform.position;
                    holdingGameObject.transform.rotation = hit.transform.rotation;

                    var previousSeat = npc.CurrentSeat;
                    if (previousSeat != null && previousSeat.occupant == npc)
                    {
                        previousSeat.ClearOccupant();
                        ReevaluateAround(previousSeat);
                    }

                    seat.SetOccupant(npc);
                    
                    if (!seat.transform.name.Contains("Stay"))npc.transform.GetChild(0).GetComponent<Animator>().SetBool("Sit", true);

                    if (npc.species == Species.Elefante || (npc.species == Species.Camaleon && npc.actsAsElefante))
                    {
                        TryReassignNeighbor(seat.leftNeighbor);
                        TryReassignNeighbor(seat.rightNeighbor);
                    }

                    npc.EvaluateNeighbors();
                    ReevaluateAround(seat);
                }
            }
            else
            {
                holdingGameObject.transform.position = draggedPosition;
                holdingGameObject.transform.rotation = draggedRotation;
            }
        }
        else
        {
            holdingGameObject.transform.position = draggedPosition;
            holdingGameObject.transform.rotation = draggedRotation;
        }
        holdingGameObject.layer = LayerMask.NameToLayer("Grabbable");
        holdingGameObject = null;
        holding = false;

        npc.transform.GetChild(0).GetComponent<Animator>().SetBool("Grab", false);

    }

    private void TryReassignNeighbor(Seat neighbor)
    {
        if (neighbor == null || neighbor.occupant == null) return;

        var other = neighbor.occupant;
        if (other.assignedSeat == null) return;
        if (!other.assignedSeat.IsSeatFreeFor(other)) return;

        var assigned = other.assignedSeat;
        other.transform.position = assigned.transform.position;
        other.transform.rotation = assigned.transform.rotation;

        neighbor.ClearOccupant();
        assigned.SetOccupant(other);

        other.EvaluateNeighbors();
        ReevaluateAround(assigned);
        ReevaluateAround(neighbor);
    }

    private void ReevaluateAround(Seat seat)
    {
        if (seat.leftNeighbor?.occupant != null)
            seat.leftNeighbor.occupant.EvaluateNeighbors();
        if (seat.rightNeighbor?.occupant != null)
            seat.rightNeighbor.occupant.EvaluateNeighbors();
        if (seat.frontNeighbor?.occupant != null)
            seat.frontNeighbor.occupant.EvaluateNeighbors();
        if (seat.frontLeftNeighbor?.occupant != null)
            seat.frontLeftNeighbor.occupant.EvaluateNeighbors();
        if (seat.frontRightNeighbor?.occupant != null)
            seat.frontRightNeighbor.occupant.EvaluateNeighbors();
        if (seat.backNeighbor?.occupant != null)
            seat.backNeighbor.occupant.EvaluateNeighbors();
        if (seat.backLeftNeighbor?.occupant != null)
            seat.backLeftNeighbor.occupant.EvaluateNeighbors();
        if (seat.backRightNeighbor?.occupant != null)
            seat.backRightNeighbor.occupant.EvaluateNeighbors();
    }
}
