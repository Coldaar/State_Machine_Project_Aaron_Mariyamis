using UnityEngine;

public enum AbilityPickupType
{
    Glider,
    Sword
}

[RequireComponent(typeof(Collider2D))]
public class AbilityPickup : MonoBehaviour
{
    [SerializeField] private AbilityPickupType ability;

    public AbilityPickupType Ability => ability;

    private void Awake()
    {
        Collider2D pickupCollider = GetComponent<Collider2D>();
        pickupCollider.isTrigger = true;
    }
}
