using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer backgroundRenderer;

    public void SetBackground(string backgroundName)
    {
        Sprite bgSprite = Resources.Load<Sprite>("Sprites/Backgrounds/" + backgroundName);
        if (bgSprite == null)
        {
            // Try to load default background if specific one not found
            bgSprite = Resources.Load<Sprite>("Sprites/Backgrounds/1");
            if (bgSprite == null)
            {
                return; // No background found, keep current one
            }
        }

        backgroundRenderer.sprite = bgSprite;
        FitBackgroundToCamera(backgroundRenderer);
    }

    private void FitBackgroundToCamera(SpriteRenderer bgRenderer)
    {
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        Sprite sprite = bgRenderer.sprite;
        float spriteWidth = sprite.bounds.size.x;
        float spriteHeight = sprite.bounds.size.y;

        bgRenderer.transform.localScale = new Vector3(
            width / spriteWidth,
            height / spriteHeight,
            1f
        );
    }

    // Test
}
