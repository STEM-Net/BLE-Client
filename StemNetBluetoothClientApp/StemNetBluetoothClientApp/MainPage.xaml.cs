using StemNetBluetoothClientApp.BLE;

namespace StemNetBluetoothClientApp;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
        OutputLbl.Text = "Attempting to connect...";
        Task.Run(ConnectAndSetOutput);
	}

	private async Task ConnectAndSetOutput()
	{
        string output = await BLEReader.Read();
        MainThread.BeginInvokeOnMainThread(() =>
        {
            OutputLbl.Text = output;
            SemanticScreenReader.Announce(OutputLbl.Text);
        });
    }
}

