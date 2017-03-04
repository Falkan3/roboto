using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour {
    bool isOn = true;
    Collider2D[] collidedItems = { };
    short tileRotation;
    Vector2 forceToApply;
    public float forceMultiplier = 10f;

    public LayerMask layerMask;

    #region inits
    public bool IsOn
    {
        get
        {
            return isOn;
        }

        set
        {
            isOn = value;
        }
    }

    public short TileRotation
    {
        get
        {
            return tileRotation;
        }

        set
        {
            tileRotation = value;
        }
    }
    #endregion

    void Start ()
    {
        switch (TileRotation) {
            case 0:
                forceToApply = new Vector2(1 * forceMultiplier, 0);
                break;
            case 1:
                forceToApply = new Vector2(0, 1 * forceMultiplier);
                break;
            case 2:
                forceToApply = new Vector2(-1* forceMultiplier, 0);
                break;
            case 3:
                forceToApply = new Vector2(0, -1* forceMultiplier);
                break;
        }
	}
	
	void Update ()
    {
        if (IsOn)
        {
            detect(transform.position, 0.5f);
        }
    }

    void FixedUpdate()
    {
        for(int i = 0; i<collidedItems.Length; i++)
        {
            Rigidbody2D temp = collidedItems[i].GetComponent<Rigidbody2D>();
            if(temp != null)
            {
                temp.AddForce(forceToApply);
            }
               
        }
    }

    void detect(Vector3 center, float radius)
    {
        collidedItems = Physics2D.OverlapCircleAll(center, radius, layerMask);
    }
}
