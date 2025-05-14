using cloud_app_dev_exam_project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace cloud_app_dev_exam_project
{
    public class CartDisplayItem : INotifyPropertyChanged
    {
        public CartItem CartItem { get; set; }
        public ListableItem ListableItem { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TotalPrice));
                    OnSelectionChanged(); 
                }
            }
        }

        public int Quantity
        {
            get => CartItem.Quantity;
            set
            {
                if (CartItem.Quantity != value)
                {
                    CartItem.Quantity = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TotalPrice)); 
                    OnSelectionChanged(); 
                }
            }
        }

        public decimal TotalPrice => Quantity * (ListableItem?.Price ?? 0);

        public event Action SelectionChanged;

        private void OnSelectionChanged()
        {
            SelectionChanged?.Invoke();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
