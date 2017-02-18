using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingOrderOnlyStart : MonoBehaviour {
    private Renderer objRenderer;

    void Start()
    {
        objRenderer = GetComponent<Renderer>();
        objRenderer.sortingOrder = (int)(transform.position.y * -10);
    }
}
