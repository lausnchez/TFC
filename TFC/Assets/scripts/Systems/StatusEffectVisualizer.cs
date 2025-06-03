using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatusEffectVisualizer : MonoBehaviour
{
    //public Transform iconParent;
    //public GameObject iconQuemadura;

    //private List<GameObject> activeIcons = new List<GameObject>();

    //void Awake()
    //{
    //    if (iconParent == null)
    //    {
    //        iconParent = transform.Find("StatusCanvas/StatusIcons");

    //        if (iconParent == null)
    //        {
    //            Debug.LogError("iconParent no encontrado automáticamente.");
    //        }
    //    }
    //}

    //public void UpdateIcons(List<Enemies.EnemyAttack> attackList)
    //{
    //    iconParent = transform.Find("StatusCanvas/StatusIcons");
    //    if (iconParent == null)
    //    {
    //        Debug.LogError("iconParent está NULL. Asegúrate de asignarlo en el Inspector."); 
    //        return;
    //    }
    //    try
    //    {
    //        // Limpia iconos antiguos
    //        foreach (var icon in activeIcons)
    //            Destroy(icon);
    //        activeIcons.Clear();
    //        foreach (Enemies.EnemyAttack attack in attackList)
    //        {
    //            if (attack.id == 1)
    //            {
    //                GameObject icon = Instantiate(iconQuemadura, iconParent, false);
    //                icon.transform.localPosition = new Vector3(activeIcons.Count * 0.5f, 0, 0); // Espaciado entre íconos
    //                activeIcons.Add(icon);
    //            }
    //        }
    //    }
    //    catch (System.Exception e)
    //    {
    //        Debug.LogError("Error al actualizar los iconos: " + e.Message);
    //    }
    //}

    public RectTransform iconParent; // StatusIcons_Player
    public GameObject iconPrefabQuemadura;

    public void UpdateIcons(List<Enemies.EnemyAttack> currentEffects)
    {
        // Limpia iconos anteriores
        foreach (Transform child in iconParent)
        {
            Destroy(child.gameObject);
        }

        // Instancia iconos según los efectos activos
        foreach (Enemies.EnemyAttack effect in currentEffects)
        {
            if (effect.id == 1)
            {
                Instantiate(iconPrefabQuemadura, iconParent);
            }

            // Agrega más if para otros efectos
        }
    }
}
