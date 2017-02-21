using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LoadingSegmentManager : MonoBehaviour {

    private GameObject panel;
    public RawImage segmentObj;
    private int numberOfSegments;
    private List<RawImage> listOfSegments = new List<RawImage>();

    float height;
    float width;

    int index = 0;

    // Use this for initialization
    void Start () {
        panel = GameObject.Find("LoadingPanel");

        height = 2f * Camera.main.orthographicSize;
        width = height * Camera.main.aspect;

        numberOfSegments = Mathf.FloorToInt(width-60 / (segmentObj.rectTransform.rect.width + 30));
        //Debug.Log(width + " segment width: " + segmentObj.rectTransform.rect.width + " capacity: " + numberOfSegments);

        initializeSegments();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    private void initializeSegments()
    {
        for(int i =0; i<numberOfSegments; i++)
        {
            RawImage go = Instantiate(segmentObj, new Vector2(transform.position.x + 30 + i * 45, transform.position.y), transform.rotation) as RawImage;
            go.transform.SetParent(panel.transform, false);
            listOfSegments.Add(go);
        }

        StartCoroutine(StartFadingIn());
    }

    IEnumerator StartFadingIn()
    {
        while (true)
        {
            if (index < numberOfSegments - 1)
            {
                listOfSegments[index].GetComponent<UIFade>().FadeIn();
                index++;
            }
            else
            {
                index = 0;
                StartCoroutine(StartFadingOut());
                yield break;
            }
                

            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator StartFadingOut()
    {
        while (true)
        {
            if (index < numberOfSegments - 1)
            {
                listOfSegments[index].GetComponent<UIFade>().FadeOut();
                index++;
            }
            else
            {
                index = 0;
                StartCoroutine(StartFadingIn());
                yield break;
            }


            yield return new WaitForSeconds(0.1f);
        }
    }
}
