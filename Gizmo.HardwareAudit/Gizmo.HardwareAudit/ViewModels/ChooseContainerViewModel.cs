using Gizmo.HardwareAudit.Classes.Helpers;
using Gizmo.HardwareAudit.Interfaces;
using Gizmo.HardwareAudit.Models;

namespace Gizmo.HardwareAudit.ViewModels
{
    public class ChooseContainerViewModel : BaseViewModel
    {
        private ContainerItem root;
        public ContainerItem Root
        {
            get => root;
            set
            {
                if (root == value) return;
                root = value;
                OnPropertyChanged();
            }
        }
        public ContainerItem SelectedContainer => Root.SelectedItem;

        public ChooseContainerViewModel(TreeItem treeItem, bool useSelectedParentId)
        {
            Root = new ContainerItem();

            if (useSelectedParentId)
                TreeItemHelper.GetContainersFromTreeItem(treeItem, Root.Children, treeItem.SelectedItem.ParentId);
            else
                TreeItemHelper.GetAllContainersFromTreeItem(treeItem, Root.Children);

            Root.Initialise();
            if (Root != null)
            {
                Root.PropertyChanged += SelectedItem_PropertyChanged;
            }
        }

        private void SelectedItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("SelectedItem"))
            {
                OnNamedPropertyChanged("SelectedContainer");
            }
        }

    }
}
