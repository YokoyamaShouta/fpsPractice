using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera PCamera;
	private float xRotation = 0f;

	public float xSensitivity = 30f;
	public float ySensitivity = 30f;

	public void ProcessLook(Vector2 input)
	{
		float mouseX = input.x;
		float mouseY = input.y;

		xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
		xRotation = Mathf.Clamp(xRotation, -80f, 80f);
		PCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
		transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
	}

	private void Start()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}
}
