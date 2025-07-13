using UnityEngine;

public interface IDamagable
{
    void TakeDamage(int amount, Vector3 hitPoint, Vector3 hitNormal);
}
