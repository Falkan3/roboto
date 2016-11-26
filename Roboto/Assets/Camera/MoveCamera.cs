using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoveCamera : MonoBehaviour {
    public int speed;
    private bool disabled = false;
    Vector2 moveVector;

    public float scrollsensitivity;

    //private GameObject mainInputField;

    // Use this for initialization
    void Start () {
        //mainInputField = GameObject.Find("InputField");
    }
	
	// Update is called once per frame
	void FixedUpdate() {
        float input_x = Input.GetAxisRaw("Horizontal");
        float input_y = Input.GetAxisRaw("Vertical");

        bool isWalking = (Mathf.Abs(input_x) + Mathf.Abs(input_y)) > 0;

        if (isWalking && disabled==false)
        {
            /*
            if (mainInputField.GetComponent<InputField>().isFocused == false)
            {
                moveVector = new Vector2(input_x, input_y).normalized * speed * Time.deltaTime;
                transform.Translate(moveVector);
            }
            */
            moveVector = new Vector2(input_x, input_y).normalized * speed * Time.deltaTime;
            transform.Translate(moveVector);
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
}
