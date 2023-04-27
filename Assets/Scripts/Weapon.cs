/*using Enemies_NPCs.Enemy_Behaviour;
using GameSystems;
using UnityEngine;
using Utilities;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Weapon : MonoBehaviour
{
    public enum WeaponType
    {
        Ranged,
        CloseCombat,
        Throwable
    }

    public float attackModifier;
    public int range;
    internal Collider2D Collider;
    internal Rigidbody2D Rigidbody;
    public Animation firing;
    private static WeaponType _type;
    internal readonly bool HasChildren = (_type == WeaponType.Ranged);

    public GameObject children;

    private string name { get; set; }

    private string Desc { get; set; }
    
    /// <summary> Constructor for the Weapon class. It takes in two parameters, name and type, and sets the
    /// name and Desc properties to values from an ItemData object.</summary>
    /// <param name="name"> The name of the item.</param>
    /// <param name="type"> /// the type of weapon, used to determine the damage and range. </param>
    /// <returns> A weapon object</returns>
    public Weapon(string name, WeaponType type)
    {
        _type = type;
        ItemData data = Resources.Load<ItemData>(@"Inventory\" + name);
        name = data.nameKey;
        Desc = data.descriptionKey;
    }
    
    // Still a lot of things to do here

    private void Start()
    {
        firing = GetComponent<Animation>();
    }
}*/