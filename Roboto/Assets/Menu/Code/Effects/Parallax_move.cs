using System;
using UnityEngine;
using UnityEngine.UI;

public class Parallax_move : MonoBehaviour
{
    public GameObject[] panels;
    public ParallaxObject[] objArray;

    [Serializable]
    public class ParallaxObject
    {
        [SerializeField]
        private GameObject gameObj;
        private RectTransform rt;
        private GameObject duplicateGameObj;
        private RectTransform duplicateGameObjRt;
        [SerializeField]
        private float speed = 0.2f;

        public ParallaxObject(GameObject gameObj)
        {
            this.gameObj = gameObj;
            this.Rt = gameObj.GetComponent<RectTransform>();
        }

        public GameObject GameObj
        {
            get
            {
                return gameObj;
            }

            set
            {
                gameObj = value;
            }
        }

        public RectTransform Rt
        {
            get
            {
                return rt;
            }

            set
            {
                rt = value;
            }
        }

        public GameObject DuplicateGameObj
        {
            get
            {
                return duplicateGameObj;
            }

            set
            {
                duplicateGameObj = value;
            }
        }

        public RectTransform DuplicateGameObjRt
        {
            get
            {
                return duplicateGameObjRt;
            }

            set
            {
                duplicateGameObjRt = value;
            }
        }

        public float Speed
        {
            get
            {
                return speed;
            }

            set
            {
                speed = value;
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        for(int i = 0; i<objArray.Length;i++)
        {
            objArray[i].Rt = objArray[i].GameObj.GetComponent<RectTransform>();
            objArray[i].DuplicateGameObj = new GameObject(objArray[i].GameObj.name + "_duplicate");
            objArray[i].DuplicateGameObj.layer = gameObject.layer;
            objArray[i].DuplicateGameObjRt = objArray[i].DuplicateGameObj.AddComponent<RectTransform>();
            objArray[i].DuplicateGameObj.AddComponent<CanvasRenderer>();
            objArray[i].DuplicateGameObj.AddComponent<RawImage>();
            objArray[i].DuplicateGameObj.GetComponent<RawImage>().texture = objArray[i].GameObj.GetComponent<RawImage>().texture;
            objArray[i].DuplicateGameObj.transform.SetParent(objArray[i].GameObj.transform.parent, false);
            objArray[i].DuplicateGameObjRt.anchorMin = new Vector2(0, 0);
            objArray[i].DuplicateGameObjRt.anchorMax = new Vector2(1, 1);
            objArray[i].DuplicateGameObjRt.offsetMin = new Vector2(objArray[i].DuplicateGameObjRt.offsetMin.x, 0);
            objArray[i].DuplicateGameObjRt.offsetMax = new Vector2(objArray[i].DuplicateGameObjRt.offsetMax.x, 0);
            objArray[i].DuplicateGameObjRt.localScale = objArray[i].Rt.localScale;

            objArray[i].GameObj.transform.SetAsLastSibling();
            objArray[i].DuplicateGameObj.transform.SetAsLastSibling();

            objArray[i].DuplicateGameObjRt.localPosition = new Vector3(objArray[i].Rt.rect.xMin - objArray[i].DuplicateGameObjRt.rect.width / 2 + 2, 0);
        }
        //Move panel in hierarchy
        for(int i=0; i<panels.Length; i++)
        {
            panels[i].transform.SetAsLastSibling();
        }
        
        //
    }

    void OnGUI()
    {
        for (int i = 0; i < objArray.Length; i++)
        {
            objArray[i].Rt.localPosition += new Vector3(objArray[i].Speed * Time.deltaTime, 0);
            objArray[i].DuplicateGameObjRt.localPosition += new Vector3(objArray[i].Speed * Time.deltaTime, 0);

            if (objArray[i].Rt.localPosition.x > objArray[i].Rt.rect.width)
            {
                objArray[i].Rt.localPosition = new Vector3(0 - objArray[i].Rt.rect.width, 0);
            }
            if (objArray[i].DuplicateGameObjRt.localPosition.x > objArray[i].DuplicateGameObjRt.rect.width)
            {
                objArray[i].DuplicateGameObjRt.localPosition = new Vector3(objArray[i].Rt.rect.xMin - objArray[i].DuplicateGameObjRt.rect.width / 2 + 2, 0);
            }
        }
    }
}