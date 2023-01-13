using UnityEngine;

public class FixCameraAspectRatio : MonoBehaviour
{
	public delegate void OnAspectRatioFixed();
	public OnAspectRatioFixed onAspectRatioFixed;

	public int width;
	public int height;

	private float _desiredAspectRatio;
	Vector2 resolution;


#if UNITY_EDITOR
	// Update is called once per frame
	void Update()
	{

		if (resolution.x != Screen.width || resolution.y != Screen.height)
		{

			FixAspectRatio();

			resolution.x = Screen.width;
			resolution.y = Screen.height;
		}

	}
#endif
	void Awake()
	{
		_desiredAspectRatio = width / (float)height;

		resolution.x = Screen.width;
		resolution.y = Screen.height;

		FixAspectRatio();
	}

	private void FixAspectRatio()
	{
		float currentAspectRatio = Camera.main.pixelWidth / (float)Camera.main.pixelHeight;

		if (currentAspectRatio < _desiredAspectRatio)
		{
			float supposedHeight = width / currentAspectRatio;
			float newSize = Camera.main.orthographicSize * supposedHeight / height;

			Camera.main.orthographicSize = newSize;
		}

		onAspectRatioFixed?.Invoke();
	}
}
