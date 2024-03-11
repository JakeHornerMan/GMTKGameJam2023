using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RerollButton : MonoBehaviour
{
    [SerializeField] private Sprite[] diceSprites; // Assign these in the inspector

    private SpriteRenderer spriteRenderer;
    private Image image;

    private Button button;

    private void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
    }

    public void SetDiceFace(int number)
    {
        if (number >= 1 && number <= 6)
        {
            image.sprite = diceSprites[number];
        }
        else
        {
            Debug.LogError("Invalid dice number");
        }
    }


    public void DisableDice()
    {
        button.enabled = false;

        image.sprite = diceSprites[0];

        image.color = new Vector4(image.color.r, image.color.g, image.color.b, 0.5f);
    }

}
