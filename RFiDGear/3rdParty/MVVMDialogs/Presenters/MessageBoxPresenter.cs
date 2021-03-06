﻿using MvvmDialogs.Presenters;
using RFiDGear.ViewModel;
using System.Windows;

namespace RFiDGear.Presenters
{
    public class MessageBoxPresenter : IDialogBoxPresenter<MessageBoxViewModel>
    {
        public void Show(MessageBoxViewModel vm)
        {
            vm.Result = MessageBox.Show(vm.Message, vm.Caption, vm.Buttons, vm.Image);
        }
    }
}