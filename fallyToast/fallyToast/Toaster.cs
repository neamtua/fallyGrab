using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToastNotifications;

namespace fallyToast
{
    public class Toaster
    {
        public void Show(string title,string text,int duration,string animation,string direction,string url="",string image="",string type="")
        {
            if (duration <= 0)
            {
                duration = -1;
            }

            ToastNotifications.FormAnimator.AnimationMethod animationMethod = new ToastNotifications.FormAnimator.AnimationMethod();

            if (animation.ToLower()=="slide")
                animationMethod = ToastNotifications.FormAnimator.AnimationMethod.Slide;
            else if (animation.ToLower()=="roll")
                animationMethod = ToastNotifications.FormAnimator.AnimationMethod.Roll;
            else if (animation.ToLower() == "center")
                animationMethod = ToastNotifications.FormAnimator.AnimationMethod.Center;
            else if (animation.ToLower() == "fade")
                animationMethod = ToastNotifications.FormAnimator.AnimationMethod.Fade;
            else animationMethod = ToastNotifications.FormAnimator.AnimationMethod.Slide;

            foreach (ToastNotifications.FormAnimator.AnimationMethod method in Enum.GetValues(typeof(ToastNotifications.FormAnimator.AnimationMethod)))
            {
                if (string.Equals(method.ToString(), animation))
                {
                    animationMethod = method;
                    break;
                }
            }

            ToastNotifications.FormAnimator.AnimationDirection animationDirection = new ToastNotifications.FormAnimator.AnimationDirection();
            if (direction.ToLower()=="right")
                animationDirection = ToastNotifications.FormAnimator.AnimationDirection.Right;
            else if (direction.ToLower()=="up")
                animationDirection = ToastNotifications.FormAnimator.AnimationDirection.Up;
            else if (direction.ToLower() == "down")
                animationDirection = ToastNotifications.FormAnimator.AnimationDirection.Down;
            else if (direction.ToLower() == "left")
                animationDirection = ToastNotifications.FormAnimator.AnimationDirection.Left;
            else
                animationDirection = ToastNotifications.FormAnimator.AnimationDirection.Up;

            foreach (FormAnimator.AnimationDirection d in Enum.GetValues(typeof(ToastNotifications.FormAnimator.AnimationDirection)))
            {
                if (string.Equals(d.ToString(), direction))
                {
                    animationDirection = d;
                    break;
                }
            }

            Notification toastNotification = new Notification(title, text, duration, animationMethod, animationDirection,url,image,type);
            toastNotification.Show();
        }
    }
}
