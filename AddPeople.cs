using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;


public class AddPeople : MonoBehaviour
{
    private float maxy=460;
    private float miny=312;
    private float minz=10;
    private float maxz=990;
    private float minx=10;
    private float maxx=1990;

    public int nb_people_to_add = 100;
    public List<Transform> list_targets_transform;
    private List<ScriptableObject> list_targets;
    private List<agent_secret> list_007;

    public GameObject myPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private float f(float x)//renvoie la hauteur où spawn en fonction de la position  
    {
        float a=(maxy-miny)/maxx;
        float b = miny-minx*a;

        return a*x+b;
    }

    void make_POIs()
    {
        list_007 = new List<agent_secret>();
        list_targets = new List<ScriptableObject>();
        for(int i=0 ; i<list_targets_transform.Count ; i++)
        {
            ScriptableObject new_attraction_instance = ScriptableObject.CreateInstance("attraction");
            ((attraction)new_attraction_instance).point_d_interet = list_targets_transform[i];
            ((attraction)new_attraction_instance).is_at_maximum_capacity = false;
            ((attraction)new_attraction_instance).list_d_attente = new List<agent_secret>();
            ((attraction)new_attraction_instance).numero_participant = 0;
            ((attraction)new_attraction_instance).visit_time = 5f;
            ((attraction)new_attraction_instance).entrance = ((attraction)new_attraction_instance).point_d_interet.position;
            ((attraction)new_attraction_instance).exit = new Vector3(((attraction)new_attraction_instance).entrance.x + 100f, ((attraction)new_attraction_instance).entrance.y, ((attraction)new_attraction_instance).entrance.z);
            ((attraction)new_attraction_instance).maximum_simultaneous_visitors = 3;
            ((attraction)new_attraction_instance).actual_simultaneous_visitors = 0;
            ((attraction)new_attraction_instance).activity = "go_to_exit";
            list_targets.Add(new_attraction_instance);
        }
    }

    void Start()
    {
        NavMesh.avoidancePredictionTime = NavMesh.avoidancePredictionTime*2;
        make_POIs();
    }
    public void Click()// créer nb_people_to_add agent_secrets aux positions aléatoirres
    {
        for (int i=0 ; i<nb_people_to_add ;i++)
        {   
            if(list_007.Count<3000)
            {
                //position aléatoire
                float xpos=Random.Range(minx, maxx);
                float zpos=Random.Range(minz,maxz);
                float ypos=f(xpos);
                Vector3 position = new Vector3(xpos,ypos,zpos);
                
                GameObject g_agent007 = Instantiate(myPrefab,position,Quaternion.identity);

                //initialise MoveTo qui permet à l'agent de se déplacer
                MoveTo mt = g_agent007.AddComponent<MoveTo>();
                mt.Init(list_targets);
                mt.init_done = true;

                //créer l'agent secret
                agent_secret agent007 = new agent_secret();
                agent007.my_position = g_agent007.transform.position;
                agent007.is_in_queue = false;
                list_007.Add(agent007);
            }
            else 
            {
                Debug.Log("Ah non ! Trop d'agents secrets là ; c'est pas discret. "+ list_007.Count);
                return;
            }
        }
    }
}
