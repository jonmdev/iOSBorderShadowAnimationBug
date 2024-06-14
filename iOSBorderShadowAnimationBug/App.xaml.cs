using Microsoft.Maui.Controls.Shapes;

namespace iOSBorderShadowAnimationBug {
    public partial class App : Application {
        enum TestProjects {
            testProject1, //==> DEMONSTRATES FLASH AT CORNERS OF LOWER SCREEN "HELLO" BORDER
                          //AS BORDER IS INITIALLY DRAWN SQUARE (RATHER THAN ROUND) ON THE FIRST "CLICK ME" BUTTON CLICK

            testProject2 //==> DEMONSTRATES FLASH OF PRIOR COLOR ON EACH UPDATE
                         //AS EACH TIME BORDER IS REDRAWN THE COLORATION LAGS ONE FRAME
        }
        public App() {

            //=================================
            //SET WHICH TEST PROJECT HERE
            //=================================
            TestProjects testProject = TestProjects.testProject1;
            //=================================

            ContentPage mainPage = new();
            mainPage.BackgroundColor = Colors.CornflowerBlue;

            if (testProject == TestProjects.testProject1) {
                InitializeComponent();
                MainPage = new AppShell();
            }
            else {
                MainPage = mainPage;
            }
            AbsoluteLayout abs = new();
            mainPage.Content = abs;

            Border border = new();
            border.BackgroundColor = Colors.Purple;
            border.StrokeShape = new RoundRectangle() { CornerRadius = 20 };
            border.Shadow = new Shadow() { Brush = Colors.Black, Radius = 5, Offset = new Point(2, 5) };
            abs.Add(border);

            Label label = new();
            label.HorizontalOptions = LayoutOptions.Center;
            label.Text = "TEST LABEL";
            label.TextColor = Colors.White;
            label.FontSize = 30;
            border.Content = label;

            mainPage.SizeChanged += delegate {
                double screenWidth = mainPage.Width;
                double screenHeight = mainPage.Height;
                border.WidthRequest = screenWidth * 0.8;
                border.TranslationX = (screenWidth - border.WidthRequest) * 0.5;
                border.TranslationY = screenHeight * 0.3;
            };

            List<Color> colorList = new() { Colors.Purple, Colors.OrangeRed, Colors.Turquoise, Colors.DarkCyan };
            List<string> textList = new() { "Hello", "Goodbye" };
            
            //timer
            var timer = Application.Current.Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromSeconds(1 / 2f); //run timer at 2 fps 
            timer.IsRepeating = true;
            timer.Tick += delegate {
                border.IsVisible = !border.IsVisible;
                if (border.IsVisible) {
                    label.Text = textList[Random.Shared.Next(0, textList.Count)];
                    Color newBg = colorList[Random.Shared.Next(0, colorList.Count)];
                    while (newBg == border.BackgroundColor) { newBg = colorList[Random.Shared.Next(0, colorList.Count)]; }
                    border.BackgroundColor = newBg;
                }
            };
            timer.Start();
        }
    }
}
