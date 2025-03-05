using UnityEngine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class Shoot : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject shootImage;
    [SerializeField] Variables weapon;
    [SerializeField] ParticleSystem prewarm;
    [SerializeField] ParticleSystem fire;
    private float shootingInterval = 0f;
    private Transform cameraTransform;
    bool isShooting = false;
    float time = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraTransform = GetComponentsInChildren<Transform>()[1];
        prewarm.Stop();
        fire.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        time += isShooting ? Time.deltaTime : 0;
        if (time > shootingInterval)
        {
            time = 0;
            shoot();
            crosshair.GetComponent<Animator>().speed = 1 / (float)weapon.declarations.Get("ShootingSpeed");
        }
        crosshair.GetComponent<Animator>().SetBool("Shooting",isShooting);
        shootingInterval = (float)weapon.declarations.Get("ShootingSpeed");
    }
    void OnShoot(InputValue value)
    {
        if (!GameManager.Instance.paused)
        {
            isShooting = !isShooting;
            time = isShooting? shootingInterval : 0;
            prewarm.Play();
        }
    }
    void shoot()
    {
        SoundManager.Instance.PlayShootingSound();
        fire.Play();
        if(Physics.Raycast(cameraTransform.position, cameraTransform.forward,out RaycastHit hitInfo, (float)weapon.declarations.Get("ShootingRange"),layerMask))
        {
            if (hitInfo.collider != null) {
                if (hitInfo.collider.tag != "enemy")
                {
                    GameObject plane = Instantiate(shootImage, hitInfo.point + 0.01f * hitInfo.normal, Quaternion.FromToRotation(shootImage.transform.up, hitInfo.normal));
                    plane.SetActive(true);
                    plane.transform.localRotation = Quaternion.AngleAxis(Random.value * 360, plane.transform.up) * plane.transform.localRotation;
                }
                else
                {
                    hitInfo.collider.GetComponent<Enemy>().TakeDamege((float)weapon.declarations.Get("damege"));
                }
            }
        }
    }
}
