using System;
using UnityEngine;

public class FollowLight : MonoBehaviour
{
    public enum LightType
    {
        R,
        G,
        B
    }

    [SerializeField] private LightType lightType;
    [SerializeField] private float maxDistance;
    [SerializeField] private Collider2D collider;
    
    private bool playerWasInRange;
    private Transform targetLight;

    private void Awake()
    {
        if(lightType == LightType.R)
            targetLight = GameObject.Find("Player").GetComponent<GetLight>().GetRlight;
        if(lightType == LightType.G)
            targetLight = GameObject.Find("Player").GetComponent<GetLight>().GetGlight;
        if(lightType == LightType.B)
            targetLight = GameObject.Find("Player").GetComponent<GetLight>().GetBlight;
    }

    void FixedUpdate()
    {
        if (Vector3.Distance(transform.parent.position, targetLight.position) <= maxDistance)
        {
            transform.SetPositionAndRotation(targetLight.position, targetLight.rotation);
            if (!playerWasInRange)
            {
                playerWasInRange = true;
                collider.enabled = true;
            }
        }
        else if(playerWasInRange)
        {
            playerWasInRange = false;
            collider.enabled = false;
        }
    }
}
