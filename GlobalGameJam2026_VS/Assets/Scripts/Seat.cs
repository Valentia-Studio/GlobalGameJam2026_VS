using UnityEngine;

public class Seat : MonoBehaviour
{
    public Seat leftNeighbor;
    public Seat rightNeighbor;
    public Seat frontNeighbor;
    public Seat frontLeftNeighbor;
    public Seat frontRightNeighbor;
    public Seat backNeighbor;
    public Seat backLeftNeighbor;
    public Seat backRightNeighbor;

    public NPC occupant;

    public bool isBlocked;
    public NPC blockedBy;

    public Material blockedSeatMaterial;
    public Material defaultSeatMaterial;

    void Awake()
    {
        var rend = GetComponent<MeshRenderer>();
        if (rend == null)
            rend = GetComponentInChildren<MeshRenderer>();
        if (rend != null && defaultSeatMaterial == null)
        {
            defaultSeatMaterial = rend.sharedMaterial;
        }
        ApplySeatMaterial();
    }

    public void SetOccupant(NPC npc)
    {
        var previous = occupant;
        if (previous != null && previous != npc)
        {
            previous.CurrentSeat = null;
        }

        if (previous != null && (previous.species == Species.Elefante || (previous.species == Species.Camaleon && previous.actsAsElefante)))
        {
            if (leftNeighbor != null) leftNeighbor.ClearBlockIfBy(previous);
            if (rightNeighbor != null) rightNeighbor.ClearBlockIfBy(previous);
        }

        occupant = npc;

        if (npc != null)
        {
            npc.CurrentSeat = this;

            if (npc.species == Species.Camaleon)
            {
                var effective = npc.GetChameleonEffectiveSpeciesForSeat(this);
                npc.actsAsElefante = (effective == Species.Elefante);
            }
        }

        if (npc != null && (npc.species == Species.Elefante || (npc.species == Species.Camaleon && npc.actsAsElefante)))
        {
            if (leftNeighbor != null) leftNeighbor.SetBlockedBy(npc);
            if (rightNeighbor != null) rightNeighbor.SetBlockedBy(npc);
        }

        ApplySeatMaterial();
    }

    public void ClearOccupant()
    {
        if (occupant != null)
        {
            if (occupant.species == Species.Elefante || (occupant.species == Species.Camaleon && occupant.actsAsElefante))
            {
                if (leftNeighbor != null) leftNeighbor.ClearBlockIfBy(occupant);
                if (rightNeighbor != null) rightNeighbor.ClearBlockIfBy(occupant);
            }

            occupant.CurrentSeat = null;
            occupant = null;
        }
        ApplySeatMaterial();
    }

    public void SetBlockedBy(NPC npc)
    {
        isBlocked = true;
        blockedBy = npc;
        ApplySeatMaterial();
    }

    public void ClearBlockIfBy(NPC npc)
    {
        if (isBlocked && blockedBy == npc)
        {
            isBlocked = false;
            blockedBy = null;
            ApplySeatMaterial();
        }
    }

    public bool IsSeatFreeFor(NPC npc)
    {
        if (occupant != null && occupant != npc)
            return false;
        if (isBlocked && blockedBy != npc)
            return false;
        return true;
    }

    private void ApplySeatMaterial()
    {
        var rend = GetComponent<MeshRenderer>();
        if (rend == null)
            rend = GetComponentInChildren<MeshRenderer>();
        if (rend == null) return;

        if (isBlocked && blockedSeatMaterial != null)
        {
            rend.material = blockedSeatMaterial;
        }
        else if (defaultSeatMaterial != null)
        {
            rend.material = defaultSeatMaterial;
        }
    }
}
