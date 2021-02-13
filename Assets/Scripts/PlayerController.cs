using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSensetivity = 15f;


    [SerializeField]
    private float thrusterForce = 1000f;

    [SerializeField]
    private float thrusterFuelBurnSpeed = 1f;
    [SerializeField]
    private float thrusterFuelRegenSpeed = 0.3f;
    private float thrusterFuelAmount = 1f;

    public float GetThrusterFuelAmount()
    {
        return thrusterFuelAmount;
    }


    [Header("Spring Settings")]
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 40f;

    // component caching
    private PlayerMotor motor;
    private ConfigurableJoint joint;
    private Animator animator;

    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        animator = GetComponent<Animator>();

        SetJointSettings(jointSpring);

    }

    void Update()
    {
        // calculate movement velocity as a 3d vector
        float _xMove = Input.GetAxis("Horizontal"); // -1 to 1
        float _zMove = Input.GetAxis("Vertical");   // -1 to 1

        Vector3 moveHorizontal = transform.right * _xMove;
        Vector3 moveVertical = transform.forward * _zMove;

        // final movement vector
        Vector3 velocity = (moveHorizontal + moveVertical) * speed;

        // animate movement
        animator.SetFloat("ForwardVelocity", _zMove);

        // apply our movement
        motor.Move(velocity);

        // calculate rotation as a 3d vector (turning around)
        float _yRotation = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRotation, 0f) * lookSensetivity;

        // Apply rotation
        motor.Rotate(_rotation);



        // calculate camera rotation as a 3d vector(turning around)
        float _xRotation = Input.GetAxisRaw("Mouse Y");

        float _cameraRotationX = _xRotation * lookSensetivity;

        // Apply camera rotation
        motor.RotateCamera(_cameraRotationX);


        // Calculate the thrusterforce based on player input
        Vector3 _thrusterForce = Vector3.zero;
        if (Input.GetButton("Jump") && thrusterFuelAmount > 0f )
        {
            thrusterFuelAmount -= thrusterFuelBurnSpeed * Time.deltaTime;

            _thrusterForce = Vector3.up * thrusterForce;
            SetJointSettings(0f);
        }
        else
        {
            thrusterFuelAmount += thrusterFuelRegenSpeed * Time.deltaTime;

            SetJointSettings(jointSpring);
        }

        thrusterFuelAmount = Mathf.Clamp(thrusterFuelAmount, 0f, 1f);

        // Apply the Thruster Force
        motor.ApplyThruster(_thrusterForce);
    }

    private void SetJointSettings ( float _jointSpring)
    {
        joint.yDrive = new JointDrive 
        { 
            positionSpring = jointSpring,
            maximumForce = jointMaxForce 
        };
    }  


}
