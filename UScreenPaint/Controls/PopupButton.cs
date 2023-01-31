using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace UScreenPaint.Controls
{
    public class PopupButton : ToggleButton
    {
        private Popup _popup;

        #region Popup Content

        /// <summary>
        /// PopupContent dependency property.
        /// </summary>
        public static readonly DependencyProperty PopupContentProperty = DependencyProperty.Register(
            nameof(PopupContent), typeof(object), typeof(PopupButton));

        /// <summary>
        /// PopupContent content.
        /// </summary>
        [Category("Common")]
        [Description("Gets or sets popup content content.")]
        public object PopupContent
        {
            get { return (object)GetValue(PopupContentProperty); }
            set { SetValue(PopupContentProperty, value); }
        }

        #endregion

        static PopupButton()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(PopupButton), new FrameworkPropertyMetadata(typeof(PopupButton)));
        }

        public PopupButton()
        {
            _popup = new Popup();
            _popup.PlacementTarget = this;
            _popup.Placement = PlacementMode.Bottom;
            _popup.SetBinding(Popup.ChildProperty, new Binding(nameof(PopupContent)) { Source = this });
            _popup.SetBinding(Popup.IsOpenProperty, new Binding(nameof(IsChecked)) { Source = this, Mode = BindingMode.TwoWay });
            _popup.AllowsTransparency = true;
            _popup.PopupAnimation = PopupAnimation.Slide;
            _popup.StaysOpen = false;
            _popup.Opened += (sender, e) => _popup.Child.Focus();
        }
    }
}
