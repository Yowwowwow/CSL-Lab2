using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rb; //Player Property Variable

    //Forward Speed Variables
    private float _currentSpeed = 0f;
    [Tooltip("The maximum forward speed the kart can reach.")]
    public float maxSpeed;
    [Tooltip("The maxiwan backward speed the kart can reach.")]
    public float minSpeed = -5;
    [SerializeField, Tooltip("The acceleration.")]
    private float acceleration;

    private Vector3 _turnAngle;
    [SerializeField, Tooltip("Max angular speed for rotations.")]
    private float maxRotationAngle;
    [SerializeField, Tooltip("Rotation Speed.")]
    private float rotationSpeed;
    [SerializeField, Tooltip("Hand use to show rotations.")]
    private Transform hands;
    [SerializeField, Tooltip("Front left wheel.")]
    private Transform frontLeftWheel;
    [SerializeField, Tooltip("Front right wheel.")]
    private Transform frontRightWheel;
    [SerializeField] WirelessMotionController cont;


    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Camera.main.fieldOfView = 60 + 1f * (cont.vvv - 512) / 512 * 30;
        Steer();
        Move();
    }

    // Move the player forward
    void Move()
    {
        // Calculate current speed
        if (cont.move == 1) //Move forward when press W
        {
            _currentSpeed = Mathf.Lerp(_currentSpeed, maxSpeed, Time.fixedDeltaTime * acceleration * 1f);
            Debug.Log("Move forward");
        }
        else if (Input.GetKey(KeyCode.S)) //Move backward when press S
        {
            _currentSpeed = Mathf.Lerp(_currentSpeed, minSpeed, Time.fixedDeltaTime * acceleration * 2f);
            Debug.Log("Move backward");
        }
        else //Slow down when no key's pressed
        {
            _currentSpeed = Mathf.Lerp(_currentSpeed, 0, Time.fixedDeltaTime * acceleration * 2f);
        }

        RotateRigidbody();

        Vector3 velocity = _currentSpeed * transform.forward;
        velocity.y = _rb.velocity.y; // Keep the gravity
        _rb.velocity = velocity; // Apply the speed to the rigidbody
    }

    //Steer the kart's front wheels
    private void Steer()
    {
        print(cont.yaw);
        float lower = 10, upper = 90;
        float x = (Mathf.Abs(cont.yaw) - lower) / (upper - lower);
        if (x > 1) x = 1;
        if (cont.yaw <= -lower) //Rotate left when press A
        {
            RotateVisual(maxRotationAngle, rotationSpeed * 1f * x);
        }
        else if (cont.yaw >= lower) //Rotate right when press D
        {
            RotateVisual(-maxRotationAngle, rotationSpeed * 1f * x);
        }
        else //Rotate back when no key's pressed
        {
            RotateVisual(0, rotationSpeed * 0.1f);
        }
    }

    //Turn the kart's rigidbody's direction according to frontwheel
    private void RotateRigidbody()
    {
        _turnAngle = frontLeftWheel.eulerAngles - transform.eulerAngles;
        _turnAngle.y = RegularizeAngle(_turnAngle.y);
        Quaternion deltaRotation = Quaternion.Euler(Mathf.Sign(_currentSpeed) * Time.fixedDeltaTime * _turnAngle);
        _rb.MoveRotation(_rb.rotation * deltaRotation);
    }

    //Regularize angle between -180~180 
    private float RegularizeAngle(float angle)
    {
        //equivalent to angle = (angle + 540) % 360 - 180;
        angle = (angle > 180) ? angle - 360 : angle;
        angle = (angle < -180) ? angle + 360 : angle;
        return angle;
    }

    //Rotate the kart's visual to target angle
    private void RotateVisual(float targetAngle, float rotateSpeed)
    {
        float handAngle = RegularizeAngle(hands.localRotation.eulerAngles.z);
        float wheelAngle = RegularizeAngle(frontLeftWheel.localRotation.eulerAngles.y);
        hands.Rotate(0, 0, (targetAngle - handAngle) * Time.fixedDeltaTime * rotateSpeed, Space.Self);
        frontLeftWheel.Rotate(0, (-targetAngle - wheelAngle) * Time.fixedDeltaTime * rotateSpeed, 0, Space.Self);
        frontRightWheel.Rotate(0, (-targetAngle - wheelAngle) * Time.fixedDeltaTime * rotateSpeed, 0, Space.Self);
    }

}
