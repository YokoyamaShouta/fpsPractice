using UnityEngine;

public class ShotgunWeapon : Weapon
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public Camera fpsCamera;

    [SerializeField, Tooltip("射撃時の弾数")]
    private float pelletCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        verticalAngle = 5f;
        horizontalAngel = 12f;
        maxAmmo = 10;
        speed = 50f;
        fireRate = 5f;        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextFireTime && nowAmmo > 0)
        {
            Shot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    public override void Shot()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            nowAmmo++;
            nextFireTime = Time.time + fireRate;

            for (int i = 0; i < pelletCount; i++)
            {
				float x = Random.Range(-verticalAngle, verticalAngle);
				float y = Random.Range(-horizontalAngel, horizontalAngel);
				Vector3 direction = fpsCamera.transform.forward;
				direction = Quaternion.Euler(x, y, 0) * direction;
				GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
				bullet.GetComponent<Rigidbody>().AddForce(direction * speed, ForceMode.Impulse);
				if (Physics.Raycast(fpsCamera.transform.position, direction,
					out RaycastHit hit))
				{
					// ヒットしたオブジェクトの処理
					Debug.Log("Hit: " + hit.collider.name);
				}
			}
        }
    }

	public override void Reload()
	{
		base.Reload();
	}
}
