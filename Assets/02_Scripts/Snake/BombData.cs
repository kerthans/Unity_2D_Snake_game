using System.Collections;
using UnityEngine;

public class BombData : MonoBehaviour
{
    public float duration = 5f; // 炸弹持续时间
    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        StartCoroutine(ExplodeAfterDuration());
    }

    private IEnumerator ExplodeAfterDuration()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject); // 销毁炸弹
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player")) // 使用 CompareTag 方法
        {
            playerMovement.ReduceLength(); // 调用玩家减少长度的方法
            Destroy(gameObject); // 销毁炸弹
        }
    }
}