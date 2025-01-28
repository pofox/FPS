using UnityEngine;
using UnityEngine.InputSystem;

public class PickUp : MonoBehaviour
{
    [SerializeField] Transform weapons;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform cameraTransform;
    private Pickable Pickable = null;
    private Pickable oldPickable = null;
    private void Update()
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hitInfo, 10.0f, layerMask))
        {
            if (hitInfo.collider != null)
            {
                Pickable = hitInfo.collider.gameObject.GetComponent<Pickable>();
            }
        }
    }
    void OnPickup(InputValue value)
    {
        if (Pickable != null&&!GameManager.Instance.paused)
        {
            Pickable.pickMe(weapons);
        }
    }
}
