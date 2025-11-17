using UnityEngine;
using System.Collections;
public class PickUpGun : MonoBehaviour
{
	public Transform equipPosition;
	public float distance = 10f;
	public float dropForce = 2f;
	private GameObject[] weapons = new GameObject[2]; // 2スロット
	private GameObject currentWeapon;
	private GameObject targetWeapon;
	private int currentSlot = -1;
	private bool canGetWeapon;
	private bool waitingForSlotSelection = false;
	void Update()
	{
		CheckWeapon();
		// 武器を拾う
		if (canGetWeapon && Input.GetKeyDown(KeyCode.E) && !waitingForSlotSelection)
		{
			TryPickUp();
		}
		// 武器を捨てる
		if (currentWeapon != null && Input.GetKeyDown(KeyCode.Q))
		{
			Drop(currentSlot);
		}
		// 武器切替
		if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchWeapon(0);
		if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchWeapon(1);
	}
	void CheckWeapon()
	{
		RaycastHit hit;
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, distance))
		{
			canGetWeapon = hit.transform.CompareTag("grabbable");
			if (canGetWeapon) targetWeapon = hit.transform.gameObject;
		}
		else
		{
			canGetWeapon = false;
			targetWeapon = null;
		}
	}
	void TryPickUp()
	{
		// 空きスロットがあれば装備
		for (int i = 0; i < weapons.Length; i++)
		{
			if (weapons[i] == null)
			{
				EquipWeapon(i);
				return;
			}
		}
		// 空きスロットがない場合は選択待機
		Debug.Log("空きスロットがありません。どの武器と入れ替えますか？ 1 or 2");
		waitingForSlotSelection = true;
		StartCoroutine(WaitForSlotSelection());
	}
	IEnumerator WaitForSlotSelection()
	{
		bool selected = false;
		while (!selected)
		{
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				ReplaceWeapon(0);
				selected = true;
			}
			else if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				ReplaceWeapon(1);
				selected = true;
			}
			yield return null;
		}
		waitingForSlotSelection = false;
	}
	void ReplaceWeapon(int slot)
	{
		if (slot < 0 || slot >= weapons.Length) return;
		Drop(slot);
		EquipWeapon(slot);
	}
	void EquipWeapon(int slot)
	{
		weapons[slot] = targetWeapon;
		currentWeapon = weapons[slot];
		currentSlot = slot;
		// 手に持たせる
		weapons[slot].transform.parent = equipPosition;
		weapons[slot].transform.localPosition = Vector3.zero;
		weapons[slot].transform.localRotation = Quaternion.identity;
		Rigidbody rb = weapons[slot].GetComponent<Rigidbody>();
		if (rb != null) rb.isKinematic = true;
		currentWeapon.SetActive(true);
	}
	void Drop(int slot)
	{
		if (weapons[slot] != null)
		{
			Rigidbody rb = weapons[slot].GetComponent<Rigidbody>();
			weapons[slot].transform.parent = null;
			if (rb != null)
			{
				rb.isKinematic = false;
				// 少し前方向に投げる
				rb.AddForce(Camera.main.transform.forward * dropForce, ForceMode.Impulse);
			}
			weapons[slot] = null;
			currentWeapon = null;
			currentSlot = -1;
		}
	}
	void SwitchWeapon(int slot)
	{
		if (slot < 0 || slot >= weapons.Length) return;
		if (weapons[slot] == null) return;
		// 現在の武器を非表示
		if (currentWeapon != null) currentWeapon.SetActive(false);
		// 新しい武器を装備
		currentWeapon = weapons[slot];
		currentSlot = slot;
		// 親子付けと位置リセット
		currentWeapon.transform.parent = equipPosition;
		currentWeapon.transform.localPosition = Vector3.zero;
		currentWeapon.transform.localRotation = Quaternion.identity;
		Rigidbody rb = currentWeapon.GetComponent<Rigidbody>();
		if (rb != null) rb.isKinematic = true;
		currentWeapon.SetActive(true);
	}
}