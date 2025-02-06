using UnityEngine;
using System.Collections;

public class PoisonMushroomData : MonoBehaviour
{
    public float duration = 5f; // 毒蘑菇效果持续时间
    public float lifetimeMin = 5f; // 毒蘑菇最短存在时间
    public float lifetimeMax = 15f; // 毒蘑菇最长存在时间

    private void Start()
    {
        StartCoroutine(AutoDestroy());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.ApplyPoisonMushroomEffect(duration);
                Destroy(gameObject); // 销毁道具
            }
        }
    }

    private IEnumerator AutoDestroy()
    {
        float lifetime = Random.Range(lifetimeMin, lifetimeMax);
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}