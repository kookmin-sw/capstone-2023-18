using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleBalence : MonoBehaviour
{
	public Rigidbody vehicleBody;
	public float LeanTorqueAmount;
	private float leanAngle;
	public AnimationCurve BalanceTorqueCurve;
	public bool AlwaysAddTorque;


	private KartController kartController;

	Vector3 normalDir;

	private void Start()
	{
		kartController = vehicleBody.GetComponent<KartController>();
	}

	void FixedUpdate()
	{
		Balence();
	}


	void Balence()
	{
		normalDir = kartController.normalDir;

		Vector3 projectedBikeUpDir = Vector3.ProjectOnPlane(vehicleBody.transform.up, Vector3.Cross(normalDir, vehicleBody.transform.right));

		leanAngle = Vector3.SignedAngle(normalDir, projectedBikeUpDir, Vector3.Cross(vehicleBody.transform.right, normalDir));

		Vector3 LeanTorque = new Vector3(0, 0, 100 * LeanTorqueAmount
			* (-leanAngle / 90)
			* BalanceTorqueCurve.Evaluate(Mathf.Abs(kartController.carVelocity.z)));

		if (AlwaysAddTorque)
		{
			vehicleBody.AddRelativeTorque(LeanTorque);
		}
		else if (kartController.grounded)
		{
			vehicleBody.AddRelativeTorque(LeanTorque);
		}
	}
}
