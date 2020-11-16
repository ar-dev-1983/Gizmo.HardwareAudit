using Gizmo.WPF;
using System.Windows;

namespace Gizmo.HardwareAudit.Behaviors
{
    public class UITreeListScrollInToViewBehavior
	{
		public static readonly DependencyProperty IsScrollIntoViewProperty = DependencyProperty.RegisterAttached("IsScrollIntoView", typeof(bool), typeof(UITreeListScrollInToViewBehavior), new PropertyMetadata(default(bool), PropertyChangedCallback));

		public static void SetIsScrollIntoView(DependencyObject element, bool value)
		{
			element.SetValue(IsScrollIntoViewProperty, value);
		}

		public static bool GetIsScrollIntoView(DependencyObject element)
		{
			return (bool)element.GetValue(IsScrollIntoViewProperty);
		}

		private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			var treeViewItem = dependencyObject as UITreeListItem;
			if (treeViewItem == null)
			{
				return;
			}

			if (!((bool)dependencyPropertyChangedEventArgs.OldValue) &&
				((bool)dependencyPropertyChangedEventArgs.NewValue))
			{
				treeViewItem.Unloaded += UITreeListItemOnUnloaded;
				treeViewItem.Selected += UITreeListItemOnSelected;
			}
		}

		private static void UITreeListItemOnUnloaded(object sender, RoutedEventArgs routedEventArgs)
		{
			var treeViewItem = sender as UITreeListItem;
			if (treeViewItem == null)
			{
				return;
			}

			treeViewItem.Unloaded -= UITreeListItemOnUnloaded;
			treeViewItem.Selected -= UITreeListItemOnSelected;
		}

		private static void UITreeListItemOnSelected(object sender, RoutedEventArgs routedEventArgs)
		{
			var treeViewItem = sender as UITreeListItem;
			treeViewItem?.BringIntoView();
		}
	}
}
