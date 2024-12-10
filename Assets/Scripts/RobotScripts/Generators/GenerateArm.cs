using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[ExecuteAlways]
public class GenerateArm : MonoBehaviour
{
    private GameObject _armSec1;
    private GameObject _armSec1Model;
    private Rigidbody _rb;
    private HingeJoint _hj;

    [SerializeField] private bool continuousAim;

    [SerializeField] private Vector3 target;

    [Tooltip("leave Null for inline Continuous Aiming")]
    [SerializeField] private GameObject indicatorAim;

    [SerializeField] private float angleOffset;
    
    [SerializeField] private float[] setPoints;
    private float[] _setPointBuffer;

    [SerializeField] private Buttons[] setPointButtons;
    private Buttons[] _setPointButtonsBuffer;
    
    [SerializeField] private float armLength = 16;
    [SerializeField] private float armWidth = 14;
    [SerializeField] private float armHeight = 2;
    [SerializeField] private float armWeight = 8;
    
    private GameObject _mainRobot;
    private Rigidbody _robotRb;
    
    private PlayerInput _playerInput;
    
    private InputActionMap _inputMap;

    private float _activeTarget;
    
    [SerializeField] private ControlType controlType;
    
    private int _setpointSequence;

    private bool _sequenceDebounce;

    private JointMotor _jm;

    private float _startingAngle;

    private float _position;
    
    private Buttons _lastButton;

    [SerializeField] private float stowAngle = 0;
    
    void Start()
    {
        Startup();
    }

