using UnityEngine;

public enum Species
{
    Elefante,
    Tigre,
    Raton,
    Cebra,
    Buho,
    Camaleon
}

public class NPC : MonoBehaviour
{
    public Species species;
    public Material canSitMaterial;
    public Material cannotSitMaterial;

    public GameObject canSitParticlesPrefab;
    public GameObject cannotSitParticlesPrefab;

    public Seat CurrentSeat;

    public Seat assignedSeat;

    public bool actsAsElefante;

    private Material _lastAppliedMaterial;

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

            var speciesToEvaluate = species == Species.Camaleon
                ? GetChameleonEffectiveSpecies(left, right, front, frontLeft, frontRight, back, backLeft, backRight)
                : species;

            if (species == Species.Camaleon)
            {
                var animator = GetComponentInChildren<Animator>();

                if (animator != null)
                {
                    animator.SetBool("Elephant", speciesToEvaluate==Species.Elefante);
                    animator.SetBool("Mouse", speciesToEvaluate==Species.Raton);
                    animator.SetBool("Tiger", speciesToEvaluate==Species.Tigre);
                    animator.SetBool("Zebra", speciesToEvaluate==Species.Cebra);

                    if (speciesToEvaluate==Species.Camaleon || speciesToEvaluate==Species.Buho) animator.SetBool("Zebra", true);

                    SoundManager.instance.PlaySound(SoundList.instance.chameleon_Transform);

                }

             }

            bool prevActsAsElefante = actsAsElefante;
            actsAsElefante = species == Species.Camaleon && speciesToEvaluate == Species.Elefante;

            if (species == Species.Camaleon)
            {
                if (actsAsElefante && !prevActsAsElefante)
                {
                    if (left != null) left.SetBlockedBy(this);
                    if (right != null) right.SetBlockedBy(this);
                }
                else if (!actsAsElefante && prevActsAsElefante)
                {
                    if (left != null) left.ClearBlockIfBy(this);
                    if (right != null) right.ClearBlockIfBy(this);
                }
            }

            Debug.Log($"[NPC] {name} ({species}) en seat {CurrentSeat.name} -> especie efectiva: {speciesToEvaluate} | actuaComoElefante: {actsAsElefante}");

