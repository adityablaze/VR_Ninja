using System.Xml.Serialization;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    [SerializeField] private AudioSource soundfxObject;
    public static SoundFXManager instance;
    private void Awake()
    {
        if(instance == null){
            instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume){
        AudioSource audioSource = Instantiate(soundfxObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
        float clipLenght = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLenght);
    }
}
