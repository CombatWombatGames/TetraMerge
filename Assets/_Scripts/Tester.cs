using System.Collections;
using UnityEngine;

#if UNITY_EDITOR 
public class Tester : MonoBehaviour
{
    void Start()
    {
        //StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(1);
        FindObjectOfType<InputDisabler>().DisableInput();
    }
}
#endif