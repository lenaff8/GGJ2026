using System;
using UnityEngine;

public class ColorReactive : MonoBehaviour
{
    [SerializeField] private bool reactive;
    [SerializeField] private bool hideOnLight;
    [SerializeField] private MyColor requiredColor;
   
    [Header("Do not touch")]
    [SerializeField] private Material colorReactive;
    [SerializeField] private Material  colorReactiveHide;
    [SerializeField] private Collider2D R,G,B,H;
    
    private Color[] colors =
    {
        Color.red,
        Color.green,
        Color.blue,
        Color.cyan,
        Color.magenta,
        Color.yellow,
        Color.white
    };
    
    private enum MyColor
    {
        R = 0,
        G,
        B,
        C,
        M,
        Y,
        W
    }
    
    private void Awake()
    {
        if (reactive)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if(hideOnLight)
                sr.material = colorReactiveHide;
            else
                sr.material = colorReactive;
            Material mat = sr.material;
            mat.SetColor("_RequiredColor", colors[(int)requiredColor]);
            
            R.GetComponent<FollowLight>().enabled = true;
            G.GetComponent<FollowLight>().enabled = true;
            B.GetComponent<FollowLight>().enabled = true;
            switch (requiredColor)
            {
                case MyColor.R:
                    R.compositeOperation = Collider2D.CompositeOperation.Intersect;
                    break;
                case MyColor.G:
                    G.compositeOperation = Collider2D.CompositeOperation.Intersect;
                    break;
                case MyColor.B:
                    B.compositeOperation = Collider2D.CompositeOperation.Intersect;
                    break;
                case MyColor.C:
                    G.compositeOperation = Collider2D.CompositeOperation.Intersect;
                    B.compositeOperation = Collider2D.CompositeOperation.Intersect;
                    break;
                case MyColor.M:
                    R.compositeOperation = Collider2D.CompositeOperation.Intersect;
                    B.compositeOperation = Collider2D.CompositeOperation.Intersect;
                    break;
                case MyColor.Y: 
                    R.compositeOperation = Collider2D.CompositeOperation.Intersect;
                    G.compositeOperation = Collider2D.CompositeOperation.Intersect;
                    break;
                case MyColor.W: 
                    R.compositeOperation = Collider2D.CompositeOperation.Intersect;
                    G.compositeOperation = Collider2D.CompositeOperation.Intersect;
                    B.compositeOperation = Collider2D.CompositeOperation.Intersect;
                    break;
            }
            if(H != null)
                H.enabled = hideOnLight;
        }
    }
    
    
}
