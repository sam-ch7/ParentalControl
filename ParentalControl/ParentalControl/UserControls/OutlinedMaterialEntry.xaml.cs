using ParentalControl.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ParentalControl.UserControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OutlinedMaterialEntry : Grid
    {
        private ImageSource tempIcon;
        const string emailRegex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
        // BindableProperties
        public static readonly BindableProperty EmailValidationEnabledProperty = BindableProperty.Create(
            nameof(EmailValidationEnabled), 
            typeof(bool),
            typeof(OutlinedMaterialEntry),
            default(bool),
             
            BindingMode.OneWay, null);

        public bool EmailValidationEnabled
        {
            get => (bool)GetValue(EmailValidationEnabledProperty);
            set => SetValue(EmailValidationEnabledProperty, value);
        }
        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(OutlinedMaterialEntry),
            default(string),
            BindingMode.TwoWay,
            null,
            (bindable, oldValue, newValue) =>
            {
                var view = (OutlinedMaterialEntry)bindable;

                view.customEntry.Text = (string)newValue;
            }
        );

        public static readonly BindableProperty PlaceholderTextProperty = BindableProperty.Create(
            nameof(PlaceholderText),
            typeof(string),
            typeof(OutlinedMaterialEntry),
            default(string),
            BindingMode.OneWay,
            null,
            (bindable, oldValue, newValue) =>
            {
                var view = (OutlinedMaterialEntry)bindable;

                view.placeholderText.Text = (string)newValue;
            }
        );

        public static readonly BindableProperty HelperTextProperty = BindableProperty.Create(
            nameof(HelperText),
            typeof(string),
            typeof(OutlinedMaterialEntry),
            default(string),
            BindingMode.OneWay,
            null,
            (bindable, oldValue, newValue) =>
            {
                var view = (OutlinedMaterialEntry)bindable;

                view.helperText.Text = (string)newValue;

                if (view.errorText.IsVisible)
                    view.helperText.IsVisible = false;
                else
                    view.helperText.IsVisible = !string.IsNullOrEmpty(view.helperText.Text);
            }
        );

        public static readonly BindableProperty ErrorTextProperty = BindableProperty.Create(
            nameof(ErrorText),
            typeof(string),
            typeof(OutlinedMaterialEntry),
            default(string),
            BindingMode.OneWay,
            null,
            (bindable, oldValue, newValue) =>
            {
                var view = (OutlinedMaterialEntry)bindable;

                view.errorText.Text = (string)newValue;
            }
        );

        public static readonly BindableProperty LeadingIconProperty = BindableProperty.Create(
            nameof(LeadingIcon),
            typeof(ImageSource),
            typeof(OutlinedMaterialEntry),
            default(ImageSource),
            BindingMode.OneWay,
            null,
            (bindable, oldValue, newValue) =>
            {
                var view = (OutlinedMaterialEntry)bindable;

                view.leadingIcon.Source = (ImageSource)newValue;

                view.leadingIcon.IsVisible = !view.leadingIcon.Source.IsEmpty;
            }
        );

        public static readonly BindableProperty TrailingIconProperty = BindableProperty.Create(
            nameof(TrailingIcon),
            typeof(ImageSource),
            typeof(OutlinedMaterialEntry),
            default(ImageSource),
            BindingMode.OneWay,
            null,
            (bindable, oldValue, newValue) =>
            {
                var view = (OutlinedMaterialEntry)bindable;

                view.trailingIcon.Source = (ImageSource)newValue;

                view.trailingIcon.IsVisible = view.trailingIcon.Source != null;
            }
        );

        public static readonly BindableProperty HasErrorProperty = BindableProperty.Create(
            nameof(HasError),
            typeof(bool),
            typeof(OutlinedMaterialEntry),
            default(bool),
            BindingMode.OneWay,
            null,
            (bindable, oldValue, newValue) =>
            {
                var view = (OutlinedMaterialEntry)bindable;

                view.errorText.IsVisible = (bool)newValue;

                view.containerFrame.BorderColor = view.errorText.IsVisible ? Color.Red : Color.LightGray;

                view.helperText.IsVisible = !view.errorText.IsVisible;

                view.placeholderText.TextColor = view.errorText.IsVisible ? Color.Red : Color.LightGray;

                view.PlaceholderText = view.errorText.IsVisible ? $"{view.PlaceholderText}*" : view.PlaceholderText.Split('*')[0];

                if (view.TrailingIcon != null && !view.TrailingIcon.IsEmpty)
                    view.tempIcon = view.TrailingIcon;

                view.TrailingIcon = view.errorText.IsVisible
                    ? ImageSource.FromFile("ic_error.png")
                    : view.tempIcon;

                view.trailingIcon.IsVisible = view.errorText.IsVisible;
            }
        );

        public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(
            nameof(IsPassword),
            typeof(bool),
            typeof(OutlinedMaterialEntry),
            default(bool),
            BindingMode.OneWay,
            null,
            (bindable, oldValue, newValue) =>
            {
                var view = (OutlinedMaterialEntry)bindable;

                view.customEntry.IsPassword = (bool)newValue;

                view.passwordIcon.IsVisible = (bool)newValue;
            }
        );

        public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create(
            nameof(MaxLength),
            typeof(int),
            typeof(OutlinedMaterialEntry),
            default(int),
            BindingMode.OneWay,
            null,
            (bindable, oldValue, newValue) =>
            {
                var view = (OutlinedMaterialEntry)bindable;

                view.customEntry.MaxLength = (int)newValue;

                view.charCounterText.IsVisible = view.customEntry.MaxLength > 0;

                view.charCounterText.Text = $"0 / {view.MaxLength}";
            }
        );

        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(
            nameof(BorderColor),
            typeof(Color),
            typeof(OutlinedMaterialEntry),
            Color.Blue,
            BindingMode.OneWay
        );

        public static readonly BindableProperty ReturnCommandProperty = BindableProperty.Create(
            nameof(ReturnCommand),
            typeof(ICommand),
            typeof(OutlinedMaterialEntry),
            default(ICommand),
            BindingMode.OneWay,
            null,
            (bindable, oldValue, newValue) =>
            {
                var view = (OutlinedMaterialEntry)bindable;

                view.customEntry.ReturnCommand = (ICommand)newValue;
            }
        );

        public OutlinedMaterialEntry()
        {
            InitializeComponent();

            this.customEntry.Text = this.Text;

            this.customEntry.TextChanged += this.OnCustomEntryTextChanged;

            this.customEntry.Completed += this.OnCustomEntryCompleted;
        }

        // Event Handlers
        public event EventHandler<EventArgs> EntryCompleted;

        public event EventHandler<TextChangedEventArgs> TextChanged;

        // Properties
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public string PlaceholderText
        {
            get => (string)GetValue(PlaceholderTextProperty);
            set => SetValue(PlaceholderTextProperty, value);
        }

        public string HelperText
        {
            get => (string)GetValue(HelperTextProperty);
            set => SetValue(HelperTextProperty, value);
        }

        public string ErrorText
        {
            get => (string)GetValue(ErrorTextProperty);
            set => SetValue(ErrorTextProperty, value);
        }

        public ImageSource LeadingIcon
        {
            get => (ImageSource)GetValue(LeadingIconProperty);
            set => SetValue(LeadingIconProperty, value);
        }

        public ImageSource TrailingIcon
        {
            get => (ImageSource)GetValue(TrailingIconProperty);
            set => SetValue(TrailingIconProperty, value);
        }

        public bool HasError
        {
            get => (bool)GetValue(HasErrorProperty);
            set => SetValue(HasErrorProperty, value);
        }

        public bool IsPassword
        {
            get => (bool)GetValue(IsPasswordProperty);
            set => SetValue(IsPasswordProperty, value);
        }

        public int MaxLength
        {
            get => (int)GetValue(MaxLengthProperty);
            set => SetValue(MaxLengthProperty, value);
        }

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public Keyboard Keyboard
        {
            set => this.customEntry.Keyboard = value;
        }

        public ReturnType ReturnType
        {
            set => this.customEntry.ReturnType = value;
        }

        public ICommand ReturnCommand
        {
            get => (ICommand)GetValue(ReturnCommandProperty);
            set => SetValue(ReturnCommandProperty, value);
        }

        // Here we check if there is any text on the entry,
        // if not, we set the border and placeholder text color
        // and activate the animation to move the placeholder up
        private async Task ControlFocused()
        {
            if (string.IsNullOrEmpty(this.customEntry.Text) || this.customEntry.Text.Length > 0)
            {
                this.customEntry.Focus();

                this.containerFrame.BorderColor = this.HasError ? Color.Red : this.BorderColor;
                this.placeholderText.TextColor = this.HasError ? Color.Red : this.BorderColor;

                int y = DeviceInfo.Platform == DevicePlatform.UWP ? -25 : -20;

                await this.placeholderContainer.TranslateTo(0, y, 100, Easing.Linear);

                this.placeholderContainer.HorizontalOptions = LayoutOptions.Start;
                this.placeholderText.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
            }
            else
                await this.ControlUnfocused();
        }

        // Here we change the border and placeholder text color
        // back to normal and check if there is any text on the entry,
        // if not we launch the animation to place the placeholder
        // back over the entry
        private async Task ControlUnfocused()
        {
            this.containerFrame.BorderColor = this.HasError ? Color.Red : Color.LightGray;
            this.placeholderText.TextColor = this.HasError ? Color.Red : Color.Gray;

            this.customEntry.Unfocus();

            if (string.IsNullOrEmpty(this.customEntry.Text) || this.customEntry.MaxLength <= 0)
            {
                await this.placeholderContainer.TranslateTo(0, 0, 100, Easing.Linear);

                this.placeholderContainer.HorizontalOptions = LayoutOptions.FillAndExpand;
                this.placeholderText.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            }
        }

        private void CustomEntryFocused(object sender, FocusEventArgs e)
        {
            if (e.IsFocused)
                MainThread.BeginInvokeOnMainThread(async () => await this.ControlFocused());
        }

        private void CustomEntryUnfocused(object sender, FocusEventArgs e)
        {
            if (!e.IsFocused)
                MainThread.BeginInvokeOnMainThread(async () => await this.ControlUnfocused());
            this.Text = customEntry.Text;

            if (this.charCounterText.IsVisible)
                this.charCounterText.Text = $"{this.customEntry.Text.Length} / {this.MaxLength}";

            try
            {
                if (EmailValidationEnabled)
                {
                    HasError = !Regex.IsMatch(Text, emailRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
                }
            }
            catch
            {
                HasError = false;
            }
        }

        private void OutlinedMaterialEntryTapped(object sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () => await this.ControlFocused());
        }

        // Here we change the password type of the entry
        // in order to change the eye icon
        private void PasswordEyeTapped(object sender, EventArgs e)
        {
            this.customEntry.IsPassword = !this.customEntry.IsPassword;
        }

        // Here we set the text by every new char
        // and update the charCounter label
        private void OnCustomEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            this.Text = e.NewTextValue;

            if (this.charCounterText.IsVisible)
                this.charCounterText.Text = $"{this.customEntry.Text.Length} / {this.MaxLength}";
            //if (EmailValidationEnabled)
            //{
            //    HasError = !Regex.IsMatch(e.NewTextValue, emailRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            //}
            this.TextChanged?.Invoke(this, e);
        }

        private void OnCustomEntryCompleted(object sender, EventArgs e)
        {
            this.EntryCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}