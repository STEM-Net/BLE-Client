using StemNetBluetoothClientApp.Model.BLE;
using StemNetBluetoothClientApp.ViewModel;

namespace StemNetBluetoothClientApp;

public partial class MainPage : ContentPage
{
	public MainPage(MainViewModel mainViewModel)
	{
		InitializeComponent();
        BindingContext = mainViewModel;
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
        OutputLbl.Text = "Attempting to connect...";
        Task.Run(ConnectAndSetOutput);
	}

	private async Task ConnectAndSetOutput()
	{
        string output = await BLEReader.Read();
        this.Dispatcher.Dispatch(() =>
        {
            OutputLbl.Text = output;
            SemanticScreenReader.Announce(OutputLbl.Text);
        });
    }
}

