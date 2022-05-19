using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class MovementController : MonoBehaviour
{
    public int[] stats = new int[16];
    public GameObject statsPanel;
    public int XP=0;
    public int LVL=1;
    public int gold = 100;
    public int SkillPoints = 0;
    public bool canAttack = true;
    public Animator weaponAnimation;
    public int ChargedAttack;
    public Coroutine AttackCR;

    public int playerid;
    //camera movement related variables
    public Camera camera;
    PointerEventData PointerEventData;
    EventSystem EventSys;
    public GraphicRaycaster GR;
    private float rotY = 0f;
    private float rotX = 0f;
    public float mouseSensitivity = 100f;
    public float clampAngle = 50f;

    //movement related variables
    public bool isGrounded;
    private Vector3 movement;
    public KeyCode Up = KeyCode.W;
    public KeyCode Down = KeyCode.S;
    public KeyCode Left = KeyCode.A;
    public KeyCode Right = KeyCode.D;
    public KeyCode Jump = KeyCode.Space;

    //statistics related variables
    public Slider healthBar, manaBar, staminaBar;
    public Text healthDisplay, manaDisplay, staminaDisplay;
    public float FieldOfView=5f;

    public float speed = 5f;
    public float jumpStrength = 5f;
    public float maxHealth = 100f;
    public float health;
    public float maxMana = 20f;
    public float mana;
    public float maxStamina=30f;
    public float stamina;

    public Coroutine AutoRegen;
    public float timeRegen = 0.1f;
    public float healthRegen = 1f;
    public float manaRegen = 1f;
    public float staminaRegen = 2f;

    //cost related variables
    public float jumpStamina = 5f;
    private void Awake()
    {
        camera = GameObject.Find("Camera" + playerid).GetComponent<Camera>();
        //Cursor.lockState = CursorLockMode.Locked;

    }
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        mana = maxMana;
        stamina = maxStamina;
        setUI();
        AutoRegen=StartCoroutine(Regen());
        Cursor.lockState = CursorLockMode.Locked;
        GR = camera.GetComponent<GraphicRaycaster>();
        EventSys = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        //Cursor.visible=false;
    }
    IEnumerator Regen()
    {
        while(true)
        {
            health += healthRegen * timeRegen;
            health=Mathf.Clamp(health, 0, maxHealth);
            mana += manaRegen * timeRegen;
            mana = Mathf.Clamp(mana, 0, maxMana);
            stamina += staminaRegen * timeRegen;
            stamina = Mathf.Clamp(stamina, 0, maxStamina);
            updateUI();
            yield return new WaitForSeconds(timeRegen);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {/*
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        rotY += mouseX * mouseSensitivity * Time.deltaTime;
        rotX += -mouseY * mouseSensitivity * Time.deltaTime;
        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
        Quaternion localRotation = Quaternion.Euler(0, rotY, 0f);
        transform.rotation = localRotation;
        movement = Vector3.zero;
        camera.transform.position = transform.position + new Vector3(0f,1f,0f);
        camera.transform.rotation = Quaternion.Euler(rotX, rotY, 0f);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        movement = transform.right * x + transform.forward * z;
        GetComponent<Rigidbody>().AddForce(movement.x*speed*Time.deltaTime *100f * GetComponent<Rigidbody>().mass, 0f, movement.z * speed * Time.deltaTime *100f * GetComponent<Rigidbody>().mass);
        GameObject.Find("Player/Weapon").GetComponent<Transform>().position = transform.position + GameObject.Find("Player/Weapon").GetComponent<broadSwordAttack>().localpos;
        //   transform.position += movement.normalized * speed * Time.deltaTime;
    */
        }
    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        if (!GameObject.Find("StatsPanel") && !GameObject.Find("In-Game Menu Background"))
        {
            rotY += mouseX * mouseSensitivity * Time.deltaTime;
            rotX += -mouseY * mouseSensitivity * Time.deltaTime;
        }
        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
        Quaternion localRotation = Quaternion.Euler(0, rotY, 0f);
        transform.rotation = localRotation;
        movement = Vector3.zero;
        camera.transform.position = transform.position + new Vector3(0f, 1f, 0f);
        camera.transform.rotation = Quaternion.Euler(rotX, rotY, 0f);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        movement = transform.right * x + transform.forward * z;
        GetComponent<Rigidbody>().AddForce(movement.x * speed * Time.deltaTime * 100f * GetComponent<Rigidbody>().mass, 0f, movement.z * speed * Time.deltaTime * 100f * GetComponent<Rigidbody>().mass);
        GameObject.Find("Player/Weapon").GetComponent<Transform>().position = transform.position + GameObject.Find("Player/Weapon").GetComponent<broadSwordAttack>().localpos;

        if (!GameObject.Find("StatsPanel") && !GameObject.Find("In-Game Menu Background"))
        {
            if (isGrounded && Input.GetKeyDown(Jump) && stamina > jumpStamina)
            {
                isGrounded = false;
                UseStamina(jumpStamina);
                GetComponent<Rigidbody>().velocity += new Vector3(0f, jumpStrength, 0f);
            }
            if (Input.GetKeyDown(KeyCode.F) && !GameObject.Find("In-Game Menu Background"))
            {
                if (!GameObject.Find("StatsPanel"))
                {
                    GameObject GO = Instantiate(statsPanel);
                    GO.name = "StatsPanel";
                    GO.transform.SetParent(GameObject.Find("/Canvas").transform);
                    GO.GetComponent<RectTransform>().offsetMin = GO.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
                }
                else
                {
                    //Cursor.visible = false;
                    Destroy(GameObject.Find("StatsPanel"));
                }
            }
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                CheckGraphicRaycast();
                AttackCR=StartCoroutine(ChargedAttackCR());
            }
            if(Input.GetKeyUp(KeyCode.Mouse0))
            {
                StopCoroutine(AttackCR);
                if(ChargedAttack<40)
                {
                    GameObject.Find("Weapon").GetComponent<broadSwordAttack>().PerformAttack();
                }
                else
                {
                    GameObject.Find("Weapon").GetComponent<broadSwordAttack>().PerformChargedAttack();
                }
                ChargedAttack = 0;
            }
        }
    }
    private void CheckGraphicRaycast()
    {
        PointerEventData = new PointerEventData(EventSys);
        PointerEventData.position = Input.mousePosition;
        Debug.Log(PointerEventData.position);
        List<RaycastResult> results = new List<RaycastResult>();
        GR.Raycast(PointerEventData, results);
        Debug.Log(results.Count);
        foreach(RaycastResult result in results)
        {
            Debug.Log("Hit " + result.gameObject.name);
        }
    }
    private IEnumerator ChargedAttackCR()
    {
        while(ChargedAttack<500&&Input.GetKey(KeyCode.Mouse0))
        {
            Debug.Log("Ładowanie ataku: " + ChargedAttack + " na 500");
            ChargedAttack++;
            yield return new WaitForSeconds(0.01f);
        }
        GameObject.Find("Weapon").GetComponent<broadSwordAttack>().PerformChargedAttack();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name=="Floor")
        {
            isGrounded = true;
        }
        /*  if(collision.gameObject.name=="Wall")
          {
              Vector3 newVelocity = GetComponent<Rigidbody>().velocity;
              newVelocity.x *= 0;
              newVelocity.z *= 0;
              GetComponent<Rigidbody>().velocity = newVelocity;
          }*/
        if (collision.gameObject.name == "MouseOn")
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.name == "Floor")
        {
            isGrounded = true;
        }
        if (collision.gameObject.name == "MouseOn")
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Floor")
        {
            isGrounded = false;
        }
        if (collision.gameObject.name == "MouseOn")
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
    }
    public void UseMana(float manaAmount)
    {
        mana -= manaAmount;
    }
    public void UseStamina(float staminaAmount)
    {
        stamina -= staminaAmount;
    }
    private void setUI()
    {
        healthBar.GetComponent<Slider>().maxValue = maxHealth;
        manaBar.GetComponent<Slider>().maxValue = maxMana;
        staminaBar.GetComponent<Slider>().maxValue = maxStamina;
        updateUI();
    }
    private void updateUI()
    {
        healthBar.GetComponent<Slider>().value = health;
        manaBar.GetComponent<Slider>().value = mana;
        staminaBar.GetComponent<Slider>().value = stamina;
        healthDisplay.GetComponent<Text>().text = (int)health + "/" + (int)maxHealth;
        manaDisplay.GetComponent<Text>().text = (int)mana + "/" + (int)maxMana;
        staminaDisplay.GetComponent<Text>().text = (int)stamina + "/" + (int)maxStamina;
    }
    public void getXP(int amount)
    {
        XP += amount;
        if(XP>=(100+(LVL-1)*20))
        {
            XP -= (100 + (LVL - 1) * 20);
            LVL += 1;
            SkillPoints += 1;
        }
    }
}
