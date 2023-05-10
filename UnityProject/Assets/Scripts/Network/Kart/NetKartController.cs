using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

public class NetKartController : NetworkBehaviour
{

    [Space, Header("Suspension")]
    public float SuspensionDistance = 0.2f;
    public float suspensionForce = 30000f;
    public float suspensionDamper = 200f;
    public Transform groundCheck;
    public Transform fricAt;
    public Transform CentreOfMass;
    public Transform turningAt;
    public Transform EngineAt;

    public Rigidbody rb;

    [Space, Header("Kart Stats")]
    public float MaxSpeed = 100f;
    public float speed = 200f;
    public float DownValue = 100f;
    public float turn = 100f;
    public float friction = 70f;
    public float SideFriction = 70f;
    public float dragAngularAmount = 4f;
    public float dragAmount;
    public float TurnAngle = 30f;

    public float maxRayLength = 0.8f, slerpTime = 0.2f;
    [HideInInspector]
    public bool grounded;

    [Space, Header("Visuals")]
    public Transform[] TireMeshes;
    public Transform[] TurnTires;

    [Space, Header("Curves")]
    public AnimationCurve AngularDragCurve;
    public AnimationCurve frictionCurve;
    public AnimationCurve SideFrictionCurve;
    public AnimationCurve speedCurve;
    public bool seperateReverseCurve = false;
    public AnimationCurve ReverseCurve;
    public AnimationCurve turnCurve;
    public AnimationCurve driftCurve;
    public AnimationCurve engineCurve;


    [Header("Item")]
    public NetPlayerInfo npi;

    public float BoostPower;
    public bool isBoost;
    public AnimationCurve BoostCurve;
    public GameObject BoosterVFX;
    [Space]



    private float speedValue, SideFricValue, fricValue, turnValue, curveVelocity, brakeInput;
    [HideInInspector]
    public Vector3 carVelocity;
    [HideInInspector]
    public RaycastHit hit;
    //public bool drftSndMachVel;

    [Header("Other Settings")]
    public AudioSource engineSound;
    public bool airDrag;
    public float UpForce;
    public float SkidEnable = 20f;
    public float skidWidth = 0.12f;
    private float frictionAngle;

    
    [HideInInspector]
    public Vector3 normalDir;

    public NetKartInput input;
    public NetPlayManager npm;

    
    [Header("About Item")] 
    public bool active = true;

    public bool spinningOut = false;


    public bool isProtected = false;
    public Vector3 currentGravityDir = Vector3.up;
    [System.NonSerialized]
    public bool isSpin = false;
    Vector3 spinForward = Vector3.forward;
    Vector3 spinUp = Vector3.up;
    Vector3 spinOffset = Vector3.zero;
    
