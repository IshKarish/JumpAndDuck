using UnityEngine;

public static class SkinsManager
{
    public static void SaveSkin(Texture2D photo)
    {
        byte[] imageBytes = photo.EncodeToPNG();
        string base64Str = System.Convert.ToBase64String(imageBytes);

        PlayerPrefs.SetString("Skin", base64Str);
        PlayerPrefs.Save();
    }

    private static Texture2D LoadSkinTexture2D()
    {
        string base64Str = PlayerPrefs.GetString("Skin");
        if (string.IsNullOrEmpty(base64Str)) return null;

        byte[] imageBytes = System.Convert.FromBase64String(base64Str);

        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(imageBytes);

        return texture;
    }

    public static Sprite LoadSkinSprite()
    {
        Texture2D texture = LoadSkinTexture2D();
        if (!texture) return null;

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        return sprite;
    }

    public static void RemoveSkin()
    {
        PlayerPrefs.DeleteKey("Skin");
    }
}
