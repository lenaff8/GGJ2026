using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
public class LightAccumulator : MonoBehaviour
{
    [SerializeField] private RenderTexture lightRT;

    [SerializeField] private List<Material> platformMaterials;

    private Camera lightCam;

    private void Awake()
    {
        lightCam = GetComponent<Camera>();
        
        if (lightCam != null)
        {
            lightCam.targetTexture = lightRT;
            lightCam.clearFlags = CameraClearFlags.SolidColor;
            lightCam.backgroundColor = Color.black;
            lightCam.cullingMask = LayerMask.GetMask("Light"); 
            lightCam.orthographic = true;
        }

        foreach (var mat in platformMaterials)
        {
            mat.SetTexture("_LightMap", lightRT);
        }
    }

    private void Update()
    {
        Vector2 camMin = new Vector2(
            lightCam.transform.position.x - lightCam.orthographicSize * lightCam.aspect,
            lightCam.transform.position.y - lightCam.orthographicSize
        );

        Vector2 camSize = new Vector2(
            2f * lightCam.orthographicSize * lightCam.aspect,
            2f * lightCam.orthographicSize
        );

        foreach (var mat in platformMaterials)
        {
            mat.SetVector("_CameraMin", camMin);
            mat.SetVector("_CameraSize", camSize);
            mat.SetTexture("_LightMap", lightRT);
        }
    }

    public void ClearRT()
    {
        RenderTexture active = RenderTexture.active;
        RenderTexture.active = lightRT;
        GL.Clear(true, true, Color.black);
        RenderTexture.active = active;
    }
}
