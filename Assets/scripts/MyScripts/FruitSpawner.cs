using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FruitSpawner : MonoBehaviour
{
    [SerializeField]
    public Transform[] transforms;
    [SerializeField]
    public GameObject[] fruits;
    public float spawnForce;
    public bool startSpawner;
    public int Spawns = 5;
    
    [SerializeField] private AudioClip spawnSoundClip;

    void Start()
    {
        //StartCoroutine(Spawner());
    }

    public IEnumerator Spawner()
    {
        Debug.Log("coroutineA created");
        while(true){
            yield return new WaitForSeconds(Random.Range(1.5f,4));
            yield return StartCoroutine(SpawnSet());
            Debug.Log("gonna repeat that again!");
        }
    }

    IEnumerator SpawnSet()
    {
        int spawns = Random.Range(1,Spawns);  // random no. of fruits to spawn
        List<int> spawnPoints = new List<int>();  
        for(int i = 0; i <= spawns; i++){
            int index = Random.Range(0,7);
            while(spawnPoints.Contains(index)){
                index = Random.Range(0,7);
            }
            spawnPoints.Add(index);
        } // generate indexes of unique random numbers to spawn the fruits at
        int spawnType = Random.Range(0,2);
        for (int j=0; j < spawnPoints.Count; j++){
            int fruitIndex = Random.Range(0, fruits.Length);
            if(spawnType == 0) yield return new WaitForSeconds(0.1f);
            GameObject fruitClone = Instantiate(fruits[fruitIndex], transforms[spawnPoints[j]].position, Random.rotation);
            Rigidbody rb = fruitClone.GetComponent<Rigidbody>();
            rb.AddForce(Vector3.up * spawnForce, ForceMode.Impulse);//adding force
            Vector3 randRot = new(Random.value, Random.value, Random.value); //random val for torque
            rb.AddTorque(randRot * 1); //adding torque
            SoundFXManager.instance.PlaySoundFXClip(spawnSoundClip,transforms[spawnPoints[j]],1f); //sfx
            Debug.Log(j); //log spawn the fruits
        }
        yield return null;
    }
}
