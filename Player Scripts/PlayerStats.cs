using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private Image health_Stats, stamina_Stats;
    // Start is called before the first frame update
    public void Display_HealthStats(float healthValue)
    {
        healthValue /= 100;
        health_Stats.fillAmount = healthValue;
    }
    public void Display_StaminaStats(float staminaValue)
    {
        staminaValue /= 100f;
        stamina_Stats.fillAmount = staminaValue;
    }
}
