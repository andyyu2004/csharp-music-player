﻿using CMusicPlayer.Media.Models;
using CMusicPlayer.UI.Utility;

namespace CMusicPlayer.UI.Properties
{
    /// <summary>
    ///     Interaction logic for PropertiesWindow.xaml
    /// </summary>
    internal partial class PropertiesWindow
    {
        public PropertiesWindow(ITrack track)
        {
            InitializeComponent();
            Track = track;
            DataContext = this;
            AppBar.BarTitle = $"{Track.Artist} - {Track.Title}";

            new ApplicationBarEventHandler(this, AppBar, Close);
        }

        public ITrack Track { get; }
    }
}