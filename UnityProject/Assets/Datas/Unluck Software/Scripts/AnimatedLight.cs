namespace UnluckSoftware
{
	using UnityEngine;
	[ExecuteInEditMode]
	public class AnimatedLight :MonoBehaviour
	{
		public Light targetLight;
		
		public AnimationCurve lightIntensity = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
		public float intensitySpeed = 3;
		public float intensityMultiplier = 7;

		public AnimationCurve rangeIntensity = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
		public float rangeSpeed = 3;
		public float rangeMultiplier = 7;

		public bool cycleGradient;
		public Gradient gradientColor = new Gradient();
		public float gradientSpeed = 3;

		void Start()
		{
			if (targetLight == null) targetLight = GetComponent<Light>();
			lightIntensity.postWrapMode = WrapMode.Loop;
			rangeIntensity.postWrapMode = WrapMode.Loop;
		}

		void Update()
		{
			if (targetLight == null) return;
			UpdateIntensity();
			UpdateRange();
			UpdateGradient();
		}

		void UpdateGradient()
		{
			if (!cycleGradient) return;
			targetLight.color = gradientColor.Evaluate(Time.time * gradientSpeed %1);
		}

		void UpdateIntensity()
		{
			targetLight.intensity = lightIntensity.Evaluate(Time.time * intensitySpeed) * intensityMultiplier;
		}

		void UpdateRange()
		{
			targetLight.range = rangeIntensity.Evaluate(Time.time * rangeSpeed) * rangeMultiplier;
		}
	}
}