using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.LightningBolt;
using UnityEngine.Rendering.HighDefinition;

public class CrystalScript : MonoBehaviour
{

    // Settings
    public float intensity = 0.6f;
    public float range = 15.0f;
    public Color color = Color.blue;
    public float chargeSpeed = 1.0f;
    public bool randomColor = true;

    private float charge = 0.0f;
    private HDAdditionalLightData lightComp;
    GameObject parent;
    GameObject lightGameObject;

    // list of all crystals
    public static List<GameObject> crystals = new List<GameObject>();
    public GameObject lightningPrefab = null;

    // Start is called before the first frame update
    void Start()
    {
        // get owning object
        this.parent = gameObject;

        this.intensity = 0.6f;
        this.range = 30.0f;

        // get lightning prefab
        if (this.lightningPrefab == null)
        {
            this.lightningPrefab = GameObject.Find("Lightning1");
        }

        // set color to random value
        if (randomColor)
        {
            // hsv
            float h = Random.Range(0.0f, 1.0f);
            float s = Random.Range(0.5f, 1.0f);
            float v = Random.Range(0.5f, 1.0f);
            color = Color.HSVToRGB(h, s, v);
        }

        // set charge to random value
        charge = Random.Range(0.0f, 1.0f);

        // Create new point light and set it as a child of the parent object
        this.lightGameObject = new GameObject("Crystal Light");
        this.lightComp = lightGameObject.AddHDLight(HDLightTypeAndShape.Point);
        lightComp.color = color;
        lightComp.intensity = intensity;
        lightComp.range = range;
        lightGameObject.transform.parent = parent.transform;
        lightGameObject.transform.localPosition = new Vector3(0, 1, 0);
        lightGameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
        lightGameObject.transform.localScale = new Vector3(1, 1, 1);

        // soft shadows
        //lightComp.shadows = LightShadows.Soft;

        // add to list of crystals
        crystals.Add(this.parent);

    }

    void OnDestroy()
    {
        // remove from list of crystals
        crystals.Remove(this.parent);
    }

    void uncharge()
    {
        charge = 0.0f;
        this.lightComp.intensity = this.intensity * 10.0f;

        if(this.lightningPrefab == null)
        {
            return;
        }

        // pick other random crystal
        GameObject otherCrystal = this.parent;
        while (otherCrystal == this.parent)
        {
            int index = Random.Range(0, crystals.Count);
            otherCrystal = crystals[index];
        }

        // lightning to other crystal
        // get script
        LightningBoltScript lightningScript = this.lightningPrefab.GetComponent<LightningBoltScript>();
        // set start and end
        lightningScript.StartObject = this.parent;
        lightningScript.EndObject = otherCrystal;
        // create lightning
        lightningScript.Trigger();

        // play sound
        var audioSource = this.lightningPrefab.GetComponent<AudioSource>();
        // set position to this crystal
        audioSource.transform.position = this.parent.transform.position;

        // sound currently disabled
        //audioSource.Play();




    }

    // Update is called once per frame
    void Update()
    {

        // random charge
        if (Random.Range(0.0f, 1.0f) > 0.1f)
        {
            return;
        }

        
        const float CHARGE_MULTIPLIER = 0.2f;
        // update charge
        charge += chargeSpeed * Time.deltaTime * CHARGE_MULTIPLIER;
        if (charge > 1.0f)
        {
            this.uncharge();
        } else {
            // update light intensity
            lightComp.intensity = charge * intensity;
        }
        
    }
}
