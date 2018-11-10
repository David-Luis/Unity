using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float walkSpeed = 2;
    public float jumpSpeed = 4;

    private Rigidbody _body;
    private Collider _collider;

    private Vector3 playerSize;
    private bool jumpReleased = true;

    public AudioSource coinSound;
	
	void Start ()
    {
        _body = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();

        playerSize = _collider.bounds.size;
	}
	
	void FixedUpdate ()
    {
        Walk();
        Jump();
	}

    void Walk()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(hAxis * walkSpeed, 0, vAxis * walkSpeed) * Time.deltaTime;
        Vector3 newPos = transform.position + movement;

        _body.MovePosition(newPos);
    }

    void Jump()
    {
        float jAxis = Input.GetAxis("Jump");

        if (jAxis > 0)
        {
            if (jumpReleased && IsGrounded())
            {
                jumpReleased = false;
                Vector3 jumpVector = new Vector3(0, jAxis * jumpSpeed, 0);
                _body.AddForce(jumpVector, ForceMode.VelocityChange);
            }
        }
        else
        {
            jumpReleased = true;
        }
    }

    private bool IsGrounded()
    {
        Vector3 corner1 = transform.position + new Vector3(playerSize.x * 0.5f, -playerSize.y * 0.5f + 0.01f, playerSize.z * 0.5f);
        Vector3 corner2 = transform.position + new Vector3(-playerSize.x * 0.5f, -playerSize.y * 0.5f + 0.01f, playerSize.z * 0.5f);
        Vector3 corner3 = transform.position + new Vector3(-playerSize.x * 0.5f, -playerSize.y * 0.5f + 0.01f, -playerSize.z * 0.5f);
        Vector3 corner4 = transform.position + new Vector3(playerSize.x * 0.5f, -playerSize.y * 0.5f + 0.01f, -playerSize.z * 0.5f);

        if (Physics.Raycast(corner1, Vector3.down, 0.01f) || Physics.Raycast(corner2, Vector3.down, 0.01f) ||
            Physics.Raycast(corner3, Vector3.down, 0.01f) || Physics.Raycast(corner4, Vector3.down, 0.01f))
        {
            return true;
        }

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            GameController.instance.IncreaseScore(1);
            coinSound.Play();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Finish"))
        {
            GameController.instance.IncreaseLevel();
        }
        else if (other.CompareTag("Enemy"))
        {
            GameController.instance.ResetGame();
        }
    }
}
