using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
	[SerializeField, Tooltip("c‚ÌŠgU")]
	protected float verticalAngle;
	
    [SerializeField, Tooltip("c‚ÌŠgU")]
	protected float horizontalAngel;
		
    [SerializeField, Tooltip("c‚ÌŠgU")]
	protected int maxAmmo;

	protected int nowAmmo;

	[SerializeField, Tooltip("c‚ÌŠgU")]
	protected float speed;
    
    protected float nextFireTime;
	
    [SerializeField, Tooltip("c‚ÌŠgU")]
	protected float fireRate;
    
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	protected virtual void Start()
    {
        nowAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Shot() { }

    public virtual void Reload()
    {
        nowAmmo = maxAmmo;
    }
}
