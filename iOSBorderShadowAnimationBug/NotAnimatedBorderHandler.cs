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
        public override void PlatformArrange(Rect rect) {
            
            CATransaction.Begin();
            CATransaction.DisableActions = true;
            CATransaction.AnimationDuration = 0;
            
            //ISSUE:
            base.PlatformArrange(rect); 

            //this doesn't help our problem now as it calls BorderHandler.iOS.cs 's PlatformArrange which restarts the CATransaction
            //technically need base.base.PlatformArrange(rect) but need reflection to get it as per:
            //https://stackoverflow.com/questions/2323401/how-to-call-base-base-method
            //https://stackoverflow.com/questions/1006530/how-to-call-a-second-level-base-class-method-like-base-base-gethashcode
            
            CATransaction.Commit();
        }


        private class BorderContentView : ContentView {
            public override void LayoutSubviews() {
                NSMutableDictionary dict = new();
                dict.TryAdd(new NSString("sublayers"), new NSNull());
                dict.TryAdd(new NSString("content"), new NSNull());

                Layer.RemoveAllAnimations();
                Layer.Actions = dict;

                if (Layer.Sublayers != null) {

                    for (int i=0; i < Layer.Sublayers.Count(); i++) {
                        Layer.Sublayers[i].RemoveAllAnimations();
                        Layer.Sublayers[i].Actions = dict; //  layer.actions = ["sublayers":NSNull()] 
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