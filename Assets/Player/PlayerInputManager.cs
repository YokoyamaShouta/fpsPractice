using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.MovementActions keyInput;
    private playerMove move;
    private PlayerLook playerlook;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {        
        playerInput = new PlayerInput();
        keyInput = playerInput.Movement;
        move = GetComponent<playerMove>();
        playerlook = GetComponent<PlayerLook>();
    }

    // Update is called once per frame
    void Update()
    {
        move.Walk(keyInput.move.ReadValue<Vector2>());
        playerlook.ProcessLook(keyInput.Look.ReadValue<Vector2>());
    }
	private void OnEnable()
	{
		keyInput.Enable();
	}
	private void OnDisable()
	{
		keyInput.Disable();
	}
}
