using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Renderer[] Layers;
    public float BaseSpeed = 0.0525f;

    public ParallaxCam Target;

    void Start()
    {
        Target.OnMovement += UpdateLayers;
    }


    private void UpdateLayers(Vector3 movement)
    {
        for (int i = 0; i < Layers.Length; ++i)
        {
            Material m = Layers[i].material;
            Vector2 movement2D = new Vector2(movement.x, 0.0f);
            m.SetTextureOffset("_MainTex", m.GetTextureOffset("_MainTex") + (BaseSpeed * movement2D / (i + 1.0f)));
        }
    }
}
