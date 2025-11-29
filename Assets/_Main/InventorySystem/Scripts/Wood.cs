using UnityEngine;

public class Wood : Item
{
    public override void Use()
    {
        Instantiate(this, Vector3.zero + Vector3.right * 2, Quaternion.identity);
    }
}
