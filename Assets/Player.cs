using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputMode
{
    NONE,
    TELEPORT,
    WALK,
    FURNITURE,

    TRANSLATE,
    ROTATE,
    SCALE,

    DRAG

}

public class Player : MonoBehaviour
{
    public static Player instance;
    public InputMode activeMode = InputMode.NONE;

    public Object activeFurniturePrefab;

    [SerializeField]
    private float playerSpeed = 3.0f;

    public GameObject leftwall;
    public GameObject rightwall;

    public GameObject forwardwall;
    public GameObject backwall;

    public GameObject ceiling;
    public GameObject floor;


    void Awake()
    {
        if (instance != null)
        {
            GameObject.Destroy(instance.gameObject);
        }

        instance = this;

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        TryWalk();
    }

    public void TryWalk()
    {
        if (Input.GetMouseButton(0) && activeMode == InputMode.WALK)
        {
            Vector3 forward = Camera.main.transform.forward;

            Vector3 newPosition = transform.position + forward * Time.deltaTime * playerSpeed;

            if (newPosition.x < rightwall.transform.position.x && newPosition.x > leftwall.transform.position.x &&
                newPosition.y < ceiling.transform.position.y && newPosition.y > floor.transform.position.y &&
                newPosition.z < forwardwall.transform.position.z && newPosition.z > backwall.transform.position.z)
            {
                transform.position = newPosition;

            }

        }
    }
}