    private void Awake()
    {
        Startup();
    }
    // Update is called once per frame
    void Update()
    {
        if (!EditorApplication.isPlaying)
        {
            if (_mainRobot == null)
            {
                _mainRobot = transform.root.gameObject;
            }

            if (_robotRb == null)
            {
                _robotRb = _mainRobot.GetComponent<Rigidbody>();
            }
            
            if (_armSec1 == null)
            {
                _armSec1 = new GameObject("ArmSec1");
                _armSec1.transform.parent = transform;
                _armSec1.transform.localPosition = Vector3.zero;
                _armSec1.layer = LayerMask.NameToLayer("Robot");
            }

            if (_rb == null)
            {
                _rb = _armSec1.AddComponent<Rigidbody>();
                _rb.interpolation = RigidbodyInterpolation.Interpolate;
                _rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                _rb.excludeLayers = LayerMask.GetMask("Robot");
                _rb.useGravity = true;
                _rb.drag = 0;
            }

            if (_hj == null)
            {
                var t = transform;
                while (t.GetComponent<Rigidbody>() == null)
                {
                    t = t.parent.transform;
                }

                if (t.GetComponent<Rigidbody>() != null)
                {
                    _hj = _armSec1.AddComponent<HingeJoint>();
                    _hj.axis = new Vector3(1, 0, 0);
                    _hj.useMotor = true;
                    _hj.connectedBody = t.GetComponent<Rigidbody>();
                }
            }
            
            _rb.mass = armWeight;

            if (_armSec1Model == null)
            {
                _armSec1Model = GameObject.CreatePrimitive(PrimitiveType.Cube); 
                _armSec1Model.name = "ArmSec1Model";
                _armSec1Model.transform.parent = _armSec1.transform;
                _armSec1Model.layer = LayerMask.NameToLayer("Robot");
            }
            
            _armSec1Model.transform.localScale = new Vector3(armWidth*0.0254f, armHeight*0.0254f, armLength*0.0254f);
            _armSec1Model.transform.localPosition = new Vector3(0, 0, armLength/2*0.0254f);

            if (setPointButtons != null)
            {
                if (setPointButtons.Length != setPoints.Length)
                {
                    _setPointButtonsBuffer = setPointButtons;

                    setPointButtons = new Buttons[setPoints.Length];
                    for (int i = 0; i < _setPointButtonsBuffer.Length; i++)
                    {
                        if (i < setPoints.Length && i < _setPointButtonsBuffer.Length)
                        {
                            setPointButtons[i] = _setPointButtonsBuffer[i];
                        }
                    }
                }
            }
            else
            {
                setPoints = new float[1];
                setPointButtons = new Buttons[setPoints.Length];
            }
        }
        else
        {
            for (var i = 0; i < setPoints.Length; i++)
            {
                if (controlType == ControlType.toggle)
                {
                    if (_inputMap.FindAction(setPointButtons[i].ToString()).triggered)
                    {
                        if (Mathf.Approximately(_activeTarget, -setPoints[i]))
                        {
                            _activeTarget = -stowAngle;
                        }
                        else
                        {
                            _activeTarget = -setPoints[i];
                        }

                        float currentAngle = _hj.transform.localEulerAngles.x + 0;
                
                        if (Mathf.Abs(currentAngle) > 180)
                        {
                            currentAngle -= 360 * (currentAngle / Mathf.Abs(currentAngle));
                        }

                        _startingAngle = currentAngle;

                    }
                }
                else if (controlType == ControlType.hold)
                {
                    if (_inputMap.FindAction(setPointButtons[i].ToString()).IsPressed())
                    {
                        if (!Mathf.Approximately(_activeTarget, -setPoints[i]))
                        {
                            float currentAngle = _hj.transform.localEulerAngles.x + 0;
                
                            if (Mathf.Abs(currentAngle) > 180)
                            {
                                currentAngle -= 360 * (currentAngle / Mathf.Abs(currentAngle));
                            }

                            _startingAngle = currentAngle;
                        }
                        
                        _activeTarget = -setPoints[i];
                    }
                    else
                    {
                        _activeTarget = -stowAngle;
                    }
                }
                else if (controlType == ControlType.sequence)
                {
                    if (_inputMap.FindAction(setPointButtons[i].ToString()).triggered && _sequenceDebounce == false)
                    {
                        if (_setpointSequence < setPoints.Length)
                        {
                            if (_lastButton != setPointButtons[i])
                            {
                                _setpointSequence = 0;
                                _lastButton = setPointButtons[i];
                            }
                            
                            while (setPointButtons[i] != setPointButtons[_setpointSequence])
                            {
                                _setpointSequence ++;

                                if (_setpointSequence+1 > setPoints.Length)
                                {
                                    _setpointSequence = 0;
                                    _activeTarget = -stowAngle;
                                    return;
                                }


                            }
                            _activeTarget = -setPoints[_setpointSequence];

                            _setpointSequence += 1;
                        }
                        else
                        {
                            _setpointSequence = 0;
                            _activeTarget = -stowAngle;
                        }
                        
                        _lastButton = setPointButtons[i];
                    }
                    else
                    {

                    }
                    
                    if (_inputMap.FindAction(setPointButtons[i].ToString()).IsPressed())
                    {
                        _sequenceDebounce = true;
                    }
                    else
                    {
                        _sequenceDebounce = false;
                    }
                }
            }


            if (continuousAim && Mathf.Approximately(_activeTarget, -stowAngle))
            {
                Vector3 aimingPoint;

                if (indicatorAim == null)
                {
                    aimingPoint = _armSec1.transform.position;
                }
                else
                {
                    aimingPoint = indicatorAim.transform.position;
                }
                
                Quaternion targetShooterRotation;
                targetShooterRotation = Quaternion.LookRotation(-aimingPoint + target, Vector3.up);
                
                _position = Quaternion.Angle(_armSec1.transform.rotation, transform.rotation) + angleOffset;

                if (_armSec1.transform.localRotation.eulerAngles.x > 180)
                {
                    _position = -_position;
                }
                if (_position < 0)
                {
                    _position += 360;
                }

                _position = Mathf.Repeat(_position, 360);

                float positionError = (targetShooterRotation.eulerAngles.x - _position);

                if (Mathf.Abs(positionError) > 180)
                {
                    positionError = -1 * positionError;
                }
                _jm.force = 90000000000;
                _jm.targetVelocity = Mathf.Clamp(positionError * 8f, -360,360);
                _hj.useMotor = true;
                _hj.useSpring = false;
                _hj.motor = _jm;
            }
            else
            {
                float Target = _activeTarget;
                if (Target >= 360)
                {
                    Target -= 360;
                }

                if (Target < 0)
                {
                    Target += 360;
                }

                _position = Quaternion.Angle(_armSec1.transform.rotation, transform.rotation);

                if (_armSec1.transform.localRotation.eulerAngles.x > 180)
                {
                    _position = -_position;
                }
                if (_position < 0)
                {
                    _position += 360;
                }

                _position = Mathf.Repeat(_position, 360);
                
                

                float positionError = (Target - _position);

                if (Mathf.Abs(positionError) > 180)
                {
                    positionError = -1 * positionError;
                }
                _jm.force = 90000000000000;
                _jm.targetVelocity = Mathf.Clamp(positionError * 8f, -360,360);
                _hj.useMotor = true;
                _hj.useSpring = false;
                _hj.motor = _jm;
               
            }

        }
    }

    private void Startup()
    {
        _activeTarget = -stowAngle;
        
        _setpointSequence = 0;
        
        if (_playerInput == null)
        {
            var t = transform;
            while (t.GetComponent<PlayerInput>() == null)
            {
                t = t.parent.transform;
            }
            
            if (t.GetComponent<PlayerInput>() != null)
            {
                _playerInput = t.GetComponent<PlayerInput>();

                _inputMap = _playerInput.currentActionMap;
            }
        }

        if (transform.Find("ArmSec1"))
        {
            _armSec1 = transform.Find("ArmSec1").gameObject;

            if (_armSec1.transform.Find("ArmSec1Model"))
            {
                _armSec1Model = _armSec1.transform.Find("ArmSec1Model").gameObject;
            }

            if (_armSec1.GetComponent<Rigidbody>())
            {
                _rb = _armSec1.GetComponent<Rigidbody>();
            }

            if (_armSec1.GetComponent<HingeJoint>())
            {
                _hj = _armSec1.GetComponent <HingeJoint>();
            }
        }
    }
}