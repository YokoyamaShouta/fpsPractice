using UnityEngine;
using UnityEngine.Windows;

public class playerMove : MonoBehaviour
{
    public float speed = 5f;
	private Vector3 playerVelocity;     //  ˆÚ“®—Ê

	private CharacterController controller;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    public void Walk(Vector2 input)
    {
		Vector3 moveDirection = Vector3.zero;

		moveDirection.x = input.x;
		moveDirection.z = input.y;

		float velocity = speed;

		controller.Move(transform.TransformDirection(moveDirection) * velocity * Time.deltaTime);	
	}
}
