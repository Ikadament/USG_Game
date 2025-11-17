using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerControls : MonoBehaviour
{
    //Walk variables
    [SerializeField] private float moveSpeed = 7f;
    private float horizontalInput;

    //Jump variables
    public Rigidbody2D rb;
    public float jumpAmount = 5f;
    public float jumpCutMultiplier = 0.5f;
    public GroundCheck groundCheck;

    public bool isTalking;

    //Coyote Time Variables
    public float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    //Jump Buffer Variables
    public float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    //Change color mechanic Variables
    public SpriteRenderer spriteRenderer;
    public Color orangeColor;
    public Color blueColor;
    public bool isOrange;

    //Gamestate finder
    public GameObject gameStatePrefab;
    private string gameStateName = "GameState";
    public bool gameStateIsCreated = false;

    //portals ref bon normalement ca devrait etre un autre script
    private GameObject[] allPortalOrange;
    private GameObject[] allPortalBlue;
    private float lowOpacityValue = 0.5f;
    private float highOpacityValue = 0.9f;

    private void Start()
    {
        //portal stuff
        allPortalOrange = GameObject.FindGameObjectsWithTag("PortalOrange");
        allPortalBlue = GameObject.FindGameObjectsWithTag("PortalBleu");

        //gamestate stuff
        GameObject gameState = GameObject.Find(gameStateName);

        if(gameState == null)
        {
            Debug.Log("Cree!");
            gameState = Instantiate(gameStatePrefab);
            gameState.name = gameStateName;
            gameState.AddComponent<GameStateScript>();
            GameStateScript gameStateScript = gameState.GetComponent<GameStateScript>();
            gameStateScript.isCreated = true;
        }

        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            if(GameStateScript.instance.isOrange)
           {
                ChangeColorAndLayerMatrixToOrange();
                GameStateScript.instance.isOrange = true;
            }
            else
            {
                ChangeColorAndLayerMatrixToBlue();
                GameStateScript.instance.isOrange = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isTalking)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float horizontalMovement = horizontalInput * moveSpeed;
            rb.velocity = new Vector2(horizontalMovement, rb.velocity.y);
        }
        
    }

    private void Update()
    {

        //jumping

        if (groundCheck.isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0 && !isTalking) 
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpAmount);

            jumpBufferCounter = 0f;
        }

        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0f && !isTalking)
        {
            coyoteTimeCounter = 0f;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpCutMultiplier);
        }

        // changing color, and thus layer

        if (Input.GetKeyDown(KeyCode.L) && !isTalking)
        {
            if (GameStateScript.instance.isOrange)
            {
                ChangeColorAndLayerMatrixToBlue();
                FindObjectOfType<AudioManager>().Play("change");
            }
            else
            {
                ChangeColorAndLayerMatrixToOrange();
                FindObjectOfType<AudioManager>().Play("change");
            }

            GameStateScript.instance.isOrange = !GameStateScript.instance.isOrange;
        }

        // reset 

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                ResetLevel();
            }
        }
    }

    private void ResetLevel()
    {
        FindObjectOfType<AudioManager>().Play("pngTalk2");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ChangeColorAndLayerMatrixToBlue()
    {
        spriteRenderer.color = blueColor;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("PortalOne"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("PortalTwo"), false);

        //Change opacity of portals for visibility
        ChangePortalOpacityToHigh(allPortalBlue);
        ChangePortalOpacityToLow(allPortalOrange);

        //change layer variable from groundcheckscript
        groundCheck.layers = LayerMask.GetMask("ground", "MovingObjectsOne", "MovingObjectsTwo", "PortalTwo", "Button", "BoxButton", "MovingPlatform", "Door");
    }

    private void ChangeColorAndLayerMatrixToOrange()
    {
        spriteRenderer.color = orangeColor;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("PortalOne"), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("PortalTwo"), true);

        //Change opacity of portals for visibility
        ChangePortalOpacityToHigh(allPortalOrange);
        ChangePortalOpacityToLow(allPortalBlue);

        //change layer variable from groundcheckscript
        groundCheck.layers = LayerMask.GetMask("ground", "MovingObjectsOne", "MovingObjectsTwo", "PortalOne", "Button", "BoxButton", "MovingPlatform", "Door");
    }

    private void ChangePortalOpacityToLow(GameObject[] allPortals)
    {
        foreach (GameObject portal in allPortals)
        {
            SpriteRenderer rend = portal.GetComponent<SpriteRenderer>();
            if (rend != null)
            {
                Color color = rend.color;
                color.a = lowOpacityValue;
                rend.color = color;
            }
        }
    }

    private void ChangePortalOpacityToHigh(GameObject[] allPortals)
    {
        foreach (GameObject portal in allPortals)
        {
            SpriteRenderer rend = portal.GetComponent<SpriteRenderer>();
            if (rend != null)
            {
                Color color = rend.color;
                color.a = highOpacityValue;
                rend.color = color;
            }
        }
    }

    public void FreezePlayerMovement()
    {
        isTalking = true;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }

    public void DefrostPlayerMovement()
    {
        isTalking = false;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
