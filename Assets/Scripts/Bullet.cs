using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed = 7;
    PhotonView PV;
    float impact;
    float maxDistance = 15;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        this.transform.Translate(new Vector3(0.5f, 0, 0.3f));
        GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(0, 0, speed));
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {
            if (this.transform.position.x > maxDistance || this.transform.position.x < -maxDistance || this.transform.position.z > maxDistance || this.transform.position.z < -maxDistance)
                PhotonNetwork.Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (PV != null && PV.IsMine)
        {
            Vector3 impactVector = this.transform.forward;
            impactVector.y = 0.5f;

            Vector3 force = 0.04f * impactVector.normalized * impact;
            PV.RPC("RPC_Hit", collision.transform.gameObject.GetComponentInChildren<PhotonView>().Owner, force, collision.transform.gameObject.GetComponentInChildren<PhotonView>().Owner.ActorNumber);

            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    public void setImpact(float impact)
    {
        this.impact = impact;
    }

    [PunRPC]
    private void RPC_Hit(Vector3 force, int player)
    {
        foreach (GameObject GO in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (GO.GetComponentInChildren<PhotonView>().Owner.ActorNumber == player)
            {
                GO.GetComponent<Rigidbody>().AddForce(force / GO.GetComponent<PlayerController>().localPlayerData.endurance, ForceMode.Impulse);
            }
        }
    }
}
