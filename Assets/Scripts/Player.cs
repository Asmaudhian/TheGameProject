using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float maxSpeed = 15;
    public float speed = 8f;
    public float jumpPower = 100f;

    public GameObject clonePlayer;
    private Vector3 pos;
    // Instantiate variables for the tablePos (clone is 60 frames behind)
    private int i = 0;
    private int j = 0;
    // Declare Transforms for both player and clone
    private Transform originalTransform;
    private Transform cloneTransform;
    // Declare a table of Vector3s with a size of 120 (60fps 2s)
    private Vector3[] posTable;
    private int frameDelay = 120;


    public bool grounded;

    private Rigidbody2D rb2d;
    private Animator anim;

	// Use this for initialization
	void Start () {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();

        Application.targetFrameRate = 60;
        j = frameDelay / 2;
        posTable = new Vector3[frameDelay];
        originalTransform = this.gameObject.GetComponent<Transform>();
        pos = new Vector3(originalTransform.position.x, originalTransform.position.y, originalTransform.position.z);
        StartCoroutine(cloneStart());
       // clonePlayer.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
    }
	
	// Update is called once per frame
	void Update () {
        anim.SetBool("Grounded", grounded);
        anim.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Horizontal")));

        if (Input.GetAxis("Horizontal") < -0.1f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (Input.GetAxis("Horizontal") > 0.1f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.UpArrow))
        {
            rb2d.AddForce(Vector2.up * jumpPower);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            originalTransform.position = cloneTransform.position; // Error here, but why ? 
        }
    }

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");

        // Moving player left and right
        rb2d.AddForce((Vector2.right * speed) * h);

        // Limiting the speed of the player
        if(rb2d.velocity.x > maxSpeed)
        {
            rb2d.velocity = new Vector2(maxSpeed, rb2d.velocity.y);
        }

        if(rb2d.velocity.x < -maxSpeed)
        {
            rb2d.velocity = new Vector2(-maxSpeed, rb2d.velocity.y);
        }

        // if variables bigger than tablePos.size set to zero
        if (i > frameDelay - 1)
            i = 0;

        if(j > frameDelay - 1)
            j = 0;

        //Add current position to the current table index
        posTable[i] = (new Vector3(originalTransform.position.x, originalTransform.position.y, originalTransform.position.z));

        //if cloneTransform has been instantiated set his position to 60 frames behind the player
        if (cloneTransform)
            cloneTransform.position = posTable[j];

        // Increment the variables
        i++;
        j++;
    }

    IEnumerator cloneStart()
    {
        yield return new WaitForSeconds(frameDelay / (frameDelay / 2));
        Vector2 cloneStartPos = posTable[j];
        Instantiate(clonePlayer, cloneStartPos, Quaternion.identity);

        //Instantiate the clones transform
        cloneTransform = GameObject.Find("Clone(Clone)").transform;
    }

}
