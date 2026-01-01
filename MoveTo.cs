
    using UnityEngine;
    using UnityEngine.AI;
    using System.Collections.Generic;
    using System.Collections;

    public class MoveTo : MonoBehaviour {

      private List<ScriptableObject> list_targets;
      // private ScriptableObject attraction_actuelle;
      public int numero_participant_precedent;
      public int i_celui_avant_moi_dans_la_queue;
      public bool I_am_next;
      public bool doing_the_attraction;
      // public bool my_turn;
      public bool attraction_just_done;
      public bool once;
      public bool am_I_in_queue;
      public bool am_I_arrive_point_d_interet;

      public bool init_done = false;
      public bool stopped_after_moving;
      public int actual_target;
      private NavMeshAgent nm_agent;
      private agent_secret agent_007_au_rapport;
      public float distance_to_reach = 2f;
      public bool reaching_target;
      public bool search_target;
      public Vector3 where_I_stopped;
      public Vector3 offset_position;


      IEnumerator Coroutine_random_new_target()
      {
         search_target = false;
         yield return new WaitForSeconds(5);
         random_new_target();
      } 

      IEnumerator Coroutine_have_fun()
      {
         yield return new WaitForSeconds(5);
         attraction_just_done = true;
      } 

      void non_wait_random_new_target()
      {
         search_target = false;
         random_new_target();
      } 

      float get_non_y_distance(Vector3 target_position)//give non distance with the 007 , except it puts y equals before calcul
      {
         Vector3 target_copy = new Vector3(target_position.x, 0, target_position.z);
         Vector3 transform_copy = new Vector3(transform.position.x, 0, transform.position.z);
         return Vector3.Distance(transform_copy, target_copy);
      }

      bool f_arrived(Vector3 target_position)
      {
         return (get_non_y_distance(target_position) < distance_to_reach);
      }
      void random_new_target() // trouve une nouvelle cible aléatoirement pour le 007 
      {
         int int_random_new_target = Random.Range(0,list_targets.Count);
         if(list_targets.Count<2)
         {
            Debug.Log("random_new_target error, list_targets.Count<2"+list_targets.Count);
         }
         while(int_random_new_target == actual_target && list_targets.Count >= 2) int_random_new_target = Random.Range(0,list_targets.Count);
         actual_target = int_random_new_target;
         //nm_agent.destination = ((attraction)list_targets[actual_target]).point_d_interet.position;
         //attraction_actuelle = list_targets[actual_target];

         nm_agent.isStopped = false;
         reaching_target = true;
         Debug.Log("new_target");
      }


      public void Init(List<ScriptableObject> targets) //fais par Chatgpt , fonction d'initialisation
      {
         //récupère les points d'attraction
         list_targets = targets;

         //set_up les variables utilisées par les fonctions du script
         reaching_target = true;
         stopped_after_moving = false;
         I_am_next = false;
         doing_the_attraction = false;
         attraction_just_done = false;
         // my_turn = false;
         once = false;
         offset_position = new Vector3(0,0,10);

         //set_up l'agent aux bonnes caractéritiques 
         nm_agent = GetComponent<NavMeshAgent>();
         nm_agent.autoBraking = true;
         nm_agent.speed = nm_agent.speed*5f*5f;
         nm_agent.radius = 0.5f;
         nm_agent.acceleration = nm_agent.acceleration*5f*5f;
         nm_agent.stoppingDistance = distance_to_reach;
         nm_agent.isStopped = true;
         
         //set_up l'instance de classe 
         agent_007_au_rapport = new agent_secret();
         agent_007_au_rapport.is_in_queue = false;
         agent_007_au_rapport.my_position = transform.position;

         random_new_target();
      }

      void f_move_to() //handles all for the agent to go to its destination and change it when arrived 
      {
         
         if(search_target) non_wait_random_new_target(); 
         //StartCoroutine(Coroutine_random_new_target());
         //Debug.Log(list_targets.Count);
         
         if (get_non_y_distance(((attraction)list_targets[actual_target]).point_d_interet.position) < distance_to_reach && reaching_target) //cas où les objets ne sont pas au même y
         {
            nm_agent.isStopped = true;
            stopped_after_moving = true;
            where_I_stopped = transform.position;
         }

         if(nm_agent.isStopped ) 
         {
            //sinon il glisse à cause de la pente, je n'ai pas trouvé d'autre méthode :(
            if(stopped_after_moving) transform.position = where_I_stopped ;
            if (reaching_target)//si il a atteint la cible qu'il cherchait il lance random_new_target une seule fois
            {
               
               search_target = true;
               reaching_target = false;
            }
         }
      }

      // void new_target(Vector3 new_position)
      // {
      //    // int int_random_new_target = Random.Range(0,list_targets.Count);
      //    // if(list_targets.Count<2)
      //    // {
      //    //    Debug.Log("random_new_target error, list_targets.Count<2"+list_targets.Count);
      //    // }
      //    // while(int_random_new_target == actual_target && list_targets.Count >= 2) int_random_new_target = Random.Range(0,list_targets.Count);
      //    // actual_target = int_random_new_target;
      //    nm_agent.destination = new_position;
      //    //list_targets[actual_target] = list_targets[actual_target];

      //    nm_agent.isStopped = false;
      //    reaching_target = true;
      // }

      void have_fun()
      {
         Vector3 offset = new Vector3(20,0,0);
         nm_agent.destination = ((attraction)list_targets[actual_target]).point_d_interet.position + offset;
         StartCoroutine(Coroutine_have_fun());
      }

      void handle_attraction_and_queue()
      {
         am_I_arrive_point_d_interet = false;
         nm_agent.isStopped = false;
         if(!doing_the_attraction)
         {
            if(!agent_007_au_rapport.is_in_queue)
            {
               if(((attraction)list_targets[actual_target]).list_d_attente.Count == 0) 
               {
                  if(f_arrived(((attraction)list_targets[actual_target]).point_d_interet.position))
                  {
                     am_I_arrive_point_d_interet = true;
                     if(((attraction)list_targets[actual_target]).is_someone_doing_it)
                     {
                        I_am_next = true;
                        //my_turn = true;
                        agent_007_au_rapport.my_position = transform.position;
                        agent_007_au_rapport.is_in_queue = true;
                        ((attraction)list_targets[actual_target]).list_d_attente.Add(agent_007_au_rapport);
                        
                     }
                     else 
                     {
                        doing_the_attraction = true;
                        ((attraction)list_targets[actual_target]).is_someone_doing_it = true; 
                        ((attraction)list_targets[actual_target]).numero_participant++;
                     }
                  }
                  else nm_agent.destination = ((attraction)list_targets[actual_target]).point_d_interet.position;
               }
               else
               {
                  agent_007_au_rapport.celui_avant_moi_dans_la_queue = ((attraction)list_targets[actual_target]).list_d_attente[((attraction)list_targets[actual_target]).list_d_attente.Count-1];
                  nm_agent.destination = agent_007_au_rapport.celui_avant_moi_dans_la_queue.my_position + offset_position;
                  if(f_arrived(nm_agent.destination))
                  {
                     am_I_arrive_point_d_interet = true;
                     i_celui_avant_moi_dans_la_queue = ((attraction)list_targets[actual_target]).list_d_attente.Count-1;
                     agent_007_au_rapport.my_position = transform.position;
                     agent_007_au_rapport.is_in_queue = true;
                     ((attraction)list_targets[actual_target]).list_d_attente.Add(agent_007_au_rapport);
                     numero_participant_precedent = ((attraction)list_targets[actual_target]).numero_participant;
                  }
               }
            }
            else
            {
               if(((attraction)list_targets[actual_target]).numero_participant != numero_participant_precedent)
               {
                  agent_007_au_rapport.my_position = transform.position;
                  ((attraction)list_targets[actual_target]).list_d_attente[i_celui_avant_moi_dans_la_queue] = agent_007_au_rapport;
                  if(i_celui_avant_moi_dans_la_queue == 0)
                  {
                     // if(I_am_next)
                     // {
                     //    my_turn = true;
                     // }
                     // else 
                     I_am_next = true;
                     ((attraction)list_targets[actual_target]).list_d_attente[0] = agent_007_au_rapport;

                  }
                  else i_celui_avant_moi_dans_la_queue--;
                  numero_participant_precedent = ((attraction)list_targets[actual_target]).numero_participant;
               }
               if(I_am_next) 
               {
                  // if(my_turn)
                  // {
                  if(f_arrived(((attraction)list_targets[actual_target]).point_d_interet.position)) 
                  {
                     am_I_arrive_point_d_interet = true;
                     if(!((attraction)list_targets[actual_target]).is_someone_doing_it) 
                     {
                        agent_007_au_rapport.is_in_queue = false;
                        ((attraction)list_targets[actual_target]).list_d_attente.RemoveAt(0);
                        doing_the_attraction = true;
                        ((attraction)list_targets[actual_target]).is_someone_doing_it = true; 
                        ((attraction)list_targets[actual_target]).numero_participant++;
                     }
                     else nm_agent.isStopped = true;
                  }
                  else nm_agent.destination = ((attraction)list_targets[actual_target]).point_d_interet.position;
                  // }
                  // else nm_agent.destination = ((attraction)list_targets[actual_target]).point_d_interet.position;
               }
               else nm_agent.destination = ((attraction)list_targets[actual_target]).list_d_attente[i_celui_avant_moi_dans_la_queue].my_position + offset_position;

               if(f_arrived(nm_agent.destination))
               {
                  agent_007_au_rapport.my_position = transform.position;
                  ((attraction)list_targets[actual_target]).list_d_attente[i_celui_avant_moi_dans_la_queue + 1] = agent_007_au_rapport;
               }
            }
         }
         else
         {
            if(!once) 
            {
               once = true;
               have_fun();
            }
         }
        
         if(attraction_just_done)
         {
            attraction_just_done = false;
            doing_the_attraction = false;
            ((attraction)list_targets[actual_target]).is_someone_doing_it = false;
            agent_007_au_rapport.is_in_queue = false;
            I_am_next = false;
            // my_turn = false;
            once = false;
            random_new_target();
         }
      }

      void Update()
      {
         if(init_done)
         {
         //f_move_to();

         handle_attraction_and_queue();
         }

         am_I_in_queue = agent_007_au_rapport.is_in_queue;
      }
    }