using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TriggerDoor NotificadorPortaZombieUno;
    public GameObject CVirtPortaZombieUno;
    public GameObject CVirt2PortaZombieUno;
    public GameObject Door1PortaZombieUno;
    public GameObject Door2PortaZombieUno;
    public GameObject Zombie;
    public GameObject TriggerPortaZombieUno;
    public Zombie NotificadorZombieDeath;
    public GameObject movPlata1;
    public GameObject movPlata2;
    public GameObject movPlata3;



    void Start()
    {
        NotificadorPortaZombieUno.StartZombie += SpawnZombieAndCamStart;
        NotificadorZombieDeath.ZombieDeath += SpawnZombieAndCamEnd;
    }
    public void startButton()
    {
        SceneManager.LoadScene("testLevel");
    }

    public void exitButton()
    {
        Application.Quit();
    }
    
    void SpawnZombieAndCamStart()
    {
        Debug.Log("span execu");
        Zombie.SetActive(true);
        Door1PortaZombieUno.SetActive(false);
        Door2PortaZombieUno.SetActive(false);
        TriggerPortaZombieUno.SetActive(false);

        //change cam
        CVirtPortaZombieUno.SetActive(false);
        CVirt2PortaZombieUno.SetActive(true);
    }

    void SpawnZombieAndCamEnd()
    {

        Debug.Log("Morte execu");
        Door1PortaZombieUno.SetActive(true);
        Door2PortaZombieUno.SetActive(true);
        TriggerPortaZombieUno.SetActive(true);

        //change cam
        CVirtPortaZombieUno.SetActive(true);
        CVirt2PortaZombieUno.SetActive(false);

        //movplata
        movPlata1.GetComponent<MovPlata>().IsEnabled = true;
        movPlata2.GetComponent<MovPlata>().IsEnabled = true;
        movPlata3.GetComponent<MovPlata>().IsEnabled = true;
    }
}
