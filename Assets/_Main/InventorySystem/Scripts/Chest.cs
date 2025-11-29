using UnityEngine;

public class Chest : Item
{
    public override void Use()
    {
        Instantiate(this, Vector3.zero + Vector3.left * 2, Quaternion.identity);
    }
}