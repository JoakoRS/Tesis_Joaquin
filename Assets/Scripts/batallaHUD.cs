using UnityEngine;
using UnityEngine.UI;


public class batallaHUD : MonoBehaviour
{
    public Text nombre;
    public Text nivel;
    public Slider hp;
    public Slider exp;

    public void setHUD(Unit unit)
    {
        nombre.text = unit.nombre;
        nivel.text = "Lvl " + unit.nivel;
        hp.maxValue = unit.maxHp;
        hp.value = unit.actualHp;
        if (nombre.text == "La Roca")
        {
            exp.maxValue = unit.maxExp;
            exp.value = unit.actualExp;
        }


    }
    public void setHP(int vida)
    {
        hp.value = vida;

    }
    public void setExp(int expe)
    {
        exp.value = expe;
    }
}
