using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    [SerializeField] ObjectType objectType;
    [SerializeField] Material activeMat;
    [SerializeField] Material inactiveMat;
    [SerializeField] Material selectActiveMat;
    [SerializeField] Material selectInactiveMat;



    bool inRange;
    MeshRenderer meshRenderer;

    public enum ObjectType
    {
        Active,
        Inactive,
       
    }

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (objectType == ObjectType.Active)
        {
            ChangeMaterial(activeMat);
      
        }
        else
        {
            ChangeMaterial(inactiveMat);
 
        }

       

       
 
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public ObjectType GetObjectType()
    {
        return objectType;
    }

    public void ChangeObjectType()
    {
        if (objectType == ObjectType.Active)
        {
            objectType = ObjectType.Inactive;
            ChangeMaterial(inactiveMat);
            UpdateInnerObject();
        }
        else
        {
            objectType = ObjectType.Active;
            ChangeMaterial(activeMat);
        }

    }

    public void SelectObject(bool select = false)
    {
        if (objectType == ObjectType.Active)
        {
            if (select)
            {
                ChangeMaterial(selectActiveMat);
            }
            else
            {
                ChangeMaterial(activeMat);
            }
        }
        else
        {

            if (select)
            {
                ChangeMaterial(selectInactiveMat);
            }

            else
            {
                ChangeMaterial(inactiveMat);
            }
        }
    }


    private void ChangeMaterial(Material mat)
    {
        Material[] mats = GetComponent<MeshRenderer>().materials;

        mats[0] = mat;
       
        GetComponent<MeshRenderer>().materials = mats;
    }

    private void UpdateInnerObject()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform t = transform.GetChild(i);

            if (t.GetComponent<Object>() != null)
            {
              ///
            }
        }
    }

 

  
}
