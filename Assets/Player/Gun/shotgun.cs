using UnityEngine;

public class Shotgun : MonoBehaviour
{
	public Camera fpsCamera;
	public int pelletCount = 10;      // 一発で飛ぶ弾の数
	public float spreadAngle = 8f;    // 散らばり
	public float damage = 10f;        // 一発のダメージ
	public float range = 40f;         // 射程距離
	public float fireDelay = 0.8f;    // 発射間隔
	private float nextFireTime = 0f;

	public ParticleSystem muzzleFlash; // 銃口フラッシュ（任意）

	void Update()
	{
		if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
		{
			nextFireTime = Time.time + fireDelay;
			Shoot();
		}
	}

	void Shoot()
	{
		if (muzzleFlash) muzzleFlash.Play();

		for (int i = 0; i < pelletCount; i++)
		{
			// 角度をランダムに散らす
			float x = Random.Range(-spreadAngle, spreadAngle);
			float y = Random.Range(-spreadAngle, spreadAngle);
			Vector3 direction = fpsCamera.transform.forward;
			direction = Quaternion.Euler(x, y, 0) * direction;

			if (Physics.Raycast(fpsCamera.transform.position, direction,
				out RaycastHit hit, range))
			{
				// ヒットしたオブジェクトの処理
				Debug.Log("Hit: " + hit.collider.name);				
			}
		}
	}
}