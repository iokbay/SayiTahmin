using System;

using Xamarin.Forms;

namespace SayiTahmin
{
    public partial class NewItemPage : ContentPage
    {
        public Guess Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();

            Item = new Guess("1234", 2, 1);

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddItem", Item);
            await Navigation.PopToRootAsync();
        }
    }
}
