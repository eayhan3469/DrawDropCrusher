using UnityEngine;

public class CameraFixedAspectRatio : MonoBehaviour
{
    #region inspector properties
    [SerializeField]
    private float wantedAspectRatio = 1.5f;
    [SerializeField]
    private Color fillColor = Color.black;
    #endregion

    private Camera myCamera;
    private Camera backgroundCamera;

    void Awake()
    {
        myCamera = GetComponent<Camera>();
    }

    void Start()
    {
        // Set camera to our desired aspect ratio
        float currAspectRatio = (float)Screen.height / Screen.width;

        if (!Mathf.Approximately(currAspectRatio, wantedAspectRatio))
        {
            if (currAspectRatio < wantedAspectRatio)
            {
                // Pillarbox
                float padding = 1f - currAspectRatio / wantedAspectRatio;
                myCamera.rect = new Rect(padding / 2f, 0f, 1f - padding, 1f);
            }
            else
            {
                // Letterbox
                float padding = 1f - wantedAspectRatio / currAspectRatio;
                myCamera.rect = new Rect(0, padding / 2f, 1f, 1f - padding);
            }

            // Add a secondary, background camera just to paint the background black when letterboxing or pillarboxing 
            // is enabled.
            if (!GameObject.Find("BackgroundCam"))
            {
                backgroundCamera = new GameObject("BackgroundCam", typeof(Camera)).GetComponent<Camera>();
                backgroundCamera.depth = int.MinValue;
                backgroundCamera.clearFlags = CameraClearFlags.SolidColor;
                backgroundCamera.backgroundColor = fillColor;
                backgroundCamera.cullingMask = 0;
            }
        }
    }

}