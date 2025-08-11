using Microsoft.Maui.Controls;

namespace MauiWebView
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            webView.Source = "https://localhost:7287/dashboard";
        }
    }

}
