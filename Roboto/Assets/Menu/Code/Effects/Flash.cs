using UnityEngine;
using System.Collections;

public class Flash : MonoBehaviour {

    Color textureColor;
    private float duration = 1.0f;

    void Start () {
        textureColor = GetComponent<Renderer>().material.color;
    }
	
	void Update () {
        textureColor.a = Mathf.PingPong(Time.time, duration) / duration;
        GetComponent<Renderer>().material.color = textureColor;
    }
}
