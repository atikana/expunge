using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollThrough : MonoBehaviour
{

    bool inRange;
    Object lastSelect;

    PlayerController playerController;


    private void Awake()
    {

        lastSelect = GetComponent<Object>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Scroll();
        ClickObject();
        PickUp();
    }

    private void LateUpdate()
    {

    }

    public void LookAt(bool b)
    {
        if (b)
        {
            inRange = true;
            if (lastSelect.gameObject == gameObject)
            {
                lastSelect.SelectObject(true);
            }
        }
        else
        {
            inRange = false;
            lastSelect.SelectObject();
            lastSelect = GetComponent<Object>();
        }

    }

    private void Scroll()
    {
        bool found = false;
        if (Input.GetKeyDown(KeyCode.Tab)) // forward
        {
            // loop through the child of the object LUL?
            if (lastSelect.GetObjectType() == Object.ObjectType.Inactive)
            {
                for (int i = 0; i < lastSelect.transform.childCount; i++)
                {
                    Transform t = lastSelect.transform.GetChild(i);

                    if (t.GetComponent<Object>() != null)
                    {
                        found = true;
                        lastSelect.SelectObject();
                        lastSelect = t.GetComponent<Object>();
                        lastSelect.SelectObject(true);
                        break;
                    }
                }
            }

            if (!found)
            {
                if (lastSelect.transform.parent != null && lastSelect.transform.parent.GetComponent<Object>() != null)
                {
                    lastSelect.SelectObject();
                    lastSelect = GetComponent<Object>();
                    lastSelect.SelectObject(true);
                }
            }

        }
        else if (Input.GetKeyDown(KeyCode.LeftShift)) // backward
        {
            if (lastSelect.transform.parent != null && lastSelect.transform.parent.GetComponent<Object>() != null)
            {
                lastSelect.SelectObject();
                lastSelect = lastSelect.transform.parent.gameObject.GetComponent<Object>();
                lastSelect.SelectObject(true);
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
                                lastSelect.SelectObject();
                                lastSelect = t.GetComponent<Object>();
                                lastSelect.SelectObject(true);
                                break;
                            }
                        }

                        if (lastSelect.transform.childCount == 0)
                        {
                            break;
                        }


                    }
                    else
                    {
                        break;
                    }
                }


            }
        }


    }

    private void ClickObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // select last click
            lastSelect.ChangeObjectType();

            if (lastSelect.GetObjectType() == Object.ObjectType.Active)
            {
                // move up to the parent

                if (lastSelect.transform.parent != null && lastSelect.transform.parent.GetComponent<Object>() != null)
                {
                    lastSelect.SelectObject();
                    lastSelect = lastSelect.transform.parent.gameObject.GetComponent<Object>();
                    lastSelect.SelectObject(true);
                }
            }
            else
            {

                // move down to the next lv

                bool found = false;

                for (int i = 0; i < lastSelect.transform.childCount; i++)
                {

                    Transform t = lastSelect.transform.GetChild(i);

                    if (t.GetComponent<Object>() != null)
                    {
                        found = true;

                        lastSelect = t.GetComponent<Object>();
                        lastSelect.SelectObject(true);
                    }
                }

                if (!found)
                {
                    lastSelect.SelectObject(true);
                }


            }

        }
    }

    private void PickUp()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerController.CheckIfCarrying())
        {
            playerController.Carrying();

            Transform attachInactive = null;
            Transform attachActive = lastSelect.transform;
            Transform pickUp = null;
            bool hasInactive = false;
            bool hasParent = false;

            Object temp = lastSelect.transform.GetComponent<Object>();

            if (lastSelect.transform.parent != null && lastSelect.transform.parent.GetComponent<Object>() != null)
            {
                attachInactive = lastSelect.transform.parent;
                hasParent = true;
            }

            while (true)
            {
                bool found = false;

                if (temp.GetObjectType() == Object.ObjectType.Active)
                {
                    if (!pickUp)
                    {
                        pickUp = temp.transform;
                    }

                    if (attachActive)
                    {
                        temp.transform.parent = attachActive;
                    }

                    

                    attachActive = temp.transform;

                }
                else
                {
                    if (attachInactive)
                    {
                        temp.transform.parent = attachInactive;
                    }
                    else
                    {
                        hasInactive = true;
                        lastSelect = temp;
                    }

                    attachInactive = temp.transform;
                    temp.UpdateLeftOverMesh();
                }


                for (int i = 0; i < temp.transform.childCount; i++)
                {
                    if ((temp = temp.transform.GetChild(i).GetComponent<Object>()) != null)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    break;
                }


            }

            if (!hasInactive && hasParent)
            {
                lastSelect = lastSelect.transform.parent.GetComponent<Object>();
            }

            pickUp.tag = "pickup";
            pickUp.SetParent(playerController.transform.GetChild(0).GetChild(0));

        }
        
    }
}
