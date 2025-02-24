using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerShooting : MonoBehaviourPun
{
    // Punto desde donde se disparan las balas
    public Transform firePoint;

    // Velocidad de la bala
    public float bulletSpeed = 10f;

    // Prefab de la bala (debe estar en la carpeta Resources)
    public GameObject bulletPrefab;

    void Update()
    {
        // Verifica si el jugador local es el due�o del PhotonView y si presiona el bot�n de disparo
        if (photonView.IsMine && Input.GetButtonDown("Fire1"))
        {
            Shoot(); // Llama al m�todo para disparar
        }
    }

    void Shoot()
    {
        // Instancia la bala en la red usando PhotonNetwork.Instantiate
        // Solo el jugador local (due�o del PhotonView) ejecuta esta l�gica
        GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, firePoint.position, firePoint.rotation);

        // Inicializa la bala con la velocidad y el due�o (jugador que dispar�)
        bullet.GetComponent<Bullet>().Initialize(bulletSpeed, photonView.Owner);
    }
}
