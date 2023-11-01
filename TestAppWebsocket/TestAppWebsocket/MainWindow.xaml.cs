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
            try
            {
                var IsSecuredConnection = bool.Parse(System.Configuration.ConfigurationManager.
                AppSettings["IsSecuredConnection"]);
                if (IsSecuredConnection) {
                    webSocketService.ConnectAsync(serverUri: System.Configuration.ConfigurationManager.
                        AppSettings["ServerConnectionWSS"],
                        viewModel);
                }
                else {
                    webSocketService.ConnectAsync(serverUri: System.Configuration.ConfigurationManager.
                        AppSettings["ServerConnectionWS"],
                        viewModel);
                }

                viewModel.SetWebSocketService(webSocketService);

                Closing += OnClosed;
            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }
        }

        private void OnClosed(object? sender, EventArgs e) {
            webSocketService.DisconnectAsync();
        }
    }
}
