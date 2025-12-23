using Unity.Cinemachine;
using UnityEngine;

public class TeleportScript : MonoBehaviour
{
    [SerializeField] private Transform otherWall;
    //[SerializeField] private CinemachineCamera otherCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")||other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if(other.gameObject.layer == LayerMask.NameToLayer("Enemy") && other.transform.parent==null)
            {
                Debug.LogWarning("who?"+other.gameObject.name+"");
                //other.gameObject.GetComponent<Enemy>().ChangeDirection();
            }
            float sizeX =other.gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
            if(otherWall.name == "WallL")
            {
                other.gameObject.transform.position =  new Vector3(otherWall.position.x+10f+sizeX,other.gameObject.transform.position.y,other.gameObject.transform.position.z);
            }
            else if(otherWall.name == "WallR")
            {
                other.gameObject.transform.position = new Vector3(otherWall.position.x-10f-sizeX,other.gameObject.transform.position.y,other.gameObject.transform.position.z);
            }
            else
            {
                Debug.LogError("Name of wall"+otherWall.name);
            }
            //CameraManager.SwitchCamera(otherCamera);
            
            
        }
    }
}
