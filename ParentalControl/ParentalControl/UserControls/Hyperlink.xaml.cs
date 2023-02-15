using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ParentalControl.UserControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Hyperlink : ContentView
    {
        public Hyperlink()
        {
            InitializeComponent();
        }
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(Command), typeof(Hyperlink));
        public Command Command
        {
            get
            {
                return (Command)GetValue(CommandProperty);
            }
            set
            {
                SetValue(CommandProperty, value);
            }
        }
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(Hyperlink));
        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }
        //Object not working
        //public static new readonly BindableProperty MarginProperty = BindableProperty.Create(nameof(Margin), typeof(object), typeof(Hyperlink));
        //public new object Margin
        //{
        //    get
        //    {
        //        return (object)GetValue(MarginProperty);
        //    }
        //    set
        //    {
        //        SetValue(MarginProperty, value);
        //    }
        //}
    }
}