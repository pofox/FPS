using UnityEngine;

public class suicide : MonoBehaviour
{
    private float time = 0;
    private void Start()
    {
        time = 0;
    }
    void Update()
    {
        time += Time.deltaTime;
        if (time > 2)
        {
            Destroy(gameObject);
        }
    }
}
