﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace PuzzleGame {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        private void StartupHandler ( object sender, StartupEventArgs e ) {
            Elysium.Manager.Apply ( this, Elysium.Theme.Dark );
        }

    }
}
