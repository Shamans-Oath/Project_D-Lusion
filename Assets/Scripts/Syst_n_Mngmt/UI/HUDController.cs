using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public static HUDController instance;

    public Animator cmp_animBars, cmp_animDash, cmp_animSpecial;
    bool dashReady = true, specialReady = true;
    float tempHealth;

    public Image lifeBar, shieldBar;
    public float lerpDuration;
    public Image specialCircle, dashCircle, specialIcon, dashIcon;
    public Color initialColor;
    public Color furyColor;
    public Image furyImage;
    public Sprite[] furyIcons;
    
    float targetPointD, targetPointP, targetPointID, targetPointIP;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        dashReady = true;
        specialReady = true;
        tempHealth = 100;
    }

    // Update is called once per frame
    void Update()
    {
        /*if(specialCircle.fillAmount == 1)
        {
            targetPointP += Time.deltaTime;
            targetPointIP += Time.deltaTime;
            specialIcon.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, targetPointIP);
        }
        else
        {
            targetPointP = 0;
            targetPointIP = 0;
            specialIcon.color = new Color(1, 1, 1,0);
        }*/
    }

    public void UpdateSpecialCooldown(float timer, float duration)
    {
        float cooldownRatio = Mathf.InverseLerp(0, duration, timer);
        specialCircle.fillAmount = cooldownRatio;

        if (specialCircle.fillAmount == 1 && specialReady == false)
        {
            cmp_animSpecial.SetTrigger("SpecialReady");
            specialReady = true;
        }
        else if (specialCircle.fillAmount <= 0.5f)
        {
            specialReady = false;
        }
    }

    public void UpdateDashCooldown(float timer, float duration)
    {
        float cooldownRatio = Mathf.InverseLerp(duration, 0, timer);
        dashCircle.fillAmount = cooldownRatio;

        Debug.Log(dashReady);

        if (dashCircle.fillAmount == 1 && dashReady == false)
        {
            cmp_animDash.SetTrigger("DashReady");
            dashReady = true;
        }
        else if(dashCircle.fillAmount <= 0.5f)
        {
            dashReady = false;
        }
    }

    public void UpdateLifeBar(int currentHealth, int maxHealth)
    {
        float healthRatio = Mathf.InverseLerp(0, maxHealth, currentHealth);

        lifeBar.fillAmount = Mathf.Lerp(lifeBar.fillAmount, healthRatio, lerpDuration);

        if(currentHealth == 0)
        {
            lifeBar.fillAmount = 0;
        }
    }

    public void UpdateShieldBar(int currentShield, int maxShield)
    {
        float shieldRatio = Mathf.InverseLerp(0, maxShield, currentShield);

        shieldBar.fillAmount = Mathf.Lerp(shieldBar.fillAmount, shieldRatio, lerpDuration);
    }

    public void UpdateFuryImage(float currentFurry, float maxFurry)
    {
        if(currentFurry == 0)
        {
            furyImage.sprite = furyIcons[0];
        }
        else if(currentFurry >= maxFurry/5 && currentFurry < (maxFurry / 5) * 2)
        {
            furyImage.sprite = furyIcons[1];
        }
        else if (currentFurry >= (maxFurry / 5) * 2 && currentFurry < (maxFurry / 5) * 3)
        {
            furyImage.sprite = furyIcons[2];
        }
        else if (currentFurry >= (maxFurry / 5) * 3 && currentFurry < (maxFurry / 5) * 4)
        {
            furyImage.sprite = furyIcons[3];
        }
        else if (currentFurry == 100)
        {
            furyImage.sprite = furyIcons[4];
        }
    }
}
