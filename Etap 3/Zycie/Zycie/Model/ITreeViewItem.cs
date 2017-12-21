using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Projekt.Model
{
  //  [Serializable]
    public abstract class ITreeViewItem : ITree
    {
        #region DataContext

        [JsonProperty]
        public string Name { get; set; }
        [JsonProperty]
        public ObservableCollection<ITreeViewItem> Children { get; set; }


        #endregion
        public ITreeViewItem()
        {
            Children = new ObservableCollection<ITreeViewItem>();
            Children.Add(null);
            this._wasBuilt = false;
        }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                if (_wasBuilt)
                    return;
                Children.Clear();
                buildMyself();
                _wasBuilt = true;
            }
        }

        private bool _wasBuilt;
        private bool _isExpanded;


        public abstract void buildMyself();
        public abstract string ToString();
    }
}
