using UnityEngine;

public class damegezone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerStats>().TakeDamege(10);
            Destroy(gameObject);
        }
    }

}
