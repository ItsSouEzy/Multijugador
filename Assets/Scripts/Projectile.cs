using Photon.Pun;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float speed = 10f; // Velocidad del proyectil
    public float lifetime = 5f; // Tiempo de vida del proyectil
    private Vector3 direction; // Direcci�n del proyectil

    void Start()
    {
        Destroy(gameObject, lifetime); // Destruir el proyectil despu�s de un tiempo
    }

    void Update()
    {
        // Mover el proyectil en la direcci�n asignada
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    // M�todo para establecer la direcci�n del proyectil
    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Si el proyectil choca con un jugador, causar da�o
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.photonView.RPC("TakeDamage", RpcTarget.All, 10); // Causar 10 de da�o
            }
        }

        Destroy(gameObject); // Destruir el proyectil al chocar
    }



}
