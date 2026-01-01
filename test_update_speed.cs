using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;

public class test_update_speed : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private bool bool_count;
    // private bool once_after_count;
    private List<int>  list_count;

    IEnumerator Coroutine_wait_5_sec()
    {
        int count = 0;
        //int nb_count = list_count.Count;
        list_count.Add(count);
        bool_count = true;
        yield return new WaitForSeconds(5);
        Debug.Log("Update per second : " +list_count[0]/5);
        list_count.RemoveAt(0);
        if(list_count.Count == 0) bool_count = false;
    } 

    void Start()
    {
        bool_count = false;
        list_count = new List<int>();
        // once_after_count = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(bool_count) 
        {
            for(int i=0 ; i<list_count.Count; i++)
            {
                list_count[i]++;
            }
        }
    }

    public void Click()
    {
        StartCoroutine(Coroutine_wait_5_sec());
    }
}
