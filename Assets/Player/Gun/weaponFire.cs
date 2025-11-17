using UnityEngine;

public class weaponFire : MonoBehaviour
{
    public GameObject bulletprefab;
    public Transform bulletSpawn;
    public Camera pcamera;
    private playerMove move;

    [SerializeField]
    private float speed = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {        
        move = GetComponent<playerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            Fire();
        }
    }

    public void Fire()
    {
        GameObject bullet = Instantiate(bulletprefab, bulletSpawn.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(pcamera.transform.forward * speed, ForceMode.Impulse);
    }
}
