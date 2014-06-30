using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;
using System.Text;

public class PlayerScript : MonoBehaviour
{
    public bool inConversation;

    public bool isWalkingForward = false,
                isWalkingBack = false,
                isWalkingLeft = false,
                isWalkingRight = false;

    private float timer = 0.0f;
    public float bobbingSpeed;
    public float bobbingAmount;
    public float midpoint;
    Vector3 newPos;

    public Transform targetForward, targetBack, targetLeft, targetRight;
    public float speed;

	public AnimationManager spriteAnim;

    private bool _hitWallLeft = false,
                 _hitWallRight = false,
                 _hitWallForward = false,
                 _hitWallBack = false;

    enum LastKey { Up, Down, Left, Right };
    LastKey lastKeyPressed;

    float waveslice = 0.0f;

    public bool chooseCharacter = true;

    public Material Sanome, Sarylyn;

    public GameObject thoughtCloud, spiral;
    public GameManager spiralTimer;

    private Character currentCharacter;
    public Texture2D SarylynPortrait, SanomePortrait;

    public GameObject Klarrissa, Gonzo;

    public static bool timerStartOnce;

    public GameObject playerSpawnHallway;//, playerSpawnLabrary;


    void Start()
    {

        this.renderer.enabled = false;
        chooseCharacter = true;
        this.renderer.enabled = true;
        spiralTimer.startTime = true;
        midpoint = transform.position.y;
        
          switch (MainMenu.SelectedCharacter)
        {
            case MainMenu.CharacterChoice.SANOME:
                this.renderer.material = Sanome;
                SetAnimation("Idle");
                DialogueManager.MasterDatabase.GetActor("Player").portrait = SanomePortrait;
                if (Klarrissa != null)
                    Klarrissa.gameObject.active = false;
                break;

            case MainMenu.CharacterChoice.SARYLYN:
                this.renderer.material = Sarylyn;
                SetAnimation("Idle");
                DialogueManager.MasterDatabase.GetActor("Player").portrait = SarylynPortrait;
                if (Gonzo != null)
                    Gonzo.gameObject.active = false;
                break;
        }
    }

    void Awake()
    {
        
        //if (Application.loadedLevelName == "labrary")
            //this.gameObject.transform.position = playerSpawnLabrary.transform.position;
    }


