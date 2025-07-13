using UnityEngine;

public class DummyEnemy : MonoBehaviour, IDamagable
{
    public void TakeDamage(int amount, Vector3 hitPoint, Vector3 hitNormal)
    {
        Debug.Log("I'm being damaged");
    }

}
