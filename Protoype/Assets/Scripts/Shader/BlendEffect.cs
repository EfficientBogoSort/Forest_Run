using UnityEngine;
using System.Collections;

public class BlendEffect : MonoBehaviour
{
    public Shader shader;
    public Color blendColor;
    

    void Awake()
    {
        if (this.gameObject.GetComponent<MeshRenderer>() == null)
        {
            // Add a MeshRenderer component if the game object does not have one
            this.gameObject.AddComponent<MeshRenderer>();
        }
    }

    void OnEnable()
    {
        MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();
        renderer.material.shader = this.shader;
        renderer.material.SetColor("_BlendColor", blendColor);
    }

    void Update()
    {
        MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();
        renderer.material.SetFloat("_BlendFct", (Mathf.Sin(Time.time) + 1.0f) / 2.0f);
    }
}
