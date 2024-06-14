namespace iOSBorderShadowAnimationBug;

public partial class BorderShadowIssuePage : ContentPage {
    int count = 0;

    public BorderShadowIssuePage() {
        InitializeComponent();
    }

    private void OnCounterClicked(object sender, EventArgs e) {
        count++;

        CounterBtn.Text = count == 1 ? $"Clicked {count} time" : $"Clicked {count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);

        FirstBorder.IsVisible = !FirstBorder.IsVisible;
        SecondBorder.IsVisible = !SecondBorder.IsVisible;
    }
}