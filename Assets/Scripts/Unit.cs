using UnityEngine;

public class Unit : MonoBehaviour
{

    public string nombre;

    public int nivel;

    public int damage;

    public int maxHp;
    public int actualHp;

    public int maxExp;
    public int actualExp;
    public bool tomarDamage(int dmg)
    {
        actualHp -= dmg;
        if (actualHp <= 0)
            return true;
        else
            return false;
    }


    public void aumentarExp()
    {
        if (actualExp < 100)
        {
            actualExp += 20;
        }
    }
    public void tomarExp()
    {
        if (actualExp > 0)
        {
            actualExp -= 20;
        }
         
    }
}
