using UnityEngine;

public class FollowLight : MonoBehaviour
{
    [SerializeField] private Transform targetLight;
    [SerializeField] private float maxDistance;
    [SerializeField] private Collider2D collider;
    
    private bool playerWasInRange;
    
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
