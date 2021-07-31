using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace FoodOrders.Model
{
    public partial class MainViewModel
    {
        class GenerateDataCommandImpl : CommandBase<MainViewModel>
        {
            public GenerateDataCommandImpl(MainViewModel context) : base(context)
            {
            }

            public override bool CanExecute(object parameter)
            {
                return true;
            }

            public override void Execute(object parameter)
            {
                Context.ExecuteGenerateDataCommand();
            }
        }

        public CommandBase GenerateDataCommand { get; }

        private void ExecuteGenerateDataCommand()
        {
            Process.Start("FoodOrders.DataGenerator.exe");
        }
    }
}
