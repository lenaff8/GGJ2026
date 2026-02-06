using UnityEngine;

public class GetLight : MonoBehaviour
{
    [SerializeField] private Transform R_light;
    [SerializeField] private Transform G_light;
    [SerializeField] private Transform B_light;
    
    public Transform GetRlight
    {
        get { return R_light; }
    } 
    
    public Transform GetGlight
    {
        get { return G_light; }
    } 
    
    public Transform GetBlight
    {
        get { return B_light; }
    } 
}
