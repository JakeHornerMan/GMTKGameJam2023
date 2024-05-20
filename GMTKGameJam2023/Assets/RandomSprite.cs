using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Applies a random sprite to attached object's Sprite Renderer.
/// Chooses from a list of possible sprites randomly.
/// </summary>
public class RandomSprite : MonoBehaviour
{
    [Header("Random Sprite Values")]
    [SerializeField] private List<Sprite> possibleSprites = new();

    private SpriteRenderer spriteRenderer;

    private System.Random random = new();

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (spriteRenderer != null)
        {
            int randomIndex = random.Next(possibleSprites.Count);
            spriteRenderer.sprite = possibleSprites[randomIndex];
        }
    }
}
