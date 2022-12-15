using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum battlestate
{
    START,
    PLAYERTURN,
    WIN,
    LOST
}

public class SistemaBatalla : MonoBehaviour
{


    private int respuesta1;
    private int respuesta2;
    private int respuesta3;
    private int respuesta4;
    private int respuestaCorrecta;
    private string operacionCom;


    private int numResp = 0;
    private int numRespC = 0;
    private int contresp = 0;
    private float procentResp;
    private int nivel;
    private string nivelPref = "nivelPref";

    private int exp;
    private string expPerf = "expPref";

    private int pts;
    private string ptsPref = "ptsPref";




    // Start is called before the first frame update
    public GameObject juagdor;
    public GameObject enemigol;

    public Transform playerBS;
    public Transform enemyBS;

    Unit jugadorUnit;
    Unit enemigoUnit;

    public Text operacion;

    public Text puntuacion;


    public Text resp1;
    public Text resp2;
    public Text resp3;
    public Text resp4;

    public batallaHUD jugadorHUD;
    public batallaHUD enemigoHUD;


    public battlestate estado;

    void Start()
    {
        estado = battlestate.START;

        loadData();
        Debug.LogError(nivel);
        StartCoroutine(iniciarBatalla());

    }
    private void OnDestroy()
    {
        exp = jugadorUnit.actualExp;
        saveData();
    }
    private void loadData()
    {
        nivel = PlayerPrefs.GetInt(nivelPref);
        exp = PlayerPrefs.GetInt(expPerf);
        pts = PlayerPrefs.GetInt(ptsPref);

    }
    private void saveData()
    {
        PlayerPrefs.SetInt(nivelPref, nivel);
        PlayerPrefs.SetInt(expPerf, exp);
        PlayerPrefs.SetInt(ptsPref, pts);
    }
    // Update is called once per frame
    IEnumerator iniciarBatalla()
    {
        GameObject jugadorGO = Instantiate(juagdor, playerBS);
        jugadorUnit = jugadorGO.GetComponent<Unit>();

        GameObject enemigoGO = Instantiate(enemigol, enemyBS);
        enemigoUnit = enemigoGO.GetComponent<Unit>();

        jugadorHUD.setHUD(jugadorUnit);
        enemigoHUD.setHUD(enemigoUnit);

        jugadorHUD.setExp(exp);

        operacion.text = "Derrota al enemigo...";
        puntuacion.text = "";
        resp1.text = "";
        resp2.text = "";
        resp3.text = "";
        resp4.text = "";

        yield return new WaitForSeconds(2f);




        playerTurn();
        yield return new WaitForSeconds(1f);



    }
    void setOperacion()
    {
        operacion.text = generarOperacion();
        puntuacion.text = "Puntuacion: " + pts;
        respuesta1 = respuestaCorrecta;
        respuesta2 = respuestaCorrecta + 1;
        respuesta3 = respuestaCorrecta + 2;
        respuesta4 = respuestaCorrecta - 1;
        int pos = Random.Range(1, 4);
        switch (pos)
        {
            case 1:
                resp1.text = respuesta1.ToString();
                resp2.text = respuesta2.ToString();
                resp3.text = respuesta3.ToString();
                resp4.text = respuesta4.ToString();
                break;
            case 2:
                resp2.text = respuesta1.ToString();
                resp3.text = respuesta2.ToString();
                resp1.text = respuesta3.ToString();
                resp4.text = respuesta4.ToString();
                break;
            case 3:
                resp3.text = respuesta1.ToString();
                resp4.text = respuesta2.ToString();
                resp1.text = respuesta3.ToString();
                resp2.text = respuesta4.ToString();
                break;
            case 4:
                resp4.text = respuesta1.ToString();
                resp1.text = respuesta2.ToString();
                resp2.text = respuesta3.ToString();
                resp3.text = respuesta4.ToString();
                break;

        }

    }
    void playerTurn()
    {

        estado = battlestate.PLAYERTURN;
        setOperacion();

    }
    IEnumerator ataqueJugadorBueno()
    {
        bool estaMuerto = enemigoUnit.tomarDamage(jugadorUnit.damage);
        enemigoHUD.setHP(enemigoUnit.actualHp);
        yield return new WaitForSeconds(1f);
        resp1.color = Color.black;
        resp2.color = Color.black;
        resp3.color = Color.black;
        resp4.color = Color.black;

        numResp++;
        numRespC++;
        contresp++;
        pts += 5;

        puntuacion.text = "Puntuacion: " + pts;


        if (contresp % 5 == 0)
        {
            establecerNivel();
        }
        setOperacion();

        if (estaMuerto)
        {
            estado = battlestate.WIN;
            terminarNivel();
        }
    }


