using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Others : MonoBehaviour
{

    public Location location;

    public List<Animator> arrAnim;

    public List<Transform> lsPoint;

    public void LoadOtherRandom()
    {
        location.lsOther = new List<int>();
        int random;
        Animator anim;
        for (int i = 0; i < lsPoint.Count; i++)
        {
            if (location.indexTypeWork < 3)
            {
                int begin = Random.Range(0, 100) <= 50 ? 0 : 3; ;
                random = Random.Range(begin, location.arrPrefabOther.Length);
                anim = Instantiate(location.arrPrefabOther[random], lsPoint[i]).GetComponent<Animator>();
            }
            else
            {
                random = Random.Range(0, GameManager.Instance.arrPrefabOther.Length);
                anim = Instantiate(GameManager.Instance.arrPrefabOther[random], lsPoint[i]).GetComponent<Animator>();

            }
            location.lsOther.Add(random);
            arrAnim.Add(anim);
        }
        location.isLoaded = true;
    }

    public void LoadOtherJson()
    {
        for (int i = 0; i < lsPoint.Count; i++)
        {
            Animator anim;
            if (location.indexTypeWork < 4)
            {
                anim = Instantiate(location.arrPrefabOther[location.lsOther[i]], lsPoint[i]).GetComponent<Animator>();
            }
            else
            {
                anim = Instantiate(GameManager.Instance.arrPrefabOther[location.lsOther[i]], lsPoint[i]).GetComponent<Animator>();

            }
            arrAnim.Add(anim);
        }
        location.isLoaded = true;
    }
}
