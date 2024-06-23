using System.Collections;
using UnityEngine;

public enum ItemType
{
    Heal,
    SpeedBoost
}

public class Item : MonoBehaviour
{
    [SerializeField] private ItemType itemType;
    [SerializeField] private float amount = 10f;
    [SerializeField] private float duration = 5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ApplyEffect(collision.gameObject);
            Destroy(gameObject);
        }
    }

    private void ApplyEffect(GameObject player)
    {
        Player playerScript = player.GetComponent<Player>();
        if (playerScript != null)
        {
            switch (itemType)
            {
                case ItemType.Heal:
                    playerScript.Heal(amount);
                    break;
                case ItemType.SpeedBoost:
                    playerScript.StartCoroutine(playerScript.SpeedBoost(duration));
                    break;
                default:
                    Debug.LogWarning("Item type not implemented.");
                    break;
            }
        }
    }
}
