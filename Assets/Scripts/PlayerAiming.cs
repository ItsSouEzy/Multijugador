using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;




public class PlayerAiming : MonoBehaviourPun
{



    
    


        void Update()
        {
            // Verifica si el jugador local es el due�o del PhotonView
            if (photonView.IsMine)
            {
                // Obtener la posici�n del cursor en el mundo
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                // Lanzar un rayo desde la c�mara hacia la posici�n del cursor
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    // Calcular la direcci�n desde el jugador hacia el punto de impacto del rayo
                    Vector3 direction = hit.point - transform.position;

                    // Ignorar la componente Y para evitar rotaciones no deseadas en el eje vertical
                    direction.y = 0;

                    // Rotar el jugador hacia la direcci�n del cursor
                    if (direction != Vector3.zero)
                    {
                        // Calcular la rotaci�n objetivo usando la direcci�n
                        Quaternion targetRotation = Quaternion.LookRotation(direction);

                        // Suavizar la rotaci�n del jugador hacia la direcci�n del cursor
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
                    }
                }
            }
        }







    }
