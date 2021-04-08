using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour
{
    // Variables publicas de control, para modificarlas en el editor
    [Range(0, 15)] public float speed;
    public float maxHealth = 100f, slowDuration = .5f;
    public GameObject projectile, necromancer;
    public Transform projectileSpawn;
    public Transform[] necromancerSpawners = new Transform[4];
    public int maxNecromancerSpawns;
    public RectTransform healBar;
    public AudioClip damage;

    //Variables privadas del boss
    private BossState currentState;
    private Rigidbody2D rb2d;
    private float health, attackCD;
    private Animator animator;
    private List<Transform> targets = new List<Transform>();
    private bool isSlowed;
    private SpriteRenderer sprt;
    private int necromancers = 0;

    public float Health { get => health; set => health = value; }
    public float AttackCD { get => attackCD; set => attackCD = value; }
    public BossState CurrentState { get => currentState;}
    public Animator Animator { get => animator; set => animator = value; }
    public Rigidbody2D RB2D { get => rb2d; set => rb2d = value; }
    public int Necromancers { get => necromancers; set => necromancers = value; }

    private void Awake()
    {
        //Ejecutamos todo este codigo en el awake ya que este codigo tiene que ejecutarse antes de desactivar al boss en el spawner.

        //Se multiplica la vida del boss para que encaje con los players que hay en juego.
        maxHealth = maxHealth * GameManager.Instance.players;
        health = maxHealth;
        isSlowed = false;

        sprt = GetComponent<SpriteRenderer>();
        currentState = new BossFightState();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
    }
    private void Start()
    {
        //Establecemos la lista de los players en el start ya que necesitamos que
        //el awake del player spawner instancie a los jugadores necesarios antes.
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            targets.Add(player.transform);
        }
    }

    //Desactivamos el ataque al desactivar el boss, para asegurarnos que esto no de errores al desactivar el boss
    private void OnDisable()
    {
        CancelInvoke("Attack");
    }
    //Volvemos a activar el ataque al activar de nuevo el boss
    private void OnEnable()
    {
        InvokeRepeating("Attack", attackCD, attackCD);
    }
    // Update is called once per frame
    void Update()
    {
        currentState.endAttacking();
    }
    //Movimientos del rigidbody y el transform en el fixedUpdate
    private void FixedUpdate()
    {
        currentState.Move();
        currentState.Rotate();
    }
    //En el lateUpdate comprueba si tiene que cambiar de fase
    void LateUpdate()
    {
        currentState = currentState.GetNextState();
    }
    //Esto se encuentra en una función para que se pueda ejecutar con el InvokeRepeating
    private void Attack()
    {
        currentState.Attack();
    }

    //Función para determinar cual es el jugador mas cercano
    public Vector3 GetClosestTarget()
    {
        if (targets.Count > 1)
            if ((targets[0].position - transform.position).sqrMagnitude > (targets[1].position - transform.position).sqrMagnitude)
                return targets[1].position;
        return targets[0].position;
    }

    //Función que instancia un proyectil
    public void Shoot()
    {
        Instantiate(projectile, projectileSpawn.position, projectileSpawn.rotation);
    }

    //Función que instancia los nigromantes
    public void Spawn(int index)
    {
        necromancers++;
        Instantiate(necromancer, necromancerSpawners[index].position, necromancerSpawners[index].rotation, transform);
    }

    //Función que se ejecuta al morir el boss, incluye la terminación del juego, aunque creo que eso debería ir en el gameManager, 
    //no estaba del todo seguro de esto.
    public void Die()
    {
        GameManager.Instance.gameCompleted = true;
        Destroy(gameObject);
        SceneManager.LoadScene("EndScreen");
    }

    //Las siguientes funciones controlan el estado de congelación del boss al recibir impactos de un proyectil de hielo
    public void Froze()
    {
        //Si el enemigo no esta congelado lo congela
        if (!isSlowed)
        {
            isSlowed = true;
            ApplyIceDebuff();
            Invoke("DisableSlow", slowDuration);
        }
        //Si el enemigo esta congelado, reinicia el tiempo que tarda en quitarse el "debuff"
        else
        {
            CancelInvoke("DisableSlow");
            Invoke("DisableSlow", slowDuration);
        }
    }
    private void DisableSlow()
    {
        DismissIceDebuff();
        isSlowed = false;
    }
    public void ApplyIceDebuff()
    {
        speed *= .5f;
        sprt.color = Color.cyan;
    }
    public void DismissIceDebuff()
    {
        speed /= .5f;
        sprt.color = Color.white;
    }
}
