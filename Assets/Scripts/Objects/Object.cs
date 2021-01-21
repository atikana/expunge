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
    bool updateMesh;
    bool meshStatus;
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
        if (updateMesh)
        {
            UpdateMesh();
        }
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
            UpdateInnerObject(true);


        }
        else
        {
            objectType = ObjectType.Active;
            ChangeMaterial(activeMat);
            UpdateInnerObject(false);
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

    private void UpdateInnerObject(bool b)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform t = transform.GetChild(i);

            if (t.GetComponent<Object>() != null)
            {
                t.GetComponent<Object>().SetUpdateMesh(b);
            }
        }
    }

    private void UpdateMesh()
    {
        meshRenderer.enabled = meshStatus;
        UpdateInnerObject(meshStatus);
        updateMesh = false;
    }

    public void SetUpdateMesh(bool b)
    {
        updateMesh = true;
        meshStatus = b;
    }

 

  
}
