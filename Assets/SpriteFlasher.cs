using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFlasher : MonoBehaviour {

    [SerializeField] Color color = Color.white;
    [SerializeField] float duration = 0.1f;

    SpriteRenderer sprite;
    Shader shaderGUIText;
    Shader shaderSpritesDefault;
    Material materialOriginal;
    Material materialFlashing;
    Coroutine flashRoutine;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        shaderGUIText = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = Shader.Find("Sprites/Default");
        materialOriginal = sprite.material;
        materialFlashing = new Material(materialOriginal);
        materialFlashing.shader = shaderGUIText;
    }

    public void Flash()
    {
        StartCoroutine(FlashRoutine(color, duration));
    }

    // Use this for initialization
    public void Flash(Color color, float duration)
    {
        StartCoroutine(FlashRoutine(color, duration));
    }

    IEnumerator FlashRoutine(Color color, float duration)
    {
        materialFlashing.color = color;
        sprite.material = materialFlashing;
        yield return new WaitForSeconds(duration);
        sprite.material = materialOriginal;
    }
}
