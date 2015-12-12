using UnityEngine;
using System.Collections;

public class LoadOnClick : MonoBehaviour {

    // // Use this for initialization
    // void Start () {
    
    // }
    
    // // Update is called once per frame
    // void Update () {
    
    // }

    // public GameObject loadingImage;

    public void LoadScene(int level) {
        // loadingImage.SetActive(true);
        Application.LoadLevel(level);
    }
}
