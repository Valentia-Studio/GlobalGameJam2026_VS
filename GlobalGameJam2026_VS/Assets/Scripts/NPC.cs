using UnityEngine;

public enum Species
{
    Elefante,
    Tigre,
    Raton,
    Cebra,
    Buho
}

public class NPC : MonoBehaviour
{
    public Species species;
    public Material canSitMaterial;
    public Material cannotSitMaterial;

    public Seat CurrentSeat;

    void Start()
    {
    }

    void Update()
    {
    }

    public void EvaluateNeighbors()
    {
        bool canLeft = EvaluateWithSeat(CurrentSeat != null ? CurrentSeat.leftNeighbor : null);
        bool canRight = EvaluateWithSeat(CurrentSeat != null ? CurrentSeat.rightNeighbor : null);

        bool canSit = canLeft && canRight;
        var rend = GetComponent<MeshRenderer>();
        if (rend != null)
        {
            rend.material = canSit ? canSitMaterial : cannotSitMaterial;
        }
    }

    private bool EvaluateWithSeat(Seat seat)
    {
        if (seat == null || seat.occupant == null)
            return true;
        return CanSitNextTo(seat.occupant.species);
    }

    private bool CanSitNextTo(Species other)
    {
        switch (species)
        {
            case Species.Elefante:
                if (other == Species.Raton) return false;
                return other == Species.Tigre || other == Species.Cebra;
            case Species.Tigre:
                if (other == Species.Raton) return false;
                return other == Species.Elefante || other == Species.Cebra;
            case Species.Raton:
                if (other == Species.Tigre || other == Species.Elefante) return false;
                return other == Species.Cebra;
            case Species.Cebra:
                if (other == Species.Tigre) return false;
                return other == Species.Elefante || other == Species.Raton;
            case Species.Buho:
                // Buho no quiere sentarse con nadie
                return false;
            default:
                return false;
        }
    }
}
