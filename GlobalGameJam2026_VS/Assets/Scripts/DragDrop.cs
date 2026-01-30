using UnityEngine;

public class DragDrop : MonoBehaviour
{
    [SerializeField] LayerMask seatLayer, interactableLayer, allLayers;

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
        holdingGameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        draggedPosition = holdingGameObject.transform.position;
        draggedRotation = holdingGameObject.transform.rotation;

        holding = true;
    }

    private void Drag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, allLayers))
        {
            holdingGameObject.transform.position = hit.point + Vector3.up * 2;
            holdingGameObject.transform.LookAt(Camera.main.transform);
        }
    }

    void Drop()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, seatLayer))
        {
            holdingGameObject.transform.position = hit.transform.position;
            holdingGameObject.transform.rotation = hit.transform.rotation;
        }
        else
        {
            holdingGameObject.transform.position = draggedPosition;
            holdingGameObject.transform.rotation = draggedRotation;
        }
        holdingGameObject.layer = LayerMask.NameToLayer("Grabbable");
        holdingGameObject = null;
        holding = false;
    }
}