    void terminarNivel()
    {
        if (estado == battlestate.WIN)
        {
            operacion.text = "Ganaste :D";
            System.Threading.Thread.Sleep(1000);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (estado == battlestate.LOST)
        {
            operacion.text = "Perdiste";
        }
    }
    IEnumerator ataqueJugadorMalo()
    {
        bool estaMuerto = jugadorUnit.tomarDamage(enemigoUnit.damage);
        jugadorHUD.setHP(jugadorUnit.actualHp);
        numResp++;
        contresp++;
        yield return new WaitForSeconds(1f);
        resp1.color = Color.black;
        resp2.color = Color.black;
        resp3.color = Color.black;
        resp4.color = Color.black;

        if (contresp % 5 == 0)
        {
            establecerNivel();
        }
        setOperacion();
        yield return new WaitForSeconds(0f);

        if (estaMuerto)
        {
            estado = battlestate.LOST;
            terminarNivel();
        }
    }

    public void onAtaqueboton1()
    {

        if (estado == battlestate.PLAYERTURN)
        {
            if (resp1.text == respuestaCorrecta.ToString())
            {
                resp1.color = Color.green;
                StartCoroutine(ataqueJugadorBueno());
            }
            else
            {
                if (resp2.text == respuestaCorrecta.ToString())
                {
                    resp2.color = Color.green;
                }
                else if (resp3.text == respuestaCorrecta.ToString())
                {
                    resp3.color = Color.green;
                }
                else
                {
                    resp4.color = Color.green;
                }
                resp1.color = Color.red;
                StartCoroutine(ataqueJugadorMalo());
            }
        }
        else
        {
            return;
        }
    }
    public void onAtaqueboton2()
    {

        if (estado == battlestate.PLAYERTURN)
        {
            if (resp2.text == respuestaCorrecta.ToString())
            {
                resp2.color = Color.green;
                StartCoroutine(ataqueJugadorBueno());
            }
            else
            {
                resp2.color = Color.red;
                if (resp1.text == respuestaCorrecta.ToString())
                {
                    resp1.color = Color.green;
                }
                else if (resp3.text == respuestaCorrecta.ToString())
                {
                    resp3.color = Color.green;
                }
                else
                {
                    resp4.color = Color.green;
                }
                StartCoroutine(ataqueJugadorMalo());
            }
        }
        else
        {
            return;
        }
    }
    public void onAtaqueboton3()
    {

        if (estado == battlestate.PLAYERTURN)
        {
            if (resp3.text == respuestaCorrecta.ToString())
            {
                resp3.color = Color.green;
                StartCoroutine(ataqueJugadorBueno());
            }
            else
            {
                resp3.color = Color.red;
                if (resp2.text == respuestaCorrecta.ToString())
                {
                    resp2.color = Color.green;
                }
                else if (resp1.text == respuestaCorrecta.ToString())
                {
                    resp1.color = Color.green;
                }
                else
                {
                    resp4.color = Color.green;
                }
                StartCoroutine(ataqueJugadorMalo());
            }
        }
        else
        {
            return;
        }
    }
    public void onAtaqueboton4()
    {

        if (estado == battlestate.PLAYERTURN)
        {
            if (resp4.text == respuestaCorrecta.ToString())
            {
                resp4.color = Color.green;
                StartCoroutine(ataqueJugadorBueno());
            }
            else
            {
                resp4.color = Color.red;
                if (resp2.text == respuestaCorrecta.ToString())
                {
                    resp2.color = Color.green;
                }
                else if (resp3.text == respuestaCorrecta.ToString())
                {
                    resp3.color = Color.green;
                }
                else
                {
                    resp1.color = Color.green;
                }
                StartCoroutine(ataqueJugadorMalo());
            }
        }
        else
        {
            return;
        }
    }

    public string generarOperacion()
    {
        int tipoEjercicio = Random.Range(1, 5);
        int x;
        int y;
        int z;

        int a;
        int b;
        int c;
        switch (nivel)
        {
            case 0:

                x = Random.Range(1, 5);
                y = Random.Range(1, 5);
                z = Random.Range(1, 5);
                switch (tipoEjercicio)
                {
                    case 1:
                        respuestaCorrecta = x + y;
                        operacionCom = x + "+" + y;
                        break;
                    case 2:
                        respuestaCorrecta = x - y;
                        operacionCom = x + "-" + y;
                        break;
                    case 3:
                        respuestaCorrecta = y - x;
                        operacionCom = y + "-" + x;
                        break;
                    case 4:
                        respuestaCorrecta = x + y + z;
                        operacionCom = x + "+" + y + "+" + z;
                        break;
                    case 5:
                        respuestaCorrecta = x + y - z;
                        operacionCom = x + "+" + y + "-" + z;
                        break;

                }
                break;



            case 1:
                Debug.LogError("Nivel: " + nivel);
                x = Random.Range(1, 5);
                y = Random.Range(1, 5);
                z = Random.Range(1, 5);
                switch (tipoEjercicio)
                {
                    case 1:
                        respuestaCorrecta = (x + y) - z;
                        operacionCom = "(" + x + "+" + y + ")" + "-" + z;

                        break;
                    case 2:
                        respuestaCorrecta = (x - y) + z;
                        operacionCom = "(" + x + "-" + y + ")" + "+" + z;
                        break;
                    case 3:
                        respuestaCorrecta = (y - x) + z;
                        operacionCom = "(" + y + "-" + x + ")" + "+" + z;
                        break;
                    case 4:
                        respuestaCorrecta = x - (y + z);
                        operacionCom = x + "-(" + y + "+" + z + ")";
                        break;
                    case 5:
                        respuestaCorrecta = x - (y - z);
                        operacionCom = x + "- (" + y + "-" + z + ")";
                        break;

                }
                break;
            case 2:
                Debug.LogError("Nivel: " + nivel);
                x = Random.Range(1, 10);
                y = Random.Range(1, 10);
                z = Random.Range(1, 10);
                a = Random.Range(1, 10);
                switch (tipoEjercicio)
                {
                    case 1:
                        respuestaCorrecta = (x * y) - z + a;
                        operacionCom = "(" + x + "x" + y + ")" + "-" + z + "+" + a;
                        break;
                    case 2:
                        respuestaCorrecta = (x - y + z) - a;
                        operacionCom = "(" + x + "-" + y + "+" + z + ")" + "-" + a;
                        break;
                    case 3:
                        respuestaCorrecta = y - x + (z + a);
                        operacionCom = y + "-" + x + "+(" + z + "+" + a + ")";
                        break;
                    case 4:
                        respuestaCorrecta = x + (y - z) + a;
                        operacionCom = x + "+" + "(" + y + "-" + z + ")" + "+" + a;
                        break;
                    case 5:
                        respuestaCorrecta = x - (y * z) + a;
                        operacionCom = x + "-" + "(" + y + "X" + z + ")" + "+" + a;
                        break;

                }
                break;

            case 3:
                Debug.LogError("Nivel: " + nivel);
                x = Random.Range(1, 10);
                y = Random.Range(1, 10);
                z = Random.Range(1, 10);
                a = Random.Range(1, 10);
                b = Random.Range(1, 10);
                switch (tipoEjercicio)
                {
                    case 1:
                        respuestaCorrecta = (x * y) - (z + b) - a;
                        operacionCom = "(" + x + "X" + y + ")" + "-" + "(" + z + "+" + b + ")" + "-" + a;
                        break;
                    case 2:
                        respuestaCorrecta = (x - y - b + z) - a;
                        operacionCom = "(" + x + "-" + y + "-" + b + "+" + z + ")" + "-" + a;
                        break;
                    case 3:
                        respuestaCorrecta = y - x + (z + a * b);
                        operacionCom = y + "-" + x + " + " + "(" + z + " + " + a + "X" + b + ")";
                        break;
                    case 4:
                        respuestaCorrecta = (b + x) + y - (z + a);
                        operacionCom = "(" + b + "+" + x + ")" + "+" + y + "-" + "(" + z + "+" + a + ")";
                        break;
                    case 5:
                        respuestaCorrecta = x - (y + (z * a) + b);
                        operacionCom = x + "-" + "(" + y + "+" + "(" + z + "X" + a + ")" + "+" + b + ")";
                        break;

                }
                break;

            case 4:
                Debug.LogError("Nivel: " + nivel);
                x = Random.Range(1, 10);
                y = Random.Range(1, 10);
                z = Random.Range(1, 10);
                a = Random.Range(1, 10);
                b = Random.Range(1, 10);
                c = Random.Range(1, 10);
                switch (tipoEjercicio)
                {

                    case 1:
                        respuestaCorrecta = (x * y - c) - (z + b) - a;
                        operacionCom = "(" + x + "X" + y + "-" + c + ")" + "-" + "(" + z + "+" + b + ")" + "-" + a;
                        break;
                    case 2:
                        respuestaCorrecta = (x - y - (b + z)) - a * c;
                        operacionCom = "(" + x + " - " + y + " - " + "(" + b + " + " + z + ")" + ")" + " - " + a + "X" + c;
                        break;
                    case 3:
                        respuestaCorrecta = y - x + c + (z + a * b);
                        operacionCom = y + "-" + x + "+" + c + "+" + "(" + z + "+" + a + "X" + b + ")";
                        break;
                    case 4:
                        respuestaCorrecta = (b + x - c) + y - (z * a);
                        operacionCom = "(" + b + "+" + x + "-" + c + ")" + "+" + y + "-" + "(" + z + "X" + a + ")";
                        break;
                    case 5:
                        respuestaCorrecta = x - (y + (z * a) + (b - c));
                        operacionCom = x + "-" + "(" + y + "+" + "(" + z + "X" + a + ")" + "+" + "(" + b + "-" + c + ")" + ")";
                        break;

                }
                break;

        }

        return operacionCom;
    }
    public void establecerNivel()
    {
        procentResp = numRespC / numResp;
        if (procentResp > 0.5 && nivel != 4)
        {

            jugadorUnit.aumentarExp();
            jugadorHUD.setExp(jugadorUnit.actualExp);
            nivel++;
            Debug.LogError("Nivel: " + nivel);

        }
        else if (procentResp < 0.5 && nivel != 0)
        {
            jugadorUnit.tomarExp();
            jugadorHUD.setExp(jugadorUnit.actualExp);
            nivel--;
            Debug.LogError("Nivel: " + nivel);

        }
        numRespC = 0;
        numResp = 0;
        exp = jugadorUnit.actualExp;
    }



}




