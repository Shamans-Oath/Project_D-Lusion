using UnityEngine;


[System.Serializable]
public class LifeClass 
{
    public int maxHealth = 100;

    public int currentHealth;

    public void GainHealth(int amount)
    {
        if(currentHealth + amount >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        else 
        {
            currentHealth += amount;
        }
    }

    public void LooseHealth(int amount)
    {
        if (currentHealth - amount <= 0)
        {
            currentHealth = 0;
        }
        else
        {
            currentHealth -= amount;
        }
    }

    public void PercentageHealthGain(float percentage)
    {
        if(!(percentage > 0 && percentage <= 100)) 
        {
            Debug.Log("Invalid number");
            return;
        }
        else
        {
            int amount = (int)((maxHealth * percentage) / 100);

            GainHealth(amount);
        }
    }

    public void PercentageHealthLoss(float percentage)
    {
        if (!(percentage > 0 && percentage <= 100))
        {
            Debug.Log("Invalid number");
            return;
        }
        else
        {
            int amount = (int)((maxHealth * percentage) / 100);

            LooseHealth(amount);
        }
    }

    public void AddMaxHealth(int amount)
    {
        float readjustmentRatio = (currentHealth * 100) / maxHealth;

        maxHealth += amount;

        currentHealth = (int)((maxHealth * readjustmentRatio) / 100);
    }

    public void ReduceMaxHealth(int amount)
    {
        float readjustmentRatio = (currentHealth * 100) / maxHealth;

        maxHealth -= amount;

        currentHealth = (int)((maxHealth * readjustmentRatio) / 100);
    }

    public void AddPercetangeMaxHealth(float percentage)
    {
        if (!(percentage > 0 && percentage <= 100))
        {
            Debug.Log("Invalid number");
            return;
        }
        else
        {
            int amount = (int)((maxHealth * percentage) / 100);

            AddMaxHealth(amount);
        }
    }

    public void ReducePercetangeMaxHealth(float percentage)
    {
        if (!(percentage > 0 && percentage <= 100))
        {
            Debug.Log("Invalid number");
            return;
        }
        else
        {
            int amount = (int)((maxHealth * percentage) / 100);

            ReduceMaxHealth(amount);
        }
    }
}
