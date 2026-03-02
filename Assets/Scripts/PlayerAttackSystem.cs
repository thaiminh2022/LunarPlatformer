using UnityEngine;

public class PlayerAttackSystem : MonoBehaviour
{
    [SerializeField] LayerMask _enemyEntityLayer;
    [SerializeField] Transform _checkOrigin;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.transform.position.y > _checkOrigin.position.y)
            return;


        // layer is store as int for indexes, but layermask is store as bit
        // eg:
        // .layer = 9 => .value = 512 (2^9)
        // log2(512) = 9
        if (collision.gameObject.layer != Mathf.Log(_enemyEntityLayer.value, 2))
            return;



        if (collision.TryGetComponent<IDamagable>(out var damagable))
        {
            // each jump on top head is one hit point
            damagable.Damage(1);
        }
    }
}
