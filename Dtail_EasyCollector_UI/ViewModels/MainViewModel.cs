using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Dtail_EasyCollector_UI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ViewModelBase CurrentViewModel { get; }

        public MainViewModel()
        {
            CurrentViewModel = new CollectViewModel();
            //GarmentsList = garmentsList;
            //SearchDir = searchDir;
            //OutputDir = outputDir;
        }

        //private string garmentsList;
        //private string searchDir;
        //private string outputDir;

        //public string GarmentsList
        //{
        //    get => garmentsList;
        //    set
        //    {
        //        garmentsList = value;
        //        OnPropertyChanged(nameof(GarmentsList));
        //    }
        //}


        //public string SearchDir
        //{
        //    get => searchDir;
        //    set
        //    {
        //        searchDir = value;
        //        OnPropertyChanged(nameof(SearchDir));
        //    }
        //}

        //public string OutputDir
        //{
        //    get => outputDir;
        //    set
        //    {
        //        outputDir = value;
        //        OnPropertyChanged(nameof(OutputDir));
        //    }
        //}

        //public ICommand CollectCommand { get; }
    }
}
