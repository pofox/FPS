using UnityEngine;

public class lookat : MonoBehaviour
{
    [SerializeField] Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = player.position - transform.position;
        dir = new Vector3(dir.x, 0, dir.z).normalized;
        transform.localRotation = Quaternion.FromToRotation(transform.up, dir) * transform.localRotation;
    }
}
