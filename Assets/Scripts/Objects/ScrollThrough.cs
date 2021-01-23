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
        if (Input.GetKeyDown(KeyCode.Tab)) // forward
        {
            ScrollUp();
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ScrollDown();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            ClickObject();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            PressE();
        }
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

    private void ScrollUp()
    {
 
        bool found = false;
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


    private void ScrollDown()
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

    private void ClickObject()
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

    private void PressE()
    {

        if (playerController.CheckIfCarrying())
        {
            PickUp();
        }
        else
        {
            DropObject();
        }


    }

    private void PickUp()
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

        playerController.AddCarryItem(pickUp);

    }

    private bool DropObject()
    {
        Object temp = lastSelect;
        while (true)
        {
           if (int.Parse(temp.name) > int.Parse(playerController.returnItem().name))
            {
               
                // how to fucking add now lmao
                
               
            }
            else
            {
                break;
            }
        }
        return false;
    }

    

}
