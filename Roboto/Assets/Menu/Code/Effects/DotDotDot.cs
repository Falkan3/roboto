using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DotDotDot : MonoBehaviour {

    private Text textObject;
    private string text;
    private string dots;
    private int iterator = 0;

    void Start () {
        textObject = GetComponent<Text>();
        text = textObject.text;
        textObject.text = text;
        StartCoroutine(AppendDots());
    }

    IEnumerator AppendDots()
    {
        while (true)
        {
            if (iterator <= 0)
            {
                dots = "";
                iterator = 3;
            }
            else
            {
                dots += ". "; iterator--;
            }
            textObject.text = text + dots;
            yield return new WaitForSeconds(1);
        }
    }
}
