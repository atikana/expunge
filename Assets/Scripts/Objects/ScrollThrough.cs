using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollThrough : MonoBehaviour
{

    bool inRange;
    Object lastSelect;


    private void Awake()
    {
        lastSelect = GetComponent<Object>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Scroll();
    }

    private void LateUpdate()
    {
        
    }

    public void LookAt(bool b)
    {
        if (b)
        {
            inRange = true;
        }
        else
        {
            inRange = false;
            lastSelect = GetComponent<Object>();
        }

    }

    private void Scroll()
    {
        bool found = false;
        if (Input.GetKeyDown(KeyCode.Tab)) // forward
        {
            // loop through the child of the object LUL?
            Debug.Log("forward");
            if (lastSelect.GetObjectType() == Object.ObjectType.Inactive)
            {
                for (int i = 0; i < lastSelect.transform.childCount; i++)
                {
                    Transform t = lastSelect.transform.GetChild(i);

                    if (t.GetComponent<Object>() != null)
                    {
                        found = true;
                        lastSelect = t.GetComponent<Object>();
                        break;
                    }
                }
            }

            if (!found)
            {
                if (lastSelect.transform.parent != null  && lastSelect.transform.parent.GetComponent<Object>() != null)
                {
                    lastSelect = GetComponent<Object>();
                }
            }

            Debug.Log(gameObject.name + "  script  " + lastSelect.name);


        }
        else if (Input.GetKeyDown(KeyCode.LeftShift)) // backward
        {
            Debug.Log("backward");
            if (lastSelect.transform.parent != null && lastSelect.transform.parent.GetComponent<Object>() != null)
            {
               
                lastSelect = lastSelect.transform.parent.gameObject.GetComponent<Object>();
            }
            else
            {
                // find the last inner object that is not select lul
                while (true)
                {
                    if (lastSelect.GetObjectType() == Object.ObjectType.Inactive)
                    {
                        for (int i = 0; i < lastSelect.transform.childCount; i++)
                        {
                            Transform t = lastSelect.transform.GetChild(i);

                            if (t.GetComponent<Object>() != null)
                            {
                                found = true;
                                lastSelect = t.GetComponent<Object>();
                                break;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                
            }

            Debug.Log(gameObject.name + "  script  " + lastSelect.name);
        }

        
    }
}
