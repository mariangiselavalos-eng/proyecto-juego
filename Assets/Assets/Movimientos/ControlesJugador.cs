using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlesJugador : MonoBehaviour
{
    public PlayerInput playerInput;

    public Vector2 direccion;
    public Rigidbody2D rb2D;

    public float velocidadMovimiento;

    public bool mirandoDerecha = true;

    public float fuerzaSalto;
    public LayerMask queEsSuelo;
    public Transform controladorSuelo;
    public Vector3 dimensionesCaja;
    public bool enSuelo;

    public bool salto = false;
    public int saltosExtrasRestantes;
    public int saltosExtras;

    private InputAction accionSaltar;
    private InputAction accionMover;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {
        accionMover = playerInput.actions["Mover"];
        accionSaltar = playerInput.actions["Saltar"];
    }
    private void OnEnable()
    {
        accionSaltar.started += _ => Saltar();

    }
    private void OnDisable()
    {

        accionSaltar.started -= _ => Saltar();

    }

    private void Update()
    {
        direccion = accionMover.ReadValue<Vector2>();
        AjustarRotacion(direccion.x);
        enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimensionesCaja, 0f, queEsSuelo);
        if (enSuelo)
        {
            saltosExtrasRestantes = saltosExtras;
        }
        if (Input.GetButtonDown("Jump"))
        {
            salto = true;
        }
    }
    private void FixedUpdate()
    {
        rb2D.linearVelocity = new Vector2(velocidadMovimiento * direccion.x, rb2D.linearVelocity.y);
        salto2(salto);
        salto = false;
    }
    private void salto2(bool salto)
    {
        if (salto)
        {
            if (enSuelo)
            {
                Salto();

            }
            else
            {
                if (salto && saltosExtrasRestantes > 0)
                {
                    Salto();
                    saltosExtrasRestantes -= 1;
                }
            }
        }

    }
    private void Salto()
    {
        //rb2D.AddForce(new Vector2(0f, fuerzaSalto));
        rb2D.linearVelocity = new Vector2(0f, fuerzaSalto);
        salto = false;
    }

    private void Saltar(InputAction.CallbackContext context)
    {
        //rb2D.AddForce(new Vector2(0, fuerzaSalto));
        Debug.Log(context.phase);
    }
    private void Saltar()
    {
        if (enSuelo)
        {
            rb2D.AddForce(new Vector2(0, fuerzaSalto), ForceMode2D.Impulse);
        }



    }

    private void AjustarRotacion(float direccionX)
    {
        if (direccionX > 0 && !mirandoDerecha)
        {
            Girar();

        }
        else if (direccionX < 0 && mirandoDerecha)
            Girar();

    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(controladorSuelo.position, dimensionesCaja);

    }

}




