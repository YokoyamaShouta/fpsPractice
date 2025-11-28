using UnityEngine;

public class ARWeapon : Weapon
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public Camera fpsCam;

    public int NowAmmo => nowAmmo;

	
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	protected override void Start()
    {
        base.Start();
        verticalAngle = 2f;
        horizontalAngel = 2f;
        maxAmmo = 10;
        speed = 50f;
        fireRate = 0.2f;
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
		if (Input.GetButton("Fire1"))
        {
            nowAmmo++;
            nextFireTime = Time.time + fireRate;
			float x = Random.Range(-verticalAngle, verticalAngle);
			float y = Random.Range(-horizontalAngel, horizontalAngel);
			Vector3 direction = fpsCam.transform.forward;
			direction = Quaternion.Euler(x, y, 0) * direction;
			GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
			bullet.GetComponent<Rigidbody>().AddForce(direction * speed, ForceMode.Impulse);

		}
	}

	public override void Reload()
    {
        base.Reload();
    }
}
