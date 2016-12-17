using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoveCamera : MonoBehaviour {
    public int speed;
    private bool disabled = false;
    Vector2 moveVector;
    private InputField[] listOfUiControls;

    public float scrollsensitivity;

    //private GameObject mainInputField;

    // Use this for initialization
    void Start () {
        //mainInputField = GameObject.Find("InputField");
        listOfUiControls = GameObject.FindObjectsOfType(typeof(InputField)) as InputField[];
    }
	
	// Update is called once per frame
	void FixedUpdate() {
        float input_x = Input.GetAxisRaw("Horizontal");
        float input_y = Input.GetAxisRaw("Vertical");

        bool isWalking = (Mathf.Abs(input_x) + Mathf.Abs(input_y)) > 0;

        if (isWalking && disabled==false)
        {
            if (checkIfUiHasFocus())
            {
                moveVector = new Vector2(input_x, input_y).normalized * speed * Time.deltaTime;
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
