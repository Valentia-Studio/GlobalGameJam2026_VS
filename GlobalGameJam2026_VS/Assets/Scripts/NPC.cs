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

    public Seat assignedSeat;

    public void EvaluateNeighbors()
    {
        bool canSit = true;

        if (CurrentSeat != null)
        {
            var left = CurrentSeat.leftNeighbor;
            var right = CurrentSeat.rightNeighbor;
            var front = CurrentSeat.frontNeighbor;
            var frontLeft = CurrentSeat.frontLeftNeighbor;
            var frontRight = CurrentSeat.frontRightNeighbor;
            var back = CurrentSeat.backNeighbor;
            var backLeft = CurrentSeat.backLeftNeighbor;
            var backRight = CurrentSeat.backRightNeighbor;

            switch (species)
            {
                case Species.Elefante:
                    if ((left != null && left.occupant != null) || (right != null && right.occupant != null))
                    {
                        canSit = false;
                        break;
                    }
                    if ((front != null && front.occupant != null && front.occupant.species == Species.Raton) ||
                        (frontLeft != null && frontLeft.occupant != null && frontLeft.occupant.species == Species.Raton) ||
                        (frontRight != null && frontRight.occupant != null && frontRight.occupant.species == Species.Raton))
                    {
                        canSit = false;
                    }
                    break;

                case Species.Raton:
                    bool leftHasTiger = left != null && left.occupant != null && left.occupant.species == Species.Tigre;
                    bool rightHasTiger = right != null && right.occupant != null && right.occupant.species == Species.Tigre;
                    canSit = !(leftHasTiger || rightHasTiger);
                    break;

                case Species.Cebra:
                    bool leftTiger = left != null && left.occupant != null && left.occupant.species == Species.Tigre;
                    bool rightTiger = right != null && right.occupant != null && right.occupant.species == Species.Tigre;
                    bool frontTiger = front != null && front.occupant != null && front.occupant.species == Species.Tigre;
                    canSit = !(leftTiger || rightTiger || frontTiger);
                    break;

                case Species.Tigre:
                    bool leftBad = left != null && left.occupant != null && (left.occupant.species == Species.Raton || left.occupant.species == Species.Cebra);
                    bool rightBad = right != null && right.occupant != null && (right.occupant.species == Species.Raton || right.occupant.species == Species.Cebra);
                    bool frontBad = front != null && front.occupant != null && (front.occupant.species == Species.Raton || front.occupant.species == Species.Cebra);
                    canSit = !(leftBad || rightBad || frontBad);
                    break;

                case Species.Buho:
                    bool anyOccupied =
                        (left != null && left.occupant != null) ||
                        (right != null && right.occupant != null) ||
                        (front != null && front.occupant != null) ||
                        (frontLeft != null && frontLeft.occupant != null) ||
                        (frontRight != null && frontRight.occupant != null) ||
                        (back != null && back.occupant != null) ||
                        (backLeft != null && backLeft.occupant != null) ||
                        (backRight != null && backRight.occupant != null);
                    canSit = !anyOccupied;
                    break;

                default:
                    canSit = true;
                    break;
            }
        }

        var rend = GetComponent<MeshRenderer>();
        if (rend != null)
        {
            rend.material = canSit ? canSitMaterial : cannotSitMaterial;
        }
    }
}
