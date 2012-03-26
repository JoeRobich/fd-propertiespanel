using System;
using System.Collections.Generic;
using System.Text;
using PropertiesPanel.Property;
using PropertiesPanel.Helpers;
using PluginCore;

namespace PropertiesPanel.Outline
{
    public class GotoDeclarationAction : PropertyAction
    {
        public GotoDeclarationAction()
            : base(ResourceHelper.GetString("PropertiesPanel.Label.GotoDeclaration"), PluginBase.MainForm.FindImage("99|9|3|-3"))
        {

        }

        public override bool IsEnabled(PropertyProvider provider, IEnumerable<PropertyItem> selectedItems)
        {
            List<PropertyItem> itemList = new List<PropertyItem>(selectedItems);

            PropertyItem[] items = itemList.ToArray();

            if (provider is OutlineProvider &&
                items.Length == 1)
            {
                OutlineItem item = (OutlineItem)items[0];
                return item.Node.Tag != null;
            }

            return false;
        }

        public override void OnActionClicked(PropertyProvider provider, IEnumerable<PropertyItem> selectedItems)
        {
            List<PropertyItem> itemList = new List<PropertyItem>(selectedItems);
            OutlineItem outlineItem = (OutlineItem)itemList[0];
            outlineItem.NavigateTo();
        }
    }
}
