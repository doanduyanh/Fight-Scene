using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDamage : MonoBehaviour
{
    public LayerMask layer;
    public float radius = 0.2f;
    public float skillDamageMultipler;

    // Update is called once per frame
    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, layer);
        if (hits.Length > 0)
        {
            
            gameObject.SetActive(false);
            foreach(Collider hit in hits)
            {
                //print(GetRootParentName(gameObject) + " hit " + layer.value + " name "+hit.name);
                if (!hit.GetComponent<HealthScript>().IsCharDead())
                {
                    hit.GetComponent<HealthScript>().ApplyDamage(GetRootParentObj(gameObject).GetComponent<CharacterBase>().damage* skillDamageMultipler);
                }
            }
            
        }
    }
    GameObject GetRootParentObj(GameObject obj)
    {
        Transform currentTransform = obj.transform;

        while (currentTransform.parent != null)
        {
            currentTransform = currentTransform.parent;
        }

        return currentTransform.gameObject;
    }
}
