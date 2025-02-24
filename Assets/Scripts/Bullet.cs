
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun
{
    // Velocidad de la bala
    private float speed;

    // Due�o de la bala (jugador que la dispar�)
    private Photon.Realtime.Player owner;

    // M�todo para inicializar la bala
    public void Initialize(float bulletSpeed, Photon.Realtime.Player bulletOwner)
    {
        speed = bulletSpeed; // Asigna la velocidad
        owner = bulletOwner; // Asigna el due�o
    }

    void Start()
    {
        // Solo el due�o del PhotonView ejecuta esta l�gica
        if (photonView.IsMine)
        {
            // Programa la destrucci�n de la bala despu�s de 1 segundo
            Invoke("DestroyBullet", 1f);
        }
    }

    void Update()
    {
        // Solo el due�o del PhotonView ejecuta esta l�gica
        if (photonView.IsMine)
        {
            // Mueve la bala hacia adelante en su direcci�n actual
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Solo el due�o del PhotonView ejecuta esta l�gica
        if (photonView.IsMine)
        {
            // Verifica si la bala choc� con algo que no sea el jugador
            if (!other.CompareTag("Player"))
            {
                DestroyBullet(); // Destruye la bala
            }
        }
    }

    void DestroyBullet()
    {
        // Solo el due�o del PhotonView ejecuta esta l�gica
        if (photonView.IsMine)
        {
            // Destruye la bala en la red
            PhotonNetwork.Destroy(gameObject);
        }
    }
}