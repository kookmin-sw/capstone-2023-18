namespace UnluckSoftware
{
	using UnityEngine;

	[ExecuteInEditMode]
	public class MoveInACircle :MonoBehaviour

	{
		public float rotateSpeed = 1f;
		public float moveSpeed = 0f;

		void Update()
		{

			transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
			transform.position += transform.forward * Time.deltaTime * moveSpeed;


		}
	}
}

