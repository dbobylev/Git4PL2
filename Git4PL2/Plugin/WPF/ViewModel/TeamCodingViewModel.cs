using Git4PL2.Plugin.Abstract;
using Git4PL2.Plugin.Diff;
using Git4PL2.Plugin.Commands;
using Git4PL2.Plugin.TeamCoding;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Git4PL2.Plugin.WPF.ViewModel
{
    class TeamCodingViewModel : PropertyChangedBase
    {
        private ITeamCodingProvider _TeamCodingProvider;

        public ObservableCollection<ICheckOutObject> CheckOutList { get; private set; }
        public DbObject DbObject { get; private set; }
        public string UserLogin { get; set; }

        private ICommand CheckInSource { get; set; }
        private ICommand CheckOutSource { get; set; }

        public ICommand CheckInCommand { get; set; }
        public ICommand CheckOutCommand { get; set; }

        public TeamCodingViewModel(ITeamCodingProvider TeamCodingProvider, IIDEProvider IDEProvider, ISettings Settings)
        {
            _TeamCodingProvider = TeamCodingProvider;

            UserLogin = Settings.TEAMCODING_LOGIN;

            DbObject = IDEProvider.GetDbObject<DbObject>(true);
            if (DbObject == null)
                DbObject = new DbObject("SIA", "LOLKEK", "PACKAGE");

            CheckInSource = NinjectCore.Get<CommandCheckIn>();
            CheckOutSource = NinjectCore.Get<CommandCheckOut>();
            CheckInCommand = new RelayCommand(CheckIn, CheckInSource.CanExecute);
            CheckOutCommand = new RelayCommand(CheckOut, CheckOutSource.CanExecute);

            CheckOutList = new ObservableCollection<ICheckOutObject>();
            FillCheckOutList();
        }

        public void FillCheckOutList()
        {
            CheckOutList.Clear();
            foreach (ICheckOutObject item in _TeamCodingProvider.GetCheckOutObjectsList())
            {
                CheckOutList.Add(item);
            };
            OnPropertyChanged("CheckOutList");
        }

        private void CheckIn(object param)
        {
            CheckInSource.Execute(param);
            FillCheckOutList();
        }

        private void CheckOut(object param)
        {
            CheckOutSource.Execute(param);
            FillCheckOutList();
        }
    }
}
