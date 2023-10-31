using System;
using System.Windows;


namespace TestAppWebsocket
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IWebSocketService webSocketService;

        public MainWindow() {
            InitializeComponent();

            var viewModel = (ApplicationNotificationVM)DataContext;

            webSocketService = new WebSocketService(new Client());
            webSocketService.ConnectAsync(serverUri: System.Configuration.ConfigurationManager.
                AppSettings["ServerConnection"], 
                viewModel);

            viewModel.SetWebSocketService(webSocketService);

            Closing += OnClosed;
        }

        private void OnClosed(object? sender, EventArgs e) {
            webSocketService.DisconnectAsync();
        }
    }
}
