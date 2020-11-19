using Gizmo.WPF;
using System.Windows;

namespace Gizmo.HardwareAudit.Behaviors
{
    //Original code came from https://stackoverflow.com/a/52495224
    //All credits to his author Peregrine https://stackoverflow.com/users/967885/peregrine (https://github.com/Peregrine66)
    //added BringExpandedChildrenIntoView behaviour to addition
    public static class UITreeListItemBehavior
    {
        public static bool GetBringSelectedItemIntoView(UITreeListItem treeListItem)
        {
            return (bool)treeListItem.GetValue(BringSelectedItemIntoViewProperty);
        }

        public static void SetBringSelectedItemIntoView(UITreeListItem treeListItem, bool value)
        {
            treeListItem.SetValue(BringSelectedItemIntoViewProperty, value);
        }

        public static bool GetBringExpandedChildrenIntoView(UITreeListItem treeListItem)
        {
            return (bool)treeListItem.GetValue(BringExpandedChildrenIntoViewProperty);
        }

        public static void SetBringExpandedChildrenIntoView(UITreeListItem treeListItem, bool value)
        {
            treeListItem.SetValue(BringExpandedChildrenIntoViewProperty, value);
        }

        public static readonly DependencyProperty BringSelectedItemIntoViewProperty = DependencyProperty.RegisterAttached(
                "BringSelectedItemIntoView",
                typeof(bool),
                typeof(UITreeListItemBehavior),
                new UIPropertyMetadata(false, BringSelectedItemIntoViewChanged));

        public static readonly DependencyProperty BringExpandedChildrenIntoViewProperty = DependencyProperty.RegisterAttached(
                "BringExpandedChildrenIntoView",
                typeof(bool),
                typeof(UITreeListItemBehavior),
                new UIPropertyMetadata(false, BringExpandedItemIntoViewChanged));

        private static void BringSelectedItemIntoViewChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (!(args.NewValue is bool))
                return;

            var item = obj as UITreeListItem;

            if (item == null)
                return;

            if ((bool)args.NewValue)
                item.Selected += OnUITreeListItemSelected;
            else
                item.Selected -= OnUITreeListItemSelected;
        }

        private static void BringExpandedItemIntoViewChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (!(args.NewValue is bool))
                return;

            var item = obj as UITreeListItem;

            if (item == null)
                return;

            if ((bool)args.NewValue)
                item.Expanded += OnUITreeListItemExpanded;
            else
                item.Expanded -= OnUITreeListItemExpanded;
        }

        private static void OnUITreeListItemSelected(object sender, RoutedEventArgs e)
        {
            var item = e.OriginalSource as UITreeListItem;
            item?.BringIntoView();
            e.Handled = true;
        }

        private static void OnUITreeListItemExpanded(object sender, RoutedEventArgs e)
        {
            var item = e.OriginalSource as UITreeListItem;
            item?.BringIntoView();
            e.Handled = true;
        }
    }
}
