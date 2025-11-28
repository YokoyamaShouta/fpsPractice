using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PickUpGun : MonoBehaviour
{
	[Header("Settings")]
	public Transform equipPosition;
	public float distance = 10f;

	const string WeaponTag = "grabbable";

	[Header("UI")]
	public TMP_Text pickupText;
	public TMP_Text swapText;
	public Image[] slotImages;
	public Sprite emptyIcon;
	public Sprite pistolIcon;
	public Sprite rifleIcon;
	public Color selectedColor = Color.yellow;
	public Color normalColor = Color.white;

	[HideInInspector] public GameObject[] weapons = new GameObject[2];
	[HideInInspector] public GameObject currentWeapon;
	[HideInInspector] public int currentSlot = -1;

	[HideInInspector] public GameObject targetWeapon;
	[HideInInspector] public bool canGetWeapon;

	[HideInInspector] public bool isChoosingSlot = false;
	GameObject weaponToPick;

	Camera cam;
	void Start()
	{
		cam = Camera.main;
	}

	void Update()
	{
		DetectWeapon();

		// キー入力
		if (Input.GetKeyDown(KeyCode.E) && canGetWeapon)
			TryPickUp();

		if (currentWeapon != null && Input.GetKeyDown(KeyCode.Q))
			Drop(currentSlot);

		if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0);
		if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1);

		// UI更新
		UpdatePickupUI();
		UpdateSlotUI();
		UpdateSwapUI();
	}

	// 拾える武器を検出
	void DetectWeapon()
	{
		RaycastHit hit;
		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, distance))
		{
			if (hit.collider.CompareTag(WeaponTag))
			{
				canGetWeapon = true;
				targetWeapon = hit.collider.gameObject;
				return;
			}
		}

		canGetWeapon = false;
		targetWeapon = null;
	}

	// 武器を拾う処理
	void TryPickUp()
	{
		if (isChoosingSlot && weaponToPick == null)
		{
			Debug.LogWarning("交換モード不正 → リセット");
			isChoosingSlot = false;
		}

		// 空きスロットがあるか
		for (int i = 0; i < 2; i++)
		{
			if (weapons[i] == null)
			{
				PickUpToSlot(i, targetWeapon);
				return;
			}
		}

		isChoosingSlot = true;
		weaponToPick = targetWeapon;
	}

	// スロット選択（交換 or 切替）
	void SelectSlot(int slot)
	{
		if (slot < 0 || slot >= 2) return;
		if (!isChoosingSlot && slot == currentSlot) return;

		// 交換中
		if (isChoosingSlot)
		{
			if (weapons[slot] != null)
				weapons[slot].SetActive(false);

			Drop(slot);
			PickUpToSlot(slot, weaponToPick);

			isChoosingSlot = false;
			weaponToPick = null;
			return;
		}

		// 通常切替
		if (weapons[slot] != null)
			SwitchWeapon(slot);
	}

	// 指定スロットに拾う
	void PickUpToSlot(int slot, GameObject weapon)
	{
		if (currentWeapon != null)
			currentWeapon.SetActive(false);

		weapons[slot] = weapon;
		currentWeapon = weapon;
		currentSlot = slot;
		
		HoldWeapon(weapon);
		weapon.SetActive(true);
	}

	// 武器を手に持つ位置に固定
	void HoldWeapon(GameObject weapon)
	{
		weapon.transform.SetParent(equipPosition);
		weapon.transform.localPosition = Vector3.zero;
		weapon.transform.localEulerAngles = new Vector3(90f, 0, 0);

		Rigidbody rb = weapon.GetComponent<Rigidbody>();
		if (rb != null)
		{
			rb.isKinematic = true;
			rb.linearVelocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
		}
	}

	// 武器を落とす
	void Drop(int slot)
	{
		if (weapons[slot] == null) return;

		GameObject w = weapons[slot];
		weapons[slot] = null;

		Vector3 dropPos = equipPosition.position + equipPosition.forward * 1.0f - equipPosition.up * 0.5f;

		w.transform.SetParent(null);
		w.transform.position = dropPos;
		w.SetActive(true);

		Rigidbody rb = w.GetComponent<Rigidbody>();
		if (rb != null)
		{
			rb.isKinematic = false;
			rb.linearVelocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
		}

		if (currentSlot == slot)
		{
			currentWeapon = null;
			currentSlot = -1;
		}
	}

	// スロット切替
	void SwitchWeapon(int slot)
	{
		if (currentWeapon != null)
			currentWeapon.SetActive(false);

		currentWeapon = weapons[slot];
		currentSlot = slot;

		HoldWeapon(currentWeapon);
		currentWeapon.SetActive(true);
	}

	// UI更新
	void UpdatePickupUI()
	{
		if (canGetWeapon && targetWeapon != null)
		{
			pickupText.gameObject.SetActive(true);
			pickupText.text = $"Pick up at E: {targetWeapon.name}";
		}
		else
		{
			pickupText.gameObject.SetActive(false);
		}
	}

	void UpdateSlotUI()
	{
		for (int i = 0; i < weapons.Length; i++)
		{
			if (weapons[i] == null)
				slotImages[i].sprite = emptyIcon;
			else if (weapons[i].name.Contains("Pistol"))
				slotImages[i].sprite = pistolIcon;
			else if (weapons[i].name.Contains("Rifle"))
				slotImages[i].sprite = rifleIcon;

			slotImages[i].color = (currentSlot == i) ? selectedColor : normalColor;
		}
	}

	void UpdateSwapUI()
	{
		swapText.gameObject.SetActive(isChoosingSlot);
		if (isChoosingSlot)
			swapText.text = "Press slot 1 or 2 to swap";
	}
}
