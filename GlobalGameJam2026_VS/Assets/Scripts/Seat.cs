using UnityEngine;

public class Seat : MonoBehaviour
{
    public Seat leftNeighbor;
    public Seat rightNeighbor;

    public NPC occupant;

    public void SetOccupant(NPC npc)
    {
        if (occupant != null && occupant != npc)
        {
            occupant.CurrentSeat = null;
        }

        occupant = npc;

        if (npc != null)
        {
            npc.CurrentSeat = this;
        }
    }

    public void ClearOccupant()
    {
        if (occupant != null)
        {
            occupant.CurrentSeat = null;
            occupant = null;
        }
    }
}
