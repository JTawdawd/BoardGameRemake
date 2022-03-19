using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PieceCardManager : MonoBehaviour
{
    public GameObject pieceCardParentObj;
    public CanvasGroup canvasGroup;
    public Text t_name;
    public Text t_currentTile;
    public Text t_team;
    public Text t_health;
    public Text t_currentCooldown;
    public Text t_damage;
    public Text t_movementRange;
    public Text t_attackRange;
    public Text t_armorType;
    public Text t_cooldown;

    public float fadeSpeed;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = pieceCardParentObj.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        Physics.Raycast(ray, out hit);

        if (hit.transform != null && hit.transform.parent.transform.GetComponent<Piece>() != null)
        {
            UpdateText(hit.transform.parent.transform.GetComponent<Piece>());
            Activate();
            if (canvasGroup.alpha < 1)
                canvasGroup.alpha += Time.deltaTime * fadeSpeed;
        }
        else
        {
            if (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            }
            else 
                Deactivate();
        }
            
    }

    void Deactivate()
    {
        if (pieceCardParentObj.activeSelf)
        {
            pieceCardParentObj.SetActive(false);
        }
    }

    void Activate()
    {
        if (!pieceCardParentObj.activeSelf)
        {
            pieceCardParentObj.SetActive(true);
        }
    }

    void UpdateText(Piece piece)
    {
        t_name.text = piece.transform.name.ToString();
        t_currentTile.text = piece.currentTile.transform.name.ToString();
        t_team.text = piece.team.ToString();
        t_health.text = "Health: " + piece.health.ToString();
        t_currentCooldown.text = "Cooldown: " + piece.currentCooldown.ToString();
        t_damage.text = "Damage: " + piece.damage.ToString();
        t_movementRange.text = "Movement Range: " + piece.movementRange.ToString();
        t_attackRange.text = "Attack Range: " + piece.attackRange.ToString();
        t_armorType.text = "Armor Type: " + piece.armorType.ToString();
        t_cooldown.text = "Max Cooldown: " + piece.cooldown.ToString();
        }
}
