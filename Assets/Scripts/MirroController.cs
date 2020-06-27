using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirroController : MonoBehaviour
{
    GameObject [] groundItems = null;

    [Range(0f,3f)]
    public float rotateSpeed = 1f;

    bool isAtDestination = true;

    bool isUp = true;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        GetGroundItemsFromScene();
        SetMirroredGroundItems();
    }

    private void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.Space) && isUp == true)
        {
            //LeanTween.rotateX(this.gameObject, -180f, rotateSpeed);
            //isUp = false;
            //StartCoroutine(rotateOverTime());
            this.gameObject.transform.LeanRotateX(180f, 1f);
        }

        //LOCK
        //if(isAtDestination == false)
        //{
        //    transform.Rotate(30 * Time.deltaTime, 0, 0);
        //    Debug.Log(this.gameObject.transform.rotation.x);
        //    if(this.gameObject.transform.rotation.x >= 179)
        //    {
        //        Debug.Log("over");
        //    }
        //}
    }

    IEnumerator rotateOverTime()
    {
        isAtDestination = false;
        Debug.Log(this.gameObject.transform.rotation.x);
        yield return new WaitUntil(() => this.gameObject.transform.rotation.x >= 170);
        isAtDestination = true;
    }

    public void GetGroundItemsFromScene(){
        groundItems = GameObject.FindGameObjectsWithTag("LevelGround");
    }

    public void SetMirroredGroundItems(){
        foreach (GameObject groundItem in groundItems) {
            GameObject mirroredGround = Instantiate(groundItem, this.gameObject.transform, true);
            mirroredGround.transform.SetPositionAndRotation(new Vector3(groundItem.transform.position.x, groundItem.transform.position.y*-1, groundItem.transform.position.z), new Quaternion(180f,0,0,0));
        }
    }




}
