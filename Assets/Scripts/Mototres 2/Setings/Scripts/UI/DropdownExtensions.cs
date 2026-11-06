using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace GameOptions
{
    /// <summary>Atajos para los dropdowns de TMPro, compartidos por todas las secciones.</summary>
    public static class DropdownExtensions
    {
        public static void SetOptions(this TMP_Dropdown dropdown, IEnumerable<string> labels)
        {
            dropdown.ClearOptions();
            dropdown.AddOptions(labels.ToList());
        }

        /// <summary>
        /// Selecciona una opcion sin ejecutar onValueChanged (solo estamos mostrando datos,
        /// no es una eleccion del jugador). Un indice fuera de rango cae en la opcion valida mas cercana.
        /// </summary>
        public static void SelectWithoutNotify(this TMP_Dropdown dropdown, int index)
        {
            dropdown.SetValueWithoutNotify(Mathf.Clamp(index, 0, dropdown.options.Count - 1));
        }
    }
}
