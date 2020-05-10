using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    float movementSpeed = .1f;
    [SerializeField]
    float strafeSpeed = .05f;
    [SerializeField]
    float xRotationSpeed = .1f;
    [SerializeField]
    float yRotationSpeed = .3f;

    Vector3 movement = Vector3.zero;
    Vector2 rotation = Vector2.zero;
    float gravity = -9.87f;
    Vector3 previousMousePosition;
	
	// Update is called once per frame
	void Update () {
        movement = (transform.right*Input.GetAxis("Horizontal") * strafeSpeed) + (transform.forward * Input.GetAxis("Vertical") * movementSpeed);
        rotation = Input.mousePosition - previousMousePosition;
        previousMousePosition = Input.mousePosition;
	}

    private void FixedUpdate()
    {
        transform.position += movement;
        transform.Rotate(-rotation.y * xRotationSpeed, rotation.x * yRotationSpeed,0f,Space.Self);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0f);
    }

}
