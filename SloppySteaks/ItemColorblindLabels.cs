using KitchenData;
using KitchenLib.References;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace KitchenSloppySteaks
{
    public static class ItemColorblindLabels
    {
        private static GameObject _template;

        private static GameObject Template
        {
            get
            {
                if (_template == null)
                {
                    _template = GameData.Main.Get<Item>(ItemReferences.PieAppleRaw).Prefab.transform.Find("Colour Blind").gameObject;
                }

                return _template;
            }
        }

        public static void AddItemColorblindLabel(this GameObject holder, string title)
        {
            GameObject gameObject = Object.Instantiate(Template);
            gameObject.name = "Colour Blind";
            gameObject.transform.SetParent(holder.transform);
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.Find("Title").gameObject.GetComponent<TextMeshPro>().text = title;
        }
    }
}
