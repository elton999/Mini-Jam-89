using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSwapper : MonoBehaviour
{

    public List<GameObject> visible;
    public List<GameObject> invisible;

    public void Swap() {
        for (int i = 0; i < visible.Count; i++)
            if (visible[i] != null) visible[i].SetActive(true);
        for (int i = 0; i < invisible.Count; i++)
            if (invisible[i] != null)invisible[i].SetActive(false);
    }

}
