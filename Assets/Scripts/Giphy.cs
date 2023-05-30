using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Giphy : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject gifCardUIPrefab;
    [SerializeField] string giphyAPIKey;
    [SerializeField] string giphyAPIBaseUrl;

    List<GameObject> gifs;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void loadGifs() 
    {
        Debug.Log("getting gifs");
        
        StartCoroutine(giphyGetRandom());
    }   

    IEnumerator giphyGetRandom()
    {
        // https://api.giphy.com/v1/gifs/random?api_key=Si5ViEEb7tiGsg74eWmCUJYRsvipGFOo&tag=&rating=g
        string responseText = "";
        string endpoint = giphyAPIBaseUrl + "random?";
        endpoint += "api_key=" + giphyAPIKey;

        UnityWebRequest www = UnityWebRequest.Get(endpoint);
        yield return www.SendWebRequest();
 
        if (www.result != UnityWebRequest.Result.Success) {
            Debug.Log(www.error);
        }
        else {
            // Show results as text
            displayImages(www.downloadHandler.text);
            // Debug.Log(responseText);
 
            // Or retrieve results as binary data
            // byte[] results = www.downloadHandler.data;
        }
    }

    void displayImages(string text) 
    {
        string res = JsonUtility.FromJson<string>(text);
        Debug.Log("display images: " + res);

    }

  
}
