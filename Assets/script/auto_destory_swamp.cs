using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class auto_destory_swamp : MonoBehaviour
{
    private ParticleSystem poison;
    private ParticleSystem swamp_2;
    private ParticleSystem flash;

    private bool poison_end = false;
    private bool swamp_2_end = false;
    private bool flash_end = false;
    // Start is called before the first frame update
    void Start()
    {
        poison = gameObject.transform.Find("Poison").GetComponent<ParticleSystem>();
        swamp_2 = gameObject.transform.Find("swamp_2").GetComponent<ParticleSystem>();
        flash = gameObject.transform.Find("Flash").GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!poison.IsAlive())
            poison_end = true;
        if (!swamp_2.IsAlive())
            swamp_2_end = true;
        if (!flash.IsAlive())
            flash_end = true;

        if (poison_end && swamp_2_end && flash_end)
            Destroy(gameObject);
    }
}
