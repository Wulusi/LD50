using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingSmog : MonoBehaviour
{
    public float scroll_speed;
    public Renderer background_renderer;

    // Update is called once per frame
    void Update()
    {
        background_renderer.material.mainTextureOffset += new Vector2(scroll_speed * Time.deltaTime, 0f);
    }
}
