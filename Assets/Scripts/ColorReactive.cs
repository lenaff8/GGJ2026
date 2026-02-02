using UnityEngine;

public class ColorReactive : MonoBehaviour
{
    public Color requiredColor = Color.red;
    public Texture overrideMainTexture; 

    private void Awake()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        Material mat = sr.material;

        mat.SetColor("_RequiredColor", requiredColor);
        mat.SetTexture("_MainTex", overrideMainTexture);
    }
}