    void Update()
    {

        if (inConversation)
            spiralTimer.showTimer = false;  
        else
            spiralTimer.showTimer = true;     
        //temp scene trans

        if (Application.loadedLevelName == "hallway")
        {
            if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter ))
            {
                Application.LoadLevel("labrary");
            }
        }
        else if (Application.loadedLevelName == "labrary")
        {
            if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter ))
            {
                //if (Application.loadedLevelName == "hallway")
                this.gameObject.transform.position = playerSpawnHallway.transform.position;
                Application.LoadLevel("hallway");
            }
        }

        if (spiralTimer.startTime)
        {

            //Headbobbing script
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            if (!isWalkingForward && !isWalkingBack && !isWalkingLeft && !isWalkingRight)//Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
            {
                timer = 0.0f;
            }
            else
            {
                waveslice = Mathf.Sin(timer);
                timer = timer + bobbingSpeed;
                if (timer > Mathf.PI * 2)
                {
                    timer = timer - (Mathf.PI * 2);
                }
            }
            if (waveslice != 0)
            {
                float translateChange = waveslice * bobbingAmount;
                float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
                totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
                translateChange = totalAxes * translateChange;
                newPos = transform.position;
                newPos.y = midpoint + translateChange;
                transform.position = newPos;
            }
            else
            {
                Vector3 findMidpoint = transform.position;
                findMidpoint.y = midpoint;
                transform.position = findMidpoint;
            }

            //if statements check to see which scene the player is in then calls the appropriate method 
            if (Application.loadedLevelName == "hallway")
                PlayerMovementHallway();

            if (Application.loadedLevelName == "labrary")
                PlayerMovementLabrary();
        }

        if (_hitWallLeft && Input.GetKeyDown(KeyCode.RightArrow))
            _hitWallLeft = false;
        if (_hitWallRight && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _hitWallRight = false;
        }
        if (_hitWallForward && Input.GetKeyDown(KeyCode.DownArrow))
            _hitWallForward = false;
        if (_hitWallBack && Input.GetKeyDown(KeyCode.UpArrow))
            _hitWallBack = false;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag != "Floor")
        {
            Vector3 centerColPoint = new Vector3();

            //Finds centermost point of collision
            foreach (ContactPoint colPoint in col.contacts)
            {
                float centerDistance = 50;
                if (Vector3.Distance(colPoint.point, transform.position) < centerDistance)
                {
                    centerDistance = Vector3.Distance(colPoint.point, transform.position);
                    centerColPoint = colPoint.point;
                }
            }

            //Checks to see if the centermost point of collision is on the same side as the player is moving.
            if (lastKeyPressed == LastKey.Up && Vector3.Distance(centerColPoint, targetForward.transform.position) < Vector3.Distance(centerColPoint, targetLeft.transform.position) && Vector3.Distance(centerColPoint, targetForward.transform.position) < Vector3.Distance(centerColPoint, targetRight.transform.position))
                _hitWallForward = true;
            if (lastKeyPressed == LastKey.Down && Vector3.Distance(centerColPoint, targetBack.transform.position) < Vector3.Distance(centerColPoint, targetLeft.transform.position) && Vector3.Distance(centerColPoint, targetBack.transform.position) < Vector3.Distance(centerColPoint, targetRight.transform.position))
                _hitWallBack = true;
            if (lastKeyPressed == LastKey.Left && Vector3.Distance(centerColPoint, targetLeft.transform.position) < Vector3.Distance(centerColPoint, targetForward.transform.position) && Vector3.Distance(centerColPoint, targetLeft.transform.position) < Vector3.Distance(centerColPoint, targetBack.transform.position))
                _hitWallLeft = true;
            if (lastKeyPressed == LastKey.Right && Vector3.Distance(centerColPoint, targetRight.transform.position) < Vector3.Distance(centerColPoint, targetForward.transform.position) && Vector3.Distance(centerColPoint, targetRight.transform.position) < Vector3.Distance(centerColPoint, targetBack.transform.position))
                _hitWallRight = true;
        }

    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.collider.tag == "Actor")
        {
            spiralTimer.startTime = false;
            spiralTimer.thoughtCloud.renderer.enabled = false;
            spiralTimer.renderer.enabled = false;
            this.gameObject.renderer.enabled = false;
        }
       
    }

    //Method for player movement while in the hallway
    private void PlayerMovementHallway()
    {
        float step = speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            lastKeyPressed = LastKey.Up;
            if (!_hitWallForward)
            {
                isWalkingForward = true;
                transform.position = Vector3.MoveTowards(transform.position, targetForward.position, step);
            }
            else
            {
                isWalkingForward = false;
                Vector3 tmp = transform.position;
                transform.position = tmp;
            }
        }
        else
            isWalkingForward = false;

        if (Input.GetKey(KeyCode.DownArrow))
        {
            lastKeyPressed = LastKey.Down;
            if (!_hitWallBack)
            {
                isWalkingBack = true;
                transform.position = Vector3.MoveTowards(transform.position, targetBack.position, step);
            }
            else
            {
                isWalkingBack = false;
                Vector3 tmp = transform.position;
                transform.position = tmp;
            }
        }
        else
            isWalkingBack = false;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            lastKeyPressed = LastKey.Left;
            if (!_hitWallLeft)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetLeft.position, step);
                isWalkingLeft = true;
            }
            else
            {
                isWalkingLeft = false;
                Vector3 tmp = transform.position;
                transform.position = tmp;
            }
        }
        else
            isWalkingLeft = false;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            lastKeyPressed = LastKey.Right;
            if (!_hitWallRight)
            {
                isWalkingRight = true;
                transform.position = Vector3.MoveTowards(transform.position, targetRight.position, step);

            }
            else
            {
                isWalkingRight = false;
                Vector3 tmp = transform.position;
                transform.position = tmp;
            }
            //transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z );
        }
        else
            isWalkingRight = false;
    }

    //Method for player movement while in the labrary
    private void PlayerMovementLabrary()
    {
        float step = speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            lastKeyPressed = LastKey.Left;
            if (!_hitWallLeft)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetLeft.position, step);
                isWalkingLeft = true;
            }
            else
            {
                isWalkingLeft = false;
                Vector3 tmp = transform.position;
                transform.position = tmp;
            }
        }
        else
            isWalkingLeft = false;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            lastKeyPressed = LastKey.Right;
            if (!_hitWallRight)
            {
                isWalkingRight = true;
                transform.position = Vector3.MoveTowards(transform.position, targetRight.position, step);

            }
            else
            {
                isWalkingRight = false;
                Vector3 tmp = transform.position;
                transform.position = tmp;
            }
            //transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z );
        }
        else
            isWalkingRight = false;
    }

    public void SetAnimation(string anim, float delay = 0.0f)
    {
        StartCoroutine(SetAnimationRoutine(anim, delay));
    }
    public IEnumerator SetAnimationRoutine(string anim, float delay = 0.0f)
    {
        if (spriteAnim != null)
        {
            yield return new WaitForSeconds(delay);
            StopCoroutine("SetAnimationRoutine");
            spriteAnim.SetState(anim);
        }
    }
}