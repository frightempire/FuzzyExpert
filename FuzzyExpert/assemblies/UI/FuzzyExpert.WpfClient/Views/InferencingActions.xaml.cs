﻿using System;
using System.Windows.Controls;
using FuzzyExpert.WpfClient.ViewModels;

namespace FuzzyExpert.WpfClient.Views
{
    public partial class InferencingActions : UserControl
    {
        public InferencingActions(InferencingActionsModel model)
        {
            DataContext = model ?? throw new ArgumentNullException(nameof(model));
            InitializeComponent();
        }

        public void InitializeState(string userName)
        {
            var model = (InferencingActionsModel)DataContext;
            model.InitializeState();
            model.RefreshSettings(userName);
            model.RefreshProfiles(userName);
        }
    }
}