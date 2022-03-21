using UnityEngine;

public class OutlineScript : MonoBehaviour
{
    [SerializeField] private Material outlineMaterial = null;
    [SerializeField] private float outlineScaleFactor = 0.0f;
    [SerializeField] private Color outlineColor = Color.yellow;
    [SerializeField] GameObject _artObject = null;
    private Renderer outlineRenderer;
    private GameObject outlineObject;
    private Renderer rend;

    void Start()
    {
        outlineObject = Instantiate(_artObject, transform);
        rend = outlineObject.GetComponent<Renderer>();
        outlineRenderer = CreateOutline(outlineMaterial, outlineScaleFactor, outlineColor);
        outlineRenderer.enabled = true;
    }

    //private void Update()
    //{
        
    //}

    Renderer CreateOutline(Material outlineMat, float scaleFactor, Color color)
    {
        rend.material = outlineMat;
        rend.material.SetColor("_OutlineColor", color);
        rend.material.SetFloat("_Scale", scaleFactor);
        rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        //outlineObject.GetComponent<OutlineScript>().enabled = false;
        //outlineObject.GetComponent<Collider>().enabled = false;

        rend.enabled = false;

        return rend;
    }
}