            switch (speciesToEvaluate)
            {
                case Species.Elefante:

                    SoundManager.instance.PlaySound(SoundList.instance.elephant);

                    bool sideOccupied = (left != null && left.occupant != null) || (right != null && right.occupant != null);
                    bool mouseInFront =
                        (front != null && front.occupant != null && front.occupant.species == Species.Raton) ||
                        (frontLeft != null && frontLeft.occupant != null && frontLeft.occupant.species == Species.Raton) ||
                        (frontRight != null && frontRight.occupant != null && frontRight.occupant.species == Species.Raton);
                    canSit = !(sideOccupied || mouseInFront);
                    Debug.Log($"[Elefante] lados ocupados: {sideOccupied}, raton delante: {mouseInFront}, canSit: {canSit}");
                    break;

                case Species.Raton:

                    SoundManager.instance.PlaySound(SoundList.instance.mouse);

                    bool leftHasTiger = left != null && left.occupant != null && left.occupant.species == Species.Tigre;
                    bool rightHasTiger = right != null && right.occupant != null && right.occupant.species == Species.Tigre;
                    canSit = !(leftHasTiger || rightHasTiger);
                    Debug.Log($"[Raton] tigre izquierda: {leftHasTiger}, tigre derecha: {rightHasTiger}, canSit: {canSit}");
                    break;

                case Species.Cebra:

                    SoundManager.instance.PlaySound(SoundList.instance.zebra);

                    bool leftTiger = left != null && left.occupant != null && left.occupant.species == Species.Tigre;
                    bool rightTiger = right != null && right.occupant != null && right.occupant.species == Species.Tigre;
                    bool frontTiger = front != null && front.occupant != null && front.occupant.species == Species.Tigre;
                    canSit = !(leftTiger || rightTiger || frontTiger);
                    Debug.Log($"[Cebra] tigre izquierda: {leftTiger}, tigre derecha: {rightTiger}, tigre frente: {frontTiger}, canSit: {canSit}");
                    break;

                case Species.Tigre:

                    SoundManager.instance.PlaySound(SoundList.instance.tiger);

                    bool leftBad = left != null && left.occupant != null && (left.occupant.species == Species.Raton || left.occupant.species == Species.Cebra);
                    bool rightBad = right != null && right.occupant != null && (right.occupant.species == Species.Raton || right.occupant.species == Species.Cebra);
                    bool frontBad = front != null && front.occupant != null && (front.occupant.species == Species.Raton || front.occupant.species == Species.Cebra);
                    canSit = !(leftBad || rightBad || frontBad);
                    Debug.Log($"[Tigre] conflicto izq: {leftBad}, der: {rightBad}, frente: {frontBad}, canSit: {canSit}");
                    break;

                case Species.Buho:

                    SoundManager.instance.PlaySound(SoundList.instance.owl);

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
                    Debug.Log($"[Buho] hay ocupados alrededor: {anyOccupied}, canSit: {canSit}");
                    break;

                case Species.Camaleon:
                    SoundManager.instance.PlaySound(SoundList.instance.chameleon_Transform);

                    canSit = true;
                    Debug.Log("[Camaleon] sin vecinos relevantes, sin restricciones.");
                    break;

                default:
                    canSit = true;
                    break;
            }



        }

        var rend = GetComponent<MeshRenderer>();
        if (rend != null)
        {
            var newMat = canSit ? canSitMaterial : cannotSitMaterial;
            if (rend.material != newMat)
            {
                rend.material = newMat;
                _lastAppliedMaterial = newMat;
                SpawnParticlesForMaterial(newMat);
            }
            else if (_lastAppliedMaterial == null)
            {
                _lastAppliedMaterial = newMat;
            }
        }
    }

    private void SpawnParticlesForMaterial(Material mat)
    {
        GameObject prefab = (mat == canSitMaterial) ? canSitParticlesPrefab : cannotSitParticlesPrefab;
        if (prefab == null) return;

        var position = transform.position + Vector3.up * 1.5f;
        var instance = Instantiate(prefab, position, Quaternion.identity);
        Destroy(instance, 5f);
    }

    public Species GetChameleonEffectiveSpeciesForSeat(Seat seat)
    {
        if (seat == null) return Species.Camaleon;
        return GetChameleonEffectiveSpecies(
            seat.leftNeighbor,
            seat.rightNeighbor,
            seat.frontNeighbor,
            seat.frontLeftNeighbor,
            seat.frontRightNeighbor,
            seat.backNeighbor,
            seat.backLeftNeighbor,
            seat.backRightNeighbor
        );
    }

    private Species GetChameleonEffectiveSpecies(Seat left, Seat right, Seat front, Seat frontLeft, Seat frontRight, Seat back, Seat backLeft, Seat backRight)
    {
        bool HasNeighborSpecies(Species s, Seat seat) => seat != null && seat.occupant != null && seat.occupant.species == s;

        bool hasTiger = HasNeighborSpecies(Species.Tigre, left) || HasNeighborSpecies(Species.Tigre, right) || HasNeighborSpecies(Species.Tigre, front) ||
            HasNeighborSpecies(Species.Tigre, frontLeft) || HasNeighborSpecies(Species.Tigre, frontRight) || HasNeighborSpecies(Species.Tigre, back) ||
            HasNeighborSpecies(Species.Tigre, backLeft) || HasNeighborSpecies(Species.Tigre, backRight);
        bool hasElefante = HasNeighborSpecies(Species.Elefante, left) || HasNeighborSpecies(Species.Elefante, right) || HasNeighborSpecies(Species.Elefante, front) ||
            HasNeighborSpecies(Species.Elefante, frontLeft) || HasNeighborSpecies(Species.Elefante, frontRight) || HasNeighborSpecies(Species.Elefante, back) ||
            HasNeighborSpecies(Species.Elefante, backLeft) || HasNeighborSpecies(Species.Elefante, backRight);
        bool hasCebra = HasNeighborSpecies(Species.Cebra, left) || HasNeighborSpecies(Species.Cebra, right) || HasNeighborSpecies(Species.Cebra, front) ||
            HasNeighborSpecies(Species.Cebra, frontLeft) || HasNeighborSpecies(Species.Cebra, frontRight) || HasNeighborSpecies(Species.Cebra, back) ||
            HasNeighborSpecies(Species.Cebra, backLeft) || HasNeighborSpecies(Species.Cebra, backRight);
        bool hasRaton = HasNeighborSpecies(Species.Raton, left) || HasNeighborSpecies(Species.Raton, right) || HasNeighborSpecies(Species.Raton, front) ||
            HasNeighborSpecies(Species.Raton, frontLeft) || HasNeighborSpecies(Species.Raton, frontRight) || HasNeighborSpecies(Species.Raton, back) ||
            HasNeighborSpecies(Species.Raton, backLeft) || HasNeighborSpecies(Species.Raton, backRight);

        Debug.Log($"[Camaleon] vecinos -> Tigre:{hasTiger} Elefante:{hasElefante} Cebra:{hasCebra} Raton:{hasRaton}");

        if (hasTiger) return Species.Tigre;
        if (hasElefante) return Species.Elefante;
        if (hasCebra) return Species.Cebra;
        if (hasRaton) return Species.Raton;
        return Species.Camaleon;
    }
}
