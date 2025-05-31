using UnityEngine;
using EzySlice;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.Mathematics;
using System.Collections.Generic;
using TMPro;

public class sliceObject : MonoBehaviour
{
    public Transform startSlicePoint;
    public Transform endSlicePoint;
    public VelocityEstimator velocityEstimator;
    public LayerMask sliceableLayer;
    public LayerMask cutLayer;
    public Material slicemat;
    public float cutForce = 0;
    [SerializeField] private ParticleSystem slashParticle;
    private ParticleSystem slashParticleInstance;
    [SerializeField] private AudioClip[] cutSounds;
    [Space(30)]
    [SerializeField] private XRBaseController RightXRController;
    [Range(0,1)] public float cutVibrationIntensity;
    [Range(0,1)] public float duration;
    public float mincutSpeed = 4.0f;
    public System.Random rnd = new();

    public AudioClip[] comboSFX;
    public Transform ComboSoundPos;
    public float combotime = 0.5f;
    public float ComboTimer;
    public int comboCount = 0;

    public int score = 0;
    public TextMeshProUGUI scoreText;

    [System.Serializable]
    public struct strMaterial{
        public string materialName;
        public Material material;
        public ParticleSystem particleEff;
    }
    [SerializeField] public strMaterial[] materials;
    void Start()
    {
        score = 0;
        scoreText.text = score.ToString();
        ComboTimer = combotime;
    }
    void Update()
    {
        ComboTimer -= Time.deltaTime;
        if (ComboTimer <= 0 && comboCount > 0)
        {
            SoundFXManager.instance.PlaySoundFXClip(comboSFX[comboCount-1], ComboSoundPos, 1.0f);
            Debug.Log("Combo x" + (comboCount + 1));
            if (comboCount == 1) score += 10;
            if (comboCount == 2) score += 20;
            if (comboCount == 3) score += 30;
            if (comboCount == 4 || comboCount == 5) score += 50;
            scoreText.text = score.ToString();
            comboCount = 0;
        }
    }
    void FixedUpdate()
    {
        bool hasHit = Physics.Linecast(startSlicePoint.position, endSlicePoint.position, out RaycastHit hit, sliceableLayer);
        if(hasHit && velocityEstimator.GetVelocityEstimate().magnitude >= mincutSpeed){
            GameObject target = hit.transform.gameObject;
            Slice(target);
            //Debug.Log("cut at velocity = " + velocityEstimator.GetVelocityEstimate().magnitude);
        }
    }
    public void Slice(GameObject target){
        Vector3 velocity = velocityEstimator.GetVelocityEstimate();
        Vector3 planeNormal = Vector3.Cross(endSlicePoint.position-startSlicePoint.position, velocity);
        planeNormal.Normalize();
        Material materialToUse = slicemat; // Default material
        ParticleSystem particleToUse = slashParticle; //Default particle
        foreach (var mat in materials)
        {
            if (target.CompareTag(mat.materialName))
            {
                materialToUse = mat.material;
                particleToUse = mat.particleEff;
                break;
            }
        }
        SlicedHull hull = target.Slice(target.transform.position, planeNormal, materialToUse);
        // SlicedHull hull = target.Slice(endSlicePoint.position, planeNormal, new TextureRegion(0.0f, 0.0f, 1.0f, 1.0f), slicemat);
        if (hull != null) {
            RightXRController.SendHapticImpulse(cutVibrationIntensity, duration); // send haptic feedback
            SoundFXManager.instance.PlaySoundFXClip(cutSounds[rnd.Next(cutSounds.Length)], target.transform, 0.7f); //Play Random Cutting Sound
            Instantiate(particleToUse.gameObject, target.transform.position, Quaternion.identity); // fixed instantiation

            //Combos and scores
            if (ComboTimer >= 0)
            {
                comboCount++;
            }
            ComboTimer = combotime;
            if (target.transform.position.y >= 0.20f)
            {
                score += 10;
                scoreText.text = score.ToString();
            }

            //Creating the hulls of seperated parts
            GameObject upperhull = hull.CreateUpperHull(target, materialToUse);
            setupSlicedComponent(upperhull);
            GameObject lowerhull = hull.CreateLowerHull(target, materialToUse);
            setupSlicedComponent(lowerhull);
            Destroy(target);
        }
    }
    public void setupSlicedComponent(GameObject slicedObject){
        Rigidbody rb = slicedObject.AddComponent<Rigidbody>();
        MeshCollider meshc = slicedObject.AddComponent<MeshCollider>();
        slicedObject.AddComponent<selfDestroy>();
        meshc.convex = true;
        slicedObject.layer = cutLayer;
        rb.AddExplosionForce(cutForce,slicedObject.transform.position,1);
    }

    private void SpawnCutParticle(GameObject gameObject){
        slashParticleInstance = Instantiate(slashParticle, gameObject.transform.position, Quaternion.identity);
    }

}