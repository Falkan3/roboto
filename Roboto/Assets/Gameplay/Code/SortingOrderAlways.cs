using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingOrderAlways : MonoBehaviour {
    private Renderer objRenderer;

    void Start()
    {
        objRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        objRenderer.sortingOrder = (int)(transform.position.y * -10);
    }
}