    private float spinDecel = 100.0f;
    private float spinRate = 25f;
    private float spinHeight = 4.0f;

    
    private void Awake()
    {
        input = GetComponent<NetKartInput>();
        npi = GetComponent<NetPlayerInfo>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        engineSound = GetComponent<AudioSource>();
        grounded = false;
        rb.centerOfMass = CentreOfMass.localPosition;
        StartCoroutine(FindComponent());
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            StartCoroutine(WaitStart());
        }
    }
    IEnumerator FindComponent()
    {
        while (GameObject.Find("@PlayManager") == null)
        {
            yield return null;
        }
        GameObject.Find("@PlayManager").TryGetComponent(out npm);
    }
    void FixedUpdate()
    {
        if (IsOwner) //������ ��� Ŭ���̾�Ʈ �ܿ��� ó��
        {
            carVelocity = transform.InverseTransformDirection(rb.velocity); //local velocity of car

            curveVelocity = Mathf.Abs(carVelocity.magnitude) / 100;

            rb.AddForce(Vector3.up * (-30) * rb.mass);

            //inputs
            float turnInput = turn * input.Hmove * Time.fixedDeltaTime * 1000;
            float speedInput = speed * input.Vmove * Time.fixedDeltaTime * 1000;

            //helping veriables

            speedValue = speedInput * speedCurve.Evaluate(Mathf.Abs(carVelocity.z) / MaxSpeed);
            if (seperateReverseCurve && carVelocity.z < 0 && speedInput < 0)
            {
                speedValue = speedInput * ReverseCurve.Evaluate(Mathf.Abs(carVelocity.z) / MaxSpeed);
            }
            SideFricValue = SideFriction * SideFrictionCurve.Evaluate(Mathf.Abs(carVelocity.x / carVelocity.magnitude));
            fricValue = friction * frictionCurve.Evaluate(Mathf.Abs(carVelocity.magnitude / MaxSpeed));
            //friction * frictionCurve.Evaluate(Mathf.Abs(carVelocity.z / carVelocity.magnitude))+ friction * frictionCurve.Evaluate(Mathf.Abs(carVelocity.x / carVelocity.magnitude));
            turnValue = turnInput * turnCurve.Evaluate(carVelocity.magnitude / 100);

            //grounded check
            if (Physics.Raycast(groundCheck.position, -transform.up, out hit, maxRayLength) || spinningOut)
            {
                if (!spinningOut)
                {
                    accelarationLogic();
                    turningLogic();
                    frictionLogic();
                    brakeLogic();
                    AddAngularDrag();
                    OnDrift();
                }
                else
                {
                    // Visual rotation while spinning out
                    Debug.Log("Spinning");
                    transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.LookRotation(spinForward, spinUp), 20f * Time.fixedDeltaTime);
                    transform.localPosition = Vector3.Lerp(transform.localPosition, transform.localPosition + spinOffset, 20f * Time.fixedDeltaTime);
                    rb.AddForce(new Vector3(-rb.velocity.x, 0.0f, -rb.velocity.z) * spinDecel, ForceMode.Acceleration); // Slow down while spinning out
                }
                //for drift behaviour
                //rb.angularDrag = dragAngularAmount * driftCurve.Evaluate(Mathf.Abs(carVelocity.x) / 70);

                //draws green ground checking ray ....ingnore
                Debug.DrawLine(groundCheck.position, hit.point, Color.green);
                grounded = true;

                //rb.centerOfMass = Vector3.zero;

                normalDir = hit.normal;

                //DownForce
                rb.AddForce(-transform.up * DownValue * carVelocity.magnitude);

                //Non-Slip Code
                if (carVelocity.magnitude < 1)
                {
                    rb.drag = 2;
                }
                else
                {
                    rb.drag = dragAmount;
                }
            }
            else if (!Physics.Raycast(groundCheck.position, -transform.up, out hit, maxRayLength))
            {
                
                grounded = false;
                rb.drag = 1f;
                rb.angularDrag = 10f;
                //rb.centerOfMass = CentreOfMass.localPosition;

            }

            npi.KMH.Value = (int)(carVelocity.magnitude * 2);

        }
    }

    void Update()
    {
        if (IsOwner)
        {
            tireVisuals();
            audioControl();
            
        }
    }

    IEnumerator WaitStart()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
        while (npm.isStart.Value == false)
        {
            yield return null;
        }
        rb.constraints = RigidbodyConstraints.None;
    }

    public void audioControl()
    {
        engineSound.pitch = 2 * engineCurve.Evaluate(curveVelocity);
    }

    public void tireVisuals()
    {
        //Tire mesh rotate
        foreach (Transform mesh in TireMeshes)
        {
            mesh.transform.RotateAround(mesh.transform.position, mesh.transform.right, carVelocity.z / 3);
            mesh.transform.localPosition = Vector3.zero;
        }

        //TireTurn
        foreach (Transform FM in TurnTires)
        {
            FM.localRotation = Quaternion.Slerp(FM.localRotation, Quaternion.Euler(FM.localRotation.eulerAngles.x,
                               TurnAngle * input.Hmove, FM.localRotation.eulerAngles.z), slerpTime);
        }
    }

    public void accelarationLogic()
    {
        //speed control
        if (input.Vmove > 0.1f)
        {
            rb.AddForceAtPosition(transform.forward * speedValue, EngineAt.position);
        }
        if (input.Vmove < -0.1f)
        {
            rb.AddForceAtPosition(transform.forward * speedValue, EngineAt.position);
        }
    }

    public void turningLogic()
    {
        //turning
        if (carVelocity.z > 0.001f)
        {
            //turningAt.
            rb.AddTorque(transform.up * turnValue);
        }
        else if (carVelocity.z < 0.001f)
        {
            rb.AddTorque(transform.up * -turnValue);
        }
    }

    public void OnDrift()
    {
        if (input.Drift)
        {
            //rb.angularDrag = 3f * driftCurve.Evaluate(Mathf.Abs(carVelocity.x) / 70);
            rb.angularDrag = 4f * driftCurve.Evaluate(Mathf.Abs(Mathf.Abs(carVelocity.x) / carVelocity.magnitude));
            //rb.drag = 1f;
        }
        else
        {
            rb.angularDrag = dragAngularAmount * AngularDragCurve.Evaluate(Mathf.Abs(carVelocity.magnitude) / 200);
            //rb.drag = dragAmount;
        }
    }

    public void AddAngularDrag()
    {
        //rb.angularDrag = dragAmount * AngularDragCurve.Evaluate(Mathf.Abs(carVelocity.magnitude) / 100);
    }

    public void frictionLogic()
    {
        //Friction
        if (Mathf.Abs(carVelocity.magnitude) > 0)
        {

            //frictionAngle = (-Vector3.Angle(transform.up, Vector3.up) / 90f) + 1;
            frictionAngle = (-Vector3.Angle(transform.up, Vector3.up) / 90f) + 1;
            /*
            if (!input.Drift)
            {
                //�帮��Ʈ �� �� �� -> �� ���� ������ ����

                //���� ������
                rb.AddForceAtPosition(-carVelocity.normalized * fricValue * ((-Vector3.Angle(EngineAt.transform.up, Vector3.up) / 90f) + 1) * 100 * Mathf.Abs(carVelocity.normalized.x), EngineAt.position);
                //���� ������
                rb.AddForceAtPosition(-carVelocity.normalized * SideFricValue * ((-Vector3.Angle(EngineAt.transform.up, Vector3.up) / 90f) + 1) * 100 * Mathf.Abs(carVelocity.normalized.x), EngineAt.position);
            }
            */
            rb.AddForceAtPosition(-carVelocity.normalized * fricValue * frictionAngle * 100 * Mathf.Abs(carVelocity.normalized.x), fricAt.position);

            /*
            for (int i=0; i<2; i++)
            {
                frictionAngle = (-Vector3.Angle(TireMeshes[i].transform.up, Vector3.up) / 90f) + 1;
                if (!input.Drift && i >= 1)
                {
                    rb.AddForceAtPosition(transform.right * fricValue * frictionAngle * 100 * -carVelocity.normalized.x, turningAt.position);
                }
                else
                {
                    rb.AddForceAtPosition(transform.right * fricValue * frictionAngle * 100 * -carVelocity.normalized.x, EngineAt.position);
                }
            }
            */
            //Origin code
            /*
            frictionAngle = (-Vector3.Angle(transform.up, Vector3.up) / 90f) + 1;
            rb.AddForceAtPosition(transform.right * fricValue * frictionAngle * 100 * -carVelocity.normalized.x, fricAt.position);
        */

        }
    }

    public void brakeLogic()
    {
        //brake
        if (carVelocity.z > 1f)
        {
            rb.AddForceAtPosition(transform.forward * -brakeInput, groundCheck.position);
        }
        if (carVelocity.z < -1f)
        {
            rb.AddForceAtPosition(transform.forward * brakeInput, groundCheck.position);
        }
        /*
        if (carVelocity.z < 1f)
        {
            rb.drag = 5f;
        }
        else
        {
            rb.drag = 0.1f;
        }
        */
    }
    
    //=================== ITEM =======================
    public IEnumerator OnBooster(float BoostTime)
    {
        npi.Item.Value = (int)ITEMS.NONE;

        if (!isBoost)
        {
            isBoost = true;
            float nowBoostTIme = BoostTime;
            float originSpeed = speed;
            speed = BoostPower;
            while (BoostTime > 0)
            {
                float t = Time.fixedDeltaTime;
                BoostTime -= t;
                yield return new WaitForSeconds(t);

            }
            speed = originSpeed;
            yield return new WaitForSeconds(0.1f);
            isBoost = false;
        }
    }

    public IEnumerator OnProtected(float ProtectTime)
    {
        Debug.Log(isProtected);
        if (!isProtected)
        {
            isProtected = true;
            Debug.Log(isProtected);
            float currentProtectTime = ProtectTime;
            while (currentProtectTime > 0)
            {
                currentProtectTime -= Time.fixedDeltaTime;
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            isProtected = false;
            Debug.Log(isProtected);
        }
    }

    // Spin cycle that calculates the current spin angle
    public IEnumerator SpinCycle(int spinType, float spinAmount)
    {
        Debug.Log("Spin Start");
        // Spin start
        spinningOut = true;
        float spinDir = Mathf.Sign(0.5f - Random.value);
        //Mathf.Sign() -> 인자로 들어온 값의 부호 반환
        float curSpin = 0.0f;
        float maxSpin = spinAmount * Mathf.PI * 2.0f;

        // Actual spin cycle
        while (Mathf.Abs(curSpin) < maxSpin)
        {
            curSpin += spinDir * spinRate * Mathf.Clamp((maxSpin - Mathf.Abs(curSpin)), 0.1f, 1.0f) * Time.fixedDeltaTime;
            switch (spinType)
            {
                case (int)SpinAxis.Yaw: //좌우 y축 기준
                    Debug.Log("Spin YAW");
                    spinForward = new Vector3(Mathf.Sin(curSpin), Mathf.Sin(curSpin * 2.0f) * 0.1f, Mathf.Cos(curSpin));
                    spinUp = Vector3.up;
                    break;
                case (int)SpinAxis.Roll: //x축 기준
                    Debug.Log("Spin ROLL");
                    spinUp = new Vector3(Mathf.Sin(curSpin), Mathf.Cos(curSpin), 0.0f);
                    break;
                case (int)SpinAxis.Pitch: //축 기준
                    Debug.Log("Spin PITCH");
                    spinForward = new Vector3(0.0f, Mathf.Sin(curSpin), Mathf.Cos(curSpin));
                    spinUp = new Vector3(0.0f, Mathf.Cos(curSpin), -Mathf.Sin(curSpin));
                    break;
            }

            if (spinType != (int)SpinAxis.Yaw)
            {
                spinOffset = Vector3.up * spinHeight * Mathf.Sin((Mathf.Abs(curSpin) / Mathf.Max(maxSpin, 0.001f)) * Mathf.PI);
            }
            yield return new WaitForFixedUpdate();
        }

        // Spin end
        spinningOut = false;
        spinForward = Vector3.forward;
        spinOffset = Vector3.zero;
        spinUp = Vector3.up;
    }




    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position - maxRayLength * groundCheck.up);
            Gizmos.DrawWireCube(groundCheck.position - maxRayLength * (groundCheck.up.normalized), new Vector3(5, 0.02f, 10));
            Gizmos.color = Color.magenta;
            if (GetComponent<BoxCollider>())
            {
                Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider>().size);
            }
            if (GetComponent<CapsuleCollider>())
            {
                Gizmos.DrawWireCube(transform.position, GetComponent<CapsuleCollider>().bounds.size);
            }



            Gizmos.color = Color.red;
            foreach (Transform mesh in TireMeshes)
            {
                var ydrive = mesh.parent.parent.GetComponent<ConfigurableJoint>().yDrive;
                ydrive.positionDamper = suspensionDamper;
                ydrive.positionSpring = suspensionForce;


                mesh.parent.parent.GetComponent<ConfigurableJoint>().yDrive = ydrive;

                var jointLimit = mesh.parent.parent.GetComponent<ConfigurableJoint>().linearLimit;
                jointLimit.limit = SuspensionDistance;
                mesh.parent.parent.GetComponent<ConfigurableJoint>().linearLimit = jointLimit;

                Handles.color = Color.red;
                //Handles.DrawWireCube(mesh.position, new Vector3(0.02f, 2 * jointLimit.limit, 0.02f));
                Handles.ArrowHandleCap(0, mesh.position, mesh.rotation * Quaternion.LookRotation(Vector3.up), jointLimit.limit, EventType.Repaint);
                Handles.ArrowHandleCap(0, mesh.position, mesh.rotation * Quaternion.LookRotation(Vector3.down), jointLimit.limit, EventType.Repaint);

            }
            float wheelRadius = TurnTires[0].parent.GetComponent<SphereCollider>().radius;
            float wheelYPosition = TurnTires[0].parent.parent.localPosition.y + TurnTires[0].parent.localPosition.y;
            maxRayLength = (groundCheck.localPosition.y - wheelYPosition + (0.05f + wheelRadius));

        }
#endif
    }
}
