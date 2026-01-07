using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class attraction : ScriptableObject
{
    public bool is_at_maximum_capacity;// est ce que quelqu'un est en train de faire le parc d'attraction
    public List<agent_secret> list_d_attente; // liste de tout les agents secrets qui attendent pour faire l'attraction
    public Transform point_d_interet;
    public int numero_participant;
    public float visit_time;
    public Vector3 entrance;
    public Vector3 exit;
    public int maximum_simultaneous_visitors;
    public int actual_simultaneous_visitors;
    public string activity;
}

[System.Serializable]
public class agent_secret
{
    public bool is_in_queue; //est ce que j'attends dans la queue 
    public agent_secret celui_avant_moi_dans_la_queue; 
    //public GameObject my_gameobject;
    public Vector3 my_position;
}

