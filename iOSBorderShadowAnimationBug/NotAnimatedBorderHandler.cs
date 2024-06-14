using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

#if IOS
using UIKit;
using ContentView = Microsoft.Maui.Platform.ContentView;
using Foundation;
using CoreAnimation;
#endif 

namespace iOSBorderShadowAnimationBug {

    //TEMP BUG FIX FOR: https://github.com/dotnet/maui/issues/18204
    public class NotAnimatedBorderHandler : BorderHandler {

#if IOS
        private class BorderContentView : ContentView {
            public override void LayoutSubviews() {

                Layer.RemoveAllAnimations();

                if (Layer.Sublayers != null) {

                    for (int i=0; i < Layer.Sublayers.Count(); i++) {
                        Layer.Sublayers[i].RemoveAllAnimations();
                    }
                }
                
                base.LayoutSubviews();
            }

        }

        protected override ContentView CreatePlatformView() {
            _ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} must be set to create a {nameof(ContentView)}");
            _ = MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} cannot be null");

            return new BorderContentView {
                CrossPlatformLayout = VirtualView
            };
        }
#endif
    }
}