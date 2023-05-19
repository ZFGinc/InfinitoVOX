using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SkinControll))]
public class PlayerMove : MonoBehaviour
{
    [Header("Джойстик для управления игроком")]
    [SerializeField] private FloatingJoystick joystick;

    [Header("Объект скинов и оружий для поворота в сторону движения")]
    [SerializeField] private GameObject playerRotate;
    [SerializeField] private GameObject playerRotateToEnemy = null;
    [SerializeField] private GameObject weaponsRotateToEnemy;

    [Header("Аниматор")]
    [SerializeField] private Animator animator = null;

    private AudioSource _audioSteps;
    private SkinControll _skinControll;
    private Rigidbody rb;
    private float speed;
    private Vector2 joyVector;

    private GameObject nearest = null;
    private bool isLook = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        _audioSteps = GetComponent<AudioSource>();
        _skinControll = GetComponent<SkinControll>();
    }
    private void FixedUpdate()
    {
        joyVector = joystick.Direction;

        GameObject[] enemys = new GameObject[0];
        if (nearest == null)
        {
            enemys = GameObject.FindGameObjectsWithTag("Enemy");

            if (enemys.Length == 0)
            {
                nearest = GameObject.FindGameObjectWithTag("Boss");
            }
        }
        if (enemys.Length > 0 || nearest != null) isLook = true;

        if (animator == null || playerRotateToEnemy == null) _skinControll.SetSkin();

        Move();
        RotateToMove();
        RotateToEnemy();
    }

    public void SetAnimator(Animator anim) => animator = anim;
    public void SetTors(GameObject rotator) => playerRotateToEnemy = rotator;

    //Передвижение игрока
    private void Move()
    {
        speed = PlayerStats.Speed;

        if (joyVector.x != 0 && joyVector.y != 0) speed /= Mathf.Pow(2, 0.5f);


        rb.velocity = new Vector3(joyVector.x * speed, rb.velocity.y, joyVector.y * speed);
    }
    //Поворот игрока в сторону ходьбы
    private void RotateToMove()
    {
        if (animator == null) return;
        if ((joyVector.x != 0 || joyVector.y != 0) && !UIDataPlayer.Instance.IsBlockMove)
        {
            playerRotate.transform.rotation = Quaternion.Euler(new Vector3(0, Mathf.Atan2(joyVector.x, joyVector.y) * Mathf.Rad2Deg, 0));
            animator.SetBool("isRun", true);
            _audioSteps.volume = .02f;
        }
        else
        {
            animator.SetBool("isRun", false);
            _audioSteps.volume = 0f;
        }
    }
    //Поворот торса игрока в сторону врага
    private void RotateToEnemy()
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        bool isBoss = false;

        if (enemys.Length == 0)
        {
            nearest = GameObject.FindGameObjectWithTag("Boss");
            isBoss = true;
        }
        else
            nearest = FindClosestEnemy(transform, enemys);
        

        if (nearest == null) ResetRotateTorsWithWeapons();
        else if (Vector3.Distance(transform.position, nearest.transform.position) > ((isBoss) ? 50f : 10f))
        {
            nearest = null;
            ResetRotateTorsWithWeapons();
        }
        else if (nearest != null && isLook)
        {
            playerRotateToEnemy.transform.LookAt(nearest.transform, Vector3.up);
            weaponsRotateToEnemy.transform.LookAt(nearest.transform, Vector3.up);
            Weapons.Init.isShoot = true;
        }
        else
        {
            nearest = null;
            ResetRotateTorsWithWeapons();
        }
    }
    GameObject FindClosestEnemy(Transform player, GameObject[] enemy)
    {
        float distance = Mathf.Infinity;
        GameObject closest = null;

        foreach (GameObject go in enemy)
        {
            Vector3 diff = go.transform.position - player.position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    //Сброс поворота торса
    private void ResetRotateTorsWithWeapons()
    {
        isLook = false;
        Weapons.Init.isShoot = false;
        playerRotateToEnemy.transform.localRotation = Quaternion.Euler(0, -90, 0);
        weaponsRotateToEnemy.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
}
