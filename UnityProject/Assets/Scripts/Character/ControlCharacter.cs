using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ControlCharacter : NetworkBehaviour
{
    // Start is called before the first frame update
    NetKartInput _input;
    public Transform root;

    public float zRotate;
    public Vector3 _Rotate = new Vector3(0, 0, 0);
    public NetworkVariable<Vector3> _rootRotate = new NetworkVariable<Vector3>(new Vector3(0, 0, 0), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public float duration = 0.5f;
    public float startTime;

    void Start()
    {
        _input = gameObject.GetComponent<NetKartInput>();
        startTime = Time.time;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        if (IsOwner)
        {
            RotateSide(_input.Hmove);
        }
        else
        {
            root.eulerAngles = _rootRotate.Value;
        }
    }
    

    void RotateSide(float _hMove)
    {
        Debug.Log(_hMove);

        zRotate = Mathf.SmoothStep(zRotate, (-_hMove * 25), duration);
        _Rotate.z = zRotate;
       
        root.transform.localEulerAngles = _Rotate;
        //root.Rotate(_Rotate);
        //_rootRotate.Value = root.eulerAngles;
    }
}
