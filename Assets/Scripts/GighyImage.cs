using UnityEngine;

[System.Serializable]
public class GiphyImage : MonoBehaviour
{
    public string source;
    public string title;
    public string rating;
    
    public static GiphyImage CreateFromJson(string jsonInput) {
        return JsonUtility.FromJson<GiphyImage>(jsonInput);
    }

}