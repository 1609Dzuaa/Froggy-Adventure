using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class ImageLoader : BaseSingleton<ImageLoader>
{
    public static IEnumerator LoadImageFromPath(Image target, string path)
    {
        // Load the image from file as bytes
        byte[] imageBytes = File.ReadAllBytes(path);

        // Create a Texture2D and load image data into it
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(imageBytes);

        // Create a Sprite from the Texture2D
        Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        // Assign the Sprite to the UI Image component
        target.sprite = newSprite;

        yield return null;
    }
}
