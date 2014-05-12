using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TigerAppWPF
{
    public class ListViewItemStyleSelector : StyleSelector
    {

        public override Style SelectStyle(object item,
            DependencyObject container)
        {
            Style st = new Style();
            st.TargetType = typeof(ListViewItem);
            Setter backGroundSetter = new Setter();
            backGroundSetter.Property = ListViewItem.BackgroundProperty;
            ListView listView =
                ItemsControl.ItemsControlFromItemContainer(container)
                  as ListView;
            int index =
                listView.ItemContainerGenerator.IndexFromContainer(container);
            if (index % 2 == 0)
            {
                backGroundSetter.Value = Brushes.LightBlue;
            }
            else
            {
                backGroundSetter.Value = Brushes.Beige;
            }
            st.Setters.Add(backGroundSetter);
            return st;
        }
    }
}
