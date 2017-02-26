using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoveCameraManually : MonoBehaviour {
    public int speed;
    private bool disabled = false;
    Vector2 moveVector;
    private InputField[] listOfUiControls;

    public float scrollsensitivity;

    private float input_x;
    private float input_y;

    //private GameObject mainInputField;

    // Use this for initialization
    void Start () {
        //mainInputField = GameObject.Find("InputField");
        listOfUiControls = GameObject.FindObjectsOfType(typeof(InputField)) as InputField[];
    }
	
    void Update()
    {
        input_x = Input.GetAxisRaw("Horizontal");
        input_y = Input.GetAxisRaw("Vertical");
    }

	// Update is called once per frame
	void FixedUpdate() {
        bool isWalking = (Mathf.Abs(input_x) + Mathf.Abs(input_y)) > 0;

        if (isWalking && disabled==false)
        {
            if (checkIfUiHasFocus())
            {
                moveVector = new Vector2(input_x, input_y).normalized * Time.fixedDeltaTime * speed;
                transform.Translate(moveVector);
            }
        }

        if (!EventSystem.current.IsPointerOverGameObject())
            Camera.main.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * scrollsensitivity;
    }

    public void disableMovement()
    {
        disabled = true;
    }

    public void enableMovement()
    {
        disabled = false;
    }

    public bool checkIfUiHasFocus()
    {
        foreach(InputField item in listOfUiControls)
        {
            if (item.isFocused)
                return false;
        }
        return true;
    }
}
