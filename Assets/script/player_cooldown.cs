using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player_cooldown : MonoBehaviour
{
    public GameObject skill1_cooldown;
    public GameObject skill2_cooldown;
    public GameObject skill3_cooldown;
    public GameObject skill4_cooldown;

    public Text skill1_cooldown_text;
    public Text skill2_cooldown_text;
    public Text skill3_cooldown_text;
    public Text skill4_cooldown_text;

    private float time = 0f;
    // Start is called before the first frame update
    void Start()
    {
        //skill1_cooldown_text = skill1_cooldown.GetComponent<Text>();
        //skill2_cooldown_text = skill2_cooldown.GetComponent<Text>();
       // skill3_cooldown_text = skill3_cooldown.GetComponent<Text>();
       // skill4_cooldown_text = skill4_cooldown.GetComponent<Text>();

        skill1_cooldown_text.text = "";
        skill2_cooldown_text.text = "";
        skill3_cooldown_text.text = "";
        skill4_cooldown_text.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        this.time += Time.deltaTime;
        if (this.time > 1f)
        {
            if (skill1_cooldown_text.text != "" && int.Parse(skill1_cooldown_text.text) > 0)
            {
                Cool_timer(skill1_cooldown_text);
            }
            if (skill2_cooldown_text.text != "" && int.Parse(skill2_cooldown_text.text) > 0)
            {
               Cool_timer(skill2_cooldown_text);
            }
            if (skill3_cooldown_text.text != "" && int.Parse(skill3_cooldown_text.text) > 0)
            {
                Cool_timer(skill3_cooldown_text);
            }
            if (skill4_cooldown_text.text != "" && int.Parse(skill4_cooldown_text.text) > 0)
            {
                Cool_timer(skill4_cooldown_text);
            }
            this.time = 0;
        }
        
    }

    public void setCoolTime(Text skill_cooldown, int cooldown)
    {
        skill_cooldown.text = cooldown.ToString();
    }

    void Cool_timer(Text skill_cooldown)
    {
        int cooldown = int.Parse(skill_cooldown.text);
        cooldown--;
        if (cooldown > 0)
            skill_cooldown.text = cooldown.ToString();
        else
            skill_cooldown.text = "";
    }
}